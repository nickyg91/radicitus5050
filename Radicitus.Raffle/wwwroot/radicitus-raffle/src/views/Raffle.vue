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
            <div v-for="square in Math.ceil(squares.length / 10)" :key="square" class="columns">
                <div v-for="squareCol in squares.slice((square - 1) * 10, square * 10)" :key="squareCol" class="column">
                    <Square :class="{ selected: takenSquares.indexOf(squareCol) > -1 }" 
                        :key="squareCol" 
                        v-on:square-clicked="squareClicked" 
                        v-bind:squareNumber="squareCol" />
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
                    <button @click="acceptName" :disabled="squareName.length === 0" class="button is-info is-large">
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
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@aspnet/signalr';
@Component({
  components: {
    Raffle,
    Square,
  },
})
export default class Raffle extends Vue {
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
    public squareClicked(square: Square) {
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
        } else if(!isSquareAlreadyTakenBySomeoneElse && this.selectedSquares.length < this.maxSquares) {
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
        this.hubConnection =  new HubConnectionBuilder()
        .withUrl(`http://localhost:51135/rafflehub?username=${this.squareName}&raffleguid=${this.$route.params.guid}`)
        .build();
        this.isNameModalActive = false;
        const vueContext = this;
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
        this.$store.commit('setUser', this.squareName);
    }

    public mounted() {
        const hasUsernameBeenSet = this.$store.getters.currentUser;
        if (!hasUsernameBeenSet) {
            this.isNameModalActive = true;
        }
    }
}
</script>

