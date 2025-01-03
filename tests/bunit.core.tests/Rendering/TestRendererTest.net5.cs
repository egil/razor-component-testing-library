#if NET5_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Bunit.Rendering;

public partial class TestRendererTest : TestContext
{
	[Fact(DisplayName = "given a IComponentActivator, " +
						"when passed to constructor," +
						"then it used to create components")]
	public void Test1000()
	{
		var activatorMock = Substitute.For<IComponentActivator>();
		activatorMock.CreateInstance(typeof(Wrapper)).Returns(new Wrapper());
		using var renderer = new TestRenderer(
			Services.GetService<IRenderedComponentActivator>(),
			Services,
			NullLoggerFactory.Instance,
			Services.GetRequiredService<ComponentRegistry>(),
			activatorMock);

		renderer.RenderComponent<Wrapper>(new ComponentParameterCollection());

		activatorMock.Received(1).CreateInstance(typeof(Wrapper));
	}
}
#endif
