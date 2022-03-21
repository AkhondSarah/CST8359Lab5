using Lab5h.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5h.Data
{
    public class AnswerImageDataContext : DbContext
    {
        public AnswerImageDataContext(DbContextOptions<AnswerImageDataContext> options) : base(options) { }

        public DbSet<AnswerImage> AnswerImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerImage>().ToTable("AnswerImage");
        }
    }
}
