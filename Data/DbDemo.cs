using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegisterDemo.Model.Authentication.SignUp;

namespace RegisterDemo.Data
{
    public class DbDemo : IdentityDbContext<IdentityUser>
    {
        public DbDemo(DbContextOptions<DbDemo> options) : base(options)
        {

        }
        //public DbSet<> Registers { get; set; }
    }
}
