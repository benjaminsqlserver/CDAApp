using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CDAApp.Server.Models.CdaDB
{
    [Table("MemberContributions", Schema = "dbo")]
    public partial class MemberContribution
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
        public long ContributionID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime ContributionDate { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int MemberID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Purpose { get; set; }

        [Required]
        [ConcurrencyCheck]
        public decimal Amount { get; set; }

        public Member Member { get; set; }

    }
}