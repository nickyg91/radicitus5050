using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Radicitus.Models.Interfaces;

namespace Radicitus.Data.Contexts.Raffles.Entities
{
    [Table("rad_raffle")]
    public class RadRaffle : IRadRaffle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("id")]
        public int Id { get; set; }
        [Column("raffle_name"), StringLength(128)]
        public string RaffleName { get; set; }
        [Column("date_created_utc")]
        public DateTime DateCreatedUtc { get; set; }
        [Column("winner_name"), StringLength(128)]
        public string WinnerName { get; set; }
        [Column("amount_won")]
        public decimal? AmountWon { get; set; }
        [Column("square_worth_amount")]
        public decimal SquareWorthAmount { get; set; }
        [Column("winning_square")]
        public int? WinningSquare { get; set; }
        [Column("start_date_utc")]
        public DateTime StartDateUtc { get; set; }
        [Column("end_date_utc")]
        public DateTime EndDateUtc { get; set; }
        [ForeignKey("Id")]
        public ICollection<RaffleNumber> RaffleNumbers { get; set; }
    }
}
