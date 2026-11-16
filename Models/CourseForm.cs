using System.ComponentModel.DataAnnotations;
namespace StudyApp.Models
{
    public class CourseForm
    {
        [Required(ErrorMessage = "Введите название курса.")]
        public string Title { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Выберите преподавателя.")]
        public int TeacherId { get; set; }
        public List<int> StudentIds { get; set; } = new List<int>();
    }
}
