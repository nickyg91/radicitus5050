import axios from 'axios';
export default class RadRaffleService {
    constructor() {
        this.apiUrl = 'http://localhost:5000/api/raffle';
    }
    async createRaffle(raffle) {
        return await axios.post(`${this.apiUrl}/create`, raffle, {
            headers: {
                'Access-Control-Allow-Headers': 'x-requested-with',
            },
        });
    }
}
//# sourceMappingURL=rad-raffle.service.js.map