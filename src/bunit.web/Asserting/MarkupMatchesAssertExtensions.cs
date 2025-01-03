using AngleSharp.Dom;
using Bunit.Asserting;
using Bunit.Diffing;
using Bunit.Extensions;
using Bunit.Rendering;
using Microsoft.Extensions.Logging;

namespace Bunit;

/// <summary>
/// Assert helpers for comparing markup.
/// </summary>
public static class MarkupMatchesAssertExtensions
{
	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> markup fragment matches
	/// the <paramref name="expected"/> markup fragment, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The markup fragment to verify.</param>
	/// <param name="expected">The expected markup fragment.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches([StringSyntax("Html")]this string actual, [StringSyntax("Html")]string expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		using var parser = new BunitHtmlParser();
		var actualNodes = parser.Parse(actual);
		var expectedNodes = parser.Parse(expected);
		actualNodes.MarkupMatches(expectedNodes, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> markup fragment matches
	/// the <paramref name="expected"/> <see cref="IRenderedFragment"/>, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The markup fragment to verify.</param>
	/// <param name="expected">The expected <see cref="IRenderedFragment"/>.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches([StringSyntax("Html")]this string actual, IRenderedFragment expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var actualNodes = actual.ToNodeList(expected.Services.GetRequiredService<BunitHtmlParser>());
		actualNodes.MarkupMatches(expected, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> markup fragment matches
	/// the <paramref name="expected"/> <see cref="INodeList"/>, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The markup fragment to verify.</param>
	/// <param name="expected">The expected <see cref="INodeList"/>.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches([StringSyntax("Html")]this string actual, INodeList expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var actualNodes = actual.ToNodeList(expected.GetHtmlParser());
		actualNodes.MarkupMatches(expected, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> markup fragment matches
	/// the <paramref name="expected"/> <see cref="INode"/>, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The markup fragment to verify.</param>
	/// <param name="expected">The expected <see cref="INode"/>.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches([StringSyntax("Html")]this string actual, INode expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var actualNodes = actual.ToNodeList(expected.GetHtmlParser());
		actualNodes.MarkupMatches(expected, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> <see cref="IRenderedFragment"/> matches
	/// the <paramref name="expected"/> markup, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The rendered fragment to verify.</param>
	/// <param name="expected">The expected markup.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this IRenderedFragment actual, [StringSyntax("Html")]string expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var expectedNodes = expected.ToNodeList(actual.Services.GetRequiredService<BunitHtmlParser>());
		actual.Nodes.MarkupMatches(expectedNodes, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> <see cref="IRenderedFragment"/> matches
	/// the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragment"/>, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The rendered fragment to verify.</param>
	/// <param name="expected">The expected rendered fragment.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this IRenderedFragment actual, IRenderedFragment expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		actual.Nodes.MarkupMatches(expected.Nodes, userMessage);
	}

	/// <summary>
	/// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> matches
	/// the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragment"/>, using the <see cref="HtmlComparer"/>
	/// type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The list of nodes to verify.</param>
	/// <param name="expected">The expected rendered fragment.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this INodeList actual, IRenderedFragment expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		actual.MarkupMatches(expected.Nodes, userMessage);
	}

	/// <summary>
	/// Verifies that the <paramref name="actual"/> <see cref="INode"/> matches
	/// the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragment"/>, using the <see cref="HtmlComparer"/>
	/// type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The node to verify.</param>
	/// <param name="expected">The expected rendered fragment.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this INode actual, IRenderedFragment expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		actual.MarkupMatches(expected.Nodes, userMessage);
	}

	/// <summary>
	/// Verifies that the <paramref name="actual"/> <see cref="INode"/> matches
	/// the <paramref name="expected"/> markup, using the <see cref="HtmlComparer"/>
	/// type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The node to verify.</param>
	/// <param name="expected">The expected markup.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this INode actual, [StringSyntax("Html")]string expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var expectedNodes = expected.ToNodeList(actual.GetHtmlParser());
		actual.MarkupMatches(expectedNodes, userMessage);
	}

	/// <summary>
	/// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> matches
	/// the <paramref name="expected"/> markup, using the <see cref="HtmlComparer"/>
	/// type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The list of nodes to verify.</param>
	/// <param name="expected">The expected markup.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this INodeList actual, [StringSyntax("Html")]string expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var expectedNodes = expected.ToNodeList(actual.GetHtmlParser());
		actual.MarkupMatches(expectedNodes, userMessage);
	}

	/// <summary>
	/// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> matches
	/// the <paramref name="expected"/> <see cref="INodeList"/>, using the <see cref="HtmlComparer"/>
	/// type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The list of nodes to verify.</param>
	/// <param name="expected">The expected list of nodes.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this INodeList actual, INodeList expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var diffs = actual.CompareTo(expected);

		if (diffs.Count != 0)
			throw new HtmlEqualException(diffs, expected, actual, userMessage);
	}

	/// <summary>
	/// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> matches
	/// the <paramref name="expected"/> <see cref="INode"/>, using the <see cref="HtmlComparer"/>
	/// type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The list of nodes to verify.</param>
	/// <param name="expected">The expected node.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this INodeList actual, INode expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var diffs = actual.CompareTo(expected);

		if (diffs.Count != 0)
			throw new HtmlEqualException(diffs, expected, actual, userMessage);
	}

	/// <summary>
	/// Verifies that the <paramref name="actual"/> <see cref="INode"/> matches
	/// the <paramref name="expected"/> <see cref="INodeList"/>, using the <see cref="HtmlComparer"/>
	/// type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The node to verify.</param>
	/// <param name="expected">The expected list of nodes.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this INode actual, INodeList expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var diffs = actual.CompareTo(expected);

		if (diffs.Count != 0)
			throw new HtmlEqualException(diffs, expected, actual, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> <see cref="IRenderedFragment"/> matches
	/// the rendered markup from the <paramref name="expected"/> <see cref="RenderFragment"/>, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The rendered fragment to verify.</param>
	/// <param name="expected">The render fragment whose output to compare against.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this IRenderedFragment actual, RenderFragment expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		// TODO: This will be obsolete with: https://github.com/bUnit-dev/bUnit/issues/1018
		// As the renderer would be transient we don't have to new up an instance
		using var renderer = new TestRenderer(
			actual.Services.GetRequiredService<IRenderedComponentActivator>(),
			actual.Services.GetRequiredService<TestServiceProvider>(),
			actual.Services.GetRequiredService<ComponentRegistry>(),
			actual.Services.GetRequiredService<ILoggerFactory>());
		var renderedFragment = (IRenderedFragment)renderer.RenderFragment(expected);
		MarkupMatches(actual, renderedFragment, userMessage);
	}

	/// <summary>
	/// Verifies that the markup from the <paramref name="actual"/> matches
	/// the rendered markup from the <paramref name="expected"/> <see cref="RenderFragment"/>, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The markup to verify.</param>
	/// <param name="expected">The render fragment whose output to compare against.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this INode actual, RenderFragment expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var renderedFragment = actual.GetTestContext()?.RenderInsideRenderTree(expected) as IRenderedFragment
			?? AdhocRenderRenderFragment(expected);
		MarkupMatches(actual, renderedFragment, userMessage);
	}

	/// <summary>
	/// Verifies that the markup from the <paramref name="actual"/> matches
	/// the rendered markup from the <paramref name="expected"/> <see cref="RenderFragment"/>, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">The markup to verify.</param>
	/// <param name="expected">The render fragment whose output to compare against.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this INodeList actual, RenderFragment expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var renderedFragment = actual.GetTestContext()?.RenderInsideRenderTree(expected) as IRenderedFragment
			?? AdhocRenderRenderFragment(expected);
		MarkupMatches(actual, renderedFragment, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> elements
	/// the <paramref name="expected"/> markup fragment, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
	/// <param name="actual">A enumerable of IElements to verify.</param>
	/// <param name="expected">The expected markup fragment.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this IEnumerable<IElement> actual, [StringSyntax("Html")]string expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		MarkupMatches(actual.ToNodeList(), expected, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> element matches
	/// the <paramref name="expected"/> fragments, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> element does not match the <paramref name="expected"/> fragments.</exception>
	/// <param name="actual">An IElement to verify.</param>
	/// <param name="expected">The expected markup fragments.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this IElement actual, IEnumerable<IRenderedFragment> expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var expectedNodes = string.Join(string.Empty, expected.Select(e => e.Markup));

		MarkupMatches(actual, expectedNodes, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> elements matches
	/// the <paramref name="expected"/> elements, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> element does not match the <paramref name="expected"/> elements.</exception>
	/// <param name="actual">An element to verify.</param>
	/// <param name="expected">A list of elements.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this IElement actual, IEnumerable<IElement> expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var expectedNodes = expected.ToNodeList();

		MarkupMatches(actual, expectedNodes, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> elements matches
	/// the <paramref name="expected"/> fragment, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> elements does not match the <paramref name="expected"/> fragment.</exception>
	/// <param name="actual">A list of elements to verify.</param>
	/// <param name="expected">The expected markup fragment.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this IEnumerable<IElement> actual, IRenderedFragment expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		MarkupMatches(actual.ToNodeList(), expected, userMessage);
	}

	/// <summary>
	/// Verifies that the rendered markup from the <paramref name="actual"/> elements matches
	/// the <paramref name="expected"/> fragments, using the <see cref="HtmlComparer"/> type.
	/// </summary>
	/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> elements does not match the <paramref name="expected"/> fragments.</exception>
	/// <param name="actual">A list of elements to verify.</param>
	/// <param name="expected">A list of fragments.</param>
	/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
	[AssertionMethod]
	public static void MarkupMatches(this IEnumerable<IElement> actual, IEnumerable<IRenderedFragment> expected, string? userMessage = null)
	{
		if (actual is null)
			throw new ArgumentNullException(nameof(actual));
		if (expected is null)
			throw new ArgumentNullException(nameof(expected));

		var expectedNodes = string.Join(string.Empty, expected.Select(e => e.Markup));

		MarkupMatches(actual.ToNodeList(), expectedNodes, userMessage);
	}

	private static IRenderedFragment AdhocRenderRenderFragment(this RenderFragment renderFragment)
	{
		using var ctx = new TestContext();
		return (IRenderedFragment)ctx.RenderInsideRenderTree(renderFragment);
	}

	private static INodeList ToNodeList(this string markup, BunitHtmlParser? htmlParser)
	{
		if (htmlParser is null)
		{
			using var newHtmlParser = new BunitHtmlParser();
			return newHtmlParser.Parse(markup);
		}

		return htmlParser.Parse(markup);
	}

	private static INodeList ToNodeList(this IEnumerable<IElement> elements, BunitHtmlParser? htmlParser = null)
	{
		var nodesStr = string.Join(string.Empty, elements.Select(s => s.OuterHtml));

		if (htmlParser is null)
		{
			using var parser = new BunitHtmlParser();
			return nodesStr.ToNodeList(parser);
		}

		return nodesStr.ToNodeList(htmlParser);
	}
}
