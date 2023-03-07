using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Angular_MasterDetails.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        public int PhoneNo { get; set; }
        public string Picture { get; set; }
        public bool MaritalStatus { get; set; }

        public virtual ICollection<TestEntry> testEntries { get; set; } = new List<TestEntry>();

    }
    public class Disese
    {
        public int DiseseId { get; set; }
        public string? DiseseName { get; set; }
        public virtual ICollection<TestEntry> testEntries { get; set; } = new List<TestEntry>();
    }
    public class TestEntry
    {
        public int TestEntryId { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        [ForeignKey("Disese")]
        public int DiseseId { get; set; }

        //Nav
        public virtual Patient Patient { get; set; }
        public virtual Disese Disese { get; set; }
    }
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<Patient>  Patients { get; set; }
        public DbSet<Disese>  Diseses { get; set; }
        public DbSet<TestEntry>  TestEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Disese>().HasData
                (
                    new Disese { DiseseId = 1,  DiseseName = "Fever"},
                    new Disese { DiseseId = 2,  DiseseName = "Covid-19"},
                    new Disese { DiseseId = 3,  DiseseName = "Cold"},
                    new Disese { DiseseId = 4,  DiseseName = "Maleria"}
                    
                );
        }

        internal object Find(int patientId)
        {
            throw new NotImplementedException();
        }
    }
}
