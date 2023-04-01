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
    [Route("odata/CdaDB/MeetingAttendees")]
    public partial class MeetingAttendeesController : ODataController
    {
        private CDAApp.Server.Data.CdaDBContext context;

        public MeetingAttendeesController(CDAApp.Server.Data.CdaDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CDAApp.Server.Models.CdaDB.MeetingAttendee> GetMeetingAttendees()
        {
            var items = this.context.MeetingAttendees.AsQueryable<CDAApp.Server.Models.CdaDB.MeetingAttendee>();
            this.OnMeetingAttendeesRead(ref items);

            return items;
        }

        partial void OnMeetingAttendeesRead(ref IQueryable<CDAApp.Server.Models.CdaDB.MeetingAttendee> items);

        partial void OnMeetingAttendeeGet(ref SingleResult<CDAApp.Server.Models.CdaDB.MeetingAttendee> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CdaDB/MeetingAttendees(AttendeeID={AttendeeID})")]
        public SingleResult<CDAApp.Server.Models.CdaDB.MeetingAttendee> GetMeetingAttendee(long key)
        {
            var items = this.context.MeetingAttendees.Where(i => i.AttendeeID == key);
            var result = SingleResult.Create(items);

            OnMeetingAttendeeGet(ref result);

            return result;
        }
        partial void OnMeetingAttendeeDeleted(CDAApp.Server.Models.CdaDB.MeetingAttendee item);
        partial void OnAfterMeetingAttendeeDeleted(CDAApp.Server.Models.CdaDB.MeetingAttendee item);

        [HttpDelete("/odata/CdaDB/MeetingAttendees(AttendeeID={AttendeeID})")]
        public IActionResult DeleteMeetingAttendee(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.MeetingAttendees
                    .Where(i => i.AttendeeID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.MeetingAttendee>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMeetingAttendeeDeleted(item);
                this.context.MeetingAttendees.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMeetingAttendeeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMeetingAttendeeUpdated(CDAApp.Server.Models.CdaDB.MeetingAttendee item);
        partial void OnAfterMeetingAttendeeUpdated(CDAApp.Server.Models.CdaDB.MeetingAttendee item);

        [HttpPut("/odata/CdaDB/MeetingAttendees(AttendeeID={AttendeeID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMeetingAttendee(long key, [FromBody]CDAApp.Server.Models.CdaDB.MeetingAttendee item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MeetingAttendees
                    .Where(i => i.AttendeeID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.MeetingAttendee>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMeetingAttendeeUpdated(item);
                this.context.MeetingAttendees.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MeetingAttendees.Where(i => i.AttendeeID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Meeting,Member");
                this.OnAfterMeetingAttendeeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CdaDB/MeetingAttendees(AttendeeID={AttendeeID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMeetingAttendee(long key, [FromBody]Delta<CDAApp.Server.Models.CdaDB.MeetingAttendee> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MeetingAttendees
                    .Where(i => i.AttendeeID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.MeetingAttendee>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnMeetingAttendeeUpdated(item);
                this.context.MeetingAttendees.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MeetingAttendees.Where(i => i.AttendeeID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Meeting,Member");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMeetingAttendeeCreated(CDAApp.Server.Models.CdaDB.MeetingAttendee item);
        partial void OnAfterMeetingAttendeeCreated(CDAApp.Server.Models.CdaDB.MeetingAttendee item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CDAApp.Server.Models.CdaDB.MeetingAttendee item)
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

                this.OnMeetingAttendeeCreated(item);
                this.context.MeetingAttendees.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MeetingAttendees.Where(i => i.AttendeeID == item.AttendeeID);

                Request.QueryString = Request.QueryString.Add("$expand", "Meeting,Member");

                this.OnAfterMeetingAttendeeCreated(item);

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
