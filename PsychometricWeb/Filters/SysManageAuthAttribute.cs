using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace PsychometricWeb
{
    /// <summary>
    /// System background Login authorization certification scheme
    /// </summary>
    public class SysManageAuthAttribute : AuthorizeAttribute
    {
        public const string SysManageAuthScheme = "SysManageAuthScheme";
        public SysManageAuthAttribute()
        {
            base.AuthenticationSchemes = SysManageAuthScheme;
        }
    }


    /// <summary>
    /// Foreground user Login authentication scheme
    /// </summary>
    public class MemberAuthAttribute : AuthorizeAttribute
    {
        public const string MemberAuthScheme = "MemberAuthScheme";
        public MemberAuthAttribute()
        {
            base.AuthenticationSchemes = MemberAuthScheme;
        }
    }



    /// <summary>
    /// Authorization certification
    /// </summary>
    public static class AuthenticationFactory
    {
        /// <summary>
        /// User Login configuration
        /// </summary>
        /// <param name="services"></param>
        public static void UserAuthenConfig(this IServiceCollection services)
        {
            //Multiple Login authorization methods, foreground / background reference [https://www.cnblogs.com/sky-net/p/8669892.html]
            services.AddAuthentication(SysManageAuthAttribute.SysManageAuthScheme)
            .AddCookie(SysManageAuthAttribute.SysManageAuthScheme, o =>
            {//Backstage.
                o.LoginPath = new PathString("/Psycho/Login");
                o.AccessDeniedPath = new PathString("/Psycho/Forbidden");
                o.LogoutPath = new PathString("/Psycho/SessionOut");
                o.SlidingExpiration = true;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(40);
            }).AddCookie(MemberAuthAttribute.MemberAuthScheme, o => //Front desk.
            {
                o.LoginPath = new PathString("/Psycho/TroupeLogIn");
                o.AccessDeniedPath = new PathString("/Psycho/Forbidden");
                o.LogoutPath = new PathString("/Psycho/SessionOut");
                o.ReturnUrlParameter = "ReturnUrl";
            });

        }
    }
}
