using System.ComponentModel.DataAnnotations;

namespace CV_builder.Models
{
    public class CVModel
    {
        public int Id { get; set; }
        [MinLength(5), Required]
        public string Name { get; set; }
        [MinLength(5), Required]
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<Education> Education { get; set; } = new List<Education>();
        public List<WorkExperience> WorkExperience { get; set; } = new List<WorkExperience>();
    }
}
