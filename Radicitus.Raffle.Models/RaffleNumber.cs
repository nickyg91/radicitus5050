using System;
using System.Collections.Generic;

namespace Raffle.Models
{
    public class RaffleNumber
    {
        public string Name { get; set; }
        public List<int> Numbers { get; set; }
        public Guid RaffleGuid { get; set; }
    }
}
