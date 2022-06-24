namespace Bunit.Rendering.Internal;

public partial class HtmlizerTests : TestContext
{
	[Theory(DisplayName = "Htmlizer correctly prefixed stopPropagation and preventDefault attributes")]
	[InlineData(false, true)]
	[InlineData(true, false)]
	[InlineData(true, true)]
	public async Task Test002(bool stopPropagation, bool preventDefault)
	{
		var component = await RenderComponent<Htmlizer01Component>(parameters => parameters
			.Add(p => p.OnClick, (MouseEventArgs _) => { })
			.Add(p => p.OnClickStopPropagation, stopPropagation)
			.Add(p => p.OnClickPreventDefault, preventDefault));

		var button = component.Find("button");

		button.HasAttribute(Htmlizer.ToBlazorAttribute("onclick:stopPropagation")).ShouldBe(stopPropagation);
		button.HasAttribute(Htmlizer.ToBlazorAttribute("onclick:preventDefault")).ShouldBe(preventDefault);
	}

	[Fact(DisplayName = "Blazor ElementReferences are included in rendered markup")]
	public async Task Test001()
	{
		var cut = await RenderComponent<Htmlizer01Component>();

		var elmRefValue = cut.Find("button").GetAttribute("blazor:elementreference");

		elmRefValue.ShouldBe(cut.Instance.ButtomElmRef.Id);
	}

	[Fact(DisplayName = "Blazor ElementReferences start in markup on rerenders")]
	public async Task Test003()
	{
		var cut = await RenderComponent<Htmlizer01Component>();
		cut.Find("button").HasAttribute("blazor:elementreference").ShouldBeTrue();

		await cut.SetParametersAndRender(parameters => parameters.Add(p => p.OnClick, (MouseEventArgs _) => { }));

		cut.Find("button").HasAttribute("blazor:elementreference").ShouldBeTrue();
	}

	private class Htmlizer01Component : ComponentBase
	{
		public ElementReference ButtomElmRef { get; set; }

		[Parameter]
		public EventCallback<MouseEventArgs> OnClick { get; set; }

		[Parameter]
		public bool OnClickPreventDefault { get; set; }

		[Parameter]
		public bool OnClickStopPropagation { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			base.BuildRenderTree(builder);

			builder.OpenElement(0, "button");
			builder.AddAttribute(1, "type", "button");

			if (OnClick.HasDelegate)
			{
				builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(this, OnClick));
				builder.AddEventStopPropagationAttribute(3, "onclick", OnClickStopPropagation);
				builder.AddEventPreventDefaultAttribute(4, "onclick", OnClickPreventDefault);
			}

			builder.AddElementReferenceCapture(6, elmRef => ButtomElmRef = elmRef);

			builder.AddContent(6, "Click me!");

			builder.CloseElement();
		}
	}
}
