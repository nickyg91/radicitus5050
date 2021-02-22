import axios, { AxiosInstance, AxiosResponse } from "axios";
import RadRaffle from "@/models/raffle.model";
import RaffleNumberSelection from "@/models/raffle-number-selection.model";
import PaginatedRaffle from "@/models/paginated-raffle.model";
export default class RadRaffleService {

  constructor(private http: AxiosInstance) { }

  private headers = {
    "Access-Control-Allow-Headers": "x-requested-with"
  };
  public async createRaffle(
    raffle: RadRaffle
  ): Promise<AxiosResponse<RadRaffle>> {
    return await axios.post<RadRaffle>(
      `/api/raffle/create`,
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

  public async getPaginatedRaffles(pageNumber: number, amount: number): Promise<PaginatedRaffle> {
    return await (await axios.get(`/api/raffle/raffles/${amount}/page/${pageNumber}`)).data;
  }

  public async deleteRaffle(id: number) {
    return await (await axios.delete(`/api/raffle/remove/${id}`)).data  
  }
}
