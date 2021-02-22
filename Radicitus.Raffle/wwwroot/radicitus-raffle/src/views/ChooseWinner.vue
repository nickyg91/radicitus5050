<template>
  <div class="container">
    <div
      v-bind:key="raffle.RaffleGuid"
      v-for="raffle in raffles"
      class="section"
    >
      <div class="box" v-bind:class="{ loading: isLoadingWinner }">
        <div class="level">
          <div class="level-item">
            <p class="is-size-2">
              {{ raffle.RaffleName }}
            </p>
          </div>
        </div>
        <div v-if="raffle.WinnerName == null" class="level">
          <div class="level-item">
            <button
              @click="chooseWinner(raffle)"
              class="is-fullwidth has-text-white button is-large is-rainbow-background"
            >
              Choose Winner
            </button>
          </div>
        </div>
        <div v-if="raffle.WinnerName !== null" class="level">
          <div class="level-item">
            <p class="is-size-2 rainbow-text">
              {{ raffle.WinnerName }}
            </p>
          </div>
        </div>
        <div v-if="raffle.WinnerName !== null" class="level">
          <div class="level-item">
            <p class="is-size-2 rainbow-text">
              {{ raffle.WinningSquare }}
            </p>
          </div>
        </div>
        <div v-if="raffle.WinnerName !== null" class="level">
          <div class="level-item">
            <p class="is-size-2 rainbow-text">
              Amount Won: {{ raffle.AmountWon }}
            </p>
          </div>
        </div>
      </div>
    </div>
    <b-loading
      :is-full-page="true"
      :active.sync="isLoadingWinner"
      :can-cancel="false"
    ></b-loading>
  </div>
</template>
<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import RaffleNumberSelection from "@/models/raffle-number-selection.model";
import RadRaffleService from "@/services/rad-raffle.service";
import RadRaffle from "@/models/raffle.model";

@Component
export default class ChooseWinner extends Vue {
  public winner: RaffleNumberSelection = new RaffleNumberSelection();
  public raffles: RadRaffle[] = [];
  public isLoadingWinner = false;
  private raffleService: RadRaffleService = new RadRaffleService(this.$http);
  public async mounted() {
    this.raffles = (await this.raffleService.getRaffles()).data;
  }

  public async chooseWinner(raffle: RadRaffle) {
    this.isLoadingWinner = true;
    this.winner = (await this.raffleService.chooseWinner(raffle.Id)).data;
    this.isLoadingWinner = false;
    if (this.winner.Name !== null) {
      raffle.WinnerName = this.winner.Name;
      raffle.WinningSquare = this.winner.Number;
      this.$buefy.notification.open({
        duration: 5000,
        message: `Huzzah! You have found a winner!`,
        position: "is-bottom-right",
        type: "is-info"
      });
    } else {
      this.$buefy.notification.open({
        duration: 5000,
        message: `:( No winner found.`,
        position: "is-bottom-right",
        type: "is-danger"
      });
    }
  }
}
</script>
