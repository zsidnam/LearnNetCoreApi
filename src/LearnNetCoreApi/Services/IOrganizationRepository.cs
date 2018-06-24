using LearnNetCoreApi.Entities;
using System;
using System.Collections.Generic;

namespace LearnNetCoreApi.Services
{
    public interface IOrganizationRepository
    {
        IEnumerable<OrgList> GetOrgLists();
        OrgList GetOrgList(Guid orgListId);
        bool OrgListExists(Guid orgListId);
        void AddOrgList(OrgList orgList);
        void UpdateOrgList(OrgList orgList);
        void DeleteOrgList(OrgList orgList);

        IEnumerable<OrgListItem> GetOrgListItemsForOrgList(Guid orgListId);
        OrgListItem GetOrgListItemForOrgList(Guid orgListId, Guid orgListItemId);
        void AddOrgListItemForOrgList(Guid orgListId, OrgListItem orgListItem);
        void UpdateOrgListItemForOrgList(OrgListItem orgListItem);
        void DeleteOrgListItem(OrgListItem orgListItem);

        bool Save();
    }
}
