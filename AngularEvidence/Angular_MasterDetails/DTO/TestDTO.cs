using Angular_MasterDetails.Models;

namespace Angular_MasterDetails.DTO
{
    public class TestDTO
    {
       public int? PatientId { get; set; }

        public string PatientName { get; set; }
        
        public DateTime BirthDate { get; set; }

        public int PhoneNo { get; set; }

        public string Picture { get; set; }

        public IFormFile PictureFile { get; set; }

        public bool MaritalStatus { get; set; }

        public string? diseseStringify { get; set; }

        public Disese[] DiseseItems { get; set; }
    }
}
