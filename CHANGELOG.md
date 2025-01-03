# Changelog

All notable changes to **bUnit** will be documented in this file. The project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

<!-- The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) -->

## [Unreleased]

### Added

- Added support for xunit v3 in the bunit.template. By [@linkdotnet](https://github.com/linkdotnet).

## [1.37.7] - 2024-12-13

### Added

- Added support for `RendererInfo` and `AssignedRenderMode` (`.net9.0`).

## [1.36.0] - 2024-11-12

### Added

- Bumped .NET 9 version dependencies to stable packages.

## [1.35.3] - 2024-11-12

### Added

- Extension packages (`bunit.generators` and `bunit.web.query`) are flagged as stable.
- Implemented feature to map route templates to parameters using NavigationManager. This allows parameters to be set based on the route template when navigating to a new location. Reported by [JamesNK](https://github.com/JamesNK) in [#1580](https://github.com/bUnit-dev/bUnit/issues/1580). By [@linkdotnet](https://github.com/linkdotnet).
- 
## [1.34.0] - 2024-11-01

### Fixed

- Fixed other packages that have a CVE like `Microsoft.Extensions.Caching.Memory`. Reported by [@polajenko](https://github.com/polajenko). Fixed by [@linkdotnet](https://github.com/linkdotnet).

## [1.33.3] - 2024-10-11

### Added

- `bunit.generators` respect parameters from the base class.
- Supports components using constructor injection in `net9.0`.

### Fixed

- Use latest `System.Text.Json` due to CVE in `8.0.4`.

## [1.32.7] - 2024-10-04

### Fixed

- Ensure that a check in `WaitForAssertion` and related methods is always performed at once before the wait timer is started. If not, the timeout could occure before a wait-for check-condition had been attempted, causing tests to fail without reason. Fixed by [@egil](https://github.com/egil).

## [1.31.3] - 2024-08-16

### Fixed

- `UploadFile` should only throw an exception when the file size exceeds the maximum allowed size. Reported by [@candritzky](https://github.com/candritzky). Fixed by [@linkdotnet](https://github.com/linkdotnet).

## [1.30.3] - 2024-07-21

### Fixed

- `UploadFile` doesn't throw an exception when the file size exceeds the maximum allowed size. Reported by [@MorneZaayman](https://github.com/MorneZaayman) in [#1503](https://github.com/bUnit-dev/bUnit/issues/1503). Fixed by [@linkdotnet](https://github.com/linkdotnet).

## [1.29.5] - 2024-07-05

### Fixed

- CI build changes to force running verification on x64 based AMD CPUs.

## [1.28.9] - 2024-04-19

### Fixed

- bUnit's built-in HTML parser did not correctly parse full HTML documents that included a &lt;!DOCTYPE html> as the first element. Fixed by [@egil](https://github.com/egil).
- `@formname` directive led to an `InvalidOperationException` when used on a form element. Reported by [@suzu2469](https://github.com/suzu2469) in [#1438](https://github.com/bUnit-dev/bUnit/issues/1438).
  Fixed by [@egil](https://github.com/egil)/[@linkdotnet](https://github.com/linkdotnet).

### Changed

- `NavigationManager` is again registered as a singleton instead of scoped.

## [1.27.17] - 2024-03-02

### Added

- Support for `IKeyedServiceProvider` in net8.0. Reported by [@ViRuSTriNiTy](https://github.com/ViRuSTriNiTy). By [@linkdotnet](https://github.com/linkdotnet).
- Support for `net9.0`. NOTE, there is no commitment as of now to support net9.0 in bUnit v1. However. Support for net9.0 may move to a future v2 release of bUnit and be deprecated in v1. However, allowing bUnit to build and work with net9.0 previews allows our users to keep testing! 

### Fixed

- Support for `SupplyFromQueryParameter` in net8.0. Reported by [@aayjaychan](https://github.com/aayjaychan). Fixed by [@egil](https://github.com/egil) and [@linkdotnet](https://github.com/linkdotnet).

## [1.26.64] - 2023-12-20

### Changed

- Upgraded AngleSharp to 1.0.7. Info: The usage of `AngleSharpWrappers` is not needed anymore. Any usage of `Unwrap` should not be needed anymore.

### Fixed

- When the `TestContext` was disposed, the Blazor Renderer itself didn't dispose components under test. By [@linkdotnet](https://github.com/linkdotnet).
- When navigating, the `HistoryEntryState` on `NavigationManager` will be populated. By [@linkdotnet](https://github.com/linkdotnet).

### Added

- New overloads for `IRenderedFragmentBase.InvokeAsync` that allow retrieving the work item's return value. By [@jcparkyn](https://github.com/jcparkyn).

## [1.25.3] - 2023-11-14

- Upgrade all .NET 8 preview dependencies to .NET 8 stable.

## [1.24.10] - 2023-10-14

### Fixed

- When the `TestContext` was disposed, it disposed of all services via the service provider. However, if there were ongoing renders happening, this could cause inconsistent state in the render tree, since the `TestRenderer` could try to access the service provider to instantiate components.  
    This release changes the dispose phase such that the renderer gets disposed first, then the service provider. The disposal of any services that implement `IAsyncDisposable` is now also awaited. Fixed by [@egil](https://github.com/egil) and [@linkdotnet](https://github.com/linkdotnet). Reported by [@BenSchoen](https://github.com/BenSchoen) in <https://github.com/bUnit-dev/bUnit/issues/1227>.

### Added

- Support for custom service provider factories (`IServiceProviderFactory<TContainerBuilder>`). This enables the use of Autofac and other frameworks for dependency injection like on real-world ASP.NET Core / Blazor projects. By [@inf9144](https://github.com/inf9144).

- Ability to raise the `oncancel` and `onclose` event, that was introduced with .NET 8.

## [1.23.9] - 2023-09-06

### Fixed

- If the renderer was not idle when calling `SetParametersAndRender`, the method could return before the parameters were set and the component under test had finished rendering. This was a regression that happened in v1.21.9. Reported by [@Skintkingle](https://github.com/Skintkingle]) in <https://github.com/bUnit-dev/bUnit/issues/1188>. Fixed by [@egil](https://github.com/egil).

### Added

- `net8.0` support
- Increased timeout of `WaitForAssertion` to infinite when a debugger is attached. By [@linkdotnet](https://github.com/linkdotnet).

### Fixed

- AngleSharp IElement extension methods do not work with `IRenderedFragment.Find`. Reported by [a2er](https://github.com/a2er). Fixed by [@linkdotnet](https://github.com/linkdotnet).

## [1.22.19] - 2023-07-28

### Added

- Update bunit templates to support the target framework version of the project. By [@linkdotnet](https://github.com/linkdotnet).

### Fixed

- Calling `MarkupMatches(RenderFragment)` from a lambda passed to e.g. `WaitForAssertion` could lead to a deadlock in certain circumstances. Fixed by [@linkdotnet](https://github.com/linkdotnet). Reported by [@uecasm](https://github.com/uecasm) in <https://github.com/bUnit-dev/bUnit/issues/1143>.

- Rendering complex component hierarchies could result in a stack overflow. Fixed by [@egil](https://github.com/egil).. Reported by [@groogiam](https://github.com/groogiam) in <https://github.com/bUnit-dev/bUnit/issues/1064>.

- Remove capturing and dispatching markup updates to test frameworks synchronization context again. This could cause deadlocks and does not have any impact on test stability. Fixed by [@egil](https://github.com/egil). Reported by [@biohazard999](https://github.com/biohazard999) in <https://github.com/bUnit-dev/bUnit/issues/1144>.

## [1.21.9] - 2023-07-02

### Fixed

- Allow using 3rd party `IComponentActivator` at the same time as component factories. By [@egil](https://github.com/egil). Reported by [BenSchoen](https://github.com/BenSchoen) in <https://github.com/bUnit-dev/bUnit/issues/1129>.

- Calling `IRenderedComponent.Render()` or `IRenderedComponent.SetParametersAndRender()` did not batch up multiple synchronous re-renders after setting parameters. This is now changed such that the method causes the component to re-render with new parameters in the same way as if a parent component had passed new parameters to it. By [@egil](https://github.com/egil). Reported by [@Jcparkyn](https://github.com/Jcparkyn) in <https://github.com/bUnit-dev/bUnit/issues/1119>.

## [1.20.8] - 2023-05-21

### Added

- Added static `DefaultWaitTimeout` property to `TestContext` to enable overriding the default timeout of "wait for" methods like `WaitForAssertion` from 1 second to something else. By [@egil](https://github.com/egil).

### Fixed

- TestRenderer throws `ObjectDisposedException` if any methods is accessed after it has been disposed. It will also prevent changes to the internal render tree after it has been disposed. By [@egil](https://github.com/egil).

## [1.19.14] - 2023-04-26

### Fixed

- Custom elements with attributes throw `ArgumentException` with `MarkupMatches`. Reported by [@candritzky](https://github.com/candritzky). Fixed by [@linkdotnet](https://github.com/linkdotnet).

### Changed

- Changed test renderer such that updates to rendered components markup happen in the same synchronization context as the test framework is using (if any), if any, to avoid memory race conditions. By [@egil](https://github.com/egil).

## [1.18.4] - 2023-02-26

### Fixed

- Some characters where not properly escaped. Reported by [@pwhe23](https://github.com/pwhe23). Fixed by [@linkdotnet](https://github.com/linkdotnet).
- Clicking a submit button or submit input element inside a form, submits the form, if the submit button or submit input element does not have the `@onclick:preventDefault` attribute set. Reported by [@linkdotnet](https://github.com/linkdotnet). Fixed by [@egil](https://github.com/egil).

## [1.17.2] - 2023-02-22

- Submit buttons and input fields now no longer cause a form submit when they have the `@onclick:preventDefault` attribute. By [@JelleHissink](https://github.com/JelleHissink).

## [1.16.2] - 2023-02-07

- Changed semantic comparer to handle elements parsed outside their proper context, e.g. an `<path>` element parsed without being inside a `<svg>` element. The semantic comparer will now be able to treat those as regular elements and thus be able to compare correctly to other elements of the same type and with the same node name. By [@egil](https://github.com/egil).

## [1.15.5] - 2023-02-04

- Upgrade AngleSharp.Diffing to 0.17.1.

## [1.14.4] - 2023-01-11

### Added

- Added `IMemoryCache` by default to the Services container. By [@linkdotnet](https://github.com/linkdotnet).

### Fixed

- Added support in `FakeNavigationManager` to handle umlauts.
- Fixed a bug where attribute values did not get escaped. Reported by [@brettwinters](https://github.com/brettwinters). Fixed by [@linkdotnet](https://github.com/linkdotnet).

## [1.13.5] - 2022-12-16

This release contains a bunch of small tweaks and fixes.

## [1.12.6] - 2022-11-08

### Fixed

- The created HTML contained encoded strings. Reported by [@tobiasbrandstaedter](https://github.com/tobiasbrandstaedter). Fixed by [@linkdotnet](https://github.com/linkdotnet).

## [1.11.7] - 2022-10-13

### Added

- Added the `StateFromJson` method to the `NavigationHistory` type, to make it easy to deserialize navigation state stored as JSON during a call to `NavigationManager.NavigateTo`, e.g. as seen with the new `InteractiveRequestOptions` type available in .NET 7. By [@linkdotnet](https://github.com/linkdotnet) and [@egil](https://github.com/egil).

## [1.10.14] - 2022-09-16

### Added

- Added new test double `FakeWebAssemblyHostEnvironment` that implements `IWebAssemblyHostEnvironment`. By [@KristofferStrube](https://github.com/KristofferStrube).

- Added `Bind` method to parameter builder that makes it easier to emulate the `@bind-Value` syntax in C#-based tests.

          When writing tests in razor files, the `@bind-` directive can be directly applied like this:

  ```razor
  <MyComponent @bind-Value="myParam"></MyComponent>
  ```

          The same expression in C# syntax is more verbose like this:

  ```csharp
  RenderComponent<MyComponent>(ps => ps
    .Add(c => c.Value, value)
    .Add(c => c.ValueChanged, newValue => value = newValue)
    .Add(c => c.ValueExpression, () => value));
  ```

          With the new `Bind` method this can be done in one method:

  ```csharp
  RenderComponent<MyComponent>(ps => ps
    .Bind(c => c.Value, value, newValue => value = newValue, () => value));
  ```

          By [@linkdotnet](https://github.com/linkdotnet) and [@egil](https://github.com/egil).

- Added support for `NavigationLock`, which allows user code to intercept and prevent navigation. By [@linkdotnet](https://github.com/linkdotnet) and [@egil](https://github.com/egil).

### Fixed

- `JSInterop.VerifyInvoke` reported the wrong number of actual invocations of a given identifier. Reported by [@otori](https://github.com/otori). Fixed by [@linkdotnet](https://github.com/linkdotnet).

## [1.9.8] - 2022-06-07

### Changed

- `WaitForAssertion` method is now marked as an assertion method with the `[AssertionMethod]` attribute. This makes certain analyzers like SonarSource's [Tests should include assertions](https://rules.sonarsource.com/csharp/RSPEC-2699) happy. By [@egil](https://github.com/egil).

### Fixes

- A race condition existed between `WaitForState` / `WaitForAssertion` and `FindComponents`, if the first used the latter. Reported by [@rmihael](https://github.com/rmihael), [@SviatoslavK](https://github.com/SviatoslavK), and [@RaphaelMarcouxCTRL](https://github.com/RaphaelMarcouxCTRL). Fixed by [@egil](https://github.com/egil) and [@linkdotnet](https://github.com/linkdotnet).

- Triggering of event handlers now runs entirely inside the renderers synchronization context, avoiding race condition between elements in the DOM tree being updated by the renderer and the event triggering logic traversing the DOM tree to find event handlers to trigger. Reported by [@FlukeFan](https://github.com/FlukeFan). Fixed by [@egil](https://github.com/egil).

## [1.8.15] - 2022-05-19

### Added

- Added test helpers that make it much easier to pass files to the `InputFile` component. Learn more [in the documentation](https://bunit.dev/docs/test-doubles/input-file). By [@egil](https://github.com/egil) and [@linkdotnet](https://github.com/linkdotnet).

### Changed

- `Htmlizer` uses `StringBuilder` instead of `List<string>` to reduce allocations and improve render speed. By [@linkdotnet](https://github.com/linkdotnet).

### Fixes

- `TestServiceProvider` now implements `IAsyncDisposable`. This means `TestContext.Dispose()` now calls the async disposable method as well as the non-async version on the service provider. It does however not block or await the task returned, since that can lead to deadlocks.

          To await the disposal of async services registered in the `TestContext.Services` container, do the following:

  1. Create a new type that derives from `TestContext` and which implement `IAsyncDisposable`.
  2. In the `DisposeAsync()` method, call `Services.DisposeAsync()`.
  3. Override the `Dispose` and have it only call `Services.Dispose()`.

     Reported by [@vedion](https://github.com/vedion) and fixed by [@egil](https://github.com/egil).

## [1.7.7] - 2022-04-29

### Added

- Added method `SetAuthenticationType` to `TestAuthorizationContext` to allow for custom authentication type checks. By [@TimPurdum](https://github.com/timpurdum).

- Added `DisposeComponents` to `TestContextBase`. It will dispose and remove all components rendered by the `TestContextBase`. By [@linkdotnet](https://github.com/linkdotnet).

- Added .NET 7 as a target framework for bUnit. By [@linkdotnet](https://github.com/linkdotnet).

### Fixed

- Fixed step by step guide for building and viewing the documentation locally. By [@linkdotnet](https://github.com/linkdotnet).

- `FakeNavigationManager.NavigateTo` could lead to exceptions when navigating to external url's. Reported by [@TDroogers](https://github.com/TDroogers). Fixed by [@linkdotnet](https://github.com/linkdotnet).

## [1.6.4] - 2022-02-22

A quick minor release that primiarily fixes a regression in 1.5.12.

### Fixed

- `ClickAsync` could lead to bubbling exceptions from `GetDispatchEventTasks` even though they should be handled. Reported by [@aguacongas](aguacongas). Fixed by [@linkdotnet](https://github.com/linkdotnet).
- Added more non bubbling events to bUnit so it behaves closer to the HTML specification. [@linkdotnet](https://github.com/linkdotnet).

## [1.5.12] - 2022-02-15

This first release of 2022 includes one fix and four additions. A huge thank you to [Steven Giesel (@linkdotnet)](https://github.com/linkdotnet) and [Denis Ekart (@denisekart)](https://github.com/denisekart) for their contributions to this release.

Also a big shout out to **bUnit's sponsors** who helped make this release happen.

**The higher tier sponsors are:**

- [Progress Telerik](https://github.com/Progress-Telerik)
- [Syncfusion](https://github.com/syncfusion)
- [CTRL Informatique](https://github.com/CTRL-Informatique)

**Other sponsors are:**

- [Hassan Rezk Habib (@hassanhabib)](https://github.com/hassanhabib)
- [Jonny Larsson (@Garderoben)](https://github.com/Garderoben)
- [Domn Werner (@domn1995)](https://github.com/domn1995)
- [Mladen Macanović (@stsrki)](https://github.com/stsrki)
- [@ChristopheDEBOVE](https://github.com/ChristopheDEBOVE)
- [Steven Giesel (@linkdotnet)](https://github.com/linkdotnet)

### Added

- Added `FakeSignOutSessionStateManage` type in Blazor, that makes it easy to test components that use the `SignOutSessionStateManage` type. By [@linkdotnet](https://github.com/linkdotnet).
- Added a validation to `AddChildContent` method in `ComponentParameterCollectionBuilder` that will throw an exception if the component's `ChildContent` is a generic type. By [@denisekart](https://github.com/denisekart).
- Added more optional arguments for `Click` and `DoubleClick` extensions which were introduced in .NET 5 and .NET 6. By [@linkdotnet](https://github.com/linkdotnet).
- Added template support for `Nunit` and `MSTest` unit test frameworks. By [@denisekart](https://github.com/denisekart).

### Fixed

- Changed `GetDispatchEventTasks` for bubbling events such that handled exceptions are not rethrown later from the `WaitFor...` helpers methods. Reported by [@AndrewStrickland](https://github.com/AndrewStrickland). Fixed by [@linkdotnet](https://github.com/linkdotnet)

## [1.4.15] - 2021-12-18

This release reintroduces `Stub<TComponent>` and related back into the main library, so the "preview" library `bunit.web.mock` is already obsolete.

A big shout out to **bUnit's sponsors** who helped make this release happen.

**The higher tier sponsors are:**

- [Progress Telerik](https://github.com/Progress-Telerik)
- [Syncfusion](https://github.com/syncfusion)
- [CTRL Informatique](https://github.com/CTRL-Informatique)

**Other sponsors are:**

- [Hassan Rezk Habib (@hassanhabib)](https://github.com/hassanhabib)
- [Jonny Larsson (@Garderoben)](https://github.com/Garderoben)
- [Domn Werner (@domn1995)](https://github.com/domn1995)
- [Mladen Macanović (@stsrki)](https://github.com/stsrki)
- [@ChristopheDEBOVE](https://github.com/ChristopheDEBOVE)

### Added

- Add `ComponentFactories` extensions method that makes it easy to register an instance of a replacement component. By [@egil](https://github.com/egil).
- Add ability to pass `ServiceProviderOptions` to `TestServiceProvider` through property to allow users to customize the service provider. By [@rodolfograve](https://github.com/rodolfograve).

### Fixed

- Changed `SetParametersAndRender` such that it rethrows any exceptions thrown by the component under tests `SetParametersAsync` method. Thanks to [@bonsall](https://github.com/bonsall) for reporting the issue. Fixed by [@egil](https://github.com/egil).
- `onclick` on a button inside a form will raise the `onsubmit` event for the form itself. Reported by [@egil]. Fixed by [@linkdotnet](https://github.com/linkdotnet).
- Only forms are allowed to have a `onsubmit` event handler. When `onsubmit` is invoked from a non-form element results in an exception. Fixed by [@linkdotnet](https://github.com/linkdotnet).

## [1.3.42] - 2021-11-09

This release includes support for .NET 6, with support for all new features in Blazor with that release. There are also a number of additions and fixes, all listed below.

Big shout out to **bUnit's sponsors** who helped make this release happen.

**The higher tier sponsors are:**

- [Progress Telerik](https://github.com/Progress-Telerik)
- [Syncfusion](https://github.com/syncfusion)

**Other sponsors are:**

- [Hassan Rezk Habib (@hassanhabib)](https://github.com/hassanhabib)
- [Jonny Larsson (@Garderoben)](https://github.com/Garderoben)
- [Domn Werner (@domn1995)](https://github.com/domn1995)
- [Mladen Macanović (@stsrki)](https://github.com/stsrki)
- [@ChristopheDEBOVE](https://github.com/ChristopheDEBOVE)

### Added

List of added functionality in this release.

- Added support for writing tests of components that use the `<FocusOnNavigate>` component included in .NET 6. This includes an assertion helper method `VerifyFocusOnNavigateInvoke` on bUnit's `JSInterop` that allow you to verify that `<FocusOnNavigate>` has set focus on an element during render. For example, to verify that `h1` selector was used to pick an element to focus on, do:

  ```csharp
  // <App /> component uses <FocusOnNavigate>
  var cut = RenderComponent<App>();

  // Verifies that <FocusOnNavigate> called it's JavaScript function
  var invocation = JSInterop.VerifyFocusOnNavigateInvoke();

  // Verify that the invocation of <FocusOnNavigate> JavaScript function included the "h1" as the selector
  Assert.Equal("h1", invocation.Arguments[0]);
  ```

            By [@egil](https://github.com/egil).

- Added fake version of the `PersistentComponentState` type in Blazor that makes it possible to test components that use the type. By [@egil](https://github.com/egil).

- Added `TriggerEvent` method to make it easier to trigger custom events. By [@egil](https://github.com/egil).

- Added `History` capture in the `FakeNavigationManager`. By [@egil](https://github.com/egil).

- Added new bUnit component mocking library, available via NuGet as `bunit.web.mock`. It is currently in preview and the features/APIs of it will change!

- Added `WaitForElement` and `WaitForElements` methods. These makes it possible to wait for one or more elements to appear in the DOM before continuing a test, similar to how `WaitForAssertion` allows you to wait for an assertion to pass, or `WaitForState` allows you to wait for a predicate to pass. By [@egil](https://github.com/egil).

### Changed

- Added automatic conversion of values (types) passed to `Change()` and `Input()` event trigger methods. This means that e.g. a `DateTime` passed to `Change()` is automatically converted to a string format that Blazor expects. By [@egil](https://github.com/egil).

### Fixed

- The `Click` and `DoubleClick` extension methods now set the `MouseEventArgs.Detail` property to `1` and `2` respectively by default, unless the user specifies something else. This makes the methods more correctly emulate how Blazor reports single or double clicks on an element in the browser. Thanks to [@David-Moreira](https://github.com/David-Moreira) for the help troubleshooting this issue. By [@egil](https://github.com/egil).

- `FocusAsync()` method handler on `ElementReference` and `<FocusOnNavigate>` js handler return completed `Task`. By [@anddrzejb](https://github.com/anddrzejb).

- Fixes handling of disposed event handlers of bubbling events. See issue [#518](https://github.com/bUnit-dev/bUnit/issues/518) for details. Thanks to [@David-Moreira](https://github.com/David-Moreira) for helping debug this issue.

- Async event trigger methods are not public. In most circumstances you do not need to use them, but if you have a scenario where you want to check that something has not happened after an event handler was triggered, then you can use the async methods and await them to know when they are completed. See [#552](https://github.com/bUnit-dev/bUnit/discussions/552) for details. By [@egil](https://github.com/egil).

## [1.2.49] - 2021-08-09

### Added

List of added functionality in this release.

- Added more extensions methods to `MarkupMatchesAssertExtensions` to allow asserting with `MarkupMatches` on `IEnumerable` and `IElement`. By [@jgoday](https://github.com/jgoday).

- Added `BunitErrorBoundaryLogger` implementation of `IErrorBoundaryLogger` (needed for Blazor's ErrorBoundary component in .NET 6.0). By [@jgoday](https://github.com/jgoday).

- Added `ComponentFactories` property to the `TestContextBase` type. The `ComponentFactories` property is a `ComponentFactoryCollection` type that contains `IComponentFactory` types. These are used by bUnits component activator, whenever a component is created during testing. If no component factories is added to the collection, the standard component activator mechanism from Blazor is used. This feature makes it possible to control what components are created normally during a test, and which should be e.g. replaced by a test dummy. More info is available in issue [#388](https://github.com/bUnit-dev/bUnit/issues/388).
          Learn more about this feature on the [Controlling component instantiation](https://bunit.dev/docs/providing-input/controlling-component-instantiation) page.

- Added `HasComponent<TComponent>()` to `IRenderedFragement`. Use it to check if the rendered fragment contains a component of type `TComponent`. Added by [@egil](https://github.com/egil).

- Added `AddStub` and `Add` extension methods to `ComponentFactories` that makes it easy to configure bUnit to replace components in the render tree with stubs. Both methods have overloads that allow for fine grained selection of component types to "double" during testing. Added by [@egil](https://github.com/egil) in [#400](https://github.com/bUnit-dev/bUnit/pull/400).

### Changed

List of changes in this release.

- Updated AngleSharp and related libraries to 0.16.0. _NOTE, the new version of AngleSharp includes nullable annotations, which might affect how your code compiles, if you have nullable checking enabled in your test project._ By [@egil](https://github.com/egil).

- Updated .NET 6 dependencies to preview 5. By [@egil](https://github.com/egil).

### Fixed

List of fixes in this release.

- Fixed JSInterop error message when trying to import an unconfigured module. By [@jgoday](https://github.com/jgoday) in [#425](https://github.com/bUnit-dev/bUnit/pull/425).

- Fixed issue where a registered fall-back service provider was not made available to resolve service dependencies of components under test. Thanks to [@dady8889](https://github.com/dady8889) for the reporting the issue.

- Fixed handling of escaped uri's in FakeNavigationManager. By [@linkdotnet](https://github.com/linkdotnet) in [#460](https://github.com/bUnit-dev/bUnit/pull/460).

- Captured error message from event dispatcher in renderer that would previously be hidden from the user. Related to issue [#399](https://github.com/bUnit-dev/bUnit/issues/399).

## [1.1.5] - 2021-04-30

### Added

- All bUnit assemblies is now strong named signed.

- Added .NET 6 (preview 3) as a target framework for bUnit, bUnit.core and bUnit.web.

### Changed

- Changed bunit.template such that created projects only reference the bUnit package. Bumped other referenced packages to latest version.

- Changed TestServiceProvider to validate scopes of registered services, such that it behaves like the service provider (default IoC container) in Blazor.

## [1.0.16]

The following section list all changes since preview 02.

### Changed

List of changes in existing functionality.

- _**BREAKING CHANGE:**_ Writing tests using the test components `<Fixture>` and `<SnapshotTest>` components inside .razor files has been moved to its own library, `bunit.web.testcomponents`. This was done for several reasons:

  - The feature has been experimental since it was introduced, and it was introduced get a more natural way of specifying the component under test and any related markup used by test.
  - The feature is only supported with xUnit.
  - There are some issues related to the `SourceFileFinder` library, which is used to discover the test components.
  - A better way of writing tests in .razor files has been added to bUnit, using _"inline render fragments"_. This method works with all general purpose test frameworks, e.g. MSTest, NUnit, and xUnit, is more flexible, and offer less boilerplate code than the test components. The bUnit documentation has been updated with a guide to this style.

          The new package `bunit.web.testcomponents` is provided as is, without expectation of further development or enhancements. If you are using the test components currently for writing tests, it will continue to work for you. If you are starting a new project, or have few of these tests, consider switching to the "inline render fragments" style.

          Here is a quick comparison of the styles, using a very simple component.

          First, the test component style:

    ```razor
    @inherits TestComponentBase

    <Fixture Test="HelloWorldComponentRendersCorrectly">
      <ComponentUnderTest>
        <HelloWorld />
      </ComponentUnderTest>

      @code
      {
        void HelloWorldComponentRendersCorrectly(Fixture fixture)
        {
          // Act
          var cut = fixture.GetComponentUnderTest<HelloWorld>();

          // Assert
          cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
        }
      }
    </Fixture>

    <SnapshotTest Description="HelloWorld component renders correctly">
      <TestInput>
        <HelloWorld />
      </TestInput>
      <ExpectedOutput>
        <h1>Hello world from Blazor</h1>
      </ExpectedOutput>
    </SnapshotTest>
    ```

          The a single test in "inline render fragments" style covers both cases:

              @inherits TestContext
              @code {
                [Fact]
                public void HelloWorldComponentRendersCorrectly()
                {
                  // Act
                  var cut = Render(@<HelloWorld />);

                  // Assert
                  cut.MarkupMatches(@<h1>Hello world from Blazor</h1>);
                }
              }

          To make the snapshot test scenario even more compact, consider putting all code in one line, e.g. `Render(@<HelloWorld />).MarkupMatches(@<h1>Hello world from Blazor</h1>);`.

          For a more complete snapshot testing experience, I recommend looking at Simon Cropp's [Verify](https://github.com/VerifyTests) library, in particular the [Verify.Blazor extension to bUnit](https://github.com/VerifyTests/Verify.Blazor#verifybunit). Verify comes with all the features you expect from a snapshot testing library.

### Removed

List of now removed features.

- The `AddXunitLogger` method, which provided support for capturing `ILogger` messages and passing them to xUnit's `ITestOutputHelper`, has been removed. There were no need to keep xUnit specific code around in bUnit going forward, and there are many implementations on-line that supports this feature, so having it in bUnit made little sense. One such alternative, which bUnit has adopted internally, is to use Serilog. This looks as follows:

  1. Add the following packages to your test project: `Serilog`, `Serilog.Extensions.Logging`, and `Serilog.Sinks.XUnit`.
  2. Add the following class/extension method to your test project (which replicates the signature of the removed `AddXunitLogger` method):

     ```csharp
     using Microsoft.Extensions.DependencyInjection;
     using Microsoft.Extensions.Logging;
     using Serilog;
     using Serilog.Events;
     using Xunit.Abstractions;

     namespace Bunit
     {
     	public static class ServiceCollectionLoggingExtensions
     	{
     		public static IServiceCollection AddXunitLogger(this IServiceCollection services, ITestOutputHelper outputHelper)
     		{
     			var serilogLogger = new LoggerConfiguration()
     				.MinimumLevel.Verbose()
     				.WriteTo.TestOutput(outputHelper, LogEventLevel.Verbose)
     				.CreateLogger();

     			services.AddSingleton<ILoggerFactory>(new LoggerFactory().AddSerilog(serilogLogger, dispose: true));
     			services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

     			return services;
     		}
     	}
     }
     ```

- The `bunit.xunit` package has been removed, since it is no longer needed (there is no code left in it).

## [1.0.0-preview-02] - 2021-03-26

The following section list all changes in 1.0.0 preview 02.

The plan is to make this the last preview release of bUnit. If no big blocking bugs show up the next two weeks, a non-preview release of bUnit will be pushed out to the world.

### Added

List of new features.

- Added the ability to pass a "fallback `IServiceProvider`" to the `TestServiceProvider`, available through the `Services` property on a `TestContext`. The fallback service provider enables a few interesting scenarios, such as using an alternative IoC container, or automatically generating mocks of services components under test depend on. See the [Injecting Services into Components Under Test page](https://bunit.egilhansen.com/docs/providing-input/inject-services-into-components) for more details on this feature. By [@thopdev](https://github.com/thopdev) in [#310](https://github.com/egil/bUnit/issues/310).

- Added `Task<Expection> ITestRenderer.UnhandledException` property that returns a `Task<Exception>` that completes when the renderer captures an unhandled exception from a component under test. If a component is missing exception handling of asynchronous operations, e.g. in the `OnInitializedAsync` method, the exception will not break the test, because it happens on another thread. To have a test fail in this scenario, you can await the `UnhandledException` property on the `TestContext.Renderer` property, e.g.:

  ```csharp
  using var ctx = new TestContext();

  var cut = ctx.RenderComponent<ComponentThatThrowsDuringAsyncOperation>();

  Task<Exception?> waitTimeout = Task.Delay(500).ContinueWith(_ => Task.FromResult<Exception?>(null)).Unwrap();
  Exception? unhandledException = await Task.WhenAny<Exception?>(Renderer.UnhandledException, waitTimeout).Unwrap();

  Assert.Null(unhandledException);
  ```

          In this example, we await any unhandled exceptions from the renderer, or our wait timeout. The `waitTimeout` ensures that we will not wait forever, in case no unhandled exception is thrown.

          NOTE, a better approach is to use the `WaitForState` or `WaitForAssertion` methods, which now also throws unhandled exceptions. Using them, you do not need to set up a wait timeout explicitly.

          By [@egil](https://github.com/egil) in [#344](https://github.com/egil/bUnit/issues/344).

- Added a simple fake navigation manager, which is registered by default in bUnit's service provider. When the fake navigation manager's `NavigateTo` method is called, it does two things:

  1. Set the `Uri` property to the URI passed to the `NavigateTo` method (with the URI normalized to an absolute URI).
  2. Raise the `LocationChanged` event with the URI passed to the `NavigateTo` method.

     Lets look at an example: To verify that the `<GoesToFooOnInit>` component below calls the `NavigationManager.NavigateTo` method with the expected value, do the following:

     `<GoesToFooOnInit>` component:

     ```cshtml
     @inject NavigationManager NavMan
     @code {
       protected override void OnInitialized()
       {
         NavMan.NavigateTo("foo");
       }
     }
     ```

     Test code:

     ```csharp
     // Arrange
     using var ctx = new TestContext();
     var navMan = ctx.Services.GetRequiredService<NavigationManager>();

     // Act
     var cut = ctx.RenderComponent<GoesToFooOnInit>();

     // Assert
     Assert.Equal($"{navMan.BaseUri}foo", navMan.Uri);
     ```

     Since the `foo` input argument is normalized to an absolute URI, we have to do the same normalization in our assertion.

     The fake navigation manager's `BaseUri` is set to `http://localhost/`, but it is not recommended to use that URL directly in your code. Instead create an assertion by getting that value from the `BaseUri` property, like shown in the example above.

     By [@egil](https://github.com/egil) in [#345](https://github.com/egil/bUnit/pull/345).

- Added additional bUnit JSInterop `Setup` methods, that makes it possible to get complete control of invocation matching for the created handler. By [@egil](https://github.com/egil).

### Changed

List of changes in existing functionality.

- `WaitForAssertion` and `WaitForState` now throws unhandled exception caught by the renderer from a component under test. This can happen if a component is awaiting an asynchronous operation that throws, e.g. a API call using a misconfigured `HttpClient`. By [@egil](https://github.com/egil) in [#310](https://github.com/egil/bUnit/issues/344).

- Improvements to error message from bUnit's JSInterop when it receives an invocation that it has not been set up to handle. By [@egil](https://github.com/egil) in [#346](https://github.com/egil/bUnit/pull/346).

### Removed

List of now removed features.

### Fixed

List of any bug fixes.

## [1.0.0-preview-01] - 2020-12-24

The following section list all changes in 1.0.0 preview 01.

### Added

List of new features.

- Added support for casting `BUnitJSRuntime` to `IJSInProcessRuntime` and `IJSUnmarshalledRuntime`. By [@KristofferStrube](https://github.com/KristofferStrube) in [#279](https://github.com/egil/bUnit/pull/279)

- Added support for triggering `@ontoggle` event handlers through a dedicated `Toggle()` method. By [@egil](https://github.com/egil) in [#256](https://github.com/egil/bUnit/pull/256).

- Added out of the box support for `<Virtualize>` component. When a `<Virtualize>` component is used in a component under test, it's JavaScript interop-calls are faked by bUnits JSInterop, and it should result in all items being rendered immediately. By [@egil](https://github.com/egil) in [#240](https://github.com/egil/bUnit/issues/240).

- Added support for components that call `ElementReference.FocusAsync`. These calls are handled by the bUnits JSInterop, that also allows you to verify that `FocusAsync` has been called for a specific element. For example, if a component has rendered an `<input>` element, then the following code will verify that it has been focused using `FocusAsync`:

  ```csharp
  var cut = RenderComponent<FocusingComponent>();

  var input = cut.Find("input");

  JSInterop.VerifyFocusAsyncInvoke()
    .Arguments[0] // the first argument is the ElemenetReference
    .ShouldBeElementReferenceTo(input);
  ```

          By [@egil](https://github.com/egil) in [#260](https://github.com/egil/bUnit/pull/260).

- Added `Render(RenderFragment)` and `Render<TComponent>(RenderFragment)` methods to `TestContext`, as well as various overloads to the `MarkupMatches` methods, that also takes a `RenderFragment` as the expected value.

          The difference between the generic `Render` method and the non-generic one is that the generic returns an `IRenderedComponent<TComponent>`, whereas the non-generic one returns a `IRenderedFragment`.

          Calling `Render<TComponent>(RenderFragent)` is equivalent to calling `Render(RenderFragment).FindComponent<TComponent>()`, e.g. it returns the first component in the render tree of type `TComponent`. This is different from the `RenderComponent<TComponent>()` method, where `TComponent` _is_ the root component of the render tree.

          The main usecase for these are when writing tests inside .razor files. Here the inline syntax for declaring render fragments make these methods very useful.

          For example, to tests the `<Counter>` page/component that is part of new Blazor apps, do the following (inside a `CounterTest.razor` file):

  ```cshtml
  @code
  {
    [Fact]
    public void Counter_Increments_When_Button_Is_Clicked()
    {
      using var ctx = new TestContext();
      var cut = ctx.Render(@<Counter />);

      cut.Find("button").Click();

      cut.Find("p").MarkupMatches(@<p>Current count: 1</p>);
    }
  }
  ```

          Note: This example uses xUnit, but NUnit or MSTest works equally well.

          In addition to the new `Render` methods, a empty `BuildRenderTree` method has been added to the `TestContext` type. This makes it possible to inherit from the `TestContext` type in test components, removing the need for newing up the `TestContext` in each test.

          This means the test component above ends up looking like this:

  ```cshtml
  @inherts TestContext
  @code
  {
    [Fact]
    public void Counter_Increments_When_Button_Is_Clicked()
    {
      var cut = Render(@<Counter />);

      cut.Find("button").Click();

      cut.Find("p").MarkupMatches(@<p>Current count: 1</p>);
    }
  }
  ```

          Tip: If you have multiple test components in the same folder, you can add a `_Imports.razor` file inside it and add the `@inherits TestContext` statement in that, removing the need to add it to every test component.

          By [@egil](https://github.com/egil) in [#262](https://github.com/egil/bUnit/pull/262).

- Added support for `IJSRuntime.InvokeAsync<IJSObjectReference>(...)` calls from components. There is now a new setup helper methods for configuring how invocations towards JS modules should be handled. This is done with the various `SetupModule` methods available on the `BunitJSInterop` type available through the `TestContext.JSInterop` property. For example, to set up a module for handling calls to `foo.js`, do the following:

  ```c#
  using var ctx = new TestContext();
  var moduleJsInterop = ctx.JSInterop.SetupModule("foo.js");
  ```

          The returned `moduleJsInterop` is a `BunitJSInterop` type, which means all the normal `Setup<TResult>` and `SetupVoid` methods can be used to configure it to handle calls to the module from a component. For example, to configure a handler for a call to `hello` in the `foo.js` module, do the following:

  ```c#
  moduleJsInterop.SetupVoid("hello");
  ```

          By [@egil](https://github.com/egil) in [#288](https://github.com/egil/bUnit/pull/288).

- Added support for registering services in bUnits `Services` collection that implements `IAsyncDisposable`. Suggested by [@jmaillet](https://github.com/jmaillet) in [#249](https://github.com/egil/bUnit/issues/249).

### Changed

List of changes in existing functionality.

- bUnit's mock IJSRuntime has been moved to an "always on" state by default, in strict mode, and is now available through `TestContext`'s `JSInterop` property. This makes it possible for first party Blazor components like the `<Virtualize>` component, which depend on JSInterop, to "just work" in tests.

          **Compatible with previous releases:** To get the same effect as calling `Services.AddMockJSRuntime()` in beta-11, which used to add the mock IJSRuntime in "loose" mode, you now just need to change the mode of the already on JSInterop, i.e. `ctx.JSInterop.Mode = JSRuntimeMode.Loose`.

          **Inspect registered handlers:** Since the new design allows registering invoke handlers in the context of the `TestContext`, you might need to get already registered handlers in your individual tests. This can be done with the `TryGetInvokeHandler()` method, that will return handler that can handle the parameters passed to it. E.g. to get a handler for a `IJSRuntime.InvokaAsync<string>("getValue")`, call `ctx.JSInterop.TryGetInvokeHandler<string>("getValue")`.

          Learn more [issue #237](https://github.com/egil/bUnit/issues/237). By [@egil](https://github.com/egil) in [#247](https://github.com/egil/bUnit/pull/247).

- The `Setup<TResult>(string identifier, Func<IReadOnlyList<object?>, bool> argumentsMatcher)` and `SetupVoid(string identifier, Func<IReadOnlyList<object?>, bool> argumentsMatcher)` methods in bUnits JSInterop/MockJSRuntime has a new second parameter, an `InvocationMatcher`.

          The `InvocationMatcher` type is a delegate that receives a `JSRuntimeInvoation` and returns true. The `JSRuntimeInvoation` type contains the arguments of the invocation and the identifier for the invocation. This means old code using the `Setup` and `SetupVoid` methods should be updated to use the arguments list in `JSRuntimeInvoation`, e.g., change the following call:

           `ctx.JSInterop.Setup<string>("foo", args => args.Count == 2)` to this:
           `ctx.JSInterop.Setup<string>("foo", invocation => invocation.Arguments.Count == 2)`.

          Changed added in relation to [#240](https://github.com/egil/bUnit/issues/240) in [#257](https://github.com/egil/bUnit/issues/257) by [@egil](https://github.com/egil).

- Changed `AddTestAuthorization` such that it works in Razor-based test contexts, i.e. on the `Fixture` and `SnapshotTest` types.

### Removed

List of now removed features.

- A few bUnit internal xUnit assert helper methods, the custom `ShouldAllBe` methods, has mistakingly been part of the bunit.xunit package. These have been removed.

### Fixed

List of any bug fixes.

- When an `Add` call to the component parameter collection builder was used to select a parameter that was inherited from a base component, the builder incorrectly reported the selected property/parameter as missing on the type. Reported by [@nickmuller](https://github.com/nickmuller) in [#250](https://github.com/egil/bUnit/issues/250).

- When an element, found in the DOM tree using the `Find()`, method was removed because of an event handler trigger on it, e.g. an `cut.Find("button").Click()` event trigger method, an `ElementNotFoundException` was thrown. Reported by [@nickmuller](https://github.com/nickmuller) in [#251](https://github.com/egil/bUnit/issues/251).

- In the built-in fake authentication system in bUnit, roles and claims were not available in components through the a cascading parameter of type `Task<AuthenticationState>`. Reported by [@AFAde](https://github.com/AFAde) in [#253](https://github.com/egil/bUnit/discussions/253) and fixed in [#291](https://github.com/egil/bUnit/pull/291) by [@egil](https://github.com/egil).

## [1.0.0-beta 11] - 2020-10-26

The following section list all changes in beta-11.

### Added

List of new features.

- Two new overloads to the `RenderFragment()` and `ChildContent()` component parameter factory methods have been added that takes a `RenderFragment` as input. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- Added a `ComponentParameterCollection` type. The `ComponentParameterCollection` is a collection of component parameters, that knows how to turn those components parameters into a `RenderFragment`, which will render a component and pass any parameters inside the collection to that component. That logic was spread out over multiple places in bUnit, and is now owned by the `ComponentParameterCollection` type. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- Added additional placeholder services for `NavigationManager`, `HttpClient`, and `IStringLocalizer`, to make it easier for users to figure out why a test is failing due to missing service registration before rendering a component. By [@joro550](https://github.com/joro550) in [#223](https://github.com/egil/bUnit/pull/223).

- Added `Key` class that represents a keyboard key and helps to avoid constructing `KeyboardEventArgs` object manually. The key can be passed to `KeyPress`, `KeyDown`, or `KeyUp` helper methods to raise keyboard events. The `Key` class provides static special keys or can be obtained from character or string. Keys can be combined with key modifiers: `Key.Enter + Key.Alt`.

          For example, this makes it easier to trigger keyboard events on an element:

  ```csharp
  var cut = ctx.RenderComponent<ComponentWithKeyboardEvents>();
  var element = cut.Find("input");

  element.KeyDown(Key.Enter + Key.Control); // Triggers onkeydown event with Ctrl + Enter
  element.KeyUp(Key.Control + Key.Shift + 'B'); // Triggers onkeyup event with Ctrl + Shift  + B
  element.KeyPress('1'); // Triggers onkeypress event with key 1
  element.KeyDown(Key.Alt + "<"); // Triggers onkeydown event with Alt + <
  ```

          By [@duracellko](https://github.com/duracellko) in [#101](https://github.com/egil/bUnit/issues/101).

- Added support for registering/adding components to a test context root render tree, which components under test is rendered inside. This allows you to simplify the "arrange" step of a test when a component under test requires a certain render tree as its parent, e.g. a cascading value.

          For example, to pass a cascading string value `foo` to all components rendered with the test context, do the following:

  ```csharp
  ctx.RenderTree<CascadingValue<string>>(parameters => parameters.Add(p => p.Value, "foo"));
  var cut = ctx.RenderComponent<ComponentReceivingFoo>();
  ```

          By [@egil](https://github.com/egil) in [#236](https://github.com/egil/bUnit/pull/236).

- Added "catch-all" `Setup` method to bUnit's mock JS runtime, that allows you to specify only the type when setting up a planned invocation. By [@nemesv](https://github.com/nemesv) in [#234](https://github.com/egil/bUnit/issues/234).

### Changed

List of changes in existing functionality.

- The `ComponentParameterBuilder` has been renamed to `ComponentParameterCollectionBuilder`, since it now builds the `ComponentParameterCollection` type, introduced in this release of bUnit. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- `ComponentParameterCollectionBuilder` now allows adding cascading values that is not directly used by the component type it targets. This makes it possible to add cascading values to children of the target component. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- The `Add(object)` has been replaced by `AddCascadingValue(object)` in `ComponentParameterCollectionBuilder`, to make it more clear that an unnamed cascading value is being passed to the target component or one of its child components. It is also possible to pass unnamed cascading values using the `Add(parameterSelector, value)` method, which now correctly detect if the selected cascading value parameter is named or unnamed. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- It is now possible to call the `Add()`, `AddChildContent()` methods on `ComponentParameterCollectionBuilder`, and the factory methods `RenderFragment()`, `ChildContent()`, and `Template()`, _**multiple times**_ for the same parameter, if it is of type `RenderFragment` or `RenderFragment<TValue>`. Doing so previously would either result in an exception or just the last passed `RenderFragment` to be used. Now all the provided `RenderFragment` or `RenderFragment<TValue>` will be combined at runtime into a single `RenderFragment` or `RenderFragment<TValue>`.

          For example, this makes it easier to pass e.g. both a markup string and a component to a `ChildContent` parameter:

  ```csharp
  var cut = ctx.RenderComponent<Component>(parameters => parameters
    .AddChildContent("<h1>Below you will find a most interesting alert!</h1>")
    .AddChildContent<Alert>(childParams => childParams
      .Add(p => p.Heading, "Alert heading")
      .Add(p => p.Type, AlertType.Warning)
      .AddChildContent("<p>Hello World</p>")
    )
  );
  ```

          By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- All test doubles are now in the same namespace, `Bunit.TestDoubles`. So all import statements for `Bunit.TestDoubles.JSInterop` and `Bunit.TestDoubles.Authorization` must be changed to `Bunit.TestDoubles`. By [@egil](https://github.com/egil) in [#223](https://github.com/egil/bUnit/pull/223).

- Marked MarkupMatches methods as assertion methods to stop SonarSource analyzers complaining about missing assertions in tests. By [@egil](https://github.com/egil) in [#229](https://github.com/egil/bUnit/pull/229).

- `AddTestAuthorization` now extends `TestContext` instead of `TestServiceProvider`, and also automatically adds the `CascadingAuthenticationState` component to the root render tree. [@egil](https://github.com/egil) in [#237](https://github.com/egil/bUnit/pull/367).

### Removed

List of now removed features.

- The async event dispatcher helper methods have been removed (e.g. `ClickAsync()`), as they do not provide any benefit. If you have an event that triggers async operations in the component under test, instead use `cut.WaitForState()` or `cut.WaitForAssertion()` to await the expected state in the component.

### Fixed

List of any bug fixes.

- Using the ComponentParameterCollectionBuilder's `Add(p => p.Param, value)` method to add a unnamed cascading value didn't create an unnnamed cascading value parameter. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203). Credits to [Ben Sampica (@benjaminsampica)](https://github.com/benjaminsampica) for reporting and helping investigate this issue.
- Triggered events now bubble correctly up the DOM tree and triggers other events of the same type. This is a **potentially breaking change,** since this changes the behaviour of event triggering and thus you might see tests start breaking as a result hereof. By [@egil](https://github.com/egil) in [#119](https://github.com/egil/bUnit/issues/119).

## [1.0.0-beta 10] - 2020-09-15

The following section list all changes in beta-10.

### Added

List of new features.

- Added support for .NET 5 RC-1.

### Changed

List of changes in existing functionality.

- Related to [#189](https://github.com/egil/bUnit/issues/189), a bunch of the core `ITestRenderer` and related types have changed. The internals of `ITestRenderer` is now less exposed and the test renderer is now in control of when rendered components and rendered fragments are created, and when they are updated. This enables the test renderer to protect against race conditions when the `FindComponent`, `FindComponents`, `RenderFragment`, and `RenderComponent` methods are called.

### Fixed

List of any bug fixes.

- Fixes [#189](https://github.com/egil/bUnit/issues/189): The test renderer did not correctly protect against a race condition during initial rendering of a component, and that could in some rare circumstances cause a test to fail when it should not. This has been addressed in this release with a major rewrite of the test renderer, which now controls and owns the rendered component and rendered fragment instances which is created when a component is rendered. By [@egil](https://github.com/egil) in [#201](https://github.com/egil/bUnit/pull/201). Credits to [@Smurf-IV](https://github.com/Smurf-IV) for reporting and helping investigate this issue.

## [1.0.0-beta-9] - 2020-08-26

This release contains a couple of fixes, and adds support for .NET Preview 8 and later. There are no breaking changes in this release.

Thanks to [pharry22](https://github.com/pharry22) for submitting fixes and improvements to the documentation.

### Added

List of new features.

- Added `InvokeAsync(Func<Task>)` to `RenderedComponentInvokeAsyncExtensions`. By [@JeroenBos](https://github.com/JeroenBos) in [#151](https://github.com/egil/bUnit/pull/177).
- Added `ITestRenderer Renderer { get ; }` to `IRenderedFragment` to make it possible to simplify the `IRenderedComponentBase<TComponent>` interface. By [@JeroenBos](https://github.com/JeroenBos) in [#151](https://github.com/egil/bUnit/pull/177).
- Added support for scoped CSS to `MarkupMatches` and related comparer methods. By [@egil](https://github.com/egil) in [#195](https://github.com/egil/bUnit/pull/195).

### Changed

List of changes in existing functionality.

- Moved `InvokeAsync()`, `Render()` and `SetParametersAndRender()` methods out of `IRenderedComponentBase<TComponent>` into extension methods. By [@JeroenBos](https://github.com/JeroenBos) in [#151](https://github.com/egil/bUnit/pull/177).
- Accessing `Markup`, `Nodes` and related methods on a rendered fragment whose underlying component has been removed from the render tree (disposed) now throws a `ComponentDisposedException`. By [@egil](https://github.com/egil) in [#184](https://github.com/egil/bUnit/pull/184).
- Changed bUnit's build to target both .net 5.0 and .net standard 2.1. By [@egil](https://github.com/egil) in [#187](https://github.com/egil/bUnit/pull/187).

### Fixed

List of any bug fixes.

- Fixes [#175](https://github.com/egil/bUnit/issues/175): When a component referenced in a test, e.g. through the `FindComponent()` method was removed from the render tree, accessing the reference could caused bUnit to look for updates to it in the renderer, causing a exception to be thrown. By [@egil](https://github.com/egil) in [#184](https://github.com/egil/bUnit/pull/184).

## [1.0.0-beta-8] - 2020-07-15

Here is beta-8, a small summer vacation release this time. A few needed additions, especially around testing components that use Blazor's authentication and authorization. In addition to this, a lot of documentation has been added to <https://bunit.egilhansen.com/docs/getting-started/>.

### Added

List of new features.

- Authorization fakes added to make it much easier to test components that use authentication and authorization. Learn more in the [Faking Blazor's Authentication and Authorization](https://bunit.egilhansen.com/docs/test-doubles/faking-auth) page. By [@DarthPedro](https://github.com/DarthPedro) in [#151](https://github.com/egil/bUnit/pull/151).

- Added `MarkupMatches(this string actual ...)` extension methods. Make it easier to compare just the text content from a DON text node with a string, while still getting the benefit of the semantic HTML comparer.

### Changed

List of changes in existing functionality.

- `TestContextBase.Dispose` made virtual to allow inheritor's to override it. By [@SimonCropp](https://github.com/SimonCropp) in [#137](https://github.com/egil/bunit/pull/137).
- **[Breaking change]** Changed naming convention for JSMock feature and moved to new namespace, `Bunit.TestDoubles.JSInterop`. All classes and methods containing `Js` (meaning JavaScript) renamed to `JS` for consistency with Blazor's `IJSRuntime`. By [@yourilima](https://github.com/yourilima) in [#150](https://github.com/egil/bUnit/pull/150)

## [1.0.0-beta-7] - 2020-05-19## [1.0.0-beta-7] - 2020-05-19

There are three big changes in bUnit in this release, as well as a whole host of small new features, improvements to the API, and bug fixes. The three big changes are:

1. A splitting of the library
2. Discovery of razor base tests, and
3. A strongly typed way to pass parameters to a component under test.

There are also some breaking changes, which we will cover first.

**NOTE:** The documentation is next on the TODO list, so please bear with me while I update it to reflect all the recent changes.

### Breaking changes

Due to the big restructuring of the library, there are some breaking changes, hopefully for the better.

#### Razor test changes

Previously, the `Test` and `Setup` methods on `<Fixture>` and `<SnapshotTest>` did not have any arguments, and the test context they represented when running, was implicitly available in the scope. This has changed with this release, such that all `Test` and `Setup` methods now receive the text context as an argument, and that should be used to call e.g. `GetComponentUnderTest()` on.

For example, if you have a razor based test that looks like this currently:

```c#
<Fixture Test="Test001" Setup="TestSetup">
    <ComponentUnderTest><Counter /></ComponentUnderTest>
    <Fragment>...</Fragment>
</Fixture>
@code {
    void TestSetup() => Services.AddMockJsRuntime();

    void Test001()
    {
        var cut = GetComponentUnderTest<Counter>();
        var fragment = GetFragment();
    }
}
```

You have to change it to this:

```c#
<Fixture Test="Test001" Setup="TestSetup">
    <ComponentUnderTest><Counter /></ComponentUnderTest>
</Fixture>
@code {
    // Add a Fixture fixture argument to the setup method and use
    // the services collection inside the fixture to register dependencies
    void TestSetup(Fixture fixture) => fixture.Services.AddMockJsRuntime();

    // Add a Fixture fixture argument to the test method
    void Test001(Fixture fixture)
    {
        // Use the fixture instance to get the component under test
        var cut = fixture.GetComponentUnderTest<Counter>();
        var fragment = fixture.GetFragment();
    }
}
```

It is a little more typing, but it is also a lot more obvious what is going on, e.g. where the component under test or fragment is coming from.

In addition to this, the `Tests` and `TestsAsync` methods on `<Fixture>` have been deprecated in this release and throws a runtime exception if used. They were not very used and caused confusion about the state of the components under test between the method calls. Now you can only specify either a `Test` or `TestAsync` method per `<Fixture>`.

#### WaitForRender removed

The `WaitForRender` method has been removed entirely from the library. Since it would only wait for one render, it had a very specific use case, where as the more general `WaitForAssertion` or `WaitForState` will wait for any number of renders, until the assertion passes, or the state predicate returns true. These make them much better suited to create stable tests.

With `WaitForRender`, you would pass in an action that would cause a render before attempting your assertion, e.g.:

```c#
cut.WaitForRender(() => mockForecastService.Task.SetResult(forecasts));

Assert.Equal("...", cut.Markup);
```

This can now be changed to first call the action that will trigger the render, and then wait for an assertion to pass, using `WaitForAssertion`:

```c#
mockForecastService.Task.SetResult(forecasts);

cut.WaitForAssertion(() => Assert.Equal("...", cut.Markup));
```

The two "wait for" methods are also only available through a rendered fragment or rendered component now.

#### ComponentTestFixture deprecated

Previously, the recommended method for creating xUnit component test classes was to inherit from `ComponentTestFixture`. Due to the restructuring of the library, this type is now just a `TestContext` with static component parameters factory methods, so it does not add much value anymore.

The component parameter factory methods are now also available in the more general purpose `ComponentParameterFactory` type, which can be imported into all test classes, not just xUnit ones, using the `import static Bunit.ComponentParameterFactory` method, and then you can change your existing xUnit test classes to inherit from `TestContext` instead of `ComponentTestFixture` to keep the current functionality for xUnit test classes.

That covers the most important breaking changes. Now lets look at the other big changes.

### Splitting up the library

In this release sees bUnit refactored and split up into three different sub libraries. The reasons for doing this are:

- To make it possible to extract the direct dependency on xUnit and easily add support for NUnit or MSTest
- To make it easier to maintain distinct parts of the library going forward
- To enable future support for other non-web variants of Blazor, e.g. the Blazor Mobile Bindings.

The three parts of the library is now:

- **bUnit.core**: The core library only contains code related to the general Blazor component model, i.e. it is not specific to the web version of Blazor.
- **bUnit.web**: The web library, which has a dependency on core, provides all the specific types for rendering and testing Blazor web components.
- **bUnit.xUnit**: The xUnit library, which has a dependency on core, has xUnit specific extensions to bUnit, that enable logging to the test output through the `ILogger` interface in .net core, and an extension to xUnit's test runners, that enable it to discover and run razor based tests defined in `.razor` files.

To keep things compatible with previous releases, an additional package is available, **bUnit**, which includes all of three libraries. That means existing users should be able to keep their single `<PackageReference Include="bunit">` in their projects.

### Discovery of Razor based tests

One of the pain points of writing Razor based tests in `.razor` files was that the individual tests was not correctly discovered. That meant that if had multiple tests in a file, you would not see them in Visual Studios Test Explorer individually, you could not run them individually, and error was not reported individually.

This has changed with the _bUnit.xUnit_ library, that now includes a way for it to discover individual razor tests, currently either a `<Fixture>` or `<SnapshotTest>` inside test components defined in `.razor` files. It also enables you to navigate to the test by double clicking on it in the Test Explorer, and you can run each test individually, and see error reports individually.

**WARNING:** You still have to wait for the Blazor compiler to translate the `.razor` files into `.cs` files, before the tests show up in the Test Explorer, and the this can trip up the Test Explorer. So while this feature is a big improvement to razor based testing, it is still not perfect, and more works need to be done to refine it.

### Strongly typed component parameters

If you prefer writing your tests in C# only, you will be happy to know that there is now a new strongly typed way to pass parameters to components, using a builder. E.g., to render a `ContactInfo` component:

```c#
var cut = RenderComponent<ContactInfo>(parameters => parameters
    .Add(p => p.Name, "Egil Hansen")
    .Add(p => p.Country, "Iceland")
);
```

There are a bunch of different `Add` methods available on the builder, that allows you to easily pass in a `EventCallback`, `ChildContent`, or `RenderFragment`.

The old way using the component parameter factory methods are still available if you prefer that syntax.

NOTE: The parameter builder API is experimental at this point, and will likely change.

### NuGet downloads

The latest version of the library is available on NuGet in various incarnations:

| Name           | Type                                                                      | NuGet Download Link                                                                                                                     |
| -------------- | ------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| bUnit          | Library, includes core, web, and xUnit                                    | [![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/)                   |
| bUnit.core     | Library, only core                                                        | [![Nuget](https://img.shields.io/nuget/dt/bunit.core?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.core/)         |
| bUnit.web      | Library, web and core                                                     | [![Nuget](https://img.shields.io/nuget/dt/bunit.web?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.web/)           |
| bUnit.xUnit    | Library, xUnit and core                                                   | [![Nuget](https://img.shields.io/nuget/dt/bunit.xunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.xunit/)       |
| bUnit.template | Template, which currently creates an xUnit based bUnit test projects only | [![Nuget](https://img.shields.io/nuget/dt/bunit.template?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.template/) |

### Contributions

Thanks to [Martin Stühmer (@samtrion)](https://github.com/samtrion) and [Stef Heyenrath (@StefH)](https://github.com/StefH) for their code contributions in this release, and to [Brad Wilson (@bradwilson)](https://github.com/bradwilson) for his help with enabling xUnit to discover and run Razor based tests.

Also a big thank to all you who have contributed by raising issues, participated in issues by helping answer questions and providing input on design and technical issues.

### Added

- A new event, `OnAfterRender`, has been added to `IRenderedFragmentBase`, which `IRenderedFragment` inherits from. Subscribers will be invoked each time the rendered fragment is re-rendered. Related issue [#118](https://github.com/egil/bunit/issues/118).
- A new property, `RenderCount`, has been added to `IRenderedFragmentBase`, which `IRenderedFragment` inherits from. Its represents the number of times a rendered fragment has been rendered. Related issue [#118](https://github.com/egil/bunit/issues/118).
- A new event, `OnMarkupUpdated`, has been added to `IRenderedFragmentBase`. Subscribers will be notifid each time the rendered fragments markup has been regenerated. Related issue [#118](https://github.com/egil/bunit/issues/118).
- Due to the [concurrency bug discovered](https://github.com/egil/bunit/issues/108), the entire render notification and markup notification system has been changed.
- A new overload `RenderComponent()` and `SetParameterAndRender()`, which takes a `Action<ComponentParameterBuilder<TComponent>>` as input. That allows you to pass parameters to a component under test in a strongly typed way. Thanks to [@StefH](https://github.com/StefH) for the work on this. Related issues: [#79](https://github.com/egil/bunit/issues/79) and [#36](https://github.com/egil/bunit/issues/36).
- The two razor test types, `<Fixture>` and `<SnapshotTest>`, can now be **skipped**. by setting the `Skip="some reason for skipping"` parameter. Note, this requires support from the test runner, which current only includes bUnit.xUnit. Related issue: [#77](https://github.com/egil/bunit/issues/77).
- The two razor test types, `<Fixture>` and `<SnapshotTest>`, can now have a **timeout** specified, by setting the `Timeout="TimeSpan.FromSeconds(2)"` parameter. Note, this requires support from the test runner, which current only includes bUnit.xUnit.
- An `InvokeAsync` method has been added to the `IRenderedFragmentBase` type, which allows invoking of an action in the context of the associated renderer. Related issue: [#82](https://github.com/egil/bunit/issues/82).
- Enabled the "navigate to test" in Test Explorer. Related issue: [#106](https://github.com/egil/bunit/issues/106).
- Enabled xUnit to discover and run Razor-based tests. Thanks to [Brad Wilson (@bradwilson)](https://github.com/bradwilson) for his help with this. Related issue: [#4](https://github.com/egil/bunit/issues/4).

### Changed

- Better error description from `MarkupMatches` when two sets of markup are different.
- The `JsRuntimePlannedInvocation` can now has its response to an invocation set both before and after an invocation is received. It can also have a new response set at any time, which will be used for new invocations. Related issue: [#78](https://github.com/egil/bunit/issues/78).
- The `IDiff` assertion helpers like `ShouldHaveChanges` now takes an `IEnumerable<IDiff>` as input to make it easier to call in scenarios where only an enumerable is available. Related issue: [#87](https://github.com/egil/bunit/issues/87).
- `TextContext` now registers all its test dependencies as services in the `Services` collection. This now includes the `HtmlParser` and `HtmlComparer`. Related issue: [#114](https://github.com/egil/bunit/issues/114).

### Deprecated

- The `ComponentTestFixture` has been deprecated in this release, since it just inherits from `TestContex` and surface the component parameter factory methods. Going forward, users are encouraged to instead inherit directly from `TestContext` in their xUnit tests classes, and add a `import static Bunit.ComponentParameterFactory` to your test classes, to continue to use the component parameter factory methods. Related issue: [#108](https://github.com/egil/bunit/issues/108).

### Removed

- `<Fixture>` tests no longer supports splitting the test method/assertion step into multiple methods through the `Tests` and `TestsAsync` parameters.
- `WaitForRender` has been removed entirely from the library, as the more general purpose `WaitForAssertion` or `WaitForState` covers its use case.
- `WaitForAssertion` or `WaitForState` is no longer available on `ITestContext` types. They are _still_ available on rendered components and rendered fragments.
- `CreateNodes` method has been removed from `ITextContext`. The ability to convert a markup string to a `INodeList` is available through the `HtmlParser` type registered in `ITextContext.Services` service provider.
- `RenderEvents` has been removed from `IRenderedFragment`, and replaced by the `OnMarkupUpdated` and `OnAfterRender` events. Related issue [#118](https://github.com/egil/bunit/issues/118).
- The generic collection assertion methods `ShouldAllBe<T>(this IEnumerable<T> collection, params Action<T, int>[] elementInspectors)` and `ShouldAllBe<T>(this IEnumerable<T> collection, params Action<T>[] elementInspectors)` have been removed from the library.

### Fixed

- A concurrency issue would surface when a component under test caused asynchronous renders that was awaited using the `WaitForRender`, `WaitForState`, or `WaitForAssertion` methods. Related issue [#118](https://github.com/egil/bunit/issues/118).
- `MarkupMatches` and the related semantic markup diffing, didn't correctly ignore the `__internal_stopPropagation_` and `__internal_preventDefault_` added by Blazor to the rendered markup, when users use the `:stopPropagation` and `:preventDefault` modifiers. Thanks to [@samtrion](https://github.com/samtrion) for reporting and solving this. Related issue: [#111](https://github.com/egil/bunit/issues/111).
- `cut.FindComponent<TComponent>()` didn't return the component inside the component under test. It now searches and finds the first child component of the specified type.

* * *

## [1.0.0-beta-6] - 2020-03-01

This release includes a **name change from Blazor Components Testing Library to bUnit**. It also brings along two extra helper methods for working with asynchronously rendering components during testing, and a bunch of internal optimizations and tweaks to the code.

_Why change the name?_ Naming is hard, and I initial chose a very product-namy name, that quite clearly stated what the library was for. However, the name isn't very searchable, since it just contains generic keywords, plus, bUnit is just much cooler. It also gave me the opportunity to remove my name from all the namespaces and simplify those.

### Contributions

Hugh thanks to [Rastislav Novotný (@duracellko)](https://github.com/duracellko)) for his input and review of the `WaitForX` logic added in this release.

### NuGet

The latest version of the library is availble on NuGet:

|                                                                                                                                         | Type     | Link                                             |
| --------------------------------------------------------------------------------------------------------------------------------------- | -------- | ------------------------------------------------ |
| [![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/)                   | Library  | <https://www.nuget.org/packages/bunit/>          |
| [![Nuget](https://img.shields.io/nuget/dt/bunit.template?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.template/) | Template | <https://www.nuget.org/packages/bunit.template/> |

### Added

- **`WaitForState(Func<bool> statePredicate, TimeSpan? timeout = 1 second)` has been added to `ITestContext` and `IRenderedFragment`.**
          This method will wait (block) until the provided statePredicate returns true, or the timeout is reached (during debugging the timeout is disabled). Each time the renderer in the test context renders, or the rendered fragment renders, the statePredicate is evaluated.

          You use this method, if you have a component under test, that requires _one or more asynchronous triggered renders_, to get to a desired state, before the test can continue.

          The following example tests the `DelayedRenderOnClick.razor` component:

  ```cshtml
  // DelayedRenderOnClick.razor
  <p>Times Clicked: @TimesClicked</p>
  <button @onclick="ClickCounter">Trigger Render</button>
  @code
  {
      public int TimesClicked { get; private set; }

      async Task ClickCounter()
      {
          await Task.Delay(1); // wait 1 millisecond
          TimesClicked += 1;
      }
  }
  ```

          This is a test that uses `WaitForState` to wait until the component under test has a desired state, before the test continues:

  ```csharp
  [Fact]
  public void WaitForStateExample()
  {
      // Arrange
      var cut = RenderComponent<DelayedRenderOnClick>();

      // Act
      cut.Find("button").Click();
      cut.WaitForState(() => cut.Instance.TimesClicked == 1);

      // Assert
      cut.Find("p").TextContent.ShouldBe("Times Clicked: 1");
  }
  ```

- **`WaitForAssertion(Action assertion, TimeSpan? timeout = 1 second)` has been added to `ITestContext` and `IRenderedFragment`.**
          This method will wait (block) until the provided assertion method passes, i.e. runs without throwing an assert exception, or until the timeout is reached (during debugging the timeout is disabled). Each time the renderer in the test context renders, or the rendered fragment renders, the assertion is attempted.

          You use this method, if you have a component under test, that requires _one or more asynchronous triggered renders_, to get to a desired state, before the test can continue.

          This is a test that tests the `DelayedRenderOnClick.razor` listed above, and that uses `WaitForAssertion` to attempt the assertion each time the component under test renders:

  ```csharp
  [Fact]
  public void WaitForAssertionExample()
  {
      // Arrange
      var cut = RenderComponent<DelayedRenderOnClick>();

      // Act
      cut.Find("button").Click();

      // Assert
      cut.WaitForAssertion(
          () => cut.Find("p").TextContent.ShouldBe("Times Clicked: 1")
      );
  }
  ```

- **Added support for capturing log statements from the renderer and components under test into the test output.**
          To enable this, add a constructor to your test classes that takes the `ITestOutputHelper` as input, then in the constructor call `Services.AddXunitLogger` and pass the `ITestOutputHelper` to it, e.g.:

  ```csharp
  // ComponentTest.cs
  public class ComponentTest : ComponentTestFixture
  {
      public ComponentTest(ITestOutputHelper output)
      {
          Services.AddXunitLogger(output, minimumLogLevel: LogLevel.Debug);
      }

      [Fact]
      public void Test1() ...
  }
  ```

          For Razor and Snapshot tests, the logger can be added almost the same way. The big difference is that it must be added during _Setup_, e.g.:

  ```cshtml
  // RazorComponentTest.razor
  <Fixture Setup="Setup" ...>
      ...
  </Fixture>
  @code {
      private ITestOutputHelper _output;

      public RazorComponentTest(ITestOutputHelper output)
      {
          _output = output;
      }

      void Setup()
      {
          Services.AddXunitLogger(_output, minimumLogLevel: LogLevel.Debug);
      }
  }
  ```

- **Added simpler `Template` helper method**
          To make it easier to test components with `RenderFragment<T>` parameters (template components) in C# based tests, a new `Template<TValue>(string name, Func<TValue, string> markupFactory)` helper methods have been added. It allows you to create a mock template that uses the `markupFactory` to create the rendered markup from the template.

          This is an example of testing the `SimpleWithTemplate.razor`, which looks like this:

  ```cshtml
  @typeparam T
  @foreach (var d in Data)
  {
      @Template(d);
  }
  @code
  {
      [Parameter] public RenderFragment<T> Template { get; set; }
      [Parameter] public IReadOnlyList<T> Data { get; set; } = Array.Empty<T>();
  }
  ```

          And the test code:

  ```csharp
  var cut = RenderComponent<SimpleWithTemplate<int>>(
      ("Data", new int[] { 1, 2 }),
      Template<int>("Template", num => $"<p>{num}</p>")
  );

  cut.MarkupMatches("<p>1</p><p>2</p>");
  ```

          Using the more general `Template` helper methods, you need to write the `RenderTreeBuilder` logic yourself, e.g.:

  ```csharp
  var cut = RenderComponent<SimpleWithTemplate<int>>(
      ("Data", new int[] { 1, 2 }),
      Template<int>("Template", num => builder => builder.AddMarkupContent(0, $"<p>{num}</p>"))
  );
  ```

- **Added logging to TestRenderer.** To make it easier to understand the rendering life-cycle during a test, the `TestRenderer` will now log when ever it dispatches an event or renders a component (the log statements can be access by capturing debug logs in the test results, as mentioned above).

- **Added some of the Blazor frameworks end-2-end tests.** To get better test coverage of the many rendering scenarios supported by Blazor, the [ComponentRenderingTest.cs](https://github.com/dotnet/aspnetcore/blob/main/src/Components/test/E2ETest/Tests/ComponentRenderingTest.cs) tests from the Blazor frameworks test suite has been converted from a Selenium to a bUnit. The testing style is very similar, so few changes was necessary to port the tests. The two test classes are here, if you want to compare:

  - [bUnit's ComponentRenderingTest.cs](/main/tests/BlazorE2E/ComponentRenderingTest.cs)
  - [Blazor's ComponentRenderingTest.cs](https://github.com/dotnet/aspnetcore/blob/main/src/Components/test/E2ETest/Tests/ComponentRenderingTest.cs)

### Changed

- **Namespaces is now `Bunit`**
          The namespaces have changed from `Egil.RazorComponents.Testing.Library.*` to simply `Bunit` for the library, and `Bunit.Mocking.JSInterop` for the JSInterop mocking support.

- **Auto-refreshing `IElement`s returned from `Find()`**
          `IRenderedFragment.Find(string cssSelector)` now returns a `IElement`, which internally will refresh itself, whenever the rendered fragment it was found in, changes. This means you can now search for an element once in your test and assign it to a variable, and then continue to assert against the same instance, even after triggering renders of the component under test.

          For example, instead of having `cut.Find("p")` in multiple places in the same test, you can do `var p = cut.Find("p")` once, and the use the variable `p` all the places you would otherwise have the `Find(...)` statement.

- **Refreshable element collection returned from `FindAll`.**
          The `FindAll` query method on `IRenderedFragment` now returns a new type, the `IRefreshableElementCollection<IElement>` type, and the method also takes a second optional argument now, `bool enableAutoRefresh = false`.

          The `IRefreshableElementCollection` is a special collection type that can rerun the query to refresh its the collection of elements that are found by the CSS selector. This can either be done manually by calling the `Refresh()` method, or automatically whenever the rendered fragment renders and has changes, by setting the property `EnableAutoRefresh` to `true` (default set to `false`).

          Here are two example tests, that both test the following `ClickAddsLi.razor` component:

  ```cshtml
  <ul>
      @foreach (var x in Enumerable.Range(0, Counter))
      {
          <li>@x</li>
      }
  </ul>
  <button @onclick="() => Counter++"></button>
  @code {
      public int Counter { get; set; } = 0;
  }
  ```

          The first tests uses auto refresh, set through the optional parameter `enableAutoRefresh` passed to FindAll:

  ```csharp
  public void AutoRefreshQueriesForNewElementsAutomatically()
  {
      var cut = RenderComponent<ClickAddsLi>();
      var liElements = cut.FindAll("li", enableAutoRefresh: true);
      liElements.Count.ShouldBe(0);

      cut.Find("button").Click();

      liElements.Count.ShouldBe(1);
  }
  ```

          The second test refreshes the collection manually through the `Refresh()` method on the collection:

  ```csharp
  public void RefreshQueriesForNewElements()
  {
      var cut = RenderComponent<ClickAddsLi>();
      var liElements = cut.FindAll("li");
      liElements.Count.ShouldBe(0);

      cut.Find("button").Click();

      liElements.Refresh(); // Refresh the collection
      liElements.Count.ShouldBe(1);
  }
  ```

- **Custom exception when event handler is missing.** Attempting to triggering a event handler on an element which does not have an handler attached now throws a `MissingEventHandlerException` exception, instead of an `ArgumentException`.

### Deprecated

- **`WaitForNextRender` has been deprecated (marked as obsolete)**, since the added `WaitForState` and `WaitForAssertion` provide a much better foundation to build stable tests on. The plan is to remove completely from the library with the final 1.0.0 release.

### Removed

- **`AddMockHttp` and related helper methods have been removed.**
          The mocking of HTTPClient, supported through the [mockhttp](https://github.com/richardszalay/mockhttp) library, has been removed from the library. This was done because the library really shouldn't have a dependency on a 3. party mocking library. It adds maintenance overhead and uneeded dependencies to it.

          If you are using mockhttp, you can easily add again to your testing project. See [TODO Guide to mocking HttpClient](#) in the docs to learn how.

### Fixed

- **Wrong casing on keyboard event dispatch helpers.**
          The helper methods for the keyboard events was not probably cased, so that has been updated. E.g. from `Keypress(...)` to `KeyPress(...)`.

[unreleased]: https://github.com/bUnit-dev/bUnit/compare/v1.37.7...HEAD
[1.37.7]: https://github.com/bUnit-dev/bUnit/compare/v1.36.0...1.37.7
[1.36.0]: https://github.com/bUnit-dev/bUnit/compare/v1.35.3...v1.36.0
[1.35.3]: https://github.com/bUnit-dev/bUnit/compare/v1.34.0...1.35.3
[1.34.0]: https://github.com/bUnit-dev/bUnit/compare/v1.33.3...v1.34.0
[1.33.3]: https://github.com/bUnit-dev/bUnit/compare/v1.32.7...1.33.3
[1.32.7]: https://github.com/bUnit-dev/bUnit/compare/v1.31.3...v1.32.7
[1.31.3]: https://github.com/bUnit-dev/bUnit/compare/v1.30.3...1.31.3
[1.30.3]: https://github.com/bUnit-dev/bUnit/compare/v1.29.5...v1.30.3
[1.29.5]: https://github.com/bUnit-dev/bUnit/compare/v1.28.9...1.29.5
[1.28.9]: https://github.com/bUnit-dev/bUnit/compare/v1.27.17...v1.28.9
[1.27.17]: https://github.com/bUnit-dev/bUnit/compare/v1.26.64...1.27.17
[1.26.64]: https://github.com/bUnit-dev/bUnit/compare/v1.25.3...v1.26.64
[1.25.3]: https://github.com/bUnit-dev/bUnit/compare/v1.24.10...1.25.3
[1.24.10]: https://github.com/bUnit-dev/bUnit/compare/v1.23.9...v1.24.10
[1.23.9]: https://github.com/bUnit-dev/bUnit/compare/v1.22.19...1.23.9
[1.22.19]: https://github.com/bUnit-dev/bUnit/compare/v1.21.9...v1.22.19
[1.21.9]: https://github.com/bUnit-dev/bUnit/compare/v1.20.8...1.21.9
[1.20.8]: https://github.com/bUnit-dev/bUnit/compare/v1.19.14...v1.20.8
[1.19.14]: https://github.com/bUnit-dev/bUnit/compare/v1.18.4...1.19.14
[1.18.4]: https://github.com/bUnit-dev/bUnit/compare/v1.17.2...v1.18.4
[1.17.2]: https://github.com/bUnit-dev/bUnit/compare/v1.16.2...1.17.2
[1.16.2]: https://github.com/bUnit-dev/bUnit/compare/v1.15.5...v1.16.2
[1.15.5]: https://github.com/bUnit-dev/bUnit/compare/v1.14.4...1.15.5
[1.14.4]: https://github.com/bUnit-dev/bUnit/compare/v1.13.5...v1.14.4
[1.13.5]: https://github.com/bUnit-dev/bUnit/compare/v1.12.6...1.13.5
[1.12.6]: https://github.com/bUnit-dev/bUnit/compare/v1.11.7...v1.12.6
[1.11.7]: https://github.com/bUnit-dev/bUnit/compare/v1.10.14...v1.11.7
[1.10.14]: https://github.com/bUnit-dev/bUnit/compare/v1.9.8...v1.10.14
[1.9.8]: https://github.com/bUnit-dev/bUnit/compare/v1.8.15...v1.9.8
[1.8.15]: https://github.com/bUnit-dev/bUnit/compare/v1.7.7...v1.8.15
[1.7.7]: https://github.com/bUnit-dev/bUnit/compare/v1.6.4...v1.7.7
[1.6.4]: https://github.com/bUnit-dev/bUnit/compare/v1.5.12...v1.6.4
[1.5.12]: https://github.com/bUnit-dev/bUnit/compare/v1.4.15...v1.5.12
[1.4.15]: https://github.com/bUnit-dev/bUnit/compare/v1.3.42...v1.4.15
[1.3.42]: https://github.com/bUnit-dev/bUnit/compare/v1.2.49...v1.3.42
[1.2.49]: https://github.com/bUnit-dev/bUnit/compare/v1.1.5...v1.2.49
[1.1.5]: https://github.com/bUnit-dev/bUnit/compare/v1.0.16...v1.1.5
