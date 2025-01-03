using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering;

/// <summary>
/// Represents a bUnit <see cref="ITestRenderer"/> used to render Blazor components and fragments during bUnit tests.
/// </summary>
public class TestRenderer : Renderer, ITestRenderer
{
#if NET8_0_OR_GREATER
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_isBatchInProgress")]
	extern static ref bool GetIsBatchInProgressField(Renderer renderer);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "SetDirectParameters")]
	extern static void CallSetDirectParameters(ComponentState componentState, ParameterView parameters);
#else
	private static readonly Type RendererType = typeof(Renderer);
	private static readonly MethodInfo GetRequiredComponentStateMethod = RendererType.GetMethod("GetRequiredComponentState", BindingFlags.Instance | BindingFlags.NonPublic)!;
	private static readonly FieldInfo IsBatchInProgressField = RendererType.GetField("_isBatchInProgress", BindingFlags.Instance | BindingFlags.NonPublic)!;
#endif

	private readonly object renderTreeUpdateLock = new();
	private readonly Dictionary<int, IRenderedFragmentBase> renderedComponents = new();
	private readonly List<RootComponent> rootComponents = new();
	private readonly ILogger<TestRenderer> logger;
	private readonly IRenderedComponentActivator activator;
	private readonly ComponentRegistry registry;
	private bool disposed;
	private TaskCompletionSource<Exception> unhandledExceptionTsc = new(TaskCreationOptions.RunContinuationsAsynchronously);
	private Exception? capturedUnhandledException;

	private bool IsBatchInProgress
	{
#pragma warning disable S1144 // Unused private types or members should be removed
		get
		{
#if NET8_0_OR_GREATER
			return GetIsBatchInProgressField(this);
#else
			return (bool)(IsBatchInProgressField.GetValue(this) ?? false);
#endif
		}
#pragma warning restore S1144 // Unused private types or members should be removed
		set
		{
#if NET8_0_OR_GREATER
			GetIsBatchInProgressField(this) = value;
#else
			IsBatchInProgressField.SetValue(this, value);
#endif
		}
	}

	/// <inheritdoc/>
	public Task<Exception> UnhandledException => unhandledExceptionTsc.Task;

	/// <inheritdoc/>
	public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

	/// <summary>
	/// Gets the number of render cycles that has been performed.
	/// </summary>
	internal int RenderCount { get; private set; }

#if NET9_0_OR_GREATER
	private RendererInfo? rendererInfo;

	/// <inheritdoc/>
	[SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "The exception is raised to guide users.")]
	protected override RendererInfo RendererInfo => rendererInfo ?? throw new MissingRendererInfoException();

	/// <inheritdoc/>
	public void SetRendererInfo(RendererInfo? rendererInfo)
	{
		this.rendererInfo = rendererInfo;
	}

#endif

#if NETSTANDARD
	/// <summary>
	/// Initializes a new instance of the <see cref="TestRenderer"/> class.
	/// </summary>
	public TestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ComponentRegistry registry, ILoggerFactory loggerFactory)
		: base(services, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<TestRenderer>();
		this.activator = renderedComponentActivator;
		this.registry = registry;
	}
#elif NET5_0_OR_GREATER
	/// <summary>
	/// Initializes a new instance of the <see cref="TestRenderer"/> class.
	/// </summary>
	public TestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ComponentRegistry registry, ILoggerFactory loggerFactory)
		: base(services, loggerFactory, new BunitComponentActivator(services, services.GetRequiredService<ComponentFactoryCollection>(), null))
	{
		logger = loggerFactory.CreateLogger<TestRenderer>();
		this.activator = renderedComponentActivator;
		this.registry = registry;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TestRenderer"/> class.
	/// </summary>
	public TestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory, ComponentRegistry registry, IComponentActivator componentActivator)
		: base(services, loggerFactory, new BunitComponentActivator(services, services.GetRequiredService<ComponentFactoryCollection>(), componentActivator))
	{
		logger = loggerFactory.CreateLogger<TestRenderer>();
		this.activator = renderedComponentActivator;
		this.registry = registry;
	}
#endif

	/// <inheritdoc/>
	public IRenderedFragmentBase RenderFragment(RenderFragment renderFragment)
		=> Render(renderFragment, id => activator.CreateRenderedFragment(id));

	/// <inheritdoc/>
	public IRenderedComponentBase<TComponent> RenderComponent<TComponent>(ComponentParameterCollection parameters)
		where TComponent : IComponent
	{
		if (parameters is null)
			throw new ArgumentNullException(nameof(parameters));

		var renderFragment = parameters.ToRenderFragment<TComponent>();
		return Render(renderFragment, id => activator.CreateRenderedComponent<TComponent>(id));
	}

	/// <inheritdoc/>
	public new Task DispatchEventAsync(
		ulong eventHandlerId,
		EventFieldInfo fieldInfo,
		EventArgs eventArgs) => DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs, ignoreUnknownEventHandlers: false);

	/// <inheritdoc/>
#if !NET8_0_OR_GREATER
	public Task DispatchEventAsync(
#else
	public new Task DispatchEventAsync(
#endif
		ulong eventHandlerId,
		EventFieldInfo fieldInfo,
		EventArgs eventArgs,
		bool ignoreUnknownEventHandlers)
	{
		if (fieldInfo is null)
			throw new ArgumentNullException(nameof(fieldInfo));

		if (disposed)
			throw new ObjectDisposedException(nameof(TestRenderer));

		// Calling base.DispatchEventAsync updates the render tree
		// if the event contains associated data.
		lock (renderTreeUpdateLock)
		{
			if (disposed)
				throw new ObjectDisposedException(nameof(TestRenderer));

			var result = Dispatcher.InvokeAsync(() =>
			{
				ResetUnhandledException();

				try
				{
					logger.LogDispatchingEvent(eventHandlerId, fieldInfo, eventArgs);
					return base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs);
				}
				catch (ArgumentException ex) when (string.Equals(ex.Message, $"There is no event handler associated with this event. EventId: '{eventHandlerId}'. (Parameter 'eventHandlerId')", StringComparison.Ordinal))
				{
					if (ignoreUnknownEventHandlers)
					{
						return Task.CompletedTask;
					}

					var betterExceptionMsg = new UnknownEventHandlerIdException(eventHandlerId, fieldInfo, ex);
					return Task.FromException(betterExceptionMsg);
				}
			});

			if (result.IsFaulted && result.Exception is not null)
			{
				HandleException(result.Exception);
			}

			AssertNoUnhandledExceptions();

			return result;
		}
	}

	/// <inheritdoc/>
	public IRenderedComponentBase<TComponent> FindComponent<TComponent>(IRenderedFragmentBase parentComponent)
		where TComponent : IComponent
	{
		var foundComponents = FindComponents<TComponent>(parentComponent, 1);
		return foundComponents.Count == 1
			? foundComponents[0]
			: throw new ComponentNotFoundException(typeof(TComponent));
	}

	/// <inheritdoc/>
	public IReadOnlyList<IRenderedComponentBase<TComponent>> FindComponents<TComponent>(IRenderedFragmentBase parentComponent)
		where TComponent : IComponent
		=> FindComponents<TComponent>(parentComponent, int.MaxValue);

	/// <inheritdoc />
	public void DisposeComponents()
	{
		if (disposed)
			throw new ObjectDisposedException(nameof(TestRenderer));

		lock (renderTreeUpdateLock)
		{
			// The dispatcher will always return a completed task,
			// when dealing with an IAsyncDisposable.
			// Therefore checking for a completed task and awaiting it
			// will only work on IDisposable
			Dispatcher.InvokeAsync(() =>
			{
				ResetUnhandledException();

				foreach (var root in rootComponents)
				{
					root.Detach();
				}
			});

			rootComponents.Clear();
			registry.Clear();
			AssertNoUnhandledExceptions();
		}
	}

#if NET8_0_OR_GREATER
	/// <inheritdoc/>
	protected override IComponent ResolveComponentForRenderMode(Type componentType, int? parentComponentId,
		IComponentActivator componentActivator, IComponentRenderMode renderMode)

	{
		ArgumentNullException.ThrowIfNull(componentActivator);
		return componentActivator.CreateInstance(componentType);
	}
#endif

#if NET9_0_OR_GREATER
	/// <inheritdoc/>
	protected override IComponentRenderMode? GetComponentRenderMode(IComponent component)
	{
		ArgumentNullException.ThrowIfNull(component);

		// Search from the current component all the way up the render tree.
		// All components must have the same render mode specified (or none at all).
		// Return the render mode that is found after checking the full tree.
		return GetAndValidateRenderMode(component, childRenderMode: null);

		IComponentRenderMode? GetAndValidateRenderMode(IComponent component, IComponentRenderMode? childRenderMode)
		{
			var componentState = GetComponentState(component);
			var renderMode = GetRenderModeForComponent(componentState);

			if (childRenderMode is not null && renderMode is not null && childRenderMode != renderMode)
			{
				throw new RenderModeMisMatchException();
			}

			return componentState.ParentComponentState is null
				? renderMode ?? childRenderMode
				: GetAndValidateRenderMode(componentState.ParentComponentState.Component, renderMode ?? childRenderMode);
		}

		IComponentRenderMode? GetRenderModeForComponent(ComponentState componentState)
		{
			var renderModeAttribute = componentState.Component.GetType().GetCustomAttribute<RenderModeAttribute>();
			if (renderModeAttribute is { Mode: not null })
			{
				return renderModeAttribute.Mode;
			}

			if (componentState.ParentComponentState is not null)
			{
				var parentFrames = GetCurrentRenderTreeFrames(componentState.ParentComponentState.ComponentId);
				var foundComponentStart = false;
				for (var i = 0; i < parentFrames.Count; i++)
				{
					ref var frame = ref parentFrames.Array[i];

					if (frame.FrameType is RenderTreeFrameType.Component)
					{
						foundComponentStart = frame.ComponentId == componentState.ComponentId;
					}
					else if (foundComponentStart && frame.FrameType is RenderTreeFrameType.ComponentRenderMode)
					{
						return frame.ComponentRenderMode;
					}
				}
			}

			return null;
		}
	}
#endif

	/// <inheritdoc/>
	internal Task SetDirectParametersAsync(IRenderedFragmentBase renderedComponent, ParameterView parameters)
	{
		if (disposed)
			throw new ObjectDisposedException(nameof(TestRenderer));

		var result = Dispatcher.InvokeAsync(() =>
		{
			try
			{
				IsBatchInProgress = true;
				SetDirectParametersViaComponentState(this, renderedComponent.ComponentId, parameters);
			}
			catch (TargetInvocationException ex) when (ex.InnerException is not null)
			{
				throw ex.InnerException;
			}
			finally
			{
				IsBatchInProgress = false;
			}

			ProcessPendingRender();
		});

		if (result.IsFaulted && result.Exception is not null)
		{
			HandleException(result.Exception);
		}

		AssertNoUnhandledExceptions();

		return result;

#if NET8_0_OR_GREATER
		static void SetDirectParametersViaComponentState(TestRenderer renderer, int componentId, in ParameterView parameters)
		{
			var componentState = renderer.GetComponentState(componentId);
			CallSetDirectParameters(componentState, parameters);
		}
#else
		static void SetDirectParametersViaComponentState(TestRenderer renderer, int componentId, in ParameterView parameters)
		{
			var componentState = GetRequiredComponentStateMethod.Invoke(renderer, new object[] { componentId })!;
			var setDirectParametersMethod = componentState.GetType().GetMethod("SetDirectParameters", BindingFlags.Public | BindingFlags.Instance)!;
			setDirectParametersMethod.Invoke(componentState, new object[] { parameters });
		}
#endif
	}

	/// <inheritdoc/>
	protected override void ProcessPendingRender()
	{
		if (disposed)
		{
			logger.LogRenderCycleActiveAfterDispose();
			return;
		}

		// Blocks updates to the renderers internal render tree
		// while the render tree is being read elsewhere.
		// base.ProcessPendingRender calls UpdateDisplayAsync,
		// so there is no need to lock in that method.
		lock (renderTreeUpdateLock)
		{
			if (disposed)
			{
				logger.LogRenderCycleActiveAfterDispose();
				return;
			}

			base.ProcessPendingRender();
		}
	}

	/// <inheritdoc/>
	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
	{
		var renderEvent = new RenderEvent();
		PrepareRenderEvent(renderBatch);
		ApplyRenderEvent(renderEvent);

		return Task.CompletedTask;

		void PrepareRenderEvent(in RenderBatch renderBatch)
		{
			for (var i = 0; i < renderBatch.DisposedComponentIDs.Count; i++)
			{
				var id = renderBatch.DisposedComponentIDs.Array[i];
				renderEvent.SetDisposed(id);
			}

			for (int i = 0; i < renderBatch.UpdatedComponents.Count; i++)
			{
				ref var update = ref renderBatch.UpdatedComponents.Array[i];
				renderEvent.SetUpdated(update.ComponentId, update.Edits.Count > 0);
			}

			foreach (var (_, rc) in renderedComponents)
			{
				LoadChangesIntoRenderEvent(rc.ComponentId);
			}
		}

		void LoadChangesIntoRenderEvent(int componentId)
		{
			var status = renderEvent.GetOrCreateStatus(componentId);
			if (status.FramesLoaded || status.Disposed)
				return;

			var frames = GetCurrentRenderTreeFrames(componentId);
			renderEvent.AddFrames(componentId, frames);

			for (var i = 0; i < frames.Count; i++)
			{
				ref var frame = ref frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					// If a child component of the current components has been
					// disposed, there is no reason to load the disposed components
					// render tree frames. This can also cause a stack overflow if
					// the current component was previously a child of the disposed
					// component (is that possible?)
					var childStatus = renderEvent.GetOrCreateStatus(frame.ComponentId);
					if (childStatus.Disposed)
					{
						logger.LogDisposedChildInRenderTreeFrame(componentId, frame.ComponentId);
					}
					// The assumption is that a component cannot be in multiple places at
					// once. However, in case this is not a correct assumption, this
					// ensures that a child components frames are only loaded once.
					else if (!renderEvent.GetOrCreateStatus(frame.ComponentId).FramesLoaded)
					{
						LoadChangesIntoRenderEvent(frame.ComponentId);
					}

					if (childStatus.Rendered || childStatus.Changed || childStatus.Disposed)
					{
						status.Rendered = status.Rendered || childStatus.Rendered;

						// The current component should also be marked as changed if the child component is
						// either changed or disposed, as there is a good chance that the child component
						// contained markup which is no longer visible.
						status.Changed = status.Changed || childStatus.Changed || childStatus.Disposed;
					}
				}
			}
		}
	}

	private void ApplyRenderEvent(RenderEvent renderEvent)
	{
		RenderCount++;

		foreach (var (componentId, status) in renderEvent.Statuses)
		{
			if (status.UpdatesApplied || !renderedComponents.TryGetValue(componentId, out var rc))
			{
				continue;
			}

			if (status.Disposed)
			{
				renderedComponents.Remove(componentId);
				rc.OnRender(renderEvent);
				renderEvent.SetUpdatedApplied(componentId);
				logger.LogComponentDisposed(componentId);
				continue;
			}

			if (status.UpdateNeeded)
			{
				rc.OnRender(renderEvent);
				renderEvent.SetUpdatedApplied(componentId);

				// RC can replace the instance of the component it is bound
				// to while processing the update event, e.g. during the
				// initial render of a component.
				if (componentId != rc.ComponentId)
				{
					renderedComponents.Remove(componentId);
					renderedComponents.Add(rc.ComponentId, rc);
					renderEvent.SetUpdatedApplied(rc.ComponentId);
				}

				logger.LogComponentRendered(rc.ComponentId);
			}
		}
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if (disposed)
			return;

		lock (renderTreeUpdateLock)
		{
			if (disposed)
				return;

			disposed = true;

			if (disposing)
			{
				foreach (var rc in renderedComponents.Values)
				{
					rc.Dispose();
				}

				renderedComponents.Clear();
				unhandledExceptionTsc.TrySetCanceled();
			}

			Dispatcher.InvokeAsync(() => base.Dispose(disposing));
		}
	}

	private TResult Render<TResult>(RenderFragment renderFragment, Func<int, TResult> activator)
		where TResult : IRenderedFragmentBase
	{
		if (disposed)
			throw new ObjectDisposedException(nameof(TestRenderer));

		var renderTask = Dispatcher.InvokeAsync(() =>
		{
			ResetUnhandledException();

			var root = new RootComponent(renderFragment);
			var rootComponentId = AssignRootComponentId(root);
			var result = activator(rootComponentId);
			renderedComponents.Add(rootComponentId, result);
			rootComponents.Add(root);
			root.Render();
			return result;
		});

		TResult result;

		if (!renderTask.IsCompleted)
		{
			logger.LogAsyncInitialRender();
			result = renderTask.GetAwaiter().GetResult();
		}
		else
		{
			result = renderTask.Result;
		}

		logger.LogInitialRenderCompleted(result.ComponentId);

		AssertNoUnhandledExceptions();

		return result;
	}

	private IReadOnlyList<IRenderedComponentBase<TComponent>> FindComponents<TComponent>(IRenderedFragmentBase parentComponent, int resultLimit)
		where TComponent : IComponent
	{
		if (parentComponent is null)
			throw new ArgumentNullException(nameof(parentComponent));

		if (disposed)
			throw new ObjectDisposedException(nameof(TestRenderer));

		var result = new List<IRenderedComponentBase<TComponent>>();
		var framesCollection = new RenderTreeFrameDictionary();

		// Blocks the renderer from changing the render tree
		// while this method searches through it.
		lock (renderTreeUpdateLock)
		{
			if (disposed)
				throw new ObjectDisposedException(nameof(TestRenderer));

			FindComponentsInRenderTree(parentComponent.ComponentId);
		}

		return result;

		void FindComponentsInRenderTree(int componentId)
		{
			var frames = GetOrLoadRenderTreeFrame(framesCollection, componentId);

			for (var i = 0; i < frames.Count; i++)
			{
				ref var frame = ref frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					if (frame.Component is TComponent component)
					{
						result.Add(GetOrCreateRenderedComponent(framesCollection, frame.ComponentId, component));

						if (result.Count == resultLimit)
							return;
					}

					FindComponentsInRenderTree(frame.ComponentId);

					if (result.Count == resultLimit)
						return;
				}
			}
		}
	}

	private IRenderedComponentBase<TComponent> GetOrCreateRenderedComponent<TComponent>(RenderTreeFrameDictionary framesCollection, int componentId, TComponent component)
		where TComponent : IComponent
	{
		if (renderedComponents.TryGetValue(componentId, out var renderedComponent))
		{
			return (IRenderedComponentBase<TComponent>)renderedComponent;
		}

		LoadRenderTreeFrames(componentId, framesCollection);
		var result = activator.CreateRenderedComponent(componentId, component, framesCollection);
		renderedComponents.Add(result.ComponentId, result);

		return result;
	}

	/// <summary>
	/// Populates the <paramref name="framesCollection"/> with <see cref="ArrayRange{RenderTreeFrame}"/>
	/// starting with the one that belongs to the component with ID <paramref name="componentId"/>.
	/// </summary>
	private void LoadRenderTreeFrames(int componentId, RenderTreeFrameDictionary framesCollection)
	{
		var frames = GetOrLoadRenderTreeFrame(framesCollection, componentId);

		for (var i = 0; i < frames.Count; i++)
		{
			ref var frame = ref frames.Array[i];

			if (frame.FrameType == RenderTreeFrameType.Component && !framesCollection.Contains(frame.ComponentId))
			{
				LoadRenderTreeFrames(frame.ComponentId, framesCollection);
			}
		}
	}

	/// <summary>
	/// Gets the <see cref="ArrayRange{RenderTreeFrame}"/> from the <paramref name="framesCollection"/>.
	/// If the <paramref name="framesCollection"/> does not contain the frames, they are loaded into it first.
	/// </summary>
	private ArrayRange<RenderTreeFrame> GetOrLoadRenderTreeFrame(RenderTreeFrameDictionary framesCollection, int componentId)
	{
		if (!framesCollection.Contains(componentId))
		{
			var frames = GetCurrentRenderTreeFrames(componentId);
			framesCollection.Add(componentId, frames);
		}

		return framesCollection[componentId];
	}

	/// <inheritdoc/>
	protected override void HandleException([NotNull] Exception exception)
	{
		if (disposed)
			return;

		logger.LogUnhandledException(exception);

		capturedUnhandledException = exception;

		if (!unhandledExceptionTsc.TrySetResult(capturedUnhandledException))
		{
			unhandledExceptionTsc = new TaskCompletionSource<Exception>(TaskCreationOptions.RunContinuationsAsynchronously);
			unhandledExceptionTsc.SetResult(capturedUnhandledException);
		}
	}

	private void ResetUnhandledException()
	{
		capturedUnhandledException = null;

		if (unhandledExceptionTsc.Task.IsCompleted)
			unhandledExceptionTsc = new TaskCompletionSource<Exception>(TaskCreationOptions.RunContinuationsAsynchronously);
	}

	private void AssertNoUnhandledExceptions()
	{
		// Ensure we are not throwing an exception while a render is ongoing.
		// This could lead to the renderer being disposed which could lead to
		// tests failing that should not be failing.
		lock (renderTreeUpdateLock)
		{
			if (disposed)
				return;

			if (capturedUnhandledException is { } unhandled)
			{
				capturedUnhandledException = null;

				if (unhandled is AggregateException { InnerExceptions.Count: 1 } aggregateException)
				{
					ExceptionDispatchInfo.Capture(aggregateException.InnerExceptions[0]).Throw();
				}
				else
				{
					ExceptionDispatchInfo.Capture(unhandled).Throw();
				}
			}
		}
	}
}
