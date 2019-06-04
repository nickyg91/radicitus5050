import Vue from 'vue';
import Component from 'vue-class-component';
import RaffleNumberSelection from '@/models/raffle-number-selection.model';
import RadRaffleService from '@/services/rad-raffle.service';
import RadRaffle from '@/models/raffle.model';

@Component
export default class ChooseWinner extends Vue {
    public winner: RaffleNumberSelection = new RaffleNumberSelection();
    public raffles: RadRaffle[] = [];
    public isLoadingWinner = false;
    private raffleService: RadRaffleService = new RadRaffleService();
    public async mounted() {
        this.raffles = (await this.raffleService.getRaffles()).data;
    }

    public async chooseWinner(raffle: RadRaffle) {
        this.isLoadingWinner = true;
        this.winner = (await this.raffleService.chooseWinner(raffle.RaffleGuid)).data;
        this.isLoadingWinner = false;
        if (this.winner.Name !== null) {
            raffle.WinnerName = this.winner.Name;
            raffle.WinningSquare = this.winner.Number;
            this.$notification.open({
                duration: 5000,
                    message: `Huzzah! You have found a winner!`,
                    position: 'is-bottom-right',
                    type: 'is-info'
                });
        } else {
            this.$notification.open({
                duration: 5000,
                    message: `:( No winner found.`,
                    position: 'is-bottom-right',
                    type: 'is-danger'
                });
        }
    }

}
