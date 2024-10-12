namespace Bunit.TestDoubles;

public class RouterTests : TestContext
{
	[Fact]
	public void NavigatingToRouteWithPageAttributeShouldSetParameters()
	{
		var cut = RenderComponent<ComponentWithPageAttribute>();
		var navigationManager = cut.Services.GetRequiredService<NavigationManager>();

		navigationManager.NavigateTo("/page/1/test");

		cut.Find("p").TextContent.ShouldBe("1 / test");
	}

	[Fact]
	public void ShouldParseMultiplePageAttributes()
	{
		var cut = RenderComponent<ComponentWithMultiplePageAttributes>();
		var navigationManager = cut.Services.GetRequiredService<NavigationManager>();

		navigationManager.NavigateTo("/page/1");

		cut.Find("p").TextContent.ShouldBe("1");
	}
	
	[Fact]
	public void WhenParameterIsSetNavigatingShouldNotResetNonPageAttributeParameters()
	{
		var cut = RenderComponent<ComponentWithOtherParameters>(p => p.Add(ps => ps.OtherNumber, 2));
		var navigationManager = cut.Services.GetRequiredService<NavigationManager>();

		navigationManager.NavigateTo("/page/1");

		cut.Find("p").TextContent.ShouldBe("1/2");
	}
	
	[Fact]
	public void GivenACatchAllRouteShouldSetParameter()
	{
		var cut = RenderComponent<ComponentWithCatchAllRoute>();
		var navigationManager = cut.Services.GetRequiredService<NavigationManager>();

		navigationManager.NavigateTo("/page/1/2/3");

		cut.Find("p").TextContent.ShouldBe("1/2/3");
	}
	
	[Fact]
	public void ShouldInvokeParameterLifeCycleEvents()
	{
		var cut = RenderComponent<ComponentWithCustomOnParametersSetAsyncsCall>(
			p => p.Add(ps => ps.IncrementOnParametersSet, 10));
		var navigationManager = cut.Services.GetRequiredService<NavigationManager>();

		navigationManager.NavigateTo("/page/1");

		cut.Find("p").TextContent.ShouldBe("11");
	}
	
	[Theory]
	[InlineData("/page/1", "1")]
	[InlineData("/page", "")]
	public void ShouldSetOptionalParameter(string uri, string expectedTextContent)
	{
		var cut = RenderComponent<ComponentWithOptionalParameter>();
		var navigationManager = cut.Services.GetRequiredService<NavigationManager>();

		navigationManager.NavigateTo(uri);

		cut.Find("p").TextContent.ShouldBe(expectedTextContent);
	}
	
	[Route("/page/{count:int}/{name}")]
	private sealed class ComponentWithPageAttribute : ComponentBase
	{
		[Parameter] public int Count { get; set; }
		[Parameter] public string Name { get; set; }
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "p");
			builder.AddContent(1, Count);
			builder.AddContent(2, " / ");
			builder.AddContent(3, Name);
			builder.CloseElement();
		}
	}
	
	[Route("/page")]
	[Route("/page/{count:int}")]
	private sealed class ComponentWithMultiplePageAttributes : ComponentBase
	{
		[Parameter] public int Count { get; set; }
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "p");
			builder.AddContent(1, Count);
			builder.CloseElement();
		}
	}
	
	[Route("/page/{count:int}")]
	private sealed class ComponentWithOtherParameters : ComponentBase
	{
		[Parameter] public int Count { get; set; }
		[Parameter] public int OtherNumber { get; set; }
		
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "p");
			builder.AddContent(1, Count);
			builder.AddContent(2, "/");
			builder.AddContent(3, OtherNumber);
			builder.CloseElement();
		}
	}
	
	[Route("/page/{*pageRoute}")]
	private sealed class ComponentWithCatchAllRoute : ComponentBase
	{
		[Parameter] public string PageRoute { get; set; }
		
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "p");
			builder.AddContent(1, PageRoute);
			builder.CloseElement();
		}
	}
	
	[Route("/page/{count:int}")]
	private sealed class ComponentWithCustomOnParametersSetAsyncsCall : ComponentBase
	{
		[Parameter] public int Count { get; set; }
		[Parameter] public int IncrementOnParametersSet { get; set; }

		protected override void OnParametersSet()
		{
			Count += IncrementOnParametersSet;
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "p");
			builder.AddContent(1, Count);
			builder.CloseElement();
		}
	}
	
	[Route("/page/{count?:int}")]
	private sealed class ComponentWithOptionalParameter : ComponentBase
	{
		[Parameter] public int? Count { get; set; }
		
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "p");
			builder.AddContent(1, Count);
			builder.CloseElement();
		}
	}
}
