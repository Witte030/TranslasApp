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
            <div class="create-card__search-container">
              <FuzzySearch
                id="receiver"
                v-model="card.receiverId"
                :items="receivers"
                :placeholder="$t('forms.create.searchReceiver')"
                :default-option="$t('forms.create.selectReceiver')"
                :required="true"
                :error-message="$t('forms.create.errors.receiverRequired')"
                :show-validation-errors="showValidationErrors"
                @item-selected="onReceiverSelected"
              />
            </div>
            <button type="button" class="create-card__add-button" @click="openModal('receiver')">+</button>
          </div>
        </div>

        <!-- Supplier Selection -->
        <div class="create-card__form-group">
          <label class="create-card__label" for="supplier">{{ $t('cards.labels.supplier') }}</label>
          <div class="create-card__select-container">
            <div class="create-card__search-container">
              <FuzzySearch
                id="supplier"
                v-model="card.supplierId"
                :items="suppliers"
                :placeholder="$t('forms.create.searchSupplier')"
                :default-option="$t('forms.create.selectSupplier')"
                :required="true"
                :error-message="$t('forms.create.errors.supplierRequired')"
                :show-validation-errors="showValidationErrors"
                @item-selected="onSupplierSelected"
              />
            </div>
            <button type="button" class="create-card__add-button" @click="openModal('supplier')">+</button>
          </div>
        </div>

        <!-- Carrier Selection -->
        <div class="create-card__form-group">
          <label class="create-card__label" for="carrier">{{ $t('cards.labels.carrier') }}</label>
          <div class="create-card__select-container">
            <div class="create-card__search-container">
              <FuzzySearch
                id="carrier"
                v-model="card.carrierId"
                :items="carriers"
                :placeholder="$t('forms.create.searchCarrier')"
                :default-option="$t('forms.create.selectCarrier')"
                :required="true"
                :error-message="$t('forms.create.errors.carrierRequired')"
                :show-validation-errors="showValidationErrors"
                @item-selected="onCarrierSelected"
              />
            </div>
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
import ModalDialog from './Modal.vue';
import EntityModal from './Entity-modal.vue'; // Import the new generic EntityModal
import FuzzySearch from './common/FuzzySearch.vue';

export default {
  name: 'CreateCard',
  components: {
    ModalDialog,
    EntityModal, // Register the new EntityModal component
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
      activeEntityType: null, // Now store the entity type string instead of component
      message: '',
      isSuccess: false,
      loading: false,
      showValidationErrors: false
    };
  },
  mounted() {
    this.loadEntities();
  },
  methods: {
    onReceiverSelected(receiver) {
      console.log('Receiver selected:', receiver);
      // Ensure the value is properly set
      if (receiver) {
        this.card.receiverId = receiver.value;
      }
    },
    
    onSupplierSelected(supplier) {
      console.log('Supplier selected:', supplier);
      // Ensure the value is properly set
      if (supplier) {
        this.card.supplierId = supplier.value;
      }
    },
    
    onCarrierSelected(carrier) {
      console.log('Carrier selected:', carrier);
      // Ensure the value is properly set
      if (carrier) {
        this.card.carrierId = carrier.value;
      }
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
    
    // Generic modal handler - Updated to work with EntityModal
    openModal(entityType) {
      this.activeEntityType = entityType; // Set the active entity type
      this.isModalOpen = true;
    },
    
    closeModal() {
      this.isModalOpen = false;
      this.activeEntityType = null; // Reset entity type when modal closes
    },
    
    async createCard() {
      // Ensure we check for validation after any possible model updates
      this.$nextTick(() => {
        this.showValidationErrors = true;
        
        if (!this.validateForm()) {
          return;
        }
        
        this.submitCardData();
      });
    },
    
    async submitCardData() {
      this.loading = true;
      this.message = '';
      
      try {
        // Create the card data payload
        const cardDto = {
          receiverId: this.card.receiverId,
          supplierId: this.card.supplierId,
          carrierId: this.card.carrierId,
          numberOfCollies: parseInt(this.card.numberOfCollies),
          numberOfPallets: parseInt(this.card.numberOfPallets),
          numberOfBundels: parseInt(this.card.numberOfBundels),
          priority: this.card.priority
        };
        
        // Wrap the cardDto in the expected format
        const payload = {
          cardDto: cardDto
        };
        
        console.log('Submitting card data:', payload);
        
        const response = await fetch('/api/card', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(payload)
        });
        
        const responseData = await response.json();
        
        if (!response.ok) {
          let errorMessage = 'Unknown error occurred';
          
          if (responseData && responseData.message) {
            errorMessage = responseData.message;
          } else if (responseData && responseData.errors) {
            // Join multiple validation errors
            errorMessage = Object.values(responseData.errors).join(', ');
          }
          
          throw new Error(errorMessage);
        }
        
        console.log('Card created successfully:', responseData);
        this.message = this.$t('forms.create.success');
        this.isSuccess = true;
        
        // Reset form
        this.resetForm();
        
      } catch (error) {
        console.error('Error creating card:', error);
        this.message = `${this.$t('forms.create.error')} ${error.message}`;
        this.isSuccess = false;
      } finally {
        this.loading = false;
      }
    },
    
    validateForm() {
      // Reset any previous error message
      this.message = '';
      
      // Debug - log the current state of the form
      console.log('Validating form with values:', {
        receiverId: this.card.receiverId,
        supplierId: this.card.supplierId,
        carrierId: this.card.carrierId
      });
      
      // Check required fields with more robust checking
      if (!this.card.receiverId || this.card.receiverId === '') {
        this.message = this.$t('forms.create.errors.receiverRequired');
        this.isSuccess = false;
        return false;
      }
      
      if (!this.card.supplierId || this.card.supplierId === '') {
        this.message = this.$t('forms.create.errors.supplierRequired');
        this.isSuccess = false;
        return false;
      }
      
      if (!this.card.carrierId || this.card.carrierId === '') {
        this.message = this.$t('forms.create.errors.carrierRequired');
        this.isSuccess = false;
        return false;
      }
      
      // Validate numeric fields
      if (isNaN(this.card.numberOfCollies) || this.card.numberOfCollies < 0) {
        this.message = this.$t('forms.create.errors.invalidCollies');
        this.isSuccess = false;
        return false;
      }
      
      if (isNaN(this.card.numberOfPallets) || this.card.numberOfPallets < 0) {
        this.message = this.$t('forms.create.errors.invalidPallets');
        this.isSuccess = false;
        return false;
      }
      
      if (isNaN(this.card.numberOfBundels) || this.card.numberOfBundels < 0) {
        this.message = this.$t('forms.create.errors.invalidBundels');
        this.isSuccess = false;
        return false;
      }
      
      return true;
    },
    
    resetForm() {
      this.card = {
        receiverId: '',
        supplierId: '',
        carrierId: '',
        numberOfCollies: 0,
        numberOfPallets: 0,
        numberOfBundels: 0,
        priority: 'Low'
      };
      this.showValidationErrors = false;
    }
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';
</style>