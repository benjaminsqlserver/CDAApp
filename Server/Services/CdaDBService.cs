using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using CDAApp.Server.Data;

namespace CDAApp.Server
{
    public partial class CdaDBService
    {
        CdaDBContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly CdaDBContext context;
        private readonly NavigationManager navigationManager;

        public CdaDBService(CdaDBContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportGendersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/genders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/genders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportGendersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/genders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/genders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGendersRead(ref IQueryable<CDAApp.Server.Models.CdaDB.Gender> items);

        public async Task<IQueryable<CDAApp.Server.Models.CdaDB.Gender>> GetGenders(Query query = null)
        {
            var items = Context.Genders.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnGendersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnGenderGet(CDAApp.Server.Models.CdaDB.Gender item);

        public async Task<CDAApp.Server.Models.CdaDB.Gender> GetGenderByGenderId(int genderid)
        {
            var items = Context.Genders
                              .AsNoTracking()
                              .Where(i => i.GenderID == genderid);

  
            var itemToReturn = items.FirstOrDefault();

            OnGenderGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnGenderCreated(CDAApp.Server.Models.CdaDB.Gender item);
        partial void OnAfterGenderCreated(CDAApp.Server.Models.CdaDB.Gender item);

        public async Task<CDAApp.Server.Models.CdaDB.Gender> CreateGender(CDAApp.Server.Models.CdaDB.Gender gender)
        {
            OnGenderCreated(gender);

            var existingItem = Context.Genders
                              .Where(i => i.GenderID == gender.GenderID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Genders.Add(gender);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(gender).State = EntityState.Detached;
                throw;
            }

            OnAfterGenderCreated(gender);

            return gender;
        }

        public async Task<CDAApp.Server.Models.CdaDB.Gender> CancelGenderChanges(CDAApp.Server.Models.CdaDB.Gender item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnGenderUpdated(CDAApp.Server.Models.CdaDB.Gender item);
        partial void OnAfterGenderUpdated(CDAApp.Server.Models.CdaDB.Gender item);

        public async Task<CDAApp.Server.Models.CdaDB.Gender> UpdateGender(int genderid, CDAApp.Server.Models.CdaDB.Gender gender)
        {
            OnGenderUpdated(gender);

            var itemToUpdate = Context.Genders
                              .Where(i => i.GenderID == gender.GenderID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(gender);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterGenderUpdated(gender);

            return gender;
        }

        partial void OnGenderDeleted(CDAApp.Server.Models.CdaDB.Gender item);
        partial void OnAfterGenderDeleted(CDAApp.Server.Models.CdaDB.Gender item);

        public async Task<CDAApp.Server.Models.CdaDB.Gender> DeleteGender(int genderid)
        {
            var itemToDelete = Context.Genders
                              .Where(i => i.GenderID == genderid)
                              .Include(i => i.Members)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnGenderDeleted(itemToDelete);


            Context.Genders.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterGenderDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMeetingAgendaToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetingagenda/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetingagenda/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMeetingAgendaToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetingagenda/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetingagenda/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMeetingAgendaRead(ref IQueryable<CDAApp.Server.Models.CdaDB.MeetingAgendum> items);

        public async Task<IQueryable<CDAApp.Server.Models.CdaDB.MeetingAgendum>> GetMeetingAgenda(Query query = null)
        {
            var items = Context.MeetingAgenda.AsQueryable();

            items = items.Include(i => i.Meeting);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnMeetingAgendaRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMeetingAgendumGet(CDAApp.Server.Models.CdaDB.MeetingAgendum item);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAgendum> GetMeetingAgendumByMeetingAgendaId(long meetingagendaid)
        {
            var items = Context.MeetingAgenda
                              .AsNoTracking()
                              .Where(i => i.MeetingAgendaID == meetingagendaid);

            items = items.Include(i => i.Meeting);
  
            var itemToReturn = items.FirstOrDefault();

            OnMeetingAgendumGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMeetingAgendumCreated(CDAApp.Server.Models.CdaDB.MeetingAgendum item);
        partial void OnAfterMeetingAgendumCreated(CDAApp.Server.Models.CdaDB.MeetingAgendum item);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAgendum> CreateMeetingAgendum(CDAApp.Server.Models.CdaDB.MeetingAgendum meetingagendum)
        {
            OnMeetingAgendumCreated(meetingagendum);

            var existingItem = Context.MeetingAgenda
                              .Where(i => i.MeetingAgendaID == meetingagendum.MeetingAgendaID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.MeetingAgenda.Add(meetingagendum);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(meetingagendum).State = EntityState.Detached;
                throw;
            }

            OnAfterMeetingAgendumCreated(meetingagendum);

            return meetingagendum;
        }

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAgendum> CancelMeetingAgendumChanges(CDAApp.Server.Models.CdaDB.MeetingAgendum item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMeetingAgendumUpdated(CDAApp.Server.Models.CdaDB.MeetingAgendum item);
        partial void OnAfterMeetingAgendumUpdated(CDAApp.Server.Models.CdaDB.MeetingAgendum item);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAgendum> UpdateMeetingAgendum(long meetingagendaid, CDAApp.Server.Models.CdaDB.MeetingAgendum meetingagendum)
        {
            OnMeetingAgendumUpdated(meetingagendum);

            var itemToUpdate = Context.MeetingAgenda
                              .Where(i => i.MeetingAgendaID == meetingagendum.MeetingAgendaID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(meetingagendum);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMeetingAgendumUpdated(meetingagendum);

            return meetingagendum;
        }

        partial void OnMeetingAgendumDeleted(CDAApp.Server.Models.CdaDB.MeetingAgendum item);
        partial void OnAfterMeetingAgendumDeleted(CDAApp.Server.Models.CdaDB.MeetingAgendum item);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAgendum> DeleteMeetingAgendum(long meetingagendaid)
        {
            var itemToDelete = Context.MeetingAgenda
                              .Where(i => i.MeetingAgendaID == meetingagendaid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMeetingAgendumDeleted(itemToDelete);


            Context.MeetingAgenda.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMeetingAgendumDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMeetingAttendeesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetingattendees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetingattendees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMeetingAttendeesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetingattendees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetingattendees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMeetingAttendeesRead(ref IQueryable<CDAApp.Server.Models.CdaDB.MeetingAttendee> items);

        public async Task<IQueryable<CDAApp.Server.Models.CdaDB.MeetingAttendee>> GetMeetingAttendees(Query query = null)
        {
            var items = Context.MeetingAttendees.AsQueryable();

            items = items.Include(i => i.Meeting);
            items = items.Include(i => i.Member);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnMeetingAttendeesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMeetingAttendeeGet(CDAApp.Server.Models.CdaDB.MeetingAttendee item);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAttendee> GetMeetingAttendeeByAttendeeId(long attendeeid)
        {
            var items = Context.MeetingAttendees
                              .AsNoTracking()
                              .Where(i => i.AttendeeID == attendeeid);

            items = items.Include(i => i.Meeting);
            items = items.Include(i => i.Member);
  
            var itemToReturn = items.FirstOrDefault();

            OnMeetingAttendeeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMeetingAttendeeCreated(CDAApp.Server.Models.CdaDB.MeetingAttendee item);
        partial void OnAfterMeetingAttendeeCreated(CDAApp.Server.Models.CdaDB.MeetingAttendee item);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAttendee> CreateMeetingAttendee(CDAApp.Server.Models.CdaDB.MeetingAttendee meetingattendee)
        {
            OnMeetingAttendeeCreated(meetingattendee);

            var existingItem = Context.MeetingAttendees
                              .Where(i => i.AttendeeID == meetingattendee.AttendeeID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.MeetingAttendees.Add(meetingattendee);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(meetingattendee).State = EntityState.Detached;
                throw;
            }

            OnAfterMeetingAttendeeCreated(meetingattendee);

            return meetingattendee;
        }

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAttendee> CancelMeetingAttendeeChanges(CDAApp.Server.Models.CdaDB.MeetingAttendee item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMeetingAttendeeUpdated(CDAApp.Server.Models.CdaDB.MeetingAttendee item);
        partial void OnAfterMeetingAttendeeUpdated(CDAApp.Server.Models.CdaDB.MeetingAttendee item);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAttendee> UpdateMeetingAttendee(long attendeeid, CDAApp.Server.Models.CdaDB.MeetingAttendee meetingattendee)
        {
            OnMeetingAttendeeUpdated(meetingattendee);

            var itemToUpdate = Context.MeetingAttendees
                              .Where(i => i.AttendeeID == meetingattendee.AttendeeID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(meetingattendee);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMeetingAttendeeUpdated(meetingattendee);

            return meetingattendee;
        }

        partial void OnMeetingAttendeeDeleted(CDAApp.Server.Models.CdaDB.MeetingAttendee item);
        partial void OnAfterMeetingAttendeeDeleted(CDAApp.Server.Models.CdaDB.MeetingAttendee item);

        public async Task<CDAApp.Server.Models.CdaDB.MeetingAttendee> DeleteMeetingAttendee(long attendeeid)
        {
            var itemToDelete = Context.MeetingAttendees
                              .Where(i => i.AttendeeID == attendeeid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMeetingAttendeeDeleted(itemToDelete);


            Context.MeetingAttendees.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMeetingAttendeeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMeetingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMeetingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/meetings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/meetings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMeetingsRead(ref IQueryable<CDAApp.Server.Models.CdaDB.Meeting> items);

        public async Task<IQueryable<CDAApp.Server.Models.CdaDB.Meeting>> GetMeetings(Query query = null)
        {
            var items = Context.Meetings.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnMeetingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMeetingGet(CDAApp.Server.Models.CdaDB.Meeting item);

        public async Task<CDAApp.Server.Models.CdaDB.Meeting> GetMeetingByMeetingId(long meetingid)
        {
            var items = Context.Meetings
                              .AsNoTracking()
                              .Where(i => i.MeetingID == meetingid);

  
            var itemToReturn = items.FirstOrDefault();

            OnMeetingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMeetingCreated(CDAApp.Server.Models.CdaDB.Meeting item);
        partial void OnAfterMeetingCreated(CDAApp.Server.Models.CdaDB.Meeting item);

        public async Task<CDAApp.Server.Models.CdaDB.Meeting> CreateMeeting(CDAApp.Server.Models.CdaDB.Meeting meeting)
        {
            OnMeetingCreated(meeting);

            var existingItem = Context.Meetings
                              .Where(i => i.MeetingID == meeting.MeetingID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Meetings.Add(meeting);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(meeting).State = EntityState.Detached;
                throw;
            }

            OnAfterMeetingCreated(meeting);

            return meeting;
        }

        public async Task<CDAApp.Server.Models.CdaDB.Meeting> CancelMeetingChanges(CDAApp.Server.Models.CdaDB.Meeting item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMeetingUpdated(CDAApp.Server.Models.CdaDB.Meeting item);
        partial void OnAfterMeetingUpdated(CDAApp.Server.Models.CdaDB.Meeting item);

        public async Task<CDAApp.Server.Models.CdaDB.Meeting> UpdateMeeting(long meetingid, CDAApp.Server.Models.CdaDB.Meeting meeting)
        {
            OnMeetingUpdated(meeting);

            var itemToUpdate = Context.Meetings
                              .Where(i => i.MeetingID == meeting.MeetingID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(meeting);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMeetingUpdated(meeting);

            return meeting;
        }

        partial void OnMeetingDeleted(CDAApp.Server.Models.CdaDB.Meeting item);
        partial void OnAfterMeetingDeleted(CDAApp.Server.Models.CdaDB.Meeting item);

        public async Task<CDAApp.Server.Models.CdaDB.Meeting> DeleteMeeting(long meetingid)
        {
            var itemToDelete = Context.Meetings
                              .Where(i => i.MeetingID == meetingid)
                              .Include(i => i.MeetingAgenda)
                              .Include(i => i.MeetingAttendees)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMeetingDeleted(itemToDelete);


            Context.Meetings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMeetingDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMemberContributionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/membercontributions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/membercontributions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMemberContributionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/membercontributions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/membercontributions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMemberContributionsRead(ref IQueryable<CDAApp.Server.Models.CdaDB.MemberContribution> items);

        public async Task<IQueryable<CDAApp.Server.Models.CdaDB.MemberContribution>> GetMemberContributions(Query query = null)
        {
            var items = Context.MemberContributions.AsQueryable();

            items = items.Include(i => i.Member);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnMemberContributionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMemberContributionGet(CDAApp.Server.Models.CdaDB.MemberContribution item);

        public async Task<CDAApp.Server.Models.CdaDB.MemberContribution> GetMemberContributionByContributionId(long contributionid)
        {
            var items = Context.MemberContributions
                              .AsNoTracking()
                              .Where(i => i.ContributionID == contributionid);

            items = items.Include(i => i.Member);
  
            var itemToReturn = items.FirstOrDefault();

            OnMemberContributionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMemberContributionCreated(CDAApp.Server.Models.CdaDB.MemberContribution item);
        partial void OnAfterMemberContributionCreated(CDAApp.Server.Models.CdaDB.MemberContribution item);

        public async Task<CDAApp.Server.Models.CdaDB.MemberContribution> CreateMemberContribution(CDAApp.Server.Models.CdaDB.MemberContribution membercontribution)
        {
            OnMemberContributionCreated(membercontribution);

            var existingItem = Context.MemberContributions
                              .Where(i => i.ContributionID == membercontribution.ContributionID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.MemberContributions.Add(membercontribution);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(membercontribution).State = EntityState.Detached;
                throw;
            }

            OnAfterMemberContributionCreated(membercontribution);

            return membercontribution;
        }

        public async Task<CDAApp.Server.Models.CdaDB.MemberContribution> CancelMemberContributionChanges(CDAApp.Server.Models.CdaDB.MemberContribution item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMemberContributionUpdated(CDAApp.Server.Models.CdaDB.MemberContribution item);
        partial void OnAfterMemberContributionUpdated(CDAApp.Server.Models.CdaDB.MemberContribution item);

        public async Task<CDAApp.Server.Models.CdaDB.MemberContribution> UpdateMemberContribution(long contributionid, CDAApp.Server.Models.CdaDB.MemberContribution membercontribution)
        {
            OnMemberContributionUpdated(membercontribution);

            var itemToUpdate = Context.MemberContributions
                              .Where(i => i.ContributionID == membercontribution.ContributionID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(membercontribution);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMemberContributionUpdated(membercontribution);

            return membercontribution;
        }

        partial void OnMemberContributionDeleted(CDAApp.Server.Models.CdaDB.MemberContribution item);
        partial void OnAfterMemberContributionDeleted(CDAApp.Server.Models.CdaDB.MemberContribution item);

        public async Task<CDAApp.Server.Models.CdaDB.MemberContribution> DeleteMemberContribution(long contributionid)
        {
            var itemToDelete = Context.MemberContributions
                              .Where(i => i.ContributionID == contributionid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMemberContributionDeleted(itemToDelete);


            Context.MemberContributions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMemberContributionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMembersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/members/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/members/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMembersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cdadb/members/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cdadb/members/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMembersRead(ref IQueryable<CDAApp.Server.Models.CdaDB.Member> items);

        public async Task<IQueryable<CDAApp.Server.Models.CdaDB.Member>> GetMembers(Query query = null)
        {
            var items = Context.Members.AsQueryable();

            items = items.Include(i => i.Gender);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnMembersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMemberGet(CDAApp.Server.Models.CdaDB.Member item);

        public async Task<CDAApp.Server.Models.CdaDB.Member> GetMemberByMemberId(int memberid)
        {
            var items = Context.Members
                              .AsNoTracking()
                              .Where(i => i.MemberID == memberid);

            items = items.Include(i => i.Gender);
  
            var itemToReturn = items.FirstOrDefault();

            OnMemberGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMemberCreated(CDAApp.Server.Models.CdaDB.Member item);
        partial void OnAfterMemberCreated(CDAApp.Server.Models.CdaDB.Member item);

        public async Task<CDAApp.Server.Models.CdaDB.Member> CreateMember(CDAApp.Server.Models.CdaDB.Member member)
        {
            OnMemberCreated(member);

            var existingItem = Context.Members
                              .Where(i => i.MemberID == member.MemberID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Members.Add(member);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(member).State = EntityState.Detached;
                throw;
            }

            OnAfterMemberCreated(member);

            return member;
        }

        public async Task<CDAApp.Server.Models.CdaDB.Member> CancelMemberChanges(CDAApp.Server.Models.CdaDB.Member item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMemberUpdated(CDAApp.Server.Models.CdaDB.Member item);
        partial void OnAfterMemberUpdated(CDAApp.Server.Models.CdaDB.Member item);

        public async Task<CDAApp.Server.Models.CdaDB.Member> UpdateMember(int memberid, CDAApp.Server.Models.CdaDB.Member member)
        {
            OnMemberUpdated(member);

            var itemToUpdate = Context.Members
                              .Where(i => i.MemberID == member.MemberID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(member);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMemberUpdated(member);

            return member;
        }

        partial void OnMemberDeleted(CDAApp.Server.Models.CdaDB.Member item);
        partial void OnAfterMemberDeleted(CDAApp.Server.Models.CdaDB.Member item);

        public async Task<CDAApp.Server.Models.CdaDB.Member> DeleteMember(int memberid)
        {
            var itemToDelete = Context.Members
                              .Where(i => i.MemberID == memberid)
                              .Include(i => i.MeetingAttendees)
                              .Include(i => i.MemberContributions)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMemberDeleted(itemToDelete);


            Context.Members.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMemberDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}