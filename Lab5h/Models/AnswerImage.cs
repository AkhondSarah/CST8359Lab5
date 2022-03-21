using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab5h.Models
{
    public enum Question
    {
        Computer, Earth
    }
    public class AnswerImage
    {
       
        public int AnswerImageId { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("File Name")]
        public string FileName { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Url")]
        public string Url { get; set; }

        [Required]
        public Question Question { get; set; }
    }
}
