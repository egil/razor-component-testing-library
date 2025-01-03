using Bunit.Rendering;

namespace Bunit.Extensions;

/// <summary>
/// Extensions methods for <see cref="TestContextBase"/> types.
/// </summary>
public static class TestContextBaseRenderExtensions
{
	/// <summary>
	/// Renders a component, declared in the <paramref name="renderFragment"/>, inside the <see cref="TestContextBase.RenderTree"/>.
	/// </summary>
	/// <typeparam name="TComponent">The type of component to render.</typeparam>
	/// <param name="testContext">Test context to use to render with.</param>
	/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> that contains a declaration of the component.</param>
	/// <returns>A <see cref="IRenderedComponentBase{TComponent}"/>.</returns>
	public static IRenderedComponentBase<TComponent> RenderInsideRenderTree<TComponent>(this TestContextBase testContext, RenderFragment renderFragment)
		where TComponent : IComponent
	{
		if (testContext is null)
			throw new ArgumentNullException(nameof(testContext));

		var baseResult = RenderInsideRenderTree(testContext, renderFragment);
		var component = testContext.Renderer.FindComponent<TComponent>(baseResult);
		var registry = testContext.Services.GetRequiredService<ComponentRegistry>();
		registry.Register(component.Instance);
		return component;
	}

	/// <summary>
	/// Renders a fragment, declared in the <paramref name="renderFragment"/>, inside the <see cref="TestContextBase.RenderTree"/>.
	/// </summary>
	/// <param name="testContext">Test context to use to render with.</param>
	/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> to render.</param>
	/// <returns>A <see cref="IRenderedFragmentBase"/>.</returns>
	public static IRenderedFragmentBase RenderInsideRenderTree(this TestContextBase testContext, RenderFragment renderFragment)
	{
		if (testContext is null)
			throw new ArgumentNullException(nameof(testContext));

		// Wrap fragment in a FragmentContainer so the start of the test supplied
		// razor fragment can be found after, and then wrap in any layout components
		// added to the test context.
		var wrappedInFragmentContainer = FragmentContainer.Wrap(renderFragment);
		var wrappedInRenderTree = testContext.RenderTree.Wrap(wrappedInFragmentContainer);
		var resultBase = testContext.Renderer.RenderFragment(wrappedInRenderTree);

		return testContext.Renderer.FindComponent<FragmentContainer>(resultBase);
	}
}
