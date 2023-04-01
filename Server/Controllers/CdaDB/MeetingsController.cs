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
    [Route("odata/CdaDB/Meetings")]
    public partial class MeetingsController : ODataController
    {
        private CDAApp.Server.Data.CdaDBContext context;

        public MeetingsController(CDAApp.Server.Data.CdaDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CDAApp.Server.Models.CdaDB.Meeting> GetMeetings()
        {
            var items = this.context.Meetings.AsQueryable<CDAApp.Server.Models.CdaDB.Meeting>();
            this.OnMeetingsRead(ref items);

            return items;
        }

        partial void OnMeetingsRead(ref IQueryable<CDAApp.Server.Models.CdaDB.Meeting> items);

        partial void OnMeetingGet(ref SingleResult<CDAApp.Server.Models.CdaDB.Meeting> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CdaDB/Meetings(MeetingID={MeetingID})")]
        public SingleResult<CDAApp.Server.Models.CdaDB.Meeting> GetMeeting(long key)
        {
            var items = this.context.Meetings.Where(i => i.MeetingID == key);
            var result = SingleResult.Create(items);

            OnMeetingGet(ref result);

            return result;
        }
        partial void OnMeetingDeleted(CDAApp.Server.Models.CdaDB.Meeting item);
        partial void OnAfterMeetingDeleted(CDAApp.Server.Models.CdaDB.Meeting item);

        [HttpDelete("/odata/CdaDB/Meetings(MeetingID={MeetingID})")]
        public IActionResult DeleteMeeting(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Meetings
                    .Where(i => i.MeetingID == key)
                    .Include(i => i.MeetingAgenda)
                    .Include(i => i.MeetingAttendees)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.Meeting>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMeetingDeleted(item);
                this.context.Meetings.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMeetingDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMeetingUpdated(CDAApp.Server.Models.CdaDB.Meeting item);
        partial void OnAfterMeetingUpdated(CDAApp.Server.Models.CdaDB.Meeting item);

        [HttpPut("/odata/CdaDB/Meetings(MeetingID={MeetingID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMeeting(long key, [FromBody]CDAApp.Server.Models.CdaDB.Meeting item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Meetings
                    .Where(i => i.MeetingID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.Meeting>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMeetingUpdated(item);
                this.context.Meetings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Meetings.Where(i => i.MeetingID == key);
                ;
                this.OnAfterMeetingUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CdaDB/Meetings(MeetingID={MeetingID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMeeting(long key, [FromBody]Delta<CDAApp.Server.Models.CdaDB.Meeting> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Meetings
                    .Where(i => i.MeetingID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.Meeting>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnMeetingUpdated(item);
                this.context.Meetings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Meetings.Where(i => i.MeetingID == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMeetingCreated(CDAApp.Server.Models.CdaDB.Meeting item);
        partial void OnAfterMeetingCreated(CDAApp.Server.Models.CdaDB.Meeting item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CDAApp.Server.Models.CdaDB.Meeting item)
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

                this.OnMeetingCreated(item);
                this.context.Meetings.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Meetings.Where(i => i.MeetingID == item.MeetingID);

                ;

                this.OnAfterMeetingCreated(item);

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
