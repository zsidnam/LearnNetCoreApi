using System;

namespace LearnNetCoreApi.Models
{
    public class OrgListItemDto
    {
        public Guid OrgListItemId { get; set; }

        public string Title { get; set; }

        public bool IsComplete { get; set; }

        public Guid OrgListId { get; set; }
    }
}
