using System.ComponentModel.DataAnnotations;

namespace LearnNetCoreApi.Models
{
    public class OrgListItemForCreationDto
    {
        [Required(ErrorMessage = "A title is required.")]
        [MaxLength(250, ErrorMessage = "The title cannot have more than 250 characters.")]
        public string Title { get; set; }
    }
}
