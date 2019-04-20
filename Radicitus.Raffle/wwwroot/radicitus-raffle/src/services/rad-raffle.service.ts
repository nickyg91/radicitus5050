import axios, { AxiosResponse } from 'axios';
import RadRaffle from '@/models/raffle.model';
import RaffleNumberSelection from '@/models/raffle-number-selection.model';
import RaffleNumber from '@/models/raffle-number.model';
export default class RadRaffleService {
    private apiUrl = process.env.VUE_APP_API_URL;
    private headers = {
        'Access-Control-Allow-Headers': 'x-requested-with'
    };
    public async createRaffle(raffle: RadRaffle): Promise<AxiosResponse<RadRaffle>> {
        return await axios.post<RadRaffle>(`${this.apiUrl}/api/raffle/create`, raffle, {
            headers : this.headers
        });
    }

    public async getRaffleNumbersForUser(guid: string, username: string): Promise<AxiosResponse<number[]>> {
        return await axios.get(`${this.apiUrl}/api/raffle/${guid}/${username}/numbers`, {
            headers: this.headers
        });
    }

    public async getNumbersForRaffle(raffleGuid: string): Promise<AxiosResponse<RaffleNumberSelection[]>> {
        return await axios.get(`${this.apiUrl}/api/raffle/numbers/${raffleGuid}`, {
            headers: this.headers
        });
    }
}
