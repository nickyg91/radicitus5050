using Radicitus.Models.Interfaces;

namespace Radicitus.Models.Dtos
{
    public class RaffleNumber : IRaffleNumber
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RaffleId { get; set; }
        public int Number { get; set; }
    }
}
