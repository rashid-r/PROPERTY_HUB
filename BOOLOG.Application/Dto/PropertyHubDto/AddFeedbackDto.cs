using System.ComponentModel.DataAnnotations;

namespace BOOLOG.Application.Dto.PropertyDto
{
    public class AddFeedbackDto
    {
        [Required]
        public Guid PropertyId { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
