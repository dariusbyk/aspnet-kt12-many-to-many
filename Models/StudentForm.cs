using System.ComponentModel.DataAnnotations;
namespace StudyApp.Models
{
    public class StudentForm
    {
        [Required(ErrorMessage = "Введите имя.")]
        public string Name { get; set; }
    }
}
