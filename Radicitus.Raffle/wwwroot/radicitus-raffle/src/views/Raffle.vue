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
<script lang="ts" src="@/views/Raffle.ts">
</script>


