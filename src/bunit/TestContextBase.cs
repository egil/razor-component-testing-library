using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// A test context is a factory that makes it possible to create components under tests.
/// </summary>
public abstract partial class TestContextBase : IDisposable
{
	private bool disposed;
	private BunitRenderer? testRenderer;

	/// <summary>
	/// Gets the renderer used by the test context.
	/// </summary>
	public BunitRenderer Renderer => testRenderer ??= Services.GetRequiredService<BunitRenderer>();

	/// <summary>
	/// Gets the service collection and service provider that is used when a
	/// component is rendered by the test context.
	/// </summary>
	public TestServiceProvider Services { get; }

	/// <summary>
	/// Gets the <see cref="RootRenderTree"/> that all components rendered with the
	/// <c>RenderComponent&lt;TComponent&gt;()</c> methods, are rendered inside.
	/// </summary>
	/// <remarks>
	/// Use this to add default layout- or root-components which a component under test
	/// should be rendered under.
	/// </remarks>
	public RootRenderTree RenderTree { get; } = new();

	/// <summary>
	/// Gets the <see cref="ComponentFactoryCollection"/>. Factories added to it
	/// will be used to create components during testing, starting with the last added
	/// factory. If no factories in the collection can create a requested component,
	/// then the default Blazor factory is used.
	/// </summary>
	public ComponentFactoryCollection ComponentFactories { get; } = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="TestContextBase"/> class.
	/// </summary>
	protected TestContextBase()
	{
		Services = new TestServiceProvider();
		Services.AddSingleton<IComponentActivator>(new BunitComponentActivator(ComponentFactories));
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Disposes of the test context resources, in particular it disposes the <see cref="Services"/>
	/// service provider. Any async services registered with the service provider will disposed first,
	/// but their disposal will not be awaited..
	/// </summary>
	/// <remarks>
	/// The disposing parameter should be false when called from a finalizer, and true when called from the
	/// <see cref="Dispose()"/> method. In other words, it is true when deterministically called and false when non-deterministically called.
	/// </remarks>
	/// <param name="disposing">Set to true if called from <see cref="Dispose()"/>, false if called from a finalizer.f.</param>
	[SuppressMessage("Reliability", "CA2012:Use ValueTasks correctly", Justification = "Explicitly ignoring DisposeAsync to avoid breaking changes to API surface.")]
	protected virtual void Dispose(bool disposing)
	{
		if (disposed || !disposing)
			return;

		disposed = true;

		// Ignore the async task as GetAwaiter().GetResult() can cause deadlock
		// and implementing IAsyncDisposable in TestContext will be a breaking change.
		//
		// NOTE: This has to be called before Services.Dispose().
		// If there are IAsyncDisposable services registered, calling Dispose first
		// causes the service provider to throw an exception.
		_ = Services.DisposeAsync();

		// The service provider should dispose of any
		// disposable object it has created, when it is disposed.
		Services.Dispose();
	}

	/// <summary>
	/// Disposes all components rendered via this <see cref="TestContextBase"/>.
	/// </summary>
	public void DisposeComponents()
	{
		Renderer.DisposeComponents();
	}
}
