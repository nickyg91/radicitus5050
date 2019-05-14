import Vue from 'vue';
import RadRaffleService from '@/services/rad-raffle.service';
import RadRaffle from '@/models/raffle.model';
import Component from 'vue-class-component';
import router from '@/router';

@Component({
    components: {
      Raffles
    },
  })
export default class Raffles extends Vue {
    public raffles: RadRaffle[] = [];
    public rafflesToDisplay: RadRaffle[] = [];
    private _raffleService: RadRaffleService;
    private _raffleNames = new Array<string>();
    public searchRaffles(searchTerm: string) {
        if (searchTerm.length > 4) {
            this.rafflesToDisplay = this.raffles.filter((raffle) => {
                return raffle.RaffleName.indexOf(searchTerm) > -1;
            });
        }
        if (searchTerm.length === 0) {
            this.rafflesToDisplay = this.raffles;
        }
    }

    public async mounted() {
        this._raffleService = new RadRaffleService();
        this.raffles = (await this._raffleService.getRaffles()).data;
    }

    public viewRaffle(selectedRaffle: RadRaffle) {
        this.$store.commit('setSelectedRaffle', selectedRaffle);
        router.push(`/raffle/${selectedRaffle.RaffleGuid}`);
    }
}
