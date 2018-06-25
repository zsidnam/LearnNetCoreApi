using Microsoft.AspNetCore.Mvc;
using LearnNetCoreApi.Services;
using LearnNetCoreApi.Models;
using AutoMapper;
using System.Collections.Generic;
using System;
using LearnNetCoreApi.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using LearnNetCoreApi.Helpers;
using Newtonsoft.Json;

namespace LearnNetCoreApi.Controllers
{
    [Route("api/orgLists")]
    public class OrgListsController : Controller
    {
        #region Dependency Injection

        private readonly IOrganizationRepository _orgRepository;
        private readonly ILogger<OrgListsController> _logger;
        private readonly IUrlHelper _urlHelper;

        public OrgListsController(IOrganizationRepository orgRepository,
            ILogger<OrgListsController> logger, IUrlHelper urlHelper)
        {
            _logger = logger;
            _urlHelper = urlHelper;
            _orgRepository = orgRepository;
        }

        #endregion

        [HttpGet(Name = "GetOrgLists")]
        public IActionResult GetOrgLists(OrgListResourceParameters queryParams)
        {
            var orgListsFromRepo = _orgRepository.GetOrgLists(queryParams);

            var orgLists = Mapper.Map<IEnumerable<OrgListDto>>(orgListsFromRepo);

            var previousPageLink = orgListsFromRepo.HasPrevious
                ? CreateOrgListsResourceUri(queryParams, ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = orgListsFromRepo.HasNext
                ? CreateOrgListsResourceUri(queryParams, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = orgListsFromRepo.TotalCount,
                pageSize = orgListsFromRepo.PageSize,
                currentPage = orgListsFromRepo.CurrentPage,
                totalPages = orgListsFromRepo.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationMetadata));

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

            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

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

            patchDoc.ApplyTo(orgListToPatch, ModelState);

            TryValidateModel(orgListToPatch);
            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

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

            _logger.LogInformation($"Deleted orgList {orgListId}.");

            return NoContent();
        }

        #region Private Methods

        private string CreateOrgListsResourceUri(OrgListResourceParameters queryParams, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetOrgLists",
                        new
                        {
                            pageNumber = queryParams.PageNumber - 1,
                            pageSize = queryParams.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetOrgLists",
                        new
                        {
                            pageNumber = queryParams.PageNumber + 1,
                            pageSize = queryParams.PageSize
                        });
                default:
                    return _urlHelper.Link("GetOrgLists",
                        new
                        {
                            pageNumber = queryParams.PageNumber,
                            pageSize = queryParams.PageSize
                        });
            }
        }

        #endregion
    }
}

