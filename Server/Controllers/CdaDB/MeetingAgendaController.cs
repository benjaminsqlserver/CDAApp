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
    [Route("odata/CdaDB/MeetingAgenda")]
    public partial class MeetingAgendaController : ODataController
    {
        private CDAApp.Server.Data.CdaDBContext context;

        public MeetingAgendaController(CDAApp.Server.Data.CdaDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CDAApp.Server.Models.CdaDB.MeetingAgendum> GetMeetingAgenda()
        {
            var items = this.context.MeetingAgenda.AsQueryable<CDAApp.Server.Models.CdaDB.MeetingAgendum>();
            this.OnMeetingAgendaRead(ref items);

            return items;
        }

        partial void OnMeetingAgendaRead(ref IQueryable<CDAApp.Server.Models.CdaDB.MeetingAgendum> items);

        partial void OnMeetingAgendumGet(ref SingleResult<CDAApp.Server.Models.CdaDB.MeetingAgendum> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CdaDB/MeetingAgenda(MeetingAgendaID={MeetingAgendaID})")]
        public SingleResult<CDAApp.Server.Models.CdaDB.MeetingAgendum> GetMeetingAgendum(long key)
        {
            var items = this.context.MeetingAgenda.Where(i => i.MeetingAgendaID == key);
            var result = SingleResult.Create(items);

            OnMeetingAgendumGet(ref result);

            return result;
        }
        partial void OnMeetingAgendumDeleted(CDAApp.Server.Models.CdaDB.MeetingAgendum item);
        partial void OnAfterMeetingAgendumDeleted(CDAApp.Server.Models.CdaDB.MeetingAgendum item);

        [HttpDelete("/odata/CdaDB/MeetingAgenda(MeetingAgendaID={MeetingAgendaID})")]
        public IActionResult DeleteMeetingAgendum(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.MeetingAgenda
                    .Where(i => i.MeetingAgendaID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.MeetingAgendum>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMeetingAgendumDeleted(item);
                this.context.MeetingAgenda.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMeetingAgendumDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMeetingAgendumUpdated(CDAApp.Server.Models.CdaDB.MeetingAgendum item);
        partial void OnAfterMeetingAgendumUpdated(CDAApp.Server.Models.CdaDB.MeetingAgendum item);

        [HttpPut("/odata/CdaDB/MeetingAgenda(MeetingAgendaID={MeetingAgendaID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMeetingAgendum(long key, [FromBody]CDAApp.Server.Models.CdaDB.MeetingAgendum item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MeetingAgenda
                    .Where(i => i.MeetingAgendaID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.MeetingAgendum>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMeetingAgendumUpdated(item);
                this.context.MeetingAgenda.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MeetingAgenda.Where(i => i.MeetingAgendaID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Meeting");
                this.OnAfterMeetingAgendumUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CdaDB/MeetingAgenda(MeetingAgendaID={MeetingAgendaID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMeetingAgendum(long key, [FromBody]Delta<CDAApp.Server.Models.CdaDB.MeetingAgendum> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MeetingAgenda
                    .Where(i => i.MeetingAgendaID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CDAApp.Server.Models.CdaDB.MeetingAgendum>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnMeetingAgendumUpdated(item);
                this.context.MeetingAgenda.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MeetingAgenda.Where(i => i.MeetingAgendaID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Meeting");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMeetingAgendumCreated(CDAApp.Server.Models.CdaDB.MeetingAgendum item);
        partial void OnAfterMeetingAgendumCreated(CDAApp.Server.Models.CdaDB.MeetingAgendum item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CDAApp.Server.Models.CdaDB.MeetingAgendum item)
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

                this.OnMeetingAgendumCreated(item);
                this.context.MeetingAgenda.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MeetingAgenda.Where(i => i.MeetingAgendaID == item.MeetingAgendaID);

                Request.QueryString = Request.QueryString.Add("$expand", "Meeting");

                this.OnAfterMeetingAgendumCreated(item);

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
