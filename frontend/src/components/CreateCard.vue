<template>
  <div class="create-card">
    <h1 class="create-card__title">{{ $t("forms.create.title") }}</h1>

    <!-- Create Card Form -->
    <form @submit.prevent="createCard" class="create-card__form">
      <div class="create-card__section">
        <h2 class="create-card__section-title">
          {{ $t("forms.create.entitySelection") }}
        </h2>

        <!-- Receiver Selection -->
        <div class="create-card__form-group">
          <label class="create-card__label" for="receiver">{{
            $t("cards.labels.receiver")
          }}</label>
          <div class="create-card__select-container">
            <FuzzySearch
              ref="receiverSearch"
              id="receiver"
              v-model="card.receiver"
              :items="receivers"
              label-prop="text"
              value-prop="value"
              :placeholder="$t('forms.create.searchReceiver')"
              :default-option="$t('forms.create.selectReceiver')"
              @item-selected="onReceiverSelected"
              class="create-card__search-container"
            />
            <button
              type="button"
              class="create-card__add-button"
              @click="openModal('receiver')"
            >
              +
            </button>
          </div>
        </div>

        <!-- Supplier Selection -->
        <div class="create-card__form-group">
          <label class="create-card__label" for="supplier">{{
            $t("cards.labels.supplier")
          }}</label>
          <div class="create-card__select-container">
            <FuzzySearch
              ref="supplierSearch"
              id="supplier"
              v-model="card.supplier"
              :items="suppliers"
              label-prop="text"
              value-prop="value"
              :placeholder="$t('forms.create.searchSupplier')"
              :default-option="$t('forms.create.selectSupplier')"
              @item-selected="onSupplierSelected"
              class="create-card__search-container"
            />
            <button
              type="button"
              class="create-card__add-button"
              @click="openModal('supplier')"
            >
              +
            </button>
          </div>
        </div>

        <!-- Carrier Selection -->
        <div class="create-card__form-group">
          <label class="create-card__label" for="carrier">{{
            $t("cards.labels.carrier")
          }}</label>
          <div class="create-card__select-container">
            <FuzzySearch
              ref="carrierSearch"
              id="carrier"
              v-model="card.carrier"
              :items="carriers"
              label-prop="text"
              value-prop="value"
              :placeholder="$t('forms.create.searchCarrier')"
              :default-option="$t('forms.create.selectCarrier')"
              @item-selected="onCarrierSelected"
              class="create-card__search-container"
            />
            <button
              type="button"
              class="create-card__add-button"
              @click="openModal('carrier')"
            >
              +
            </button>
          </div>
        </div>
      </div>

      <!-- Card Details Section -->
      <div class="create-card__section">
        <h2 class="create-card__section-title">
          {{ $t("forms.create.cardDetails") }}
        </h2>

        <div class="create-card__row">
          <div class="create-card__form-group">
            <label class="create-card__label" for="numberOfCollies">{{
              $t("cards.labels.numberOfCollies")
            }}</label>
            <input
              type="number"
              id="numberOfCollies"
              v-model="card.numberOfCollies"
              min="0"
              required
              class="create-card__input"
            />
          </div>

          <div class="create-card__form-group">
            <label class="create-card__label" for="numberOfPallets">{{
              $t("cards.labels.numberOfPallets")
            }}</label>
            <input
              type="number"
              id="numberOfPallets"
              v-model="card.numberOfPallets"
              min="0"
              required
              class="create-card__input"
            />
          </div>
        </div>

        <div class="create-card__row">
          <div class="create-card__form-group">
            <label class="create-card__label" for="numberOfBundels">{{
              $t("cards.labels.numberOfBundels")
            }}</label>
            <input
              type="number"
              id="numberOfBundels"
              v-model="card.numberOfBundels"
              min="0"
              required
              class="create-card__input"
            />
          </div>

          <div class="create-card__form-group">
            <label class="create-card__label" for="priority">{{
              $t("cards.labels.priority")
            }}</label>
            <select
              id="priority"
              v-model="card.priority"
              required
              class="create-card__select"
            >
              <option value="Low">{{ $t("cards.filters.low") }}</option>
              <option value="Medium">{{ $t("cards.filters.medium") }}</option>
              <option value="High">{{ $t("cards.filters.high") }}</option>
            </select>
          </div>
        </div>
      </div>

      <button type="submit" class="create-card__submit" :disabled="loading">
        {{ loading ? $t("common.processing") : $t("cards.actions.create") }}
      </button>
    </form>

    <!-- Status Message -->
    <div
      v-if="message"
      class="create-card__message"
      :class="{
        'create-card__message--success': isSuccess,
        'create-card__message--error': !isSuccess,
      }"
    >
      {{ message }}
    </div>

    <!-- Generic Modal Component -->
    <ModalDialog
      :isVisible="isModalOpen"
      @close="closeModal"
      :showCloseButton="true"
    >
      <EntityModal
        v-if="isModalOpen"
        :entityType="activeEntityType"
        @receiver-added="loadReceivers"
        @supplier-added="loadSuppliers"
        @carrier-added="loadCarriers"
        @close="closeModal"
      />
    </ModalDialog>
  </div>
</template>

<script>
import ModalDialog from "./Modal.vue";
import EntityModal from "./Entity-modal.vue";
import FuzzySearch from "./common/FuzzySearch.vue";

export default {
  name: "CreateCard",
  components: {
    ModalDialog,
    EntityModal,
    FuzzySearch,
  },
  data() {
    return {
      card: {
        receiver: null,
        supplier: null,
        carrier: null,
        numberOfCollies: 0,
        numberOfPallets: 0,
        numberOfBundels: 0,
        priority: "Low",
      },
      receivers: [],
      suppliers: [],
      carriers: [],
      message: "",
      isSuccess: false,
      loading: false,
      isModalOpen: false,
      activeEntityType: null,
      showValidationErrors: false,
    };
  },
  mounted() {
    this.loadEntities();
  },
  methods: {
    onReceiverSelected(selected) {
      this.card.receiver =
        selected && selected.value ? { ...selected.value } : null;
    },
    onSupplierSelected(selected) {
      this.card.supplier =
        selected && selected.value ? { ...selected.value } : null;
    },
    onCarrierSelected(selected) {
      this.card.carrier =
        selected && selected.value ? { ...selected.value } : null;
    },

    async loadEntities() {
      await Promise.all([
        this.loadReceivers(),
        this.loadSuppliers(),
        this.loadCarriers(),
      ]);
    },
    async loadReceivers() {
      try {
        const response = await fetch("/api/receiver");
        if (response.ok) {
          const data = await response.json();
          this.receivers = data.map((item) => ({
            value: item,
            text: item.name,
          }));
        }
      } catch (error) {
        console.error("Error loading receivers:", error);
      }
    },
    async loadSuppliers() {
      try {
        const response = await fetch("/api/supplier");
        if (response.ok) {
          const data = await response.json();
          this.suppliers = data.map((item) => ({
            value: item,
            text: item.name,
          }));
        }
      } catch (error) {
        console.error("Error loading suppliers:", error);
      }
    },
    async loadCarriers() {
      try {
        const response = await fetch("/api/carrier");
        if (response.ok) {
          const data = await response.json();
          this.carriers = data.map((item) => ({
            value: item,
            text: item.name,
          }));
        }
      } catch (error) {
        console.error("Error loading carriers:", error);
      }
    },

    createCard() {
      this.$nextTick(() => {
        this.showValidationErrors = true;
        if (!this.validateForm()) return;
        this.submitCardData();
      });
    },

    async submitCardData() {
      this.loading = true;
      this.message = "";
      try {
        // Format the data to match exactly what the backend expects
        const requestData = {
          date: new Date().toISOString(),
          receiver: this.card.receiver
            ? {
                id: this.card.receiver.id,
                name: null,
              }
            : null,
          supplier: this.card.supplier
            ? {
                id: this.card.supplier.id,
                name: null,
                priority: this.card.supplier.priority || 0,
              }
            : null,
          carrier: this.card.carrier
            ? {
                id: this.card.carrier.id,
                name: null,
              }
            : null,
          numberOfCollies: this.card.numberOfCollies,
          numberOfPallets: this.card.numberOfPallets,
          numberOfBundels: this.card.numberOfBundels,
          priority: this.getPriorityEnum(this.card.priority),
          isHandled: false,
        };

        const response = await fetch("/api/card", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(requestData),
        });

        // Read the response as text first
        const responseText = await response.text();

        // If the response is not ok, handle the error
        if (!response.ok) {
          let errorMsg = `Server error: ${response.status}`;

          // Try to parse the response text as JSON
          try {
            const errorData = JSON.parse(responseText);
            if (errorData?.message) {
              errorMsg = errorData.message;
            } else if (errorData?.errors) {
              errorMsg = Object.values(errorData.errors).join(", ");
            }
          } catch (e) {
            // If the response text is not valid JSON, use it as is (truncated)
            if (responseText) {
              errorMsg = `${response.status}: ${responseText.substring(
                0,
                100
              )}...`;
            }
          }

          throw new Error(errorMsg);
        }

        // If response is ok, no need to parse it again since we're not using it
        // Just proceed with the success case
        this.message = this.$t("forms.create.success");
        this.isSuccess = true;
        this.resetForm();
      } catch (error) {
        this.message = `${this.$t("forms.create.error")} ${error.message}`;
        this.isSuccess = false;
      } finally {
        this.loading = false;
      }
    },

    getPriorityEnum(str) {
      const priorityMap = { Low: 0, Medium: 1, High: 2 };
      return priorityMap[str] || 0;
    },

    validateForm() {
      this.message = "";
      // Make sure all required fields are set
      if (!this.card.receiver) {
        this.message = this.$t("forms.create.errors.receiverRequired");
        this.isSuccess = false;
        return false;
      }
      if (!this.card.supplier) {
        this.message = this.$t("forms.create.errors.supplierRequired");
        this.isSuccess = false;
        return false;
      }
      if (!this.card.carrier) {
        this.message = this.$t("forms.create.errors.carrierRequired");
        this.isSuccess = false;
        return false;
      }
      // Validate numeric fields
      if (isNaN(this.card.numberOfCollies) || this.card.numberOfCollies < 0) {
        this.message = this.$t("forms.create.errors.invalidCollies");
        this.isSuccess = false;
        return false;
      }
      if (isNaN(this.card.numberOfPallets) || this.card.numberOfPallets < 0) {
        this.message = this.$t("forms.create.errors.invalidPallets");
        this.isSuccess = false;
        return false;
      }
      if (isNaN(this.card.numberOfBundels) || this.card.numberOfBundels < 0) {
        this.message = this.$t("forms.create.errors.invalidBundels");
        this.isSuccess = false;
        return false;
      }
      return true;
    },

    resetForm() {
  // Reset the card data
  this.card = {
    receiver: null,
    supplier: null,
    carrier: null,
    numberOfCollies: 0,
    numberOfPallets: 0,
    numberOfBundels: 0,
    priority: "Low",
  };

  this.showValidationErrors = false;

  // Reset FuzzySearch components manually
  this.$nextTick(() => {
    // Call clearSelection on each FuzzySearch component
    if (this.$refs.receiverSearch) {
      this.$refs.receiverSearch.clearSelection();
      // Blur the input element
      if (this.$refs.receiverSearch.$refs.searchInput) {
        this.$refs.receiverSearch.$refs.searchInput.blur();
      }
    }

    if (this.$refs.supplierSearch) {
      this.$refs.supplierSearch.clearSelection();
      if (this.$refs.supplierSearch.$refs.searchInput) {
        this.$refs.supplierSearch.$refs.searchInput.blur();
      }
    }

    if (this.$refs.carrierSearch) {
      this.$refs.carrierSearch.clearSelection();
      if (this.$refs.carrierSearch.$refs.searchInput) {
        this.$refs.carrierSearch.$refs.searchInput.blur();
      }
    }

    // Additionally, blur any active element in the document
    if (document.activeElement) {
      document.activeElement.blur();
    }
  });
  
  // Scroll back to the top of the form
  window.scrollTo({
    top: 0,
    behavior: 'smooth'
  });
},

    openModal(entityType) {
      this.activeEntityType = entityType;
      this.isModalOpen = true;
    },
    closeModal() {
      this.isModalOpen = false;
      this.activeEntityType = null;
    },
  },
};
</script>

<style lang="scss">
@use "@/assets/scss/main";
</style>
