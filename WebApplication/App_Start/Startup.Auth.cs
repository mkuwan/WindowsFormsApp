using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using WebApplication.Models;
using WebApplication.Providers;

namespace WebApplication
{
    public partial class Startup
    {
        // アプリケーションによる OAuthAuthorization の使用を有効にします。その後に Web API を保護できます
        static Startup()
        {
            PublicClientId = "web";

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                AuthorizeEndpointPath = new PathString("/Account/Authorize"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // 認証の構成の詳細については、https://go.microsoft.com/fwlink/?LinkId=301864 を参照してください
        public void ConfigureAuth(IAppBuilder app)
        {
            // 要求ごとに 1 つのインスタンスを使用するよう db コンテキスト、ユーザー マネージャー、サインイン マネージャーを構成します
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // アプリケーションが Cookie を使用して、サインインしたユーザーの情報を格納できるようにします
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // ユーザーがログインするときにセキュリティ スタンプを検証するように設定します。
                    // これはセキュリティ機能の 1 つであり、パスワードを変更するときやアカウントに外部ログインを追加するときに使用されます。
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(20),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // サード パーティのログイン プロバイダーを使用してログインしているユーザーに関する情報を一時的に格納するのに Cookie を使用します
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 2 要素認証処理で第 2 の要素を確認する際に、アプリケーションがユーザー情報を一時的に保存できるようにします。
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // アプリケーションが電話やメールなどの第 2 のログイン確認要素を記憶できるようにします。
            // このオプションをオンにすると、ログイン処理における確認の 2 番目のステップが、ログインしたデバイスで記憶されます。
            // これは、ログインするときの RememberMe オプションと似ています。
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // アプリケーションがベアラー トークンを使用してユーザーを認証できるようにします
            app.UseOAuthBearerTokens(OAuthOptions);

            // 次の行のコメントを解除して、サード パーティのログイン プロバイダーを使用したログインを有効にします
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
