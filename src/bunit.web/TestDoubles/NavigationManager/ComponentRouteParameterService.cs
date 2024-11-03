using System.Globalization;
using System.Reflection;
using Bunit.Rendering;
#if NET6_0_OR_GREATER
using ParameterViewDictionary = System.Collections.Generic.Dictionary<string, object?>;
#else
using ParameterViewDictionary = System.Collections.Generic.Dictionary<string, object>;
#endif

namespace Bunit.TestDoubles;

/// <summary>
/// This is an internal service that is used to update components with route parameters.
/// It is not meant to be used outside bUnit internal classes.
/// </summary>
public sealed class ComponentRouteParameterService
{
	private readonly ComponentRegistry componentRegistry;

	/// <summary>
	/// Initializes a new instance of the <see cref="ComponentRouteParameterService"/> class.
	/// </summary>
	public ComponentRouteParameterService(ComponentRegistry componentRegistry)
	{
		this.componentRegistry = componentRegistry;
	}

	/// <summary>
	/// Triggers the components to update their parameters based on the route parameters.
	/// </summary>
	public void UpdateComponentsWithRouteParameters(Uri uri)
	{
		_ = uri ?? throw new ArgumentNullException(nameof(uri));

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
				var parameters = GetParametersFromTemplateAndUri(template, relativeUri, instance);
				if (parameters.Count > 0)
				{
					instance.SetParametersAsync(ParameterView.FromDictionary(parameters));
				}
			}
		}
	}

	private static RouteAttribute[] GetRouteAttributesFromComponent(IComponent instance) =>
		instance.GetType()
			.GetCustomAttributes(typeof(RouteAttribute), true)
			.Cast<RouteAttribute>()
			.ToArray();

	private static ParameterViewDictionary GetParametersFromTemplateAndUri(string template, string relativeUri, IComponent instance)
	{
		var templateSegments = template.Trim('/').Split("/");
		var uriSegments = relativeUri.Trim('/').Split("/");

		if (templateSegments.Length > uriSegments.Length)
		{
			return [];
		}

		var parameters = new ParameterViewDictionary();

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
				parameters[property.Name] = isCatchAllParameter
					? string.Join("/", uriSegments.Skip(i))
					: GetValue(uriSegments[i], property);
			}
			else if (templateSegment != uriSegments[i])
			{
				return [];
			}
		}

		return parameters;
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
