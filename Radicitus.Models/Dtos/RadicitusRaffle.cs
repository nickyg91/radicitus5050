using System;
using System.Collections.Generic;
using Radicitus.Models.Interfaces;

namespace Radicitus.Models.Dtos
{
    public class RadicitusRaffle : IRadRaffle
    {
        public int Id { get; set; }
        public string RaffleName { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string WinnerName { get; set; }
        public decimal? AmountWon { get; set; }
        public decimal SquareWorthAmount { get; set; }
        public int? WinningSquare { get; set; }
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
        public List<RaffleNumber> RaffleNumbers { get; set; }
    }
}
