using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using CDAApp.Server.Models.CdaDB;

namespace CDAApp.Server.Data
{
    public partial class CdaDBContext : DbContext
    {
        public CdaDBContext()
        {
        }

        public CdaDBContext(DbContextOptions<CdaDBContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CDAApp.Server.Models.CdaDB.MeetingAgendum>()
              .HasOne(i => i.Meeting)
              .WithMany(i => i.MeetingAgenda)
              .HasForeignKey(i => i.MeetingID)
              .HasPrincipalKey(i => i.MeetingID);

            builder.Entity<CDAApp.Server.Models.CdaDB.MeetingAttendee>()
              .HasOne(i => i.Meeting)
              .WithMany(i => i.MeetingAttendees)
              .HasForeignKey(i => i.MeetingID)
              .HasPrincipalKey(i => i.MeetingID);

            builder.Entity<CDAApp.Server.Models.CdaDB.MeetingAttendee>()
              .HasOne(i => i.Member)
              .WithMany(i => i.MeetingAttendees)
              .HasForeignKey(i => i.MemberID)
              .HasPrincipalKey(i => i.MemberID);

            builder.Entity<CDAApp.Server.Models.CdaDB.MemberContribution>()
              .HasOne(i => i.Member)
              .WithMany(i => i.MemberContributions)
              .HasForeignKey(i => i.MemberID)
              .HasPrincipalKey(i => i.MemberID);

            builder.Entity<CDAApp.Server.Models.CdaDB.Member>()
              .HasOne(i => i.Gender)
              .WithMany(i => i.Members)
              .HasForeignKey(i => i.GenderID)
              .HasPrincipalKey(i => i.GenderID);
            this.OnModelBuilding(builder);
        }

        public DbSet<CDAApp.Server.Models.CdaDB.Gender> Genders { get; set; }

        public DbSet<CDAApp.Server.Models.CdaDB.MeetingAgendum> MeetingAgenda { get; set; }

        public DbSet<CDAApp.Server.Models.CdaDB.MeetingAttendee> MeetingAttendees { get; set; }

        public DbSet<CDAApp.Server.Models.CdaDB.Meeting> Meetings { get; set; }

        public DbSet<CDAApp.Server.Models.CdaDB.MemberContribution> MemberContributions { get; set; }

        public DbSet<CDAApp.Server.Models.CdaDB.Member> Members { get; set; }
    }
}