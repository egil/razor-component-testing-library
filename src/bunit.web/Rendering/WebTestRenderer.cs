#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
#endif
using Microsoft.Extensions.Logging;
#if NET5_0_OR_GREATER
using Microsoft.JSInterop;
#endif

namespace Bunit.Rendering;

/// <summary>
/// Represents a <see cref="ITestRenderer"/> that is used when rendering
/// Blazor components for the web.
/// </summary>
public class WebTestRenderer : TestRenderer
{
	/// <summary>
	/// Initializes a new instance of the <see cref="WebTestRenderer"/> class.
	/// </summary>
	public WebTestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ComponentRegistry componentRegistry, ILoggerFactory loggerFactory)
		: base(renderedComponentActivator, services, componentRegistry, loggerFactory)
	{
#if NET5_0_OR_GREATER
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
#endif
	}

#if NET5_0_OR_GREATER
	/// <summary>
	/// Initializes a new instance of the <see cref="WebTestRenderer"/> class.
	/// </summary>
	public WebTestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ComponentRegistry componentRegistry, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
		: base(renderedComponentActivator, services, loggerFactory, componentRegistry, componentActivator)
	{
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
	}
#endif
}
