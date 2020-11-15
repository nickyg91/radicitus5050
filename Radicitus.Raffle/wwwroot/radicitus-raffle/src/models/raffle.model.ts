import RaffleNumber from "@/models/raffle-number.model";

export default class RadRaffle {
  public RaffleName: string;
  public Id: number;
  public DateCreatedUtc: Date;
  public StartDateUtc: Date;
  public EndDateUtc: Date;
  public StartDate: Date;
  public EndDate: Date;
  public WinnerName?: string;
  public AmountWon?: number;
  public SquareWorthAmount: number;
  public WinningSquare: number;
  public RaffleNumbers: RaffleNumber[];
}
