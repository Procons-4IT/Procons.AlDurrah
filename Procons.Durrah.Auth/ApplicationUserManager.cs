namespace Procons.Durrah.Auth
{
    using Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Procons.DataBaseHelper;
    using Sap.Data.Hana;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        IUserStore<ApplicationUser> _store;
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            _store = store;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            //var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>());

            // Configure validation logic for usernames
            appUserManager.UserValidator = new UserValidator<ApplicationUser>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            //appUserManager.EmailService = new Procons.Durrah.Services.EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }

            return appUserManager;
        }

        public override Task<ApplicationUser> FindAsync(string userName, string password)
        {
            var dbHelper = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();
            dbHelper.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            var result = dbHelper.ExecuteQuery(string.Format("SELECT * FROM \"{0}\".\"OCRD\" WHERE \"CardCode\"='{1}'", ConfigurationManager.AppSettings["DatabaseName"], userName));
            ApplicationUser user = null;
            while (result.Read())
            {
                user = new ApplicationUser()
                {
                    UserName = result["CardCode"].ToString(),
                    EmailConfirmed = true,
                    FirstName = result["CardName"].ToString(),
                    LastName = result["CardName"].ToString(),
                    UserType = result["CardType"].ToString()
                };
            }
            return Task.FromResult(user);
        }

        public override Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType)
        {
            ClaimsIdentity _claimsIdentity = new ClaimsIdentity();
            _claimsIdentity.AddClaim(new Claim("UserName", user.UserName));
            _claimsIdentity.AddClaim(new Claim("Type", user.UserType));
            return Task.FromResult(_claimsIdentity);
        }
    }
}