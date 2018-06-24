using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNetCoreApi.Entities
{
    [Table("OrgList")]
    public class OrgList
    {
        public OrgList()
        {
            OrgListItems = new List<OrgListItem>();
        }

        [Key]
        public Guid OrgListId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public ICollection<OrgListItem> OrgListItems { get; set; }
    }
}
