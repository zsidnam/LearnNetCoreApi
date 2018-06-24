using Microsoft.AspNetCore.Mvc;
using LearnNetCoreApi.Services;
using LearnNetCoreApi.Models;
using AutoMapper;
using System.Collections.Generic;
using System;
using LearnNetCoreApi.Entities;
using Microsoft.AspNetCore.JsonPatch;
using LearnNetCoreApi.Helpers;
using Microsoft.Extensions.Logging;

namespace LearnNetCoreApi.Controllers
{
    [Route("api/orgLists/{orgListId}/orgListItems")]
    public class OrgListItemsController : Controller
    {
        #region Dependency Injection

        private IOrganizationRepository _orgRepository;
        private ILogger<OrgListItemsController> _logger;

        public OrgListItemsController(IOrganizationRepository orgRepository, ILogger<OrgListItemsController> logger)
        {
            _logger = logger;
            _orgRepository = orgRepository;
        }

        #endregion

        [HttpGet()]
        public IActionResult GetOrgListItemsForOrgList(Guid orgListId)
        {
            if (!_orgRepository.OrgListExists(orgListId)) return NotFound();

            var orgListItemsForOrgListFromRepo = _orgRepository.GetOrgListItemsForOrgList(orgListId);

            var orgListItemsForOrgList = Mapper.Map<IEnumerable<OrgListItemDto>>(orgListItemsForOrgListFromRepo);

            return Ok(orgListItemsForOrgList);
        }

        [HttpGet("{orgListItemId}", Name = "GetOrgListItemForOrgList")]
        public IActionResult GetOrgListItemForOrgList(Guid orgListId, Guid orgListItemId)
        {
            if (!_orgRepository.OrgListExists(orgListId)) return NotFound();

            var orgListItemForOrgListFromRepo = _orgRepository.GetOrgListItemForOrgList(orgListId, orgListItemId);
            if (orgListItemForOrgListFromRepo == null) return NotFound();

            var orgListItemForOrgList = Mapper.Map<OrgListItemDto>(orgListItemForOrgListFromRepo);

            return Ok(orgListItemForOrgList);
        }

        [HttpPost()]
        public IActionResult AddOrgListItemForOrgList(Guid orgListId, [FromBody] OrgListItemForCreationDto orgListItem)
        {
            if (orgListItem == null) return BadRequest();

            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            if (!_orgRepository.OrgListExists(orgListId)) return NotFound();

            var orgListItemEntity = Mapper.Map<OrgListItem>(orgListItem);
            _orgRepository.AddOrgListItemForOrgList(orgListId, orgListItemEntity);

            if (!_orgRepository.Save())
            {
                throw new Exception("Creating an orgListItem for an orgList failed on save.");
            }

            var orgListitemToReturn = Mapper.Map<OrgListItemDto>(orgListItemEntity);

            return CreatedAtRoute("GetOrgListItemForOrgList",
                new { orgListId = orgListId, orgListItemId = orgListitemToReturn.OrgListItemId },
                orgListitemToReturn);
        }

        [HttpPut("{orgListItemId}")]
        public IActionResult UpdateOrgListItemForOrgList(Guid orgListId, Guid orgListItemId,
            [FromBody] OrgListItemForUpdateDto orgListItem)
        {
            if (orgListItem == null) return BadRequest();

            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            if (!_orgRepository.OrgListExists(orgListId)) return NotFound();

            var orgListItemForOrgListFromRepo = _orgRepository.GetOrgListItemForOrgList(orgListId, orgListItemId);
            if (orgListItemForOrgListFromRepo == null) return NotFound();

            Mapper.Map(orgListItem, orgListItemForOrgListFromRepo);
            _orgRepository.UpdateOrgListItemForOrgList(orgListItemForOrgListFromRepo);

            if (!_orgRepository.Save())
            {
                throw new Exception($"Updating orgListItem {orgListItemId} for orgList {orgListId} failed on save.");
            }

            return NoContent();
        }

        [HttpPatch("{orgListItemId}")]
        public IActionResult PartiallyUpdateOrgListItemForOrgList(Guid orgListId, Guid orgListItemId,
            [FromBody] JsonPatchDocument<OrgListItemForUpdateDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            if (!_orgRepository.OrgListExists(orgListId)) return NotFound();

            var orgListItemForOrgListFromRepo = _orgRepository.GetOrgListItemForOrgList(orgListId, orgListItemId);
            if (orgListItemForOrgListFromRepo == null) return NotFound();

            var orgListItemToPatch = Mapper.Map<OrgListItemForUpdateDto>(orgListItemForOrgListFromRepo);

            patchDoc.ApplyTo(orgListItemToPatch, ModelState);

            TryValidateModel(orgListItemToPatch);
            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            Mapper.Map(orgListItemToPatch, orgListItemForOrgListFromRepo);
            _orgRepository.UpdateOrgListItemForOrgList(orgListItemForOrgListFromRepo);

            if (!_orgRepository.Save())
            {
                throw new Exception($"Updating orgListItem {orgListItemId} for orgList {orgListId} failed on save.");
            }

            return NoContent();
        }

        [HttpDelete("{orgListItemId}")]
        public IActionResult DeleteOrgListItemForOrgList(Guid orgListId, Guid orgListItemId)
        {
            if (!_orgRepository.OrgListExists(orgListId)) return NotFound();

            var orgListItemForOrgListFromRepo = _orgRepository.GetOrgListItemForOrgList(orgListId, orgListItemId);
            if (orgListItemForOrgListFromRepo == null) return NotFound();

            _orgRepository.DeleteOrgListItem(orgListItemForOrgListFromRepo);

            if (!_orgRepository.Save())
            {
                throw new Exception($"Deleting orgListItem {orgListItemId} for orgList {orgListId} failed on save.");
            }

            _logger.LogInformation($"OrgListItem {orgListItemId} for orgList {orgListId} was deleted.");

            return NoContent();
        }
    }
}
