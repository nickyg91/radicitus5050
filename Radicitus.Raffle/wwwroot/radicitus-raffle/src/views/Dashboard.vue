<template>
  <div class="section">
    <div class="container">
      <h1 class="title">
        Raffle Dashboard
      </h1>
    </div>
    <div class="container mt-2 has-text-right">
      <button
        @click="showCreateRaffleModal"
        class="button is-fullwidth-mobile is-success"
      >
        <span class="icon">
          <i class="fa fa-plus"> </i>
        </span>
        <span class="icon-text">
          Create Raffle
        </span>
      </button>
      <hr />
    </div>
    <div class="container">
      <b-field grouped group-multiline>
        <b-select
          @change.native="onPageChanged(pageNumber)"
          v-model="amountPerPage"
        >
          <option value="10">10</option>
          <option value="15">15</option>
          <option value="20">20</option>
          <option value="25">25</option>
        </b-select>
      </b-field>
      <b-table
        :data="paginatedRaffle.Raffles"
        :hoverable="true"
        :loading="isLoading"
        :paginated="true"
        :per-page="amountPerPage"
        :backend-pagination="true"
        :total="paginatedRaffle.TotalRaffles"
        @page-change="onPageChanged"
      >
        <b-table-column field="RaffleName" sortable label="Name" v-slot="props">
          {{ props.row.RaffleName }}
        </b-table-column>
        <b-table-column
          field="WinnerName"
          sortable
          label="Winner"
          v-slot="props"
        >
          {{ props.row.WinnerName ? props.row.WinnerName : "N/A" }}
        </b-table-column>
        <b-table-column
          field="SquareWorthAmount"
          sortable
          label="Square Amount"
          v-slot="props"
        >
          {{ props.row.SquareWorthAmount }}
        </b-table-column>
        <b-table-column
          field="DateCreatedUtc"
          sortable
          label="Created"
          v-slot="props"
        >
          {{ props.row.DateCreatedUtc | formatDate }}
        </b-table-column>
        <b-table-column
          field="StartDateUtc"
          sortable
          label="Starts"
          v-slot="props"
        >
          {{ props.row.StartDateUtc | formatDate }}
        </b-table-column>
        <b-table-column field="EndDateUtc" sortable label="Ends" v-slot="props">
          {{ props.row.EndDateUtc | formatDate }}
        </b-table-column>
        <b-table-column field="EndDateUtc" sortable label="Ends" v-slot="props">
          {{ props.row.EndDateUtc | formatDate }}
        </b-table-column>
        <b-table-column width="50" field="" sortable label="" v-slot="props">
          <button @click="remove(props.row.Id)" class="button is-danger">
            <span class="icon">
              <i class="fa fa-times"> </i>
            </span>
            <span class="icon-text">
              Delete
            </span>
          </button>
        </b-table-column>
      </b-table>
    </div>
  </div>
</template>
<script lang="ts">
import PaginatedRaffle from "@/models/paginated-raffle.model";
import RadRaffleService from "@/services/rad-raffle.service";
import Vue from "vue";
import Component from "vue-class-component";
import CreateRaffle from "@/views/CreateRaffle.vue";
@Component({
  components: {
    CreateRaffle
  }
})
export default class Dashboard extends Vue {
  private service: RadRaffleService = new RadRaffleService(this.$http);
  pageNumber = 1;
  amountPerPage = 10;
  paginatedRaffle = new PaginatedRaffle();
  public isLoading = true;
  public async created() {
    this.paginatedRaffle = await this.service.getPaginatedRaffles(
      this.pageNumber,
      this.amountPerPage
    );
    this.isLoading = false;
  }

  public async remove(id: number) {
    this.$buefy.dialog.confirm({
      message:
        "Are you sure you want to remove this raffle? This action is permanent.",
      onConfirm: async () => {
        try {
          this.isLoading = true;
          await this.service.deleteRaffle(id);
          this.paginatedRaffle = await this.service.getPaginatedRaffles(
            this.pageNumber,
            this.amountPerPage
          );
          this.$buefy.notification.open({
            message: "Raffle deleted successfully!",
            type: "is-success",
            position: "is-bottom-right",
            hasIcon: true
          });
        } catch (ex) {
          this.$buefy.notification.open({
            message: "An error occurred while deleting the Raffle!",
            type: "is-danger",
            position: "is-bottom-right",
            hasIcon: true
          });
        }
        this.isLoading = false;
      },
      hasIcon: true,
      icon: "exclamation-triangle",
      trapFocus: true,
      confirmText: "Confirm",
      cancelText: "Cancel",
      type: "is-danger"
    });
  }

  public async onPageChanged(page: number) {
    this.isLoading = true;
    this.pageNumber = page;
    this.paginatedRaffle = await this.service.getPaginatedRaffles(
      page,
      this.amountPerPage
    );
    this.isLoading = false;
  }

  public showCreateRaffleModal() {
    const modal = this.$buefy.modal.open({
      component: CreateRaffle,
      events: {
        raffleCreated: async () => {
          this.onPageChanged(this.pageNumber);
          modal.close();
        }
      }
    });
  }
}
</script>
