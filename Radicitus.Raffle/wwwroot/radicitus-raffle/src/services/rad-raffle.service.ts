import axios, { AxiosResponse } from 'axios';
import RadRaffle from '@/models/raffle.model';
import RaffleNumberSelection from '@/models/raffle-number-selection.model';
export default class RadRaffleService {
    private headers = {
        'Access-Control-Allow-Headers': 'x-requested-with'
    };
    public async createRaffle(raffle: RadRaffle): Promise<AxiosResponse<RadRaffle>> {
        return await axios.post<RadRaffle>(`api/raffle/create`, raffle, {
            headers : this.headers
        });
    }

    public async getRaffleNumbersForUser(guid: string, username: string): Promise<AxiosResponse<number[]>> {
        return await axios.get(`api/raffle/${guid}/${username}/numbers`, {
            headers: this.headers
        });
    }

    public async getNumbersForRaffle(raffleGuid: string): Promise<AxiosResponse<RaffleNumberSelection[]>> {
        return await axios.get(`api/raffle/numbers/${raffleGuid}`, {
            headers: this.headers
        });
    }

    public async getRaffleByGuid(raffleGuid: string): Promise<AxiosResponse<RadRaffle>> {
        return await axios.get(`api/raffle/${raffleGuid}`, {
            headers: this.headers
        });
    }

    public async getRaffles(): Promise<AxiosResponse<RadRaffle[]>> {
        return await axios.get(`api/raffle/raffles`, {
            headers: this.headers
        });
    }

    public async chooseWinner(guid: string): Promise<AxiosResponse<RaffleNumberSelection>> {
        return await axios.get(`api/raffle/winner/${guid}`, {
            headers: this.headers
        });
    }
}
