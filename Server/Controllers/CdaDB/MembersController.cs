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
    [Route("odata/CdaDB/Members")]
    public partial class MembersController : ODataController
    {
        private CDAApp.Server.Data.CdaDBContext context;

        public MembersController(CDAApp.Server.Data.CdaDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CDAApp.Server.Models.CdaDB.Member> GetMembers()
        {
            var items = this.context.Members.AsQueryable<CDAApp.Server.Models.CdaDB.Member>();
            this.OnMembersRead(ref items);

            return items;
        }

        partial void OnMembersRead(ref IQueryable<CDAApp.Server.Models.CdaDB.Member> items);

        partial void OnMemberGet(ref SingleResult<CDAApp.Server.Models.CdaDB.Member> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CdaDB/Members(MemberID={MemberID})")]
        public SingleResult<CDAApp.Server.Models.CdaDB.Member> GetMember(int key)
        {
            var items = this.context.Members.Where(i => i.MemberID == key);
            var result = SingleResult.Create(items);

            OnMemberGet(ref result);

            return result;
        }
        partial void OnMemberDeleted(CDAApp.Server.Models.CdaDB.Member item);
        partial void OnAfterMemberDeleted(CDAApp.Server.Models.CdaDB.Member item);

        [HttpDelete("/odata/CdaDB/Members(MemberID={MemberID})")]
        public IActionResult DeleteMember(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Members
                    .Where(i => i.MemberID == key)
                    .Include(i => i.MeetingAttendees)
                    .Include(i => i.MemberContributions)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.Member>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMemberDeleted(item);
                this.context.Members.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMemberDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMemberUpdated(CDAApp.Server.Models.CdaDB.Member item);
        partial void OnAfterMemberUpdated(CDAApp.Server.Models.CdaDB.Member item);

        [HttpPut("/odata/CdaDB/Members(MemberID={MemberID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMember(int key, [FromBody]CDAApp.Server.Models.CdaDB.Member item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Members
                    .Where(i => i.MemberID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.Member>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMemberUpdated(item);
                this.context.Members.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Members.Where(i => i.MemberID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Gender");
                this.OnAfterMemberUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CdaDB/Members(MemberID={MemberID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMember(int key, [FromBody]Delta<CDAApp.Server.Models.CdaDB.Member> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Members
                    .Where(i => i.MemberID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.Member>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnMemberUpdated(item);
                this.context.Members.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Members.Where(i => i.MemberID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Gender");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMemberCreated(CDAApp.Server.Models.CdaDB.Member item);
        partial void OnAfterMemberCreated(CDAApp.Server.Models.CdaDB.Member item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CDAApp.Server.Models.CdaDB.Member item)
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

                this.OnMemberCreated(item);
                this.context.Members.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Members.Where(i => i.MemberID == item.MemberID);

                Request.QueryString = Request.QueryString.Add("$expand", "Gender");

                this.OnAfterMemberCreated(item);

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
