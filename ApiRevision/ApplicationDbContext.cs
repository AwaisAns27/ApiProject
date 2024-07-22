using ApiRevision.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiRevision
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student {Id= 1, GrNo = 234,Name ="Awais Ansari",Stream = "Maths", DateOfAdmission= new DateTime(2020,08,24)},
                new Student { Id = 2, GrNo = 454, Name = "Sadain Ansari", Stream = "Commerce", DateOfAdmission = new DateTime(2021, 03, 01) },
                new Student { Id = 3, GrNo = 986, Name = "Arish Khan", Stream = "Medical", DateOfAdmission = DateTime.Now }

                );
        }

    }
}
