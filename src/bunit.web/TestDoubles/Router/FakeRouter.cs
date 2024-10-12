using System.Globalization;
using System.Reflection;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace Bunit.TestDoubles.Router;

internal sealed class FakeRouter : IDisposable
{
	private readonly NavigationManager navigationManager;
	private readonly ComponentRegistry componentRegistry;

	public FakeRouter(NavigationManager navigationManager, ComponentRegistry componentRegistry)
	{
		this.navigationManager = navigationManager;
		this.componentRegistry = componentRegistry;
		navigationManager.LocationChanged += UpdatePageParameters;
	}

	public void Dispose() => navigationManager.LocationChanged -= UpdatePageParameters;

	private void UpdatePageParameters(object? sender, LocationChangedEventArgs e)
	{
		var uri = new Uri(e.Location);
		var relativeUri = uri.PathAndQuery;

		foreach (var instance in componentRegistry.Components)
		{
			var routeAttributes = GetRouteAttributesFromComponent(instance);

			if (routeAttributes.Length == 0)
			{
				continue;
			}

			foreach (var template in routeAttributes.Select(r => r.Template))
			{
				var templateSegments = template.Trim('/').Split("/");
				var uriSegments = relativeUri.Trim('/').Split("/");

				if (templateSegments.Length > uriSegments.Length)
				{
					continue;
				}
#if NET6_0_OR_GREATER
				var parameters = new Dictionary<string, object?>();
#else
				var parameters = new Dictionary<string, object>();
#endif

				for (var i = 0; i < templateSegments.Length; i++)
				{
					var templateSegment = templateSegments[i];
					if (templateSegment.StartsWith('{') && templateSegment.EndsWith('}'))
					{
						var parameterName = GetParameterName(templateSegment);
						var property = GetParameterProperty(instance, parameterName);

						if (property is null)
						{
							continue;
						}

						var isCatchAllParameter = templateSegment[1] == '*';
						if (!isCatchAllParameter)
						{
							parameters[property.Name] = GetValue(uriSegments[i], property);
						}
						else
						{
							parameters[parameterName] = string.Join("/", uriSegments.Skip(i));
						}
					}
					else if (templateSegment != uriSegments[i])
					{
						break;
					}
				}

				if (parameters.Count == 0)
				{
					continue;
				}

				// Shall we await this? This should be synchronous in most cases
				// If not, very likely the user has overriden the SetParametersAsync method
				// And should use WaitForXXX methods to await the desired state
				instance.SetParametersAsync(ParameterView.FromDictionary(parameters));
			}
		}
	}

	private static RouteAttribute[] GetRouteAttributesFromComponent(IComponent instance)
	{
		var routeAttributes = instance
			.GetType()
			.GetCustomAttributes(typeof(RouteAttribute), true)
			.Cast<RouteAttribute>()
			.ToArray();
		return routeAttributes;
	}

	private static string GetParameterName(string templateSegment) =>
		templateSegment
			.Trim('{', '}', '*')
			.Replace("?", string.Empty, StringComparison.OrdinalIgnoreCase)
			.Split(':')[0];

	private static PropertyInfo? GetParameterProperty(object instance, string propertyName)
	{
		var propertyInfos = instance.GetType()
			.GetProperties(BindingFlags.Public | BindingFlags.Instance);

		return Array.Find(propertyInfos, prop => prop.GetCustomAttributes(typeof(ParameterAttribute), true).Any() &&
		                                         string.Equals(prop.Name, propertyName, StringComparison.OrdinalIgnoreCase));
	}

	private static object GetValue(string value, PropertyInfo property)
	{
		var underlyingType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
		return Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
	}
}
