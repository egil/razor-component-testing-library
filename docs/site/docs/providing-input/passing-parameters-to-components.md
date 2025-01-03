---
uid: passing-parameters-to-components
title: Passing parameters to components
---

# Passing parameters to components

bUnit comes with a number of ways to pass parameters to components under test:

1. In tests written in `.razor` files, passing parameters is most easily done with inside an inline Razor template passed to the `Render` method, although the parameter passing option available in tests written in C# files is also available here.

2. In tests written in `.cs` files, bUnit includes a strongly typed builder. There are two methods in bUnit that allow passing parameters in C#-based test code:

   - `RenderComponent` method on the test context, which is used to render a component initially.
   - `SetParametersAndRender` method on a rendered component, which is used to pass new parameters to an already rendered component.

In the following sub sections, we will show both `.cs`- and `.razor`-based test code; just click between them using the tabs.

> [!NOTE]
> The examples below are written using **xUnit**, but the code is the same with **NUnit** and **MSTest**.
>
> The example tests are inheriting from the `TestContext` as described in the <xref:writing-tests> page. If your test class is not inheriting from the `TestContext`, then you should explicitly instantiate the `TestContext` in your tests. The recommended way is to inherit from the `TestContext`. An example below will demonstrate how to instantiate a `TestContext` per test method.

## Regular parameters

A regular parameter is one that is declared using the `[Parameter]` attribute. The following subsections will cover both _non_-Blazor type parameters, e.g. `int` and `List<string>`, and the special Blazor types like `EventCallback` and `RenderFragment`.

### Non-Blazor type parameters

Let's look at an example of passing parameters that takes types which are _not_ special to Blazor, i.e.:

[!code-csharp[NonBlazorTypesParams](../../../samples/components/NonBlazorTypesParams.cs#L10-L17)]

This can be done like this:

# [C# test code](#tab/csharp)

[!code-csharp[NonBlazorTypesParamsTest.cs](../../../samples/tests/xunit/NonBlazorTypesParamsTest.cs#L10-L24)]

The example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add` method, which takes a parameter selector expression that selects the parameter using a lambda, and forces you to provide the correct type for the value. This makes the builder's methods strongly typed and refactor-safe.

# [Razor test code](#tab/razor)

[!code-cshtml[NonBlazorTypesParamsTest.razor](../../../samples/tests/razor/NonBlazorTypesParamsTest.razor)]

The example passes a inline Razor template to the `Render()` method. The parameters to the component is just passed like normal in Razor code.

***

### EventCallback parameters

This example will pass parameters to the following two `EventCallback` parameters:

[!code-csharp[EventCallbackParams](../../../samples/components/EventCallbackParams.cs#L10-L17)]

This can be done like this:

# [C# test code](#tab/csharp)

[!code-csharp[EventCallbackParamsTest.cs](../../../samples/tests/xunit/EventCallbackParamsTest.cs#L11-L27)]

The example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add` method, which takes a parameter selector expression that selects the parameter using a lambda, and forces you to provide the correct type of callback method. This makes the builder's methods strongly typed and refactor-safe.

# [Razor test code](#tab/razor)

[!code-cshtml[EventCallbackParamsTest.razor](../../../samples/tests/razor/EventCallbackParamsTest.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The parameters to the component is just passed like normal in Razor code.

***

### ChildContent parameters

The `ChildContent` parameter in Blazor is represented by a `RenderFragment`. In Blazor, this can be regular HTML markup, it can be Razor markup, e.g. other component declarations, or a mix of the two. If it is another component, then that component can also receive child content, and so forth.

The following subsections have different examples of child content being passed to the following component:

[!code-csharp[ChildContentParams.razor](../../../samples/components/ChildContentParams.cs#L10-L14)]

#### Passing HTML to the ChildContent parameter

# [C# test code](#tab/csharp)

[!code-csharp[ChildContentParamsTest.cs](../../../samples/tests/xunit/ChildContentParams1Test.cs#L11-L22)]

The example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `AddChildContent` method to pass an HTML markup string as the input to the `ChildContent` parameter.

# [Razor test code](#tab/razor)

[!code-cshtml[ChildContentParamsTest.razor](../../../samples/tests/razor/ChildContentParams1Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The child content, some HTML markup, is just passed like normal in Razor code.

***

#### Passing a component without parameters to the ChildContent parameter

To pass a component, e.g. the classic `<Counter>` component, which does not take any parameters itself, to a `ChildContent` parameter, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[ChildContentParamsTest.cs](../../../samples/tests/xunit/ChildContentParams2Test.cs#L11-L22)]

The example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `AddChildContent<TChildComponent>` method, where `TChildComponent` is the (child) component that should be passed to the component under test's `ChildContent` parameter.

# [Razor test code](#tab/razor)

[!code-cshtml[ChildContentParamsTest.razor](../../../samples/tests/razor/ChildContentParams2Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The child content, some Razor markup, is just passed like normal in Razor code.

***

#### Passing a component with parameters to the ChildContent parameter

To pass a component with parameters to a component under test, e.g. the `<Alert>` component with the following parameters, do the following:

[!code-csharp[Alert.razor](../../../samples/components/Alert.razor#L2-L4)]

# [C# test code](#tab/csharp)

[!code-csharp[ChildContentParamsTest.cs](../../../samples/tests/xunit/ChildContentParams3Test.cs#L11-L26)]

The example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `AddChildContent<TChildComponent>` method, where `TChildComponent` is the (child) component that should be passed to the component under test. The `AddChildContent<TChildComponent>` method takes an optional <xref:Bunit.ComponentParameterCollectionBuilder`1> as input, which can be used to pass parameters to the `TChildComponent` component, which in this case is the `<Alert>` component.

# [Razor test code](#tab/razor)

[!code-cshtml[ChildContentParamsTest.razor](../../../samples/tests/razor/ChildContentParams3Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The child content, some Razor markup, and parameters to the child component, is just passed like normal in Razor code.

***

#### Passing a mix of Razor and HTML to a ChildContent parameter

Some times you need to pass multiple different types of content to a ChildContent parameter, e.g. both some markup and a component. This can be done in the following way:

# [C# test code](#tab/csharp)

[!code-csharp[ChildContentParamsTest.cs](../../../samples/tests/xunit/ChildContentParams4Test.cs#L11-L27)]

Passing a mix of markup and components to a `ChildContent` parameter is done by simply calling the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `AddChildContent()` methods as seen here.

# [Razor test code](#tab/razor)

[!code-cshtml[ChildContentParamsTest.razor](../../../samples/tests/razor/ChildContentParams4Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The child content, some Razor markup, and parameters to the child component, is just passed like normal in Razor code.

***

### RenderFragment parameters

A `RenderFragment` parameter is very similar to the special `ChildContent` parameter described in the previous section, since a `ChildContent` parameter _is_ of type `RenderFragment`. The only difference is the name, which must be anything other than `ChildContent`.

In Blazor, a `RenderFragment` parameter can be regular HTML markup, it can be Razor markup, e.g. other component declarations, or it can be a mix of the two. If it is another component, then that component can also receive child content, and so forth.

The following subsections have different examples of content being passed to the following component's `RenderFragment` parameter:

[!code-csharp[RenderFragmentParams.razor](../../../samples/components/RenderFragmentParams.cs#L9-L13)]

#### Passing HTML to a RenderFragment parameter

# [C# test code](#tab/csharp)

[!code-csharp[RenderFragmentParamsTest.cs](../../../samples/tests/xunit/RenderFragmentParams1Test.cs#L11-L22)]

The example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add` method to pass an HTML markup string as the input to the `RenderFragment` parameter.

# [Razor test code](#tab/razor)

[!code-cshtml[RenderFragmentParamsTest.razor](../../../samples/tests/razor/RenderFragmentParams1Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The child content, some HTML markup, is just passed like normal in Razor code.

***

#### Passing a component without parameters to a RenderFragment parameter

To pass a component such as the classic `<Counter>` component, which does not take any parameters, to a `RenderFragment` parameter, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[RenderFragmentParamsTest.cs](../../../samples/tests/xunit/RenderFragmentParams2Test.cs#L11-L22)]

The example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add<TChildComponent>` method, where `TChildComponent` is the (child) component that should be passed to the `RenderFragment` parameter.

# [Razor test code](#tab/razor)

[!code-cshtml[RenderFragmentParamsTest.razor](../../../samples/tests/razor/RenderFragmentParams2Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The child content, some Razor markup, is just passed like normal in Razor code.

***

#### Passing a component with parameters to a RenderFragment parameter

To pass a component with parameters to a `RenderFragment` parameter, e.g. the `<Alert>` component with the following parameters, do the following:

[!code-csharp[Alert.razor](../../../samples/components/Alert.razor#L2-L4)]

# [C# test code](#tab/csharp)

[!code-csharp[RenderFragmentParamsTest.cs](../../../samples/tests/xunit/RenderFragmentParams3Test.cs#L11-L26)]

The example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add<TChildComponent>` method, where `TChildComponent` is the (child) component that should be passed to the `RenderFragment` parameter. The `Add<TChildComponent>` method takes an optional <xref:Bunit.ComponentParameterCollectionBuilder`1> as input, which can be used to pass parameters to the `TChildComponent` component, which in this case is the `<Alert>` component.

# [Razor test code](#tab/razor)

[!code-cshtml[RenderFragmentParamsTest.razor](../../../samples/tests/razor/RenderFragmentParams3Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The child content, some Razor markup, and parameters to the child component, is just passed like normal in Razor code.

***

#### Passing a mix of Razor and HTML to a RenderFragment parameter

Some times you need to pass multiple different types of content to a `RenderFragment` parameter, e.g. both markup and and a component. This can be done in the following way:

# [C# test code](#tab/csharp)

[!code-csharp[RenderFragmentParamsTest.cs](../../../samples/tests/xunit/RenderFragmentParams4Test.cs#L11-L27)]

Passing a mix of markup and components to a `RenderFragment` parameter is simply done by calling the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add()` methods or using the `ChildContent()` factory methods in <xref:Bunit.ComponentParameterFactory>, as seen here.

# [Razor test code](#tab/razor)

[!code-cshtml[RenderFragmentParamsTest.razor](../../../samples/tests/razor/RenderFragmentParams4Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The child content, some HTML and Razor markup, and parameters to the child component, is just passed like normal in Razor code.

***

### Templates parameters

Template parameters are closely related to the `RenderFragment` parameters described in the previous section. The difference is that a template parameter is of type `RenderFragment<TValue>`. As with a regular `RenderFragment`, a `RenderFragment<TValue>` template parameter can consist of regular HTML markup, it can be Razor markup, e.g. other component declarations, or it can be a mix of the two. If it is another component, then that component can also receive child content, and so forth.

The following examples renders a template component which has a `RenderFragment<TValue>` template parameter:

[!code-csharp[TemplateParams.razor](../../../samples/components/TemplateParams.razor)]

#### Passing HTML-based templates

To pass a template into a `RenderFragment<TValue>` parameter that just consists of regular HTML markup, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[TemplateParamsTest.cs](../../../samples/tests/xunit/TemplateParams1Test.cs#L11-L21)]

The examples pass a HTML markup template into the component under test. This is done with the help of a `Func<TValue, string>` delegate which takes whatever the template value is as input, and returns a (markup) string. The delegate is automatically turned into a `RenderFragment<TValue>` type and passed to the template parameter.

The example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add` method to first add the data to the `Items` parameter and then to a `Func<TValue, string>` delegate.

The delegate creates a simple markup string in the example.

# [Razor test code](#tab/razor)

[!code-cshtml[TemplateParamsTest.razor](../../../samples/tests/razor/TemplateParams1Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The child template content, some HTML markup, is just passed like normal in Razor code.

**NOTE:** Before the .NET 6 version of the Blazor compiler, this example does not work.

***

#### Passing a component-based template

To pass a template into a `RenderFragment<TValue>` parameter, which is based on a component that receives the template value as input (in this case, the `<Item>` component listed below), do the following:

[!code-csharp[Item.razor](../../../samples/components/Item.razor)]

# [C# test code](#tab/csharp)

[!code-csharp[TemplateParamsTest.cs](../../../samples/tests/xunit/TemplateParams2Test.cs#L11-L25)]

The example creates a template with the `<Item>` component listed above.

# [Razor test code](#tab/razor)

[!code-cshtml[TemplateParamsTest.razor](../../../samples/tests/razor/TemplateParams2Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The child template content, some HTML and Razor markup, is just passed like normal in Razor code.

**NOTE:** Before the .NET 6 version of the Blazor compiler, this example does not work.

***

### Unmatched parameters

An unmatched parameter is a parameter that is passed to a component under test, and which does not have an explicit `[Parameter]` parameter but instead is captured by a `[Parameter(CaptureUnmatchedValues = true)]` parameter.

In the follow examples, we will pass an unmatched parameter to the following component:

[!code-csharp[UnmatchedParams](../../../samples/components/UnmatchedParams.cs#L10-L14)]

# [C# test code](#tab/csharp)

[!code-csharp[UnmatchedParamsTest.cs](../../../samples/tests/xunit/UnmatchedParamsTest.cs#L11-L22)]

The examples passes in the parameter `some-unknown-param` with the value `a value` to the component under test.

# [Razor test code](#tab/razor)

[!code-cshtml[UnmatchedParamsTest.razor](../../../samples/tests/razor/UnmatchedParamsTest.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The parameter is just passed like normal in Razor code.

***

## Cascading Parameters and Cascading Values

Cascading parameters are properties with the `[CascadingParameter]` attribute. There are two variants: **named** and **unnamed** cascading parameters. In Blazor, the `<CascadingValue>` component is used to provide values to cascading parameters, which we also do in tests written in `.razor` files. However, for tests written in `.cs` files we need to do it a little differently.

The following examples will pass cascading values to the `<CascadingParams>` component listed below:

[!code-csharp[CascadingParams.razor](../../../samples/components/CascadingParams.razor)]

### Passing unnamed cascading values

To pass the unnamed `IsDarkTheme` cascading parameter to the `<CascadingParams>` component, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[CascadingParamsTest.cs](../../../samples/tests/xunit/CascadingParams1Test.cs#L11-L24)]

The example pass the variable `isDarkTheme` to the cascading parameter `IsDarkTheme` using the `Add` method on the <xref:Bunit.ComponentParameterCollectionBuilder`1> with the parameter selector to explicitly select the desired cascading parameter and pass the unnamed parameter value that way.

# [Razor test code](#tab/razor)

[!code-cshtml[CascadingParamsTest.razor](../../../samples/tests/razor/CascadingParams1Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The cascading value is just passed like normal in Razor code.

***

### Passing named cascading values

To pass a named cascading parameter to the `<CascadingParams>` component, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[CascadingParamsTest.cs](../../../samples/tests/xunit/CascadingParams2Test.cs#L11-L22)]

The example pass in the value `Name of User` to the cascading parameter with the name `LoggedInUser`. Note that the name of the parameter is not the same as the property of the parameter, e.g. `LoggedInUser` vs. `UserName`. The example uses the `Add` method on the <xref:Bunit.ComponentParameterCollectionBuilder`1> with the parameter selector to select the cascading parameter property and pass the parameter value that way.

# [Razor test code](#tab/razor)

[!code-cshtml[CascadingParamsTest.razor](../../../samples/tests/razor/CascadingParams2Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The cascading value is just passed like normal in Razor code.

***

### Passing multiple, named and unnamed, cascading values

To pass all cascading parameters to the `<CascadingParams>` component, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[CascadingParamsTest.cs](../../../samples/tests/xunit/CascadingParams3Test.cs#L11-L26)]

The example passes both the unnamed `IsDarkTheme` cascading parameter and the two named cascading parameters (`LoggedInUser`, `LoggedInEmail`). It does this using the `Add` method on the <xref:Bunit.ComponentParameterCollectionBuilder`1> with the parameter selector to select both the named and unnamed cascading parameters and pass values to them that way.

# [Razor test code](#tab/razor)

[!code-cshtml[CascadingParamsTest.razor](../../../samples/tests/razor/CascadingParams3Test.razor)]

The example passes a inline Razor template to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. The cascading value is just passed like normal in Razor code.

***

## Rendering a component under test inside other components

It is possible to nest a component under tests inside other components, if that is required to test it. For example, to nest the `<HelloWorld>` component inside the `<Wrapper>` component do the following:

# [C# test code](#tab/csharp)

[!code-csharp[NestedComponentTest](../../../samples/tests/xunit/NestedComponentTest.cs#L11-L23)]

The example renders the `<HelloWorld>` component inside the `<Wrapper>` component. What is special in both cases is the use of the `FindComponent<HelloWorld>()` that returns a `IRenderedComponent<HelloWorld>`. This is needed because the `RenderComponent<Wrapper>` method call returns an `IRenderedComponent<Wrapper>` instance, that provides access to the instance of the `<Wrapper>` component, but not the `<HelloWorld>`-component instance.

# [Razor test code](#tab/razor)

[!code-cshtml[NestedComponentTest](../../../samples/tests/razor/NestedComponentTest.razor)]

The example passes a inline Razor template to the `Render<TComponent>()` method. What is different here from the previous examples is that we use the generic version of the `Render<TComponent>` method, which is a shorthand for `Render(...).FindComponent<TComponent>()`.

***

## Configure two-way with component parameters (`@bind` directive)

To set up [two-way binding to a pair of component parameters](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/data-binding#binding-with-component-parameters) on a component under test, e.g. the `Value` and `ValueChanged` parameter pair on the component below, do the following:

[!code-csharp[TwoWayBinding.razor](../../../samples/components/TwoWayBinding.razor)]

# [C# test code](#tab/csharp)

[!code-csharp[TwoWayBindingTest.cs](../../../samples/tests/xunit/TwoWayBindingTest.cs#L5-L19)]

The example uses the `Bind` method to setup two-way binding between the `Value` parameter and `ValueChanged` parameter, and the local variable in the test method (`currentValue`). The `Bind` method is a shorthand for calling the the `Add` method for the `Value` parameter and `ValueChanged` parameter individually.

# [Razor test code](#tab/razor)

[!code-cshtml[TwoWayBindingTest.razor](../../../samples/tests/razor/TwoWayBindingTest.razor)]

The example uses the standard `@bind-Value` directive in Blazor to set up two way binding between the `Value` parameter and `ValueChanged` parameter and the local variable in the test method (`currentValue`).

> [!WARNING]
> When using `@bind` in conjunction with razor test-files the razor component should **not** inherit from `ComponentBase` (which is the default). The simplest solution would be to inherit from `TestContext` (as seen in the example above) which also brings the benefits as described on top of this page.
***

## `TestContext` per test method
There are scenarios where it is not possible or not desirable to inherit from `TestContext` in the test class. In those cases, it is possible to create a new `TestContext` instance per test method. As the `TestContext` class implements `IDisposable`, it is recommended to use the `using` statement to ensure that the instance is disposed of after the test method has run.

# [C# test code](#tab/csharp)

[!code-csharp[HelloWorldExplicitContext.cs](../../../samples/tests/xunit/HelloWorldExplicitContextTest.cs#L6-L20)]

# [Razor test code](#tab/razor)

[!code-cshtml[HelloWorldExplicitContext.razor](../../../samples/tests/xunit/HelloWorldExplicitContextRazorTest.razor)]

***

## Getting an `InvalidOperationException`
When the razor syntax is used and the test throws the following exception:

> System.InvalidOperationException: The render handle is not yet assigned.

This usually means that the test class (Blazor component where the tests is declared in) is direclty inheriting from `ComponentBase`, as is the default for all Blazor components.

The solution is to inherit from bUnits `TestContext` instead, i.e.:
```razor
@inherits TestContext

@code {
    [Fact]
    public void Test1()
    {
        // test code
    }
}
```

## Limitations of rendering a `RenderFragment` inside a test
  
When rendering a `RenderFragment` using the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method, the created <xref:Bunit.IRenderedFragment> is static. This means that it will not re-render even if events are triggered.
```razor
@inherits TestContext

@code {
  [Fact]
  public void Button_clicked_string_gets_updated()
  {
    var output = string.Empty;
    var cut = Render(@<button @onclick='@(() => output = "Success")'>@output</button>);
    
    cut.Find("button").Click();
    
    // This will pass, as events are triggered and the function get executed
    output.ShouldBe("Success");

    // This will fail, as the markup will not get updated
    cut.Find("button").TextContent.ShouldBe("Success");
  }
}
```

## Passing query parameters (`SupplyParameterFromQuery`) to a component

In .NET 6 and later, components can receive parameters from a query string if the parameter is annotated with the `[SupplyParameterFromQuery]` attribute in addition to the `[Parameter]` attribute. 

In .NET 8 however, the `[Parameter]` attribute is no longer required, which means a value cannot be passed to the component during testing using the normal methods, e.g. the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add` method, if a component parameter is only annotated with the `[SupplyParameterFromQuery]` attribute. Instead, pass a query string parameters by setting it using the <xref:Bunit.TestDoubles.FakeNavigationManager>.

For example: 

```razor
@code {
  [SupplyParameterFromQuery]
  public string Name { get; set; }
}
```

A simple example of how to test a component that receives parameters from the query string:

```razor
@inherits TestContext

@code {
  [Fact]
  public void Component_receives_parameters_from_query_string()
  {
    var navigationManager = Services.GetRequiredService<NavigationManager>();
    var uri = navigationManager.GetUriWithQueryParameter("Name", "bUnit");
    navigationManager.NavigateTo(uri);

    var cut = RenderComponent<SupplyFromQueryParameterComponent>();

    cut.Instance.Name.ShouldBe("bUnit");
  }
}
```

## Setting parameters via routing
In Blazor, components can receive parameters via routing. This is particularly useful for passing data to components based on the URL. To enable this, the component parameters need to be annotated with the `[Parameter]` attribute and the `@page` directive (or `RouteAttribute` in code behind files).

An example component that receives parameters via routing:

```razor
@page "/counter/{initialCount:int}"

<p>Count: @InitialCount</p>

@code {
  [Parameter]
  public int InitialCount { get; set; }
}
```

To test a component that receives parameters via routing, set the parameters using the `NavigationManager`:

```razor
@inherits TestContext

@code {
  [Fact]
  public void Component_receives_parameters_from_route()
  {
    var cut = RenderComponent<ExampleComponent>();
    var navigationManager = Services.GetRequiredService<NavigationManager>();

    navigationManager.NavigateTo("/counter/123");    

    cut.Find("p").TextContent.ShouldBe("Count: 123");
  }
}
```

## Further Reading

- <xref:inject-services>
