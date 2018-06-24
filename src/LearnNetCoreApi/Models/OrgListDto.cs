using System;

namespace LearnNetCoreApi.Models
{
    public class OrgListDto
    {
        public Guid OrgListId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
