import axios from "axios";
export default class RadRaffleService {
    constructor() {
        this.url = process.env.VUE_APP_API_URL;
        this.headers = {
            "Access-Control-Allow-Headers": "x-requested-with"
        };
    }
    async createRaffle(raffle) {
        return await axios.post(`${this.url}/api/raffle/create`, raffle, {
            headers: this.headers
        });
    }
    async getRaffleNumbersForUser(id, username) {
        return await axios.get(`${this.url}/api/raffle/${id}/${username}/numbers`, {
            headers: this.headers
        });
    }
    async getNumbersForRaffle(id) {
        return await axios.get(`${this.url}/api/raffle/numbers/${id}`, {
            headers: this.headers
        });
    }
    async getRaffleById(id) {
        return await axios.get(`${this.url}/api/raffle/${id}`, {
            headers: this.headers
        });
    }
    async getRaffles() {
        return await axios.get(`${this.url}/api/raffle/raffles`, {
            headers: this.headers
        });
    }
    async chooseWinner(id) {
        return await axios.get(`${this.url}/api/raffle/winner/${id}`, {
            headers: this.headers
        });
    }
}
//# sourceMappingURL=rad-raffle.service.js.map