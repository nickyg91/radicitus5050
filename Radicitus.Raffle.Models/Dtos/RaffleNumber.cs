using System;
using System.Collections.Generic;

namespace Radicitus.Models.Dtos
{
    public class RaffleNumber
    {
        public string Name { get; set; }
        public List<int> Numbers { get; set; }
        public Guid RaffleGuid { get; set; }
    }
}
