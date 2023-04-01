using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using CDAApp.Server.Data;

namespace CDAApp.Server.Controllers
{
    public partial class ExportCdaDBController : ExportController
    {
        private readonly CdaDBContext context;
        private readonly CdaDBService service;

        public ExportCdaDBController(CdaDBContext context, CdaDBService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/CdaDB/genders/csv")]
        [HttpGet("/export/CdaDB/genders/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGendersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetGenders(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/genders/excel")]
        [HttpGet("/export/CdaDB/genders/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGendersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetGenders(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/meetingagenda/csv")]
        [HttpGet("/export/CdaDB/meetingagenda/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMeetingAgendaToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMeetingAgenda(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/meetingagenda/excel")]
        [HttpGet("/export/CdaDB/meetingagenda/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMeetingAgendaToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMeetingAgenda(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/meetingattendees/csv")]
        [HttpGet("/export/CdaDB/meetingattendees/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMeetingAttendeesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMeetingAttendees(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/meetingattendees/excel")]
        [HttpGet("/export/CdaDB/meetingattendees/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMeetingAttendeesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMeetingAttendees(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/meetings/csv")]
        [HttpGet("/export/CdaDB/meetings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMeetingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMeetings(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/meetings/excel")]
        [HttpGet("/export/CdaDB/meetings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMeetingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMeetings(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/membercontributions/csv")]
        [HttpGet("/export/CdaDB/membercontributions/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMemberContributionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMemberContributions(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/membercontributions/excel")]
        [HttpGet("/export/CdaDB/membercontributions/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMemberContributionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMemberContributions(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/members/csv")]
        [HttpGet("/export/CdaDB/members/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMembersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMembers(), Request.Query), fileName);
        }

        [HttpGet("/export/CdaDB/members/excel")]
        [HttpGet("/export/CdaDB/members/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMembersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMembers(), Request.Query), fileName);
        }
    }
}
