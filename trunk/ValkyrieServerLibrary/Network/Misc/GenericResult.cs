namespace Gablarski
{
	/// <summary>
	/// Generic result responses.
	/// </summary>
	public enum GenericResult
		: byte
	{
		/// <summary>
		/// Failed for an unknown reason.
		/// </summary>
		FailedUnknown = 0,

		/// <summary>
		/// Succeeded.
		/// </summary>
		Success = 1,

		/// <summary>
		/// Failed due to insufficient permissions.
		/// </summary>
		FailedPermissions = 2,
	}
}