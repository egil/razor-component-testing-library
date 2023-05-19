using System.Runtime.ExceptionServices;

namespace Bunit;

/// <summary>
/// Re-render extension methods, optionally with new parameters, for <see cref="IRenderedComponent{TComponent}"/>.
/// </summary>
public static class RenderedComponentRenderExtensions
{
	/// <summary>
	/// Render the component under test again.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to re-render.</param>
	/// <typeparam name="TComponent">The type of the component.</typeparam>
	public static void Render<TComponent>(this IRenderedComponent<TComponent> renderedComponent)
		where TComponent : IComponent
		=> SetParametersAndRender(renderedComponent, ParameterView.Empty);

	/// <summary>
	/// Render the component under test again with the provided <paramref name="parameters"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to re-render with new parameters.</param>
	/// <param name="parameters">Parameters to pass to the component upon rendered.</param>
	/// <typeparam name="TComponent">The type of the component.</typeparam>
	public static void SetParametersAndRender<TComponent>(this IRenderedComponent<TComponent> renderedComponent, ParameterView parameters)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		var result = renderedComponent.InvokeAsync(() =>
			renderedComponent.Instance.SetParametersAsync(parameters));

		if (result.IsFaulted && result.Exception is not null)
		{
			if (result.Exception.InnerExceptions.Count == 1)
			{
				ExceptionDispatchInfo.Capture(result.Exception.InnerExceptions[0]).Throw();
			}
			else
			{
				ExceptionDispatchInfo.Capture(result.Exception).Throw();
			}
		}
		else if (!result.IsCompleted)
		{
			result.GetAwaiter().GetResult();
		}
	}

	/// <summary>
	/// Render the component under test again with the provided <paramref name="parameters"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to re-render with new parameters.</param>
	/// <param name="parameters">Parameters to pass to the component upon rendered.</param>
	/// <typeparam name="TComponent">The type of the component.</typeparam>
	public static void SetParametersAndRender<TComponent>(this IRenderedComponent<TComponent> renderedComponent, params ComponentParameter[] parameters)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);
		ArgumentNullException.ThrowIfNull(parameters);

		SetParametersAndRender(renderedComponent, ToParameterView(parameters));
	}

	/// <summary>
	/// Render the component under test again with the provided parameters from the <paramref name="parameterBuilder"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to re-render with new parameters.</param>
	/// <param name="parameterBuilder">An action that receives a <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</param>
	/// <typeparam name="TComponent">The type of the component.</typeparam>
	public static void SetParametersAndRender<TComponent>(this IRenderedComponent<TComponent> renderedComponent, Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);
		ArgumentNullException.ThrowIfNull(parameterBuilder);

		var builder = new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder);
		SetParametersAndRender(renderedComponent, ToParameterView(builder.Build()));
	}

	private static ParameterView ToParameterView(IReadOnlyCollection<ComponentParameter> parameters)
	{
		var parameterView = ParameterView.Empty;

		if (parameters.Count > 0)
		{
			var paramDict = new Dictionary<string, object?>(StringComparer.Ordinal);

			foreach (var param in parameters)
			{
				if (param.IsCascadingValue)
					throw new InvalidOperationException($"You cannot provide a new cascading value through the {nameof(SetParametersAndRender)} method.");
				if (param.Name is null)
					throw new InvalidOperationException("A parameters name is required.");

				paramDict.Add(param.Name, param.Value);
			}

			// Nullable is disabled to get around the issue with different annotations
			// between .netstandard2.1 and net6.
#nullable disable
			parameterView = ParameterView.FromDictionary(paramDict);
#nullable restore
		}

		return parameterView;
	}
}