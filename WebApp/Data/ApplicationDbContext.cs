using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;


namespace WebApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<User> Users { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        /*optionsBuilder.UseNpgsql("User ID=ygjaninextpljp;Password=2879c0f120dbe1b5f52d5722cd26148934e374996d2f06c931b8305ac3835c59;Host=ec2-52-18-116-67.eu-west-1.compute.amazonaws.com;" +
                                 "Port=5432;Database=d2sr2q6lq8crbr;Pooling=true;" +
                                 "TrustServerCertificate=True;");*/
    }
}