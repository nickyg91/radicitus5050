namespace Radicitus.Models.Interfaces
{
    public interface IRaffleNumber
    {
        int Id { get; }
        string Name { get; }
        int RaffleId { get; }
        int Number { get; }
    }
}
