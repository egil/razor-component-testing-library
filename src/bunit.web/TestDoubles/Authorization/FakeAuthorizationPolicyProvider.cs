using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Test class that provides an implementation of IAuthorizationPolicyProvider that
	/// assists user in testing components that use authentication and authorization 
	/// </summary>
	public class FakeAuthorizationPolicyProvider : IAuthorizationPolicyProvider
	{
		private readonly AuthorizationOptions options = new AuthorizationOptions();

		/// <summary>
		/// Gets the default authorization policy.
		/// </summary>
		/// <returns>Default policy.</returns>
		public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
			=> Task.FromResult(options.DefaultPolicy);

		/// <summary>
		/// Gets the fallback authorization policy.
		/// </summary>
		/// <returns>Fallback policy.</returns>
		public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
			=> Task.FromResult(options.FallbackPolicy);

		/// <summary>
		/// Get the current policy, which in this implementation just returns a test policy.
		/// </summary>
		/// <param name="policyName">Policy name.</param>
		/// <returns>Test policy for the specified name.</returns>
		public Task<AuthorizationPolicy> GetPolicyAsync(string policyName) => Task.FromResult(
			new AuthorizationPolicy(new[]
			{
					new TestPolicyRequirement { PolicyName = policyName }
			},
			new[] { $"TestScheme:{policyName}" }));
	}

	/// <summary>
	/// Test requirement that supports the minimum requirement - just a policy name.
	/// </summary>
	public class TestPolicyRequirement : IAuthorizationRequirement
	{
		/// <summary>
		/// Gets or sets the policy name for this requirement.
		/// </summary>
		public string PolicyName { get; set; } = string.Empty;
	}
}
