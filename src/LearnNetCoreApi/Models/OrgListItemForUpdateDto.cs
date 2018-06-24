using System.ComponentModel.DataAnnotations;

namespace LearnNetCoreApi.Models
{
    public class OrgListItemForUpdateDto
    {
        [Required(ErrorMessage = "A title is required.")]
        [MaxLength(250, ErrorMessage = "The title cannot have more than 250 characters.")]
        public string Title { get; set; }

        public bool IsComplete { get; set; }
    }
}
