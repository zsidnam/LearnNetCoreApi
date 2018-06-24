using Microsoft.AspNetCore.Mvc;
using LearnNetCoreApi.Services;
using LearnNetCoreApi.Models;
using AutoMapper;
using System.Collections.Generic;
using System;
using LearnNetCoreApi.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace LearnNetCoreApi.Controllers
{
    [Route("api/orgLists")]
    public class OrgListsController : Controller
    {
        // TODO: Add logger
        // TODO: Add validation

        #region Dependency Injection

        private IOrganizationRepository _orgRepository;

        public OrgListsController(IOrganizationRepository orgRepository)
        {
            _orgRepository = orgRepository;
        }

        #endregion

        [HttpGet()]
        public IActionResult GetOrgLists()
        {
            var orgListsFromRepo = _orgRepository.GetOrgLists();

            var orgLists = Mapper.Map<IEnumerable<OrgListDto>>(orgListsFromRepo);

            return Ok(orgLists);
        }

        [HttpGet("{orgListId}", Name = "GetOrgList")]
        public IActionResult GetOrgList(Guid orgListId)
        {
            var orgListFromRepo = _orgRepository.GetOrgList(orgListId);

            if (orgListFromRepo == null) return NotFound();

            var orgList = Mapper.Map<OrgListDto>(orgListFromRepo);

            return Ok(orgList);
        }

        [HttpPost()]
        public IActionResult AddOrgList([FromBody] OrgListForCreationDto orgList)
        {
            if (orgList == null) return BadRequest();

            var orgListEntity = Mapper.Map<OrgList>(orgList);
            _orgRepository.AddOrgList(orgListEntity);

            if (!_orgRepository.Save())
            {
                throw new Exception("Creating an orgList failed on save.");
            }

            var orgListToReturn = Mapper.Map<OrgListDto>(orgListEntity);

            return CreatedAtRoute("GetOrgList",
                new { orgListid = orgListToReturn.OrgListId },
                orgListToReturn);
        }

        [HttpPatch("{orgListId}")]
        public IActionResult PartiallyUpdateOrgList(Guid orgListId,
            [FromBody] JsonPatchDocument<OrgListForUpdateDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            var orgListFromRepo = _orgRepository.GetOrgList(orgListId);
            if (orgListFromRepo == null) return NotFound();

            var orgListToPatch = Mapper.Map<OrgListForUpdateDto>(orgListFromRepo);

            patchDoc.ApplyTo(orgListToPatch); // Add validation for this

            Mapper.Map(orgListToPatch, orgListFromRepo);
            _orgRepository.UpdateOrgList(orgListFromRepo);

            if (!_orgRepository.Save())
            {
                throw new Exception($"Updating orgList {orgListId} failed on save.");
            }

            return NoContent();
        }

        [HttpDelete("{orgListId}")]
        public IActionResult DeleteOrgList(Guid orgListId)
        {
            var orgListFromRepo = _orgRepository.GetOrgList(orgListId);
            if (orgListFromRepo == null) return NotFound();

            _orgRepository.DeleteOrgList(orgListFromRepo);

            if (!_orgRepository.Save())
            {
                throw new Exception($"Deleting orgList {orgListId} failed on save.");
            }

            return NoContent();
        }
    }
}

