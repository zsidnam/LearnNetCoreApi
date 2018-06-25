using System.ComponentModel.DataAnnotations;

namespace LearnNetCoreApi.Models
{
    public class OrgListForUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
