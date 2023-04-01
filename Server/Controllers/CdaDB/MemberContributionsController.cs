using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CDAApp.Server.Controllers.CdaDB
{
    [Route("odata/CdaDB/MemberContributions")]
    public partial class MemberContributionsController : ODataController
    {
        private CDAApp.Server.Data.CdaDBContext context;

        public MemberContributionsController(CDAApp.Server.Data.CdaDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CDAApp.Server.Models.CdaDB.MemberContribution> GetMemberContributions()
        {
            var items = this.context.MemberContributions.AsQueryable<CDAApp.Server.Models.CdaDB.MemberContribution>();
            this.OnMemberContributionsRead(ref items);

            return items;
        }

        partial void OnMemberContributionsRead(ref IQueryable<CDAApp.Server.Models.CdaDB.MemberContribution> items);

        partial void OnMemberContributionGet(ref SingleResult<CDAApp.Server.Models.CdaDB.MemberContribution> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CdaDB/MemberContributions(ContributionID={ContributionID})")]
        public SingleResult<CDAApp.Server.Models.CdaDB.MemberContribution> GetMemberContribution(long key)
        {
            var items = this.context.MemberContributions.Where(i => i.ContributionID == key);
            var result = SingleResult.Create(items);

            OnMemberContributionGet(ref result);

            return result;
        }
        partial void OnMemberContributionDeleted(CDAApp.Server.Models.CdaDB.MemberContribution item);
        partial void OnAfterMemberContributionDeleted(CDAApp.Server.Models.CdaDB.MemberContribution item);

        [HttpDelete("/odata/CdaDB/MemberContributions(ContributionID={ContributionID})")]
        public IActionResult DeleteMemberContribution(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.MemberContributions
                    .Where(i => i.ContributionID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.MemberContribution>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMemberContributionDeleted(item);
                this.context.MemberContributions.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMemberContributionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMemberContributionUpdated(CDAApp.Server.Models.CdaDB.MemberContribution item);
        partial void OnAfterMemberContributionUpdated(CDAApp.Server.Models.CdaDB.MemberContribution item);

        [HttpPut("/odata/CdaDB/MemberContributions(ContributionID={ContributionID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMemberContribution(long key, [FromBody]CDAApp.Server.Models.CdaDB.MemberContribution item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MemberContributions
                    .Where(i => i.ContributionID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.MemberContribution>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMemberContributionUpdated(item);
                this.context.MemberContributions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MemberContributions.Where(i => i.ContributionID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Member");
                this.OnAfterMemberContributionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CdaDB/MemberContributions(ContributionID={ContributionID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMemberContribution(long key, [FromBody]Delta<CDAApp.Server.Models.CdaDB.MemberContribution> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MemberContributions
                    .Where(i => i.ContributionID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.MemberContribution>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnMemberContributionUpdated(item);
                this.context.MemberContributions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MemberContributions.Where(i => i.ContributionID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Member");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMemberContributionCreated(CDAApp.Server.Models.CdaDB.MemberContribution item);
        partial void OnAfterMemberContributionCreated(CDAApp.Server.Models.CdaDB.MemberContribution item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CDAApp.Server.Models.CdaDB.MemberContribution item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnMemberContributionCreated(item);
                this.context.MemberContributions.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MemberContributions.Where(i => i.ContributionID == item.ContributionID);

                Request.QueryString = Request.QueryString.Add("$expand", "Member");

                this.OnAfterMemberContributionCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
