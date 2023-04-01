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
    public partial class Members
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

        protected IEnumerable<CDAApp.Server.Models.CdaDB.Member> members;

        protected RadzenDataGrid<CDAApp.Server.Models.CdaDB.Member> grid0;
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
                var result = await CdaDBService.GetMembers(filter: $@"(contains(FirstName,""{search}"") or contains(MiddleName,""{search}"") or contains(LastName,""{search}"") or contains(Email,""{search}"") or contains(PhoneNumber,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "Gender", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                members = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Members" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            member = new CDAApp.Server.Models.CdaDB.Member();
        }

        protected async Task EditRow(CDAApp.Server.Models.CdaDB.Member args)
        {
            isEdit = true;
            member = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CDAApp.Server.Models.CdaDB.Member member)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CdaDBService.DeleteMember(memberId:member.MemberID);

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
                    Detail = $"Unable to delete Member" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await CdaDBService.ExportMembersToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Gender", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "Members");
            }

            if (args == null || args.Value == "xlsx")
            {
                await CdaDBService.ExportMembersToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Gender", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
}, "Members");
            }
        }
        protected bool errorVisible;
        protected CDAApp.Server.Models.CdaDB.Member member;

        protected IEnumerable<CDAApp.Server.Models.CdaDB.Gender> gendersForGenderID;


        protected int gendersForGenderIDCount;
        protected CDAApp.Server.Models.CdaDB.Gender gendersForGenderIDValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task gendersForGenderIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await CdaDBService.GetGenders(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"{args.Filter}", orderby: $"{args.OrderBy}");
                gendersForGenderID = result.Value.AsODataEnumerable();
                gendersForGenderIDCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Gender" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await CdaDBService.UpdateMember(memberId:member.MemberID, member) : await CdaDBService.CreateMember(member);
               //refresh page after create/update
               NavigationManager.NavigateTo("/members",true);

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