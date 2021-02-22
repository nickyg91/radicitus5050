<template>
  <div>
    <section class="section">
      <div class="box">
        <h4 class="title">Create a 50/50 Raffle</h4>
        <hr />
        <form @submit.prevent="submitRaffle">
          <div class="field">
            <label class="label is-large"> Name </label>
            <input
              v-validate="'required|min:5'"
              name="name"
              type="text"
              class="input"
              v-model="raffle.RaffleName"
            />
          </div>
          <div class="field">
            <label class="label is-large"> Square Worth </label>
            <input
              v-validate="'required|integer'"
              type="number"
              name="squareWorth"
              class="input"
              v-model.number="raffle.SquareWorthAmount"
            />
          </div>
          <div class="field">
            <b-field custom-class="is-large" label="Start Date">
              <b-datepicker
                name="startDate"
                v-validate="'required'"
                v-model="raffle.StartDateUtc"
                placeholder="Click to select..."
                icon="calendar"
              >
              </b-datepicker>
            </b-field>
          </div>
          <div class="field">
            <b-field custom-class="is-large" label="End Date">
              <b-datepicker
                name="endDate"
                v-validate="'required'"
                v-model="raffle.EndDateUtc"
                placeholder="Click to select..."
                icon="calendar"
              >
              </b-datepicker>
            </b-field>
          </div>
          <div class="field">
            <button type="submit" class="is-fullwidth is-large button is-dark">
              Submit
            </button>
          </div>
        </form>
      </div>
    </section>
    <section class="has-text-centered" v-if="raffle.Id && raffle.Id > 0">
      <div>Distribute this url for the 50/50 raffle to everyone.</div>
      <div>https://radicitusguild.us/raffle/{{ raffle.Id }}</div>
    </section>
    <b-loading
      :is-full-page="true"
      :active.sync="isLoading"
      :can-cancel="false"
    ></b-loading>
  </div>
</template>
<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import RadRaffle from "@/models/raffle.model";
import RadRaffleService from "@/services/rad-raffle.service";
@Component
export default class CreateRaffle extends Vue {
  public raffle: RadRaffle = new RadRaffle();
  private radServices: RadRaffleService = new RadRaffleService(this.$http);
  private isLoading = false;
  public mounted() {
    this.raffle.StartDateUtc = new Date();
    this.raffle.EndDateUtc = new Date();
  }
  public async submitRaffle() {
    const isValid = await this.$validator.validateAll();
    if (isValid) {
      try {
        this.isLoading = true;
        const resp = await this.radServices.createRaffle(this.raffle);
        this.raffle.Id = resp.data.Id;
        this.$buefy.notification.open({
          message: "Raffle created successfully!",
          type: "is-success",
          position: "is-bottom-right",
          hasIcon: true
        });
        this.$emit("raffleCreated");
      } catch (ex) {
        this.$buefy.notification.open({
          message: "An error has occurred while creating the Raffle.",
          type: "is-danger",
          position: "is-bottom-right",
          hasIcon: true
        });
      } finally {
        this.isLoading = false;
      }
    }
  }
}
</script>
