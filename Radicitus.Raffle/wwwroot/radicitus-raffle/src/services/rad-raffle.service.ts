import axios, { AxiosResponse } from 'axios';
import RadRaffle from '@/models/raffle.model';
export default class RadRaffleService {
    private apiUrl = 'http://localhost:5000/api/raffle';

    public async createRaffle(raffle: RadRaffle): Promise<AxiosResponse<RadRaffle>> {
        return await axios.post<RadRaffle>(`${this.apiUrl}/create`, raffle, {
            headers : {
                'Access-Control-Allow-Headers': 'x-requested-with',
            },
        });
    }
}
