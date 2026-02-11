using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.IdentityModule;


namespace NetFoodia.Persistence.IdentityData.DataSeed
{
    public class IdentityDataInatializer : IDataInatializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataInatializer> _logger;

        public IdentityDataInatializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<IdentityDataInatializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task InatializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("CharityAdmin"));
                    await _roleManager.CreateAsync(new IdentityRole("Volunteer"));
                    await _roleManager.CreateAsync(new IdentityRole("Donor"));
                }

                if (!_userManager.Users.Any())
                {
                    var User = new ApplicationUser()
                    {
                        FullName = "Ahmed Samy",
                        UserName = "AhmedSamy",
                        Email = "AhmedSamy@gmail.com",
                        PhoneNumber = "01016334658",
                        Role = "Admin"
                    };

                    await _userManager.CreateAsync(User, "P@ssw0rd");
                    await _userManager.AddToRoleAsync(User, "Admin");

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While Seeding Identity Database : Message = {ex.Message}");
            }
        }

    }
}
