using System.Collections.Generic;

namespace LearnNetCoreApi.Models
{
    public class OrgListForCreationDto
    {
        public OrgListForCreationDto()
        {
            OrgListItems = new List<OrgListItemForCreationDto>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<OrgListItemForCreationDto> OrgListItems { get; set; }
    }
}
