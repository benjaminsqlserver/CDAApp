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
    public partial class MemberContributions
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

        protected IEnumerable<CDAApp.Server.Models.CdaDB.MemberContribution> memberContributions;

        protected RadzenDataGrid<CDAApp.Server.Models.CdaDB.MemberContribution> grid0;
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
                var result = await CdaDBService.GetMemberContributions(filter: $@"(contains(Purpose,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "Member", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                memberContributions = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load MemberContributions" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            memberContribution = new CDAApp.Server.Models.CdaDB.MemberContribution();
            //set today's date as default
            memberContribution.ContributionDate= DateTime.Now;
        }

        protected async Task EditRow(CDAApp.Server.Models.CdaDB.MemberContribution args)
        {
            isEdit = true;
            memberContribution = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CDAApp.Server.Models.CdaDB.MemberContribution memberContribution)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CdaDBService.DeleteMemberContribution(contributionId:memberContribution.ContributionID);

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
                    Detail = $"Unable to delete MemberContribution" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await CdaDBService.ExportMemberContributionsToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Member", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "MemberContributions");
            }

            if (args == null || args.Value == "xlsx")
            {
                await CdaDBService.ExportMemberContributionsToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Member", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "MemberContributions");
            }
        }
        protected bool errorVisible;
        protected CDAApp.Server.Models.CdaDB.MemberContribution memberContribution;

        protected IEnumerable<CDAApp.Server.Models.CdaDB.Member> membersForMemberID;


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

                dynamic result = isEdit ? await CdaDBService.UpdateMemberContribution(contributionId:memberContribution.ContributionID, memberContribution) : await CdaDBService.CreateMemberContribution(memberContribution);
                NavigationManager.NavigateTo("/member-contributions", true);

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