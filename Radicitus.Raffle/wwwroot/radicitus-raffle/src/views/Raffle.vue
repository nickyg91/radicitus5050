<style lang="scss" scoped>
    @import "~bulma/sass/utilities/_all";
    .selected {
       background-color: $grey-dark;
       color: white;
    }
    .whos-here {
        width: 200px;
        height: 470px;
        overflow-y: scroll;
    }
    .whos-here-container {
        -webkit-transition-duration: 0.3s;
        -moz-transition-duration: 0.3s;
        -o-transition-duration: 0.3s;
        transition-duration: 0.3s;
        position: fixed;
        top: 100px;
        left: 0;
        margin-left:-225px;
    }
    .whos-here-shown {
        -webkit-transition-duration: 0.3s;
        -moz-transition-duration: 0.3s;
        -o-transition-duration: 0.3s;
        transition-duration: 0.3s;
        position: fixed;
        top: 100px;
        margin-left: 0 !important;
    }
    .toggle-slideout {
        left: 0;
        top: 55px;
        position: fixed;
        margin-bottom: 5px;
        margin-left:-5px;
    }
</style>
<template>
    <div class="section">
        <div class="has-text-centered">
            <p class="has-text-weight-bold is-size-2">{{raffle.RaffleName}}</p>
        </div>
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
        <div class=toggle-slideout>
            <button @click="showSlideout" class="button is-info">
                <font-awesome-icon icon="users"></font-awesome-icon>
            </button>
        </div>
        <div v-bind:class="{'whos-here-shown': isSlideoutShown}" class="whos-here-container">
            <div class="box whos-here">
                <div v-for="user in joinedUsers" v-bind:key="user.connectionId">
                    {{user.name}}
                </div>
            </div>
        </div>
    </div>
</template>
<script lang="ts" src="@/views/Raffle.ts">
</script>


