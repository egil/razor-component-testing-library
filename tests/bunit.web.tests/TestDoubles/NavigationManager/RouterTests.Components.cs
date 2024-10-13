namespace Bunit.TestDoubles;

public partial class RouterTests : TestContext
{
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
	
	[Route("/page/{count:int}")]
	private sealed class ComponentThatNavigatesToSelfOnButtonClick : ComponentBase
	{
		[Parameter] public int Count { get; set; }
		
		[Inject] private NavigationManager NavigationManager { get; set; }
		
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "button");
			builder.AddAttribute(1, "onclick", EventCallback.Factory.Create(this, () => NavigationManager.NavigateTo($"/page/{Count + 1}")));
			builder.AddContent(2, "Increment");
			builder.CloseElement();
			builder.OpenElement(3, "p");
			builder.AddContent(4, Count);
			builder.CloseElement();
		}
	}
	
#if NET7_0_OR_GREATER
	[Route("/page/{count:int}")]
	private sealed class ComponentThatNavigatesToSelfOnButtonClickIntercepted : ComponentBase
	{
		[Parameter] public int Count { get; set; }
		
		[Inject] private NavigationManager NavigationManager { get; set; }
		
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "button");
			builder.AddAttribute(1, "onclick", EventCallback.Factory.Create(this, () => NavigationManager.NavigateTo($"/page/{Count + 1}")));
			builder.AddContent(2, "Increment");
			builder.CloseElement();
			builder.OpenElement(3, "p");
			builder.AddContent(4, Count);
			builder.CloseElement();
			builder.OpenComponent<NavigationLock>(5);
			builder.AddAttribute(6, "OnBeforeInternalNavigation", 
				EventCallback.Factory.Create<LocationChangingContext>(this,
					InterceptNavigation
				));
			builder.CloseComponent();
		}
		
		private static void InterceptNavigation(LocationChangingContext context)
		{
			context.PreventNavigation();
		}
	}
#endif
}
