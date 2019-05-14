<style lang="scss" scoped>
    @import "~bulma/sass/utilities/_all";
</style>
<template>
    <div class="section">
        <div class="section">
            <div class="has-text-centered">
                <img src="@/assets/Logo_Radicitus_Black.png"/>
                <h1 class="is-size-1">Raffles</h1>
            </div>
            <div class="container">
                <div v-bind:key="raffle.RaffleGuid" v-for="raffle in raffles" class="columns">
                    <div class="column is-offset-2 is-two-thirds">
                        <div class="box">
                            <div class="level">
                                <div class="level-left">
                                    <div class="is-size-5 level-item">
                                        <p class="has-text-weight-bold is-size-2">Name</p>
                                    </div>
                                </div>
                                <div class="level-right">
                                    <div class="is-size-5 level-item">
                                        <p class="has-text-weight-bold is-size-2">Start Date</p>
                                    </div>
                                </div>
                                <div class="level-right">
                                    <div class="is-size-5 level-item">
                                        <p class="has-text-weight-bold is-size-2">End Date</p>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="level">
                                <div class="level-left">
                                    <div class="is-size-5 level-item">
                                        {{raffle.RaffleName}}
                                    </div>
                                </div>
                                <div class="level-right">
                                    <div class="is-size-5 level-item">
                                        {{raffle.StartDate | formatDate}}
                                    </div>
                                </div>
                                <div class="level-right">
                                    <div class="is-size-5 level-item">
                                        {{raffle.EndDate | formatDate}}
                                    </div>
                                </div>
                            </div>
                            <div class="section">
                                <a class="button is-large is-dark is-fullwidth" @click="viewRaffle(raffle)">
                                    View
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script lang="ts" src="@/views/Raffles.ts">
</script>
