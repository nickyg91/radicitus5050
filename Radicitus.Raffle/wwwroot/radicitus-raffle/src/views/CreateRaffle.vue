<template>
  <div>
      <section class="hero is-dark">
        <div class="hero-body">
          <div class="container">
            <h1 class="title is-white">
              Create a 50/50 Raffle
            </h1>
          </div>
        </div>
      </section>
      <section class="section">
        <div class="columns">
          <div class="column is-offset-one-third">
            <div class="box">
              <form @submit.prevent="submitRaffle">
                <div class="field">
                  <label class="label is-large">
                    Name
                  </label>
                  <input v-validate="'required|alpha|min:5'" name="name" type="text" class="input" v-model="raffle.RaffleName"/>
                </div>
                <div class="field">
                  <label class="label is-large">
                    Square Worth
                  </label>
                  <input v-validate="'required|integer'" type="text" name="squareWorth" class="input" v-model="raffle.SquareWorthAmount"/>
                </div>
                <div class="field">
                  <b-field 
                      custom-class="is-large" label="Start Date">
                    <b-datepicker
                      name="startDate"
                      v-validate="'required'"
                      v-model="raffle.StartDate" 
                      placeholder="Click to select..."
                      icon="calendar">
                    </b-datepicker>
                  </b-field>
                </div>
                <div class="field">
                  <b-field 
                      custom-class="is-large" label="End Date">
                    <b-datepicker
                      name="endDate"
                      v-validate="'required'"
                      v-model="raffle.EndDate" 
                      placeholder="Click to select..."
                      icon="calendar">
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
          </div>
          <div class="column">
          </div>
        </div>
      </section>
      <section class="has-text-centered" v-if="raffle.RaffleGuid">
        {{raffle.RaffleGuid}}
      </section>
      <b-loading :is-full-page="true" :active.sync="isLoading" :can-cancel="false"></b-loading>
  </div>
</template>
<script lang="ts">
import {Inject, Vue, Component} from 'vue-property-decorator';
import RadRaffle from '@/models/raffle.model';
import RadRaffleService from '@/services/rad-raffle.service';
@Component
export default class CreateRaffle extends Vue {
  private radServices: RadRaffleService = new RadRaffleService();
  private isLoading = false;
  public raffle: RadRaffle = new RadRaffle();
  public mounted() {
    this.raffle.StartDate = new Date();
    this.raffle.EndDate = new Date();
  }
  public async submitRaffle() {
    const isValid = await this.$validator.validateAll();
    if (isValid) {
        try {
          this.isLoading = true;
          const addedRaffle = await this.radServices.createRaffle(this.raffle);
        } catch(ex) {

        } finally {
          this.isLoading = false;
        }
    }
  }
}
</script>