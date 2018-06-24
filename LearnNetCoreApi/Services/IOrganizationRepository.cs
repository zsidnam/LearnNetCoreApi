using LearnNetCoreApi.Entities;
using System;
using System.Collections.Generic;

namespace LearnNetCoreApi.Services
{
    public interface IOrganizationRepository
    {
        IEnumerable<OrgList> GetOrgLists();
        OrgList GetOrgList(Guid orgListId);

        IEnumerable<OrgListItem> GetOrgListItems(Guid orgListId);
        OrgListItem GetOrgListItemForOrgList(Guid orgListId, Guid orgListItemId);

        bool Save();
    }
}
