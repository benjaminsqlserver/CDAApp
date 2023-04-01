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
    public partial class MeetingAgenda
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

        protected IEnumerable<CDAApp.Server.Models.CdaDB.MeetingAgendum> meetingAgenda;

        protected RadzenDataGrid<CDAApp.Server.Models.CdaDB.MeetingAgendum> grid0;
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
                var result = await CdaDBService.GetMeetingAgenda(filter: $@"(contains(MeetingAgendaName,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "Meeting", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                meetingAgenda = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load MeetingAgenda" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            meetingAgendum = new CDAApp.Server.Models.CdaDB.MeetingAgendum();
        }

        protected async Task EditRow(CDAApp.Server.Models.CdaDB.MeetingAgendum args)
        {
            isEdit = true;
            meetingAgendum = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CDAApp.Server.Models.CdaDB.MeetingAgendum meetingAgendum)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CdaDBService.DeleteMeetingAgendum(meetingAgendaId:meetingAgendum.MeetingAgendaID);

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
                    Detail = $"Unable to delete MeetingAgendum" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await CdaDBService.ExportMeetingAgendaToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Meeting", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "MeetingAgenda");
            }

            if (args == null || args.Value == "xlsx")
            {
                await CdaDBService.ExportMeetingAgendaToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Meeting", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "MeetingAgenda");
            }
        }
        protected bool errorVisible;
        protected CDAApp.Server.Models.CdaDB.MeetingAgendum meetingAgendum;

        protected IEnumerable<CDAApp.Server.Models.CdaDB.Meeting> meetingsForMeetingID;


        protected int meetingsForMeetingIDCount;
        protected CDAApp.Server.Models.CdaDB.Meeting meetingsForMeetingIDValue;

        [Inject]
        protected SecurityService Security { get; set; }
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
        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await CdaDBService.UpdateMeetingAgendum(meetingAgendaId:meetingAgendum.MeetingAgendaID, meetingAgendum) : await CdaDBService.CreateMeetingAgendum(meetingAgendum);

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