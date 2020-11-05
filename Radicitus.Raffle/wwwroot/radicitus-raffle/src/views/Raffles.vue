<style lang="scss" scoped>
@import "~bulma/sass/utilities/_all";
</style>
<template>
  <div class="section">
    <div class="has-text-centered">
      <img src="@/assets/Logo_Radicitus_Black.png" />
      <h1 class="is-size-1">Raffles</h1>
    </div>
    <div class="columns">
      <div class="column is-offset-2 is-two-thirds">
        <input
          type="text"
          placeholder="Search raffles..."
          class="input"
          v-on:keyup="searchRaffles($event)"
        />
        <hr />
      </div>
    </div>
    <div
      v-bind:key="raffle.RaffleGuid"
      v-for="raffle in rafflesToDisplay"
      class="columns"
    >
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
                {{ raffle.RaffleName }}
              </div>
            </div>
            <div class="level-right">
              <div class="is-size-5 level-item">
                {{ raffle.StartDate | formatDate }}
              </div>
            </div>
            <div class="level-right">
              <div class="is-size-5 level-item">
                {{ raffle.EndDate | formatDate }}
              </div>
            </div>
          </div>
          <div class="section">
            <div class="has-text-centered">
              <p class="is-size-2">Winner: {{ raffle.WinnerName }}</p>
            </div>
          </div>
          <div class="section">
            <div class="has-text-centered">
              <p class="is-size-2">
                Winning Number: {{ raffle.WinningSquare }}
              </p>
            </div>
          </div>
          <div class="section">
            <div class="has-text-centered">
              <p class="is-size-2">Amount Won: {{ raffle.AmountWon }}</p>
            </div>
          </div>
          <div class="section">
            <a
              class="button is-large is-dark is-fullwidth"
              @click="viewRaffle(raffle)"
            >
              View
            </a>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import Vue from "vue";
import RadRaffleService from "@/services/rad-raffle.service";
import RadRaffle from "@/models/raffle.model";
import Component from "vue-class-component";
import router from "@/router";

@Component({
  components: {
    Raffles,
  },
})
export default class Raffles extends Vue {
  public raffles: RadRaffle[] = [];
  public rafflesToDisplay: RadRaffle[] = [];
  private _raffleService: RadRaffleService;
  public searchRaffles(event: any) {
    const val = event.target.value.toLowerCase();
    if (event.target.value.length > 4) {
      this.rafflesToDisplay = this.raffles.filter((raffle) => {
        return raffle.RaffleName.toLowerCase().indexOf(val) > -1;
      });
    }
    if (event.target.value.length === 0) {
      this.rafflesToDisplay = this.raffles;
    }
  }

  public async mounted() {
    this._raffleService = new RadRaffleService();
    this.raffles = (await this._raffleService.getRaffles()).data;
    this.rafflesToDisplay = this.raffles;
  }

  public viewRaffle(selectedRaffle: RadRaffle) {
    this.$store.commit('setSelectedRaffle', selectedRaffle);
    router.push(`/raffle/${selectedRaffle.Id}`);
  }
}
</script>
