using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CDAApp.Server.Models.CdaDB
{
    [Table("MeetingAgenda", Schema = "dbo")]
    public partial class MeetingAgendum
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
        public long MeetingAgendaID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long MeetingID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string MeetingAgendaName { get; set; }

        public Meeting Meeting { get; set; }

    }
}