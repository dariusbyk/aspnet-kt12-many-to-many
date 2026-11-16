using System.ComponentModel.DataAnnotations;
namespace StudyApp.Models
{
    public class TeacherForm
    {
        [Required(ErrorMessage = "Введите имя.")]
        public string Name { get; set; }
    }
}
