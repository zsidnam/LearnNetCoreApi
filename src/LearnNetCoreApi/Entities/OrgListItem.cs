using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNetCoreApi.Entities
{
    [Table("OrgListItem")]
    public class OrgListItem
    {
        [Key]
        public Guid OrgListItemId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        public bool IsComplete { get; set; }

        public Guid OrgListId { get; set; }

        [ForeignKey("OrgListId")]
        public OrgList OrgList { get; set; }
    }
}
