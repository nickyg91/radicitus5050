<template>
    <div class="section">
        <div class="container">
            <div v-for="number in numberOfRows" :key="number" class="columns">
                <div v-for="n in squaresPerRow" :key="n" class="column">
                    <Square :key="n * number" v-on:square-clicked="squareClicked" v-bind:squareNumber="n * number" />
                </div>
            </div>
        </div>
    </div>
</template>
<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import Square from '@/components/Square.vue';
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
    public squareName = 'test';
    public selectedSquares = new Array<number>();
    public squareClicked(square: Square) {
        console.log(square);
        if (this.selectedSquares.length !== this.maxSquares) {
            this.selectedSquares.push(square.$props.squareNumber);
            square.$set(square, 'squareName', 'test');
        }
    }
    public mounted() {
        const hasUsernameBeenSet = this.$store.getters.currentUser !== null;
        if (!hasUsernameBeenSet) {
            // show modal here
        }
    }
}
</script>

