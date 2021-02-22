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
  margin-left: -225px;
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
  margin-left: -5px;
}
</style>
<template>
  <div class="section">
    <div class="has-text-centered">
      <p class="has-text-weight-bold is-size-2">{{ raffle.RaffleName }}</p>
    </div>
    <div class="container">
      <div
        v-for="square in Math.ceil(squares.length / 10)"
        :key="square"
        class="columns"
      >
        <div
          v-for="squareCol in squares.slice((square - 1) * 10, square * 10)"
          :key="squareCol"
          class="column"
        >
          <Square
            :key="squareCol"
            v-on:square-clicked="squareClicked"
            v-bind:squareNumber="squareCol"
          ></Square>
        </div>
      </div>
    </div>
    <b-modal :can-cancel="false" :active.sync="isNameModalActive">
      <div class="box">
        <div class="has-text-centered">
          <i class="is-size-1 has-text-danger fas fa-exclamation-circle"></i>
          <p class="is-size-3">Looks like we don't know you?</p>
        </div>
        <div class="has-text-centered">
          <p class="is-size-5">
            Tell us who you are so you can pick your numbers.
          </p>
        </div>
        <div class="has-text-centered">
          <div class="field">
            <div class="control">
              <input
                type="text"
                class="input"
                v-model="squareName"
                placeholder="Put your name here"
              />
            </div>
          </div>
          <button
            @click="acceptName"
            :disabled="squareName.length === 0"
            class="button is-info is-large"
          >
            Accept
          </button>
        </div>
      </div>
    </b-modal>
    <div class="toggle-slideout">
      <button @click="showSlideout" class="button is-info">
        <i class="fa fa-users"></i>
      </button>
    </div>
    <div
      v-bind:class="{ 'whos-here-shown': isSlideoutShown }"
      class="whos-here-container"
    >
      <div class="box whos-here">
        <div v-if="joinedUsers.length < 1">
          Looks like there isn't anyone else in here at the moment.
        </div>
        <div v-for="user in joinedUsers" v-bind:key="user.ConnectionId">
          {{ user.Name }}
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import Square from "@/components/Square.vue";
import RaffleCookie from "@/models/raffle.cookie.model";
import Raffle from "@/models/raffle.model";
import RadRaffleService from "@/services/rad-raffle.service";
import { HubConnection, HubConnectionBuilder } from "@aspnet/signalr";
import UserConnection from "@/models/raffle-hub-user.model";
import RaffleNumberSelection from "../models/raffle-number-selection.model";
@Component({
  components: {
    Square
  }
})
export default class RaffleView extends Vue {
  public isSlideoutShown = false;
  public raffle!: Raffle;
  public squares = Array.from(Array(100).keys()).map(x => x + 1);
  public totalSquares = 100;
  public numberOfRows = 10;
  public squaresPerRow = 10;
  public maxSquares = 5;
  public squareName = "";
  public isNameModalActive = false;
  public selectedSquares = new Array<number>();
  public takenSquares = new Array<number>();
  public hubConnection: HubConnection;
  public joinedUsers = new Array<UserConnection>();
  private raffleService = new RadRaffleService(this.$http);
  public showSlideout() {
    this.isSlideoutShown = !this.isSlideoutShown;
  }

  public async created() {
    this.raffle = this.$store.getters.selectedRaffle;
    if (this.raffle.Id == null) {
      this.raffle = (
        await this.raffleService.getRaffleById(
          (this.$route.params.raffleId as unknown) as number
        )
      ).data;
    }
  }

  public beforeMount() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`/rafflehub?raffleId=${this.$route.params.raffleId}`)
      .build();
    this.hubConnection.on("SendNumbers", result => {
      const childSquare = this.$children.filter(
        x => x.$props.squareNumber === result.Number
      )[0];
      if (result.IsRemoved) {
        const idxOfRemovedNumber = this.takenSquares.indexOf(result.Number);
        this.takenSquares.splice(idxOfRemovedNumber, 1);
        childSquare.$set(childSquare, "squareName", "");
        childSquare.$el.classList.remove("selected");
      } else {
        childSquare.$set(childSquare, "squareName", result.Name);
        this.takenSquares.push(result.Number);
        childSquare.$el.classList.add("selected");
      }
    });
    this.hubConnection.on(
      "PopulateConnectedUsers",
      (result: UserConnection[]) => {
        this.joinedUsers = result;
      }
    );
    this.hubConnection.on("UserConnected", (result: UserConnection) => {
      const nameExists =
        this.joinedUsers
          .map(x => x.Name.toLowerCase())
          .indexOf(this.squareName.toLowerCase()) > -1;
      if (nameExists) {
        return;
      }
      this.joinedUsers.push(result);
      this.$buefy.toast.open({
        position: "is-bottom",
        type: "is-info",
        message: `${result.Name} has joined.`
      });
    });
    this.hubConnection.on("UserLeft", (result: UserConnection) => {
      const index = this.joinedUsers
        .map(x => x.ConnectionId)
        .indexOf(result.ConnectionId);
      this.joinedUsers.splice(index, 1);
      this.$buefy.toast.open({
        position: "is-bottom",
        type: "is-warning",
        message: `${result.Name} has left.`
      });
    });

    const cookie: RaffleCookie = this.$cookies.get(
      `raffle_${this.$route.params.raffleId}`
    );
    this.hubConnection.start().then(async result => {
      if (cookie) {
        // get numbers for this user and set their name automatically.
        this.squareName = cookie.Name;
        this.$store.commit("setUser", this.squareName);
        this.hubConnection.invoke("UserConnectedToRaffle", cookie.Name);
        const userNumbers = await this.raffleService.getRaffleNumbersForUser(
          (this.$route.params.raffleId as unknown) as number,
          cookie.Name
        );
        if (userNumbers) {
          this.selectedSquares = userNumbers.data;
        }
      } else {
        const hasUsernameBeenSet = this.$store.getters.currentUser;
        if (!hasUsernameBeenSet) {
          this.isNameModalActive = true;
        }
      }
    });
  }

  public async mounted() {
    const takenRaffleNumbers = await this.raffleService.getNumbersForRaffle(
      (this.$route.params.raffleId as unknown) as number
    );
    takenRaffleNumbers.data.forEach((raffle: RaffleNumberSelection) => {
      const childSquare = this.$children.filter(
        x => x.$props.squareNumber === raffle.Number
      )[0];
      if (childSquare) {
        childSquare.$set(childSquare, "squareName", raffle.Name);
        this.takenSquares.push(raffle.Number);
        childSquare.$el.classList.add("selected");
      }
    });
  }

  public squareClicked(square: Square) {
    if (this.raffle.WinnerName !== null) {
      return;
    }
    const isSquareAlreadyTakenBySomeoneElse =
      this.takenSquares.indexOf(square.$props.squareNumber) > -1;
    const indexOfClickedNumber = this.selectedSquares.indexOf(
      square.$props.squareNumber
    );
    const doIHaveThisNumber = indexOfClickedNumber > -1;
    if (doIHaveThisNumber) {
      this.selectedSquares.splice(indexOfClickedNumber, 1);
      const indexOfTakenSquare = this.takenSquares.indexOf(
        square.$props.squareNumber
      );
      this.takenSquares.splice(indexOfTakenSquare);
      this.$set(square, "squareName", "");
      square.$el.classList.remove("selected");
      this.hubConnection.invoke("BroadcastSelectedNumbersToRaffleGroup", {
        Name: this.squareName,
        Number: square.$props.squareNumber,
        IsRemoved: true
      });
    } else if (
      !isSquareAlreadyTakenBySomeoneElse &&
      this.selectedSquares.length < this.maxSquares
    ) {
      this.selectedSquares.push(square.$props.squareNumber);
      square.$set(square, "squareName", this.squareName);
      square.$el.classList.add("selected");
      this.hubConnection.invoke("BroadcastSelectedNumbersToRaffleGroup", {
        Name: this.squareName,
        Number: square.$props.squareNumber,
        IsRemoved: false
      });
    }
  }

  public acceptName() {
    const nameExists =
      this.joinedUsers
        .map(x => x.Name.toLowerCase())
        .indexOf(this.squareName.toLowerCase()) > -1;
    if (nameExists) {
      this.$buefy.toast.open({
        position: "is-bottom",
        type: "is-danger",
        message: `Sorry, but ${this.squareName} has already been taken.`
      });
      return;
    }
    this.isNameModalActive = false;
    this.$store.commit("setUser", this.squareName);
    const raffleCookie = new RaffleCookie();
    raffleCookie.Name = this.squareName;
    raffleCookie.Raffle = new Raffle();
    raffleCookie.Raffle.Id = (this.$route.params.id as unknown) as number;
    const twoWeeksFromNow = new Date();
    twoWeeksFromNow.setDate(twoWeeksFromNow.getDate() + 14);
    this.$cookies.set(
      `raffle_${this.$route.params.raffleId}`,
      JSON.stringify(raffleCookie),
      twoWeeksFromNow
    );
    this.hubConnection.invoke("UserConnectedToRaffle", this.squareName);
  }
}
</script>
