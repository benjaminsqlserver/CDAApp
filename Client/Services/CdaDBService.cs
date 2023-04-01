
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace CDAApp.Client
{
    public partial class CdaDBService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public CdaDBService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/CdaDB/");
        }


        public async System.Threading.Tasks.Task ExportGendersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/genders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/genders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportGendersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/genders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/genders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetGenders(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.Gender>> GetGenders(Query query)
        {
            return await GetGenders(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.Gender>> GetGenders(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Genders");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetGenders(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.Gender>>(response);
        }

        partial void OnCreateGender(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.Gender> CreateGender(CDAApp.Server.Models.CdaDB.Gender gender = default(CDAApp.Server.Models.CdaDB.Gender))
        {
            var uri = new Uri(baseUri, $"Genders");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(gender), Encoding.UTF8, "application/json");

            OnCreateGender(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.Gender>(response);
        }

        partial void OnDeleteGender(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteGender(int genderId = default(int))
        {
            var uri = new Uri(baseUri, $"Genders({genderId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteGender(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetGenderByGenderId(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.Gender> GetGenderByGenderId(string expand = default(string), int genderId = default(int))
        {
            var uri = new Uri(baseUri, $"Genders({genderId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetGenderByGenderId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.Gender>(response);
        }

        partial void OnUpdateGender(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateGender(int genderId = default(int), CDAApp.Server.Models.CdaDB.Gender gender = default(CDAApp.Server.Models.CdaDB.Gender))
        {
            var uri = new Uri(baseUri, $"Genders({genderId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", gender.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(gender), Encoding.UTF8, "application/json");

            OnUpdateGender(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMeetingAgendaToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetingagenda/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetingagenda/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMeetingAgendaToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetingagenda/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetingagenda/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMeetingAgenda(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.MeetingAgendum>> GetMeetingAgenda(Query query)
        {
            return await GetMeetingAgenda(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.MeetingAgendum>> GetMeetingAgenda(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"MeetingAgenda");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMeetingAgenda(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.MeetingAgendum>>(response);
        }

        partial void OnCreateMeetingAgendum(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAgendum> CreateMeetingAgendum(CDAApp.Server.Models.CdaDB.MeetingAgendum meetingAgendum = default(CDAApp.Server.Models.CdaDB.MeetingAgendum))
        {
            var uri = new Uri(baseUri, $"MeetingAgenda");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(meetingAgendum), Encoding.UTF8, "application/json");

            OnCreateMeetingAgendum(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.MeetingAgendum>(response);
        }

        partial void OnDeleteMeetingAgendum(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMeetingAgendum(long meetingAgendaId = default(long))
        {
            var uri = new Uri(baseUri, $"MeetingAgenda({meetingAgendaId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMeetingAgendum(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMeetingAgendumByMeetingAgendaId(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAgendum> GetMeetingAgendumByMeetingAgendaId(string expand = default(string), long meetingAgendaId = default(long))
        {
            var uri = new Uri(baseUri, $"MeetingAgenda({meetingAgendaId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMeetingAgendumByMeetingAgendaId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.MeetingAgendum>(response);
        }

        partial void OnUpdateMeetingAgendum(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMeetingAgendum(long meetingAgendaId = default(long), CDAApp.Server.Models.CdaDB.MeetingAgendum meetingAgendum = default(CDAApp.Server.Models.CdaDB.MeetingAgendum))
        {
            var uri = new Uri(baseUri, $"MeetingAgenda({meetingAgendaId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", meetingAgendum.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(meetingAgendum), Encoding.UTF8, "application/json");

            OnUpdateMeetingAgendum(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMeetingAttendeesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetingattendees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetingattendees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMeetingAttendeesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetingattendees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetingattendees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMeetingAttendees(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.MeetingAttendee>> GetMeetingAttendees(Query query)
        {
            return await GetMeetingAttendees(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.MeetingAttendee>> GetMeetingAttendees(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"MeetingAttendees");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMeetingAttendees(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.MeetingAttendee>>(response);
        }

        partial void OnCreateMeetingAttendee(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAttendee> CreateMeetingAttendee(CDAApp.Server.Models.CdaDB.MeetingAttendee meetingAttendee = default(CDAApp.Server.Models.CdaDB.MeetingAttendee))
        {
            var uri = new Uri(baseUri, $"MeetingAttendees");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(meetingAttendee), Encoding.UTF8, "application/json");

            OnCreateMeetingAttendee(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.MeetingAttendee>(response);
        }

        partial void OnDeleteMeetingAttendee(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMeetingAttendee(long attendeeId = default(long))
        {
            var uri = new Uri(baseUri, $"MeetingAttendees({attendeeId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMeetingAttendee(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMeetingAttendeeByAttendeeId(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAttendee> GetMeetingAttendeeByAttendeeId(string expand = default(string), long attendeeId = default(long))
        {
            var uri = new Uri(baseUri, $"MeetingAttendees({attendeeId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMeetingAttendeeByAttendeeId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.MeetingAttendee>(response);
        }

        partial void OnUpdateMeetingAttendee(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMeetingAttendee(long attendeeId = default(long), CDAApp.Server.Models.CdaDB.MeetingAttendee meetingAttendee = default(CDAApp.Server.Models.CdaDB.MeetingAttendee))
        {
            var uri = new Uri(baseUri, $"MeetingAttendees({attendeeId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", meetingAttendee.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(meetingAttendee), Encoding.UTF8, "application/json");

            OnUpdateMeetingAttendee(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMeetingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMeetingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMeetings(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.Meeting>> GetMeetings(Query query)
        {
            return await GetMeetings(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.Meeting>> GetMeetings(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Meetings");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMeetings(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.Meeting>>(response);
        }

        partial void OnCreateMeeting(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.Meeting> CreateMeeting(CDAApp.Server.Models.CdaDB.Meeting meeting = default(CDAApp.Server.Models.CdaDB.Meeting))
        {
            var uri = new Uri(baseUri, $"Meetings");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(meeting), Encoding.UTF8, "application/json");

            OnCreateMeeting(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.Meeting>(response);
        }

        partial void OnDeleteMeeting(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMeeting(long meetingId = default(long))
        {
            var uri = new Uri(baseUri, $"Meetings({meetingId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMeeting(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMeetingByMeetingId(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.Meeting> GetMeetingByMeetingId(string expand = default(string), long meetingId = default(long))
        {
            var uri = new Uri(baseUri, $"Meetings({meetingId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMeetingByMeetingId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.Meeting>(response);
        }

        partial void OnUpdateMeeting(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMeeting(long meetingId = default(long), CDAApp.Server.Models.CdaDB.Meeting meeting = default(CDAApp.Server.Models.CdaDB.Meeting))
        {
            var uri = new Uri(baseUri, $"Meetings({meetingId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", meeting.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(meeting), Encoding.UTF8, "application/json");

            OnUpdateMeeting(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMemberContributionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/membercontributions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/membercontributions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMemberContributionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/membercontributions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/membercontributions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMemberContributions(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.MemberContribution>> GetMemberContributions(Query query)
        {
            return await GetMemberContributions(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.MemberContribution>> GetMemberContributions(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"MemberContributions");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMemberContributions(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.MemberContribution>>(response);
        }

        partial void OnCreateMemberContribution(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.MemberContribution> CreateMemberContribution(CDAApp.Server.Models.CdaDB.MemberContribution memberContribution = default(CDAApp.Server.Models.CdaDB.MemberContribution))
        {
            var uri = new Uri(baseUri, $"MemberContributions");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(memberContribution), Encoding.UTF8, "application/json");

            OnCreateMemberContribution(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.MemberContribution>(response);
        }

        partial void OnDeleteMemberContribution(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMemberContribution(long contributionId = default(long))
        {
            var uri = new Uri(baseUri, $"MemberContributions({contributionId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMemberContribution(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMemberContributionByContributionId(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.MemberContribution> GetMemberContributionByContributionId(string expand = default(string), long contributionId = default(long))
        {
            var uri = new Uri(baseUri, $"MemberContributions({contributionId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMemberContributionByContributionId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.MemberContribution>(response);
        }

        partial void OnUpdateMemberContribution(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMemberContribution(long contributionId = default(long), CDAApp.Server.Models.CdaDB.MemberContribution memberContribution = default(CDAApp.Server.Models.CdaDB.MemberContribution))
        {
            var uri = new Uri(baseUri, $"MemberContributions({contributionId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", memberContribution.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(memberContribution), Encoding.UTF8, "application/json");

            OnUpdateMemberContribution(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMembersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/members/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/members/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMembersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/members/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/members/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMembers(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.Member>> GetMembers(Query query)
        {
            return await GetMembers(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.Member>> GetMembers(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Members");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMembers(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CDAApp.Server.Models.CdaDB.Member>>(response);
        }

        partial void OnCreateMember(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.Member> CreateMember(CDAApp.Server.Models.CdaDB.Member member = default(CDAApp.Server.Models.CdaDB.Member))
        {
            var uri = new Uri(baseUri, $"Members");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(member), Encoding.UTF8, "application/json");

            OnCreateMember(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.Member>(response);
        }

        partial void OnDeleteMember(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMember(int memberId = default(int))
        {
            var uri = new Uri(baseUri, $"Members({memberId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMember(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMemberByMemberId(HttpRequestMessage requestMessage);

        public async Task<CDAApp.Server.Models.CdaDB.Member> GetMemberByMemberId(string expand = default(string), int memberId = default(int))
        {
            var uri = new Uri(baseUri, $"Members({memberId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMemberByMemberId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CDAApp.Server.Models.CdaDB.Member>(response);
        }

        partial void OnUpdateMember(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMember(int memberId = default(int), CDAApp.Server.Models.CdaDB.Member member = default(CDAApp.Server.Models.CdaDB.Member))
        {
            var uri = new Uri(baseUri, $"Members({memberId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", member.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(member), Encoding.UTF8, "application/json");

            OnUpdateMember(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}