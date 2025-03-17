<template>
  <div class="create-card">
    <h1 class="create-card__title">{{ $t('forms.create.title') }}</h1>

    <!-- Create Card Form -->
    <form @submit.prevent="createCard" class="create-card__form">
      <div class="create-card__section">
        <h2 class="create-card__section-title">{{ $t('forms.create.entitySelection') }}</h2>
        
        <!-- Receiver Selection -->
        <div class="create-card__form-group">
          <label class="create-card__label" for="receiver">{{ $t('cards.labels.receiver') }}</label>
          <div class="create-card__select-container">
            <FuzzySearch
              id="receiver"
              v-model="card.receiverId"
              :items="receivers"
              :placeholder="$t('forms.create.searchReceiver')"
              :default-option="$t('forms.create.selectReceiver')"
              @item-selected="onReceiverSelected"
              class="create-card__search-container"
            />
            <button type="button" class="create-card__add-button" @click="openModal('receiver')">+</button>
          </div>
        </div>

        <!-- Supplier Selection -->
        <div class="create-card__form-group">
          <label class="create-card__label" for="supplier">{{ $t('cards.labels.supplier') }}</label>
          <div class="create-card__select-container">
            <FuzzySearch
              id="supplier"
              v-model="card.supplierId"
              :items="suppliers"
              :placeholder="$t('forms.create.searchSupplier')"
              :default-option="$t('forms.create.selectSupplier')"
              @item-selected="onSupplierSelected"
              class="create-card__search-container"
            />
            <button type="button" class="create-card__add-button" @click="openModal('supplier')">+</button>
          </div>
        </div>

        <!-- Carrier Selection -->
        <div class="create-card__form-group">
          <label class="create-card__label" for="carrier">{{ $t('cards.labels.carrier') }}</label>
          <div class="create-card__select-container">
            <FuzzySearch
              id="carrier"
              v-model="card.carrierId"
              :items="carriers"
              :placeholder="$t('forms.create.searchCarrier')"
              :default-option="$t('forms.create.selectCarrier')"
              @item-selected="onCarrierSelected"
              class="create-card__search-container"
            />
            <button type="button" class="create-card__add-button" @click="openModal('carrier')">+</button>
          </div>
        </div>
      </div>

      <!-- Card Details Section -->
      <div class="create-card__section">
        <h2 class="create-card__section-title">{{ $t('forms.create.cardDetails') }}</h2>
        
        <div class="create-card__row">
          <div class="create-card__form-group">
            <label class="create-card__label" for="numberOfCollies">{{ $t('cards.labels.numberOfCollies') }}</label>
            <input type="number" id="numberOfCollies" v-model="card.numberOfCollies" min="0" required class="create-card__input" />
          </div>

          <div class="create-card__form-group">
            <label class="create-card__label" for="numberOfPallets">{{ $t('cards.labels.numberOfPallets') }}</label>
            <input type="number" id="numberOfPallets" v-model="card.numberOfPallets" min="0" required class="create-card__input" />
          </div>
        </div>

        <div class="create-card__row">
          <div class="create-card__form-group">
            <label class="create-card__label" for="numberOfBundels">{{ $t('cards.labels.numberOfBundels') }}</label>
            <input type="number" id="numberOfBundels" v-model="card.numberOfBundels" min="0" required class="create-card__input" />
          </div>

          <div class="create-card__form-group">
            <label class="create-card__label" for="priority">{{ $t('cards.labels.priority') }}</label>
            <select id="priority" v-model="card.priority" required class="create-card__select">
              <option value="Low">{{ $t('cards.filters.low') }}</option>
              <option value="Medium">{{ $t('cards.filters.medium') }}</option>
              <option value="High">{{ $t('cards.filters.high') }}</option>
            </select>
          </div>
        </div>
      </div>

      <button type="submit" class="create-card__submit" :disabled="loading">
        {{ loading ? $t('common.processing') : $t('cards.actions.create') }}
      </button>
    </form>

    <!-- Status Message -->
    <div v-if="message" 
         class="create-card__message"
         :class="{ 
           'create-card__message--success': isSuccess, 
           'create-card__message--error': !isSuccess 
         }">
      {{ message }}
    </div>
    
    <!-- Generic Modal Component -->
    <ModalDialog :isVisible="isModalOpen" @close="closeModal" :showCloseButton="true">
      <component 
        :is="activeModalComponent" 
        @close="closeModal" 
        @receiver-added="loadReceivers"
        @supplier-added="loadSuppliers"
        @carrier-added="loadCarriers" 
      />
    </ModalDialog>
  </div>
</template>

<script>
import ModalDialog from './Modal.vue';
import ReceiverModal from './ReceiverModal.vue';
import SupplierModal from './SupplierModal.vue';
import CarrierModal from './CarrierModal.vue';
import FuzzySearch from './common/FuzzySearch.vue';

export default {
  name: 'CreateCard',
  components: {
    ModalDialog,
    ReceiverModal,
    SupplierModal,
    CarrierModal,
    FuzzySearch
  },
  data() {
    return {
      card: {
        receiverId: '',
        supplierId: '',
        carrierId: '',
        numberOfCollies: 0,
        numberOfPallets: 0,
        numberOfBundels: 0,
        priority: 'Low'
      },
      receivers: [],
      suppliers: [],
      carriers: [],
      isModalOpen: false,
      activeModalComponent: null,
      message: '',
      isSuccess: false,
      loading: false
    };
  },
  mounted() {
    this.loadEntities();
  },
  methods: {
    onReceiverSelected(receiver) {
      console.log('Receiver selected:', receiver);
    },
    
    onSupplierSelected(supplier) {
      console.log('Supplier selected:', supplier);
    },
    
    onCarrierSelected(carrier) {
      console.log('Carrier selected:', carrier);
    },
    
    async loadEntities() {
      await Promise.all([
        this.loadReceivers(),
        this.loadSuppliers(),
        this.loadCarriers()
      ]);
    },
    
    async loadReceivers() {
      try {
        const response = await fetch('/api/receiver');
        if (response.ok) {
          const data = await response.json();
          this.receivers = data.map(r => ({
            value: r.id,
            text: r.name
          }));
        }
      } catch (error) {
        console.error('Error loading receivers:', error);
      }
    },
    
    async loadSuppliers() {
      try {
        const response = await fetch('/api/supplier');
        if (response.ok) {
          const data = await response.json();
          this.suppliers = data.map(s => ({
            value: s.id,
            text: s.name
          }));
        }
      } catch (error) {
        console.error('Error loading suppliers:', error);
      }
    },
    
    async loadCarriers() {
      try {
        const response = await fetch('/api/carrier');
        if (response.ok) {
          const data = await response.json();
          this.carriers = data.map(c => ({
            value: c.id,
            text: c.name
          }));
        }
      } catch (error) {
        console.error('Error loading carriers:', error);
      }
    },
    
    // Generic modal handler
    openModal(entityType) {
      // Map entity type to component name
      const componentMap = {
        receiver: 'ReceiverModal',
        supplier: 'SupplierModal',
        carrier: 'CarrierModal'
      };
      
      this.activeModalComponent = componentMap[entityType];
      this.isModalOpen = true;
    },
    
    closeModal() {
      this.isModalOpen = false;
    },
    
    async createCard() {
      this.loading = true;
      this.message = '';
      
      try {
        const response = await fetch('/api/card', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            receiverId: this.card.receiverId,
            supplierId: this.card.supplierId,
            carrierId: this.card.carrierId,
            numberOfCollies: this.card.numberOfCollies,
            numberOfPallets: this.card.numberOfPallets,
            numberOfBundels: this.card.numberOfBundels,
            priority: this.card.priority
          })
        });
        
        if (!response.ok) {
          throw new Error(`${response.status} ${response.statusText}`);
        }
        
        const result = await response.json();
        console.log('Card created successfully:', result);
        this.message = this.$t('forms.create.success');
        this.isSuccess = true;
        
        // Reset form
        this.card = {
          receiverId: '',
          supplierId: '',
          carrierId: '',
          numberOfCollies: 0,
          numberOfPallets: 0,
          numberOfBundels: 0,
          priority: 'Low'
        };
        
      } catch (error) {
        console.error('Error creating card:', error);
        this.message = `${this.$t('forms.create.error')} ${error.message}`;
        this.isSuccess = false;
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';
</style>