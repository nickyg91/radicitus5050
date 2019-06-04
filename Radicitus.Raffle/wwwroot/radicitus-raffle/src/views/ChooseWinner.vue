<template>
    <div class="container">
        <div v-bind:key="raffle.RaffleGuid" v-for="raffle in raffles" class="section">
            <div class="box"  v-bind:class="{'loading': isLoadingWinner}">
                <div class="level">
                    <div class="level-item">
                        <p class="is-size-2">
                            {{raffle.RaffleName}}
                        </p>
                    </div>
                </div>
                <div v-if="raffle.WinnerName == null" class="level">
                    <div class="level-item">
                        <button @click="chooseWinner(raffle)" class="has-text-white button is-large is-rainbow-background">
                            Choose Winner
                        </button>
                    </div>
                </div>
                <div v-if="raffle.WinnerName !== null" class="level">
                    <div class="level-item">
                        <p class="is-size-2 rainbow-text">
                            {{raffle.WinnerName}}
                        </p>
                    </div>
                </div>
                <div v-if="raffle.WinnerName !== null" class="level">
                    <div class="level-item">
                        <p class="is-size-2 rainbow-text">
                            {{raffle.WinningSquare}}
                        </p>
                    </div>
                </div>
                <div v-if="raffle.WinnerName !== null" class="level">
                    <div class="level-item">
                        <p class="is-size-2 rainbow-text">
                            Amount Won: {{raffle.AmountWon}}
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <b-loading :is-full-page="true" :active.sync="isLoadingWinner" :can-cancel="false"></b-loading>
    </div>
</template>
<script lang="ts" src="@/views/ChooseWinner.ts">

</script>
