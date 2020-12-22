using KendamaShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KendamaShop.Startup))]
namespace KendamaShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // Create application roles
            if (!roleManager.RoleExists("Admin"))
            {
                // Add admin role
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                // Add admin user
                var user = new ApplicationUser();
                user.UserName = "admin@gmail.com";
                user.Email = "admin@gmail.com";
                var adminCreated = UserManager.Create(user, "!1Admin");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("Partner"))
            {
                var role = new IdentityRole();
                role.Name = "Partner";
                roleManager.Create(role);
                // Add partner user
                var user = new ApplicationUser();
                user.UserName = "partner@gmail.com";
                user.Email = "partner@gmail.com";
                var partnerCreated = UserManager.Create(user, "!1Partner");
                if (partnerCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Partner");
                }
            }
            if (!roleManager.RoleExists("User"))
            {
                // Add user role
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
                // Add user user
                var user = new ApplicationUser();
                user.UserName = "partner@gmail.com";
                user.Email = "partner@gmail.com";
                var userCreated = UserManager.Create(user, "!1Partner");
                if (userCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "User");
                }
            }
        }
    }

    
}
