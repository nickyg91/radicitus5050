import axios, { AxiosResponse } from "axios";
import RadRaffle from "@/models/raffle.model";
import RaffleNumberSelection from "@/models/raffle-number-selection.model";
export default class RadRaffleService {
  public url: any = process.env.VUE_APP_API_URL;
  private headers = {
    "Access-Control-Allow-Headers": "x-requested-with"
  };
  public async createRaffle(
    raffle: RadRaffle
  ): Promise<AxiosResponse<RadRaffle>> {
    return await axios.post<RadRaffle>(
      `${this.url}/api/raffle/create`,
      raffle,
      {
        headers: this.headers
      }
    );
  }

  public async getRaffleNumbersForUser(
    id: number,
    username: string
  ): Promise<AxiosResponse<number[]>> {
    return await axios.get(`/api/raffle/${id}/${username}/numbers`, {
      headers: this.headers
    });
  }

  public async getNumbersForRaffle(
    id: number
  ): Promise<AxiosResponse<RaffleNumberSelection[]>> {
    return await axios.get(`/api/raffle/numbers/${id}`, {
      headers: this.headers
    });
  }

  public async getRaffleById(id: number): Promise<AxiosResponse<RadRaffle>> {
    return await axios.get(`/api/raffle/${id}`, {
      headers: this.headers
    });
  }

  public async getRaffles(): Promise<AxiosResponse<RadRaffle[]>> {
    return await axios.get(`/api/raffle/raffles`, {
      headers: this.headers
    });
  }

  public async chooseWinner(
    id: number
  ): Promise<AxiosResponse<RaffleNumberSelection>> {
    return await axios.get(`/api/raffle/winner/${id}`, {
      headers: this.headers
    });
  }
}
