using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CDAApp.Server.Models.CdaDB
{
    [Table("Members", Schema = "dbo")]
    public partial class Member
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
                get;
                set;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string FirstName { get; set; }

        [ConcurrencyCheck]
        public string MiddleName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string LastName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int GenderID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Email { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string PhoneNumber { get; set; }

        public ICollection<MeetingAttendee> MeetingAttendees { get; set; }

        public ICollection<MemberContribution> MemberContributions { get; set; }

        public Gender Gender { get; set; }

        public string FullName 
        { get
            {
                return FirstName+" "+LastName;
            }
        }

    }
}