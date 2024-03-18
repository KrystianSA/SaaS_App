namespace SaaS_App.WebApi.Application.Auth
{
    public class CookieSettings
    {
        public const string CookieName = "auth.token";
        public bool Secure { get; set; } = true;
        public SameSiteMode SameSiteMode { get; set; } = SameSiteMode.Lax;
    }
}
