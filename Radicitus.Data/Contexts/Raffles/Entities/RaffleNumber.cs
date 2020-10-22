using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Radicitus.Models.Interfaces;

namespace Radicitus.Data.Contexts.Raffles.Entities
{
    [Table("raffle_number")]
    public class RaffleNumber : IRaffleNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("id")]
        public int Id { get; set; }
        [Column("name"), StringLength(128)]
        public string Name { get; set; }
        [Column("raffle_id")]
        public int RaffleId { get; set; }
        [Column("number")]
        public int Number { get; set; }
        [ForeignKey("RaffleId")]
        public RadRaffle Raffle { get; set; }
    }
}
