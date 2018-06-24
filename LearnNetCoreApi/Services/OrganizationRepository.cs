using System;
using System.Linq;
using System.Collections.Generic;
using LearnNetCoreApi.Entities;

namespace LearnNetCoreApi.Services
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private OrganizationContext _context;

        public OrganizationRepository(OrganizationContext context)
        {
            _context = context;
        }

        public IEnumerable<OrgList> GetOrgLists()
        {
            return _context.OrgLists.OrderBy(l => l.Title);
        }

        public OrgList GetOrgList(Guid listId)
        {
            return _context.OrgLists.FirstOrDefault(l => l.OrgListId == listId);
        }

        public OrgListItem GetOrgListItemForOrgList(Guid listId, Guid listItemId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrgListItem> GetOrgListItems(Guid listId)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
