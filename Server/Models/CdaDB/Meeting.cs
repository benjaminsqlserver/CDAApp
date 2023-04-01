using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CDAApp.Server.Models.CdaDB
{
    [Table("Meetings", Schema = "dbo")]
    public partial class Meeting
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
        public long MeetingID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string MeetingLocation { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime MeetingDate { get; set; }

        public ICollection<MeetingAgendum> MeetingAgenda { get; set; }

        public ICollection<MeetingAttendee> MeetingAttendees { get; set; }

    }
}