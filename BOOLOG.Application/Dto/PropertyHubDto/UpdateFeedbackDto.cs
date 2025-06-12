using System.ComponentModel.DataAnnotations;

namespace BOOLOG.Application.Dto.PropertyHubDto
{
    public class UpdateFeedbackDto
    {
        public int Rating { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
