namespace deli.api.Constants
{
	public enum TypeIdentity
	{
		Application,
		User
	}
	public static class JWTClaimTypes
	{
		public const string Identities = "identities";
		public const string AppId = "appId";
		public const string UserId = "userId";
		public const string AppVersion = "appVersion";
		public const string Scope = "scope";
		public const string Roles = "roles";
		public const string AccountId = "accountId";
		public const string UserName = "userName";
	}
}
