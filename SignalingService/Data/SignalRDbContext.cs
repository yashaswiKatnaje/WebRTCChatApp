using Microsoft.EntityFrameworkCore;

    public class SignalRDbContext : DbContext
{
        public SignalRDbContext(DbContextOptions<SignalRDbContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }

}

