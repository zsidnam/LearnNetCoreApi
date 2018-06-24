using Microsoft.AspNetCore.Mvc;
using LearnNetCoreApi.Services;
using LearnNetCoreApi.Models;
using AutoMapper;
using System.Collections.Generic;
using System;

namespace LearnNetCoreApi.Controllers
{
    [Route("api/orglists")]
    public class OrgListsController : Controller
    {
        #region Dependency Injection

        private IOrganizationRepository _orgRepository;

        public OrgListsController(IOrganizationRepository orgRepository)
        {
            _orgRepository = orgRepository;
        }

        #endregion

        [HttpGet]
        public IActionResult GetOrgLists()
        {
            var orgListsFromRepo = _orgRepository.GetOrgLists();

            var orgLists = Mapper.Map<IEnumerable<OrgListDto>>(orgListsFromRepo);

            return Ok(orgLists);
        }

        [HttpGet("{orgListId}")]
        public IActionResult GetOrgList(Guid orgListId)
        {
            var orgListFromRepo = _orgRepository.GetOrgList(orgListId);

            if (orgListFromRepo == null) return NotFound();

            var orgList = Mapper.Map<OrgListDto>(orgListFromRepo);
            return Ok(orgList);

        }
    }
}

