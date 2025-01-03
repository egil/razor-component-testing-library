using Bunit.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace Bunit.TestDoubles;

using URI = Uri;

/// <summary>
/// Represents a fake <see cref="NavigationManager"/> that captures calls to
/// <see cref="NavigationManager.NavigateTo(string, bool)"/> for testing purposes.
/// </summary>
public sealed class FakeNavigationManager : NavigationManager
{
	private readonly TestContextBase testContextBase;
	private readonly ComponentRouteParameterService componentRouteParameterService;
	private readonly Stack<NavigationHistory> history = new();

	/// <summary>
	/// The navigation history captured by the <see cref="FakeNavigationManager"/>.
	/// This is a stack based collection, so the first element is the latest/current navigation target.
	/// </summary>
	/// <remarks>
	/// The initial Uri is not added to the history.
	/// </remarks>
	public IReadOnlyCollection<NavigationHistory> History => history;

	/// <summary>
	/// Initializes a new instance of the <see cref="FakeNavigationManager"/> class.
	/// </summary>
	[SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "By design. Fake navigation manager defaults to local host as base URI.")]
	public FakeNavigationManager(TestContextBase testContextBase, ComponentRouteParameterService componentRouteParameterService)
	{
		this.testContextBase = testContextBase;
		this.componentRouteParameterService = componentRouteParameterService;
		Initialize("http://localhost/", "http://localhost/");
	}

#if !NET6_0_OR_GREATER
	/// <inheritdoc/>
	protected override void NavigateToCore(string uri, bool forceLoad)
	{
		var absoluteUri = GetNewAbsoluteUri(uri);
		var changedBaseUri = HasDifferentBaseUri(absoluteUri);

		if (changedBaseUri)
		{
			BaseUri = GetBaseUri(absoluteUri);
		}

		Uri = ToAbsoluteUri(uri).OriginalString;
		history.Push(new NavigationHistory(uri, new NavigationOptions(forceLoad)));

		testContextBase.Renderer.Dispatcher.InvokeAsync(() =>
		{
			Uri = absoluteUri.OriginalString;

			// Only notify of changes if user navigates within the same
			// base url (domain). Otherwise, the user navigated away
			// from the app, and Blazor's NavigationManager would
			// not notify of location changes.
			if (!changedBaseUri)
			{
				NotifyLocationChanged(isInterceptedLink: false);
			}
			else
			{
				BaseUri = GetBaseUri(absoluteUri);
			}

			componentRouteParameterService.UpdateComponentsWithRouteParameters(absoluteUri);
		});
	}
#endif

#if NET6_0_OR_GREATER
	/// <inheritdoc/>
	protected override void NavigateToCore(string uri, NavigationOptions options)
	{
		_ = uri ?? throw new ArgumentNullException(nameof(uri));
		var absoluteUri = GetNewAbsoluteUri(uri);
		var changedBaseUri = HasDifferentBaseUri(absoluteUri);

		if (changedBaseUri)
		{
			BaseUri = GetBaseUri(absoluteUri);
		}

		Uri = ToAbsoluteUri(uri).OriginalString;

		if (options.ReplaceHistoryEntry && history.Count > 0)
			history.Pop();

#if NET7_0_OR_GREATER
		HistoryEntryState = options.ForceLoad ? null : options.HistoryEntryState;
		testContextBase.Renderer.Dispatcher.InvokeAsync(async () =>
#else
		testContextBase.Renderer.Dispatcher.InvokeAsync(() =>
#endif
		{
			Uri = absoluteUri.OriginalString;

#if NET7_0_OR_GREATER
			var shouldContinueNavigation = false;
			try
			{
				shouldContinueNavigation = await NotifyLocationChangingAsync(uri, options.HistoryEntryState, isNavigationIntercepted: false).ConfigureAwait(false);
			}
			catch (Exception exception)
			{
				history.Push(new NavigationHistory(uri, options, NavigationState.Faulted, exception));
				return;
			}

			history.Push(new NavigationHistory(uri, options, shouldContinueNavigation ? NavigationState.Succeeded : NavigationState.Prevented));

			if (!shouldContinueNavigation)
			{
				return;
			}
#else
			history.Push(new NavigationHistory(uri, options));
#endif


			// Only notify of changes if user navigates within the same
			// base url (domain). Otherwise, the user navigated away
			// from the app, and Blazor's NavigationManager would
			// not notify of location changes.
			if (!changedBaseUri)
			{
				NotifyLocationChanged(isInterceptedLink: false);
			}
			else
			{
				BaseUri = GetBaseUri(absoluteUri);
			}

			componentRouteParameterService.UpdateComponentsWithRouteParameters(absoluteUri);
		});
	}
#endif

#if NET7_0_OR_GREATER
	/// <inheritdoc/>
	protected override void SetNavigationLockState(bool value) {}

	/// <inheritdoc/>
	protected override void HandleLocationChangingHandlerException(Exception ex, LocationChangingContext context)
		=> throw ex;
#endif

	private URI GetNewAbsoluteUri(string uri)
		=> new URI(uri, UriKind.RelativeOrAbsolute).IsAbsoluteUri
			? new URI(uri, UriKind.RelativeOrAbsolute) : ToAbsoluteUri(uri);

	private bool HasDifferentBaseUri(URI absoluteUri)
		=> URI.Compare(
			new URI(BaseUri, UriKind.Absolute),
			absoluteUri,
			UriComponents.SchemeAndServer,
			UriFormat.Unescaped,
			StringComparison.OrdinalIgnoreCase) != 0;

	private static string GetBaseUri(URI uri)
	{
		return uri.Scheme + "://" + uri.Authority + "/";
	}
}
