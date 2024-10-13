using Bunit.Extensions;
using Bunit.Rendering;
using Microsoft.Extensions.Logging;

namespace Bunit;

/// <summary>
/// A test context is a factory that makes it possible to create components under tests.
/// </summary>
public class TestContext : TestContextBase
{
	/// <summary>
	/// Gets bUnits JSInterop, that allows setting up handlers for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> invocations
	/// that components under tests will issue during testing. It also makes it possible to verify that the invocations has happened as expected.
	/// </summary>
	public BunitJSInterop JSInterop { get; } = new BunitJSInterop();

	/// <summary>
	/// Initializes a new instance of the <see cref="TestContext"/> class.
	/// </summary>
	public TestContext()
	{
		Services.AddDefaultTestContextServices(this, JSInterop);
	}

	/// <summary>
	/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of the component to render.</typeparam>
	/// <param name="parameters">Parameters to pass to the component when it is rendered.</param>
	/// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
	public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters)
		where TComponent : IComponent
	{
		var renderFragment = new ComponentParameterCollection { parameters }
			.ToRenderFragment<TComponent>();

		return Render<TComponent>(renderFragment);
	}

	/// <summary>
	/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of the component to render.</typeparam>
	/// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered.</param>
	/// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
	public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null)
		where TComponent : IComponent
	{
		var renderFragment = new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder)
			.Build()
			.ToRenderFragment<TComponent>();

		return Render<TComponent>(renderFragment);
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/> and returns the first <typeparamref name="TComponent"/> in the resulting render tree.
	/// </summary>
	/// <remarks>
	/// Calling this method is equivalent to calling <c>Render(renderFragment).FindComponent&lt;TComponent&gt;()</c>.
	/// </remarks>
	/// <typeparam name="TComponent">The type of component to find in the render tree.</typeparam>
	/// <param name="renderFragment">The render fragment to render.</param>
	/// <returns>The <see cref="IRenderedComponent{TComponent}"/>.</returns>
	public virtual IRenderedComponent<TComponent> Render<TComponent>(RenderFragment renderFragment)
		where TComponent : IComponent =>
		(IRenderedComponent<TComponent>)this.RenderInsideRenderTree<TComponent>(renderFragment);

	/// <summary>
	/// Renders the <paramref name="renderFragment"/> and returns it as a <see cref="IRenderedFragment"/>.
	/// </summary>
	/// <param name="renderFragment">The render fragment to render.</param>
	/// <returns>The <see cref="IRenderedFragment"/>.</returns>
	public virtual IRenderedFragment Render(RenderFragment renderFragment)
		=> (IRenderedFragment)this.RenderInsideRenderTree(renderFragment);

	/// <summary>
	/// Dummy method required to allow Blazor's compiler to generate
	/// C# from .razor files.
	/// </summary>
	protected virtual void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) { }

	/// <inheritdoc/>
	protected override ITestRenderer CreateTestRenderer()
	{
		var renderedComponentActivator = Services.GetRequiredService<IRenderedComponentActivator>();
		var logger = Services.GetRequiredService<ILoggerFactory>();
		var componentRegistry = Services.GetRequiredService<ComponentRegistry>();
#if !NET5_0_OR_GREATER
		return new WebTestRenderer(renderedComponentActivator, Services, componentRegistry, logger);
#else
		var componentActivator = Services.GetService<IComponentActivator>();
		return componentActivator is null
			? new WebTestRenderer(renderedComponentActivator, Services, componentRegistry, logger)
			: new WebTestRenderer(renderedComponentActivator, Services, componentRegistry, logger, componentActivator);
#endif

	}
}
