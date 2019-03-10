<style lang="scss" scoped>
    @import "~bulma/sass/utilities/_all";
    .selected {
       background-color: $grey-dark;
       color: white; 
    }
</style>
<template>
    <div class="section">
        <div class="container">
            <div v-for="number in numberOfRows" :key="number" class="columns">
                <div v-for="n in squaresPerRow" :key="n" class="column">
                    <Square :key="n * number" v-on:square-clicked="squareClicked" v-bind:squareNumber="n * number" />
                </div>
            </div>
        </div>
        <b-modal :can-cancel="false" :active.sync="isNameModalActive">
            <div class="box">
                <div class="has-text-centered">
                    <font-awesome-icon class="is-size-1 has-text-danger" icon="exclamation-circle"></font-awesome-icon>
                    <p class="is-size-3">
                        Looks like we don't know you?
                    </p>
                </div>
                <div class="has-text-centered">
                    <p class="is-size-5">
                        Tell us who you are so you can pick your numbers.
                    </p> 
                </div>
                <div class="has-text-centered">
                    <div class="field">
                        <div class="control">
                            <input type="text" class="input" v-model="squareName" placeholder="Put your name here" />
                        </div>
                    </div>
                    <button @click="isNameModalActive = false" :disabled="squareName.length === 0" class="button is-info is-large">
                        Accept
                    </button>
                </div>
            </div>
        </b-modal>
    </div>
</template>
<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import Square from '@/components/Square.vue';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@aspnet/signalr'
@Component({
  components: {
    Raffle,
    Square,
  },
})
export default class Raffle extends Vue {
    public totalSquares = 100;
    public numberOfRows = 10;
    public squaresPerRow = 10;
    public maxSquares = 5;
    public squareName = '';
    public isNameModalActive = false;
    public selectedSquares = new Array<number>();
    private hubConnection: HubConnection = null;
    public squareClicked(square: Square) {
        const indexOfClickedNumber = this.selectedSquares.indexOf(square.$props.squareNumber);
        if (this.selectedSquares.length < this.maxSquares && indexOfClickedNumber < 0) {
            this.selectedSquares.push(square.$props.squareNumber);
            square.$set(square, 'squareName', this.squareName);
            square.$el.classList.add('selected');
            this.hubConnection.invoke('BroadcastSelectedNumbersToRaffleGroup', {
                'Name': this.squareName,
                'Number': square.$props.squareNumber
            });
        }
        if (indexOfClickedNumber > -1) {
            this.selectedSquares.splice(indexOfClickedNumber, 1);
            square.$set(square, 'squareName', '');
            square.$el.classList.remove('selected');
        }
    }

    public acceptName() {
        this.$store.commit('currentUser', this.squareName);
    }

    public mounted() {
        const hasUsernameBeenSet = this.$store.getters.currentUser;
        if (!hasUsernameBeenSet) {
            this.isNameModalActive = true;
            this.hubConnection = new HubConnectionBuilder().withUrl('/rafflehub').build();
            this.hubConnection.start();
            this.hubConnection.on('sendNumbers', (result) => {
                var childSquare = this.$children.filter(x => x.$props.squareNumber === result.Number)[0];
                if (childSquare) {
                    childSquare.$set(childSquare, 'squareName', result.Name);
                    childSquare.$el.classList.add('selected');
                }
            });
        }
    }
}
</script>

