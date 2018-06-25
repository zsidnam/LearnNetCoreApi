using System;
using System.Linq;
using System.Collections.Generic;
using LearnNetCoreApi.Entities;
using LearnNetCoreApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LearnNetCoreApi.Services
{
    public class OrganizationRepository : IOrganizationRepository
    {
        #region Dependency Injection

        private OrganizationContext _context;

        public OrganizationRepository(OrganizationContext context)
        {
            _context = context;
        }

        #endregion

        #region OrgLists

        public PagedList<OrgList> GetOrgLists(OrgListResourceParameters queryParams)
        {
            var orgListsBeforePaging = _context.OrgLists
                .OrderBy(l => l.Title);

            return PagedList<OrgList>.Create(orgListsBeforePaging,
                queryParams.PageNumber, queryParams.PageSize);
        }

        public OrgList GetOrgList(Guid orgListId)
        {
            return _context.OrgLists
                .Include(ol => ol.OrgListItems)
                .FirstOrDefault(l => l.OrgListId == orgListId);
        }

        public bool OrgListExists(Guid orgListId)
        {
            return _context.OrgLists.Any(l => l.OrgListId == orgListId);
        }

        public void AddOrgList(OrgList orgList)
        {
            orgList.OrgListId = Guid.NewGuid();
            _context.OrgLists.Add(orgList);

            if (!orgList.OrgListItems.Any()) return;

            foreach (var item in orgList.OrgListItems)
            {
                item.OrgListItemId = Guid.NewGuid();
            }
        }

        public void UpdateOrgList(OrgList orgList)
        {
            // This implementation does not need to do anything. AutoMapper is updating
            // fields in the tracked entity for us.
        }

        public void DeleteOrgList(OrgList orgList)
        {
            _context.OrgLists.Remove(orgList);
        }

        #endregion

        #region OrgListItems

        public IEnumerable<OrgListItem> GetOrgListItemsForOrgList(Guid orgListId)
        {
            return _context.OrgListItems.Where(i => i.OrgListId == orgListId)
                .OrderBy(i => i.Title);
        }

        public OrgListItem GetOrgListItemForOrgList(Guid orgListId, Guid orgListItemId)
        {
            return _context.OrgListItems.FirstOrDefault(i => i.OrgListId == orgListId
                && i.OrgListItemId == orgListItemId);
        }

        public void AddOrgListItemForOrgList(Guid orgListId, OrgListItem orgListItem)
        {
            var orgList = GetOrgList(orgListId);
            if (orgList == null) return;

            if (orgListItem.OrgListItemId == Guid.Empty)
            {
                orgListItem.OrgListItemId = Guid.NewGuid();
            }

            orgList.OrgListItems.Add(orgListItem);
        }

        public void UpdateOrgListItemForOrgList(OrgListItem orgListItem)
        {
            // This implementation does not need to do anything. AutoMapper is updating
            // fields in the tracked entity for us.
        }

        public void DeleteOrgListItem(OrgListItem orgListItem)
        {
            _context.OrgListItems.Remove(orgListItem);
        }

        #endregion

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
