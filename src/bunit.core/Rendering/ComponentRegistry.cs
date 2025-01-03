namespace Bunit.Rendering;

/// <summary>
/// This internal class is used to keep track of all components that have been rendered.
/// This class is not intended to be used directly by users of bUnit.
/// </summary>
public sealed class ComponentRegistry
{
	private readonly HashSet<IComponent> components = [];

	/// <summary>
	/// Retrieves all components that have been rendered.
	/// </summary>
	public ISet<IComponent> Components => components;

	/// <summary>
	/// Registers a component as rendered.
	/// </summary>
	public void Register(IComponent component)
		=> components.Add(component);

	/// <summary>
	/// Removes all components from the registry.
	/// </summary>
	public void Clear() => components.Clear();
}
