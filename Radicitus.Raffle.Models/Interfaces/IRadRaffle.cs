using System;

namespace Radicitus.Models.Interfaces
{
    public interface IRadRaffle
    {   
        int Id { get; }
        string RaffleName { get; }
        DateTime DateCreatedUtc { get; }
        string WinnerName { get; }
        decimal? AmountWon { get; }
        decimal SquareWorthAmount { get; }
        int? WinningSquare { get; }
        DateTime StartDateUtc { get; }
        DateTime EndDateUtc { get; }
    }
}
