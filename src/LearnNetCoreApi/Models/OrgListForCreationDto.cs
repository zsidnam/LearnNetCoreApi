using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LearnNetCoreApi.Models
{
    public class OrgListForCreationDto
    {
        public OrgListForCreationDto()
        {
            OrgListItems = new List<OrgListItemForCreationDto>();
        }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public ICollection<OrgListItemForCreationDto> OrgListItems { get; set; }
    }
}
