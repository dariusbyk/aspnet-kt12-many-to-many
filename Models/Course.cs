using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudyApp.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        public string Title { get; set; }
        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
