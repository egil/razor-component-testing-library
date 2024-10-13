namespace Bunit.TestDoubles;

public partial class RouterTests : TestContext
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
	
	[Fact]
	public void ComponentThatNavigatesToSelfOnClickShouldBeUpdated()
	{
		var cut = RenderComponent<ComponentThatNavigatesToSelfOnButtonClick>();

		cut.Find("button").Click();

		cut.Find("p").TextContent.ShouldBe("1");
	}
	
#if NET7_0_OR_GREATER
	[Fact]
	public void ComponentThatInterceptsNavigationShouldNotBeUpdated()
	{
		var cut = RenderComponent<ComponentThatNavigatesToSelfOnButtonClickIntercepted>();

		cut.Find("button").Click();

		cut.Find("p").TextContent.ShouldBe("0");
	}
#endif
}
