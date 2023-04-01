using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CDAApp.Client.Pages
{
    public partial class MeetingAttendees
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public CdaDBService CdaDBService { get; set; }

        protected IEnumerable<CDAApp.Server.Models.CdaDB.MeetingAttendee> meetingAttendees;

        protected RadzenDataGrid<CDAApp.Server.Models.CdaDB.MeetingAttendee> grid0;
        protected int count;
        protected bool isEdit = true;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await CdaDBService.GetMeetingAttendees(filter: $@"(contains(""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "Meeting,Member", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                meetingAttendees = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load MeetingAttendees" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            meetingAttendee = new CDAApp.Server.Models.CdaDB.MeetingAttendee();
        }

        protected async Task EditRow(CDAApp.Server.Models.CdaDB.MeetingAttendee args)
        {
            isEdit = true;
            meetingAttendee = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CDAApp.Server.Models.CdaDB.MeetingAttendee meetingAttendee)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CdaDBService.DeleteMeetingAttendee(attendeeId:meetingAttendee.AttendeeID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete MeetingAttendee" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await CdaDBService.ExportMeetingAttendeesToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Meeting,Member", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "MeetingAttendees");
            }

            if (args == null || args.Value == "xlsx")
            {
                await CdaDBService.ExportMeetingAttendeesToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Meeting,Member", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "MeetingAttendees");
            }
        }
        protected bool errorVisible;
        protected CDAApp.Server.Models.CdaDB.MeetingAttendee meetingAttendee;

        protected IEnumerable<CDAApp.Server.Models.CdaDB.Meeting> meetingsForMeetingID;

        protected IEnumerable<CDAApp.Server.Models.CdaDB.Member> membersForMemberID;


        protected int meetingsForMeetingIDCount;
        protected CDAApp.Server.Models.CdaDB.Meeting meetingsForMeetingIDValue;
        protected async Task meetingsForMeetingIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await CdaDBService.GetMeetings(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"{args.Filter}", orderby: $"{args.OrderBy}");
                meetingsForMeetingID = result.Value.AsODataEnumerable();
                meetingsForMeetingIDCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Meeting" });
            }
        }

        protected int membersForMemberIDCount;
        protected CDAApp.Server.Models.CdaDB.Member membersForMemberIDValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task membersForMemberIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await CdaDBService.GetMembers(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"{args.Filter}", orderby: $"{args.OrderBy}");
                membersForMemberID = result.Value.AsODataEnumerable();
                membersForMemberIDCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Member" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await CdaDBService.UpdateMeetingAttendee(attendeeId:meetingAttendee.AttendeeID, meetingAttendee) : await CdaDBService.CreateMeetingAttendee(meetingAttendee);

            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {

        }
    }
}