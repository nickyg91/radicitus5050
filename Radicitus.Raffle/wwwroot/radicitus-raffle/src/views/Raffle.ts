import Vue from 'vue';
import Component from 'vue-class-component';
import Square from '@/components/Square.vue';
import RaffleCookie from '@/models/raffle.cookie.model';
import Raffle from '@/models/raffle.model';
import RadRaffleService from '@/services/rad-raffle.service';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
@Component({
  components: {
    RaffleView,
    Square,
  },
})
export default class RaffleView extends Vue {
    public raffle: Raffle = this.$store.getters.selectedRaffle;
    public squares = Array.from(Array(100).keys()).map((x) => x + 1);
    public totalSquares = 100;
    public numberOfRows = 10;
    public squaresPerRow = 10;
    public maxSquares = 5;
    public squareName = '';
    public isNameModalActive = false;
    public selectedSquares = new Array<number>();
    public takenSquares = new Array<number>();
    public hubConnection: HubConnection;
    private raffleService = new RadRaffleService();
    public squareClicked(square: Square) {
        if (this.raffle.WinnerName === null) {
            return;
        }
        const isSquareAlreadyTakenBySomeoneElse = this.takenSquares.indexOf(square.$props.squareNumber) > -1;
        const indexOfClickedNumber = this.selectedSquares.indexOf(square.$props.squareNumber);
        const doIHaveThisNumber = indexOfClickedNumber > -1;
        if (doIHaveThisNumber) {
            this.selectedSquares.splice(indexOfClickedNumber, 1);
            square.$set(square, 'squareName', '');
            square.$el.classList.remove('selected');
            this.hubConnection.invoke('BroadcastSelectedNumbersToRaffleGroup', {
                Name: this.squareName,
                Number: square.$props.squareNumber,
                IsRemoved: true
            });
        } else if (!isSquareAlreadyTakenBySomeoneElse && this.selectedSquares.length < this.maxSquares) {
            this.selectedSquares.push(square.$props.squareNumber);
            square.$set(square, 'squareName', this.squareName);
            square.$el.classList.add('selected');
            this.hubConnection.invoke('BroadcastSelectedNumbersToRaffleGroup', {
                Name: this.squareName,
                Number: square.$props.squareNumber,
                IsRemoved: false
            });
        }
    }

    public acceptName() {
        this.isNameModalActive = false;
        this.$store.commit('setUser', this.squareName);
        const raffleCookie = new RaffleCookie();
        raffleCookie.Name = this.squareName;
        raffleCookie.Raffle = new Raffle();
        raffleCookie.Raffle.RaffleGuid = this.$route.params.guid;
        const twoWeeksFromNow = new Date();
        twoWeeksFromNow.setDate(twoWeeksFromNow.getDate() + 14);
        this.$cookies.set(`raffle_${this.$route.params.guid}`, JSON.stringify(raffleCookie), twoWeeksFromNow);
    }

    public async mounted() {
        const vueContext = this;
        const url = process.env.VUE_APP_API_URL;
        this.hubConnection =  new HubConnectionBuilder()
        .withUrl(`${url}/rafflehub?raffleguid=${this.$route.params.guid}`)
        .build();
        this.hubConnection.on('SendNumbers', (result) => {
            const childSquare = vueContext.$children.filter((x) => x.$props.squareNumber === result.number)[0];
            if (result.isRemoved) {
                const idxOfRemovedNumber = vueContext.takenSquares.indexOf(result.number);
                vueContext.takenSquares.splice(idxOfRemovedNumber, 1);
                childSquare.$set(childSquare, 'squareName', '');
                childSquare.$el.classList.remove('selected');
            } else {
                childSquare.$set(childSquare, 'squareName', result.name);
                vueContext.takenSquares.push(result.number);
                childSquare.$el.classList.add('selected');
            }
        });
        this.hubConnection.start();
        const takenRaffleNumbers = await this.raffleService.getNumbersForRaffle(this.$route.params.guid);
        const cookie: RaffleCookie = this.$cookies.get(`raffle_${this.$route.params.guid}`);
        takenRaffleNumbers.data.forEach((raffle) => {
            const childSquare = vueContext.$children.filter((x) => x.$props.squareNumber === raffle.Number)[0];
            if (childSquare) {
                childSquare.$set(childSquare, 'squareName', raffle.Name);
                vueContext.takenSquares.push(raffle.Number);
                childSquare.$el.classList.add('selected');
            }
        });
        if (cookie) {
            // get numbers for this user and set their name automatically.
            this.squareName = cookie.Name;
            this.$store.commit('setUser', this.squareName);
            const userNumbers = await this.raffleService.getRaffleNumbersForUser(this.$route.params.guid, cookie.Name);
            if (userNumbers) {
                vueContext.selectedSquares = userNumbers.data;
            }
        } else {
            const hasUsernameBeenSet = this.$store.getters.currentUser;
            if (!hasUsernameBeenSet) {
                this.isNameModalActive = true;
            }
        }
    }
}
