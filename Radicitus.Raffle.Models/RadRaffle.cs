using System;
using System.Collections.Generic;

namespace Raffle.Models
{
    public class RadRaffle
    {
        public List<RaffleNumber> RaffleNumbers { get; set; }
        public Guid RaffleGuid { get; set; }
        public string RaffleName { get; set; }
        public DateTime DateCreated { get; set; }
        public string WinnerName { get; set; }
        public decimal AmountWon { get; set; }
        public decimal SquareWorthAmount { get; set; }
        public int WinningSquare { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
