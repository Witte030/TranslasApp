<template>
  <div class="create-card-container">
    <h1>{{ $t('forms.create.title') }}</h1>

    <!-- Create Card Form -->
    <form @submit.prevent="createCard">
      <div class="form-section">
        <h2>{{ $t('forms.create.entitySelection') }}</h2>
        
        <div class="form-group">
          <label for="receiver">{{ $t('cards.labels.receiver') }}</label>
          <div class="select-container">
            <select id="receiver" v-model="card.receiverId" required>
              <option value="" disabled selected>{{ $t('forms.create.selectReceiver') }}</option>
              <option v-for="receiver in receivers" :key="receiver.value" :value="receiver.value">
                {{ receiver.text }}
              </option>
            </select>
            <button type="button" class="quick-add-btn" @click="openReceiverModal">+</button>
          </div>
          <!-- Inline receiver modal -->
          <div v-if="isReceiverModalOpen" class="inline-modal">
            <ReceiverModal @close="closeReceiverModal" @receiver-added="loadReceivers" />
          </div>
        </div>

        <div class="form-group">
          <label for="supplier">{{ $t('cards.labels.supplier') }}</label>
          <div class="select-container">
            <select id="supplier" v-model="card.supplierId" required>
              <option value="" disabled selected>{{ $t('forms.create.selectSupplier') }}</option>
              <option v-for="supplier in suppliers" :key="supplier.value" :value="supplier.value">
                {{ supplier.text }}
              </option>
            </select>
            <button type="button" class="quick-add-btn" @click="openSupplierModal">+</button>
          </div>
          <!-- Inline supplier modal -->
          <div v-if="isSupplierModalOpen" class="inline-modal">
            <SupplierModal @close="closeSupplierModal" @supplier-added="loadSuppliers" />
          </div>
        </div>

        <div class="form-group">
          <label for="carrier">{{ $t('cards.labels.carrier') }}</label>
          <div class="select-container">
            <select id="carrier" v-model="card.carrierId" required>
              <option value="" disabled selected>{{ $t('forms.create.selectCarrier') }}</option>
              <option v-for="carrier in carriers" :key="carrier.value" :value="carrier.value">
                {{ carrier.text }}
              </option>
            </select>
            <button type="button" class="quick-add-btn" @click="openCarrierModal">+</button>
          </div>
          <!-- Inline carrier modal -->
          <div v-if="isCarrierModalOpen" class="inline-modal">
            <CarrierModal @close="closeCarrierModal" @carrier-added="loadCarriers" />
          </div>
        </div>
      </div>

      <div class="form-section">
        <h2>{{ $t('forms.create.cardDetails') }}</h2>
        
        <div class="form-row">
          <div class="form-group">
            <label for="numberOfCollies">{{ $t('cards.labels.numberOfCollies') }}</label>
            <input type="number" id="numberOfCollies" v-model="card.numberOfCollies" min="0" required />
          </div>

          <div class="form-group">
            <label for="numberOfPallets">{{ $t('cards.labels.numberOfPallets') }}</label>
            <input type="number" id="numberOfPallets" v-model="card.numberOfPallets" min="0" required />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label for="numberOfBundels">{{ $t('cards.labels.numberOfBundels') }}</label>
            <input type="number" id="numberOfBundels" v-model="card.numberOfBundels" min="0" required />
          </div>

          <div class="form-group">
            <label for="priority">{{ $t('cards.labels.priority') }}</label>
            <select id="priority" v-model="card.priority" required>
              <option value="Low">{{ $t('cards.filters.low') }}</option>
              <option value="Medium">{{ $t('cards.filters.medium') }}</option>
              <option value="High">{{ $t('cards.filters.high') }}</option>
            </select>
          </div>
        </div>
      </div>

      <button type="submit" class="submit-btn">{{ $t('cards.actions.create') }}</button>
    </form>

    <div v-if="message" :class="{ 'success': isSuccess, 'error': !isSuccess }">
      {{ message }}
    </div>
  </div>
</template>

<script>
import ReceiverModal from './ReceiverModal.vue';
import SupplierModal from './SupplierModal.vue';
import CarrierModal from './CarrierModal.vue';

export default {
  name: 'CreateCard',
  components: {
    ReceiverModal,
    SupplierModal,
    CarrierModal
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
      isReceiverModalOpen: false,
      isSupplierModalOpen: false,
      isCarrierModalOpen: false,
      message: '',
      isSuccess: false,
      loading: false
    };
  },
  mounted() {
    this.loadEntities();
  },
  methods: {
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
    
    openReceiverModal() {
      this.isReceiverModalOpen = true;
    },
    
    closeReceiverModal() {
      this.isReceiverModalOpen = false;
    },
    
    openSupplierModal() {
      this.isSupplierModalOpen = true;
    },
    
    closeSupplierModal() {
      this.isSupplierModalOpen = false;
    },
    
    openCarrierModal() {
      this.isCarrierModalOpen = true;
    },
    
    closeCarrierModal() {
      this.isCarrierModalOpen = false;
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

<style scoped>
.create-card-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

h1 {
  text-align: center;
  color: #333;
  margin-bottom: 30px;
}

h2 {
  color: #555;
  font-size: 1.2rem;
  border-bottom: 1px solid #eee;
  padding-bottom: 8px;
  margin-bottom: 15px;
}

/* Form styling */
form {
  display: flex;
  flex-direction: column;
  background-color: white;
  padding: 25px;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
  margin-bottom: 30px;
}

.form-section {
  margin-bottom: 25px;
}

.form-row {
  display: flex;
  gap: 20px;
  margin-bottom: 15px;
}

.form-group {
  flex: 1;
  margin-bottom: 15px;
  position: relative;
}

label {
  display: block;
  margin-bottom: 8px;
  font-weight: 500;
  color: #444;
  font-size: 15px;
}

input, select {
  width: 100%;
  padding: 12px;
  font-size: 16px;
  border: 1px solid #ddd;
  border-radius: 6px;
  background-color: #f9f9f9;
  transition: all 0.2s ease;
  box-sizing: border-box;
}

input:focus, select:focus {
  border-color: #4CAF50;
  background-color: #fff;
  outline: none;
  box-shadow: 0 0 0 3px rgba(76, 175, 80, 0.2);
}

input:hover, select:hover {
  border-color: #999;
}

.select-container {
  display: flex;
  align-items: center;
  gap: 10px;
}

.select-container select {
  flex: 1;
}

.quick-add-btn {
  width: 38px;
  height: 38px;
  border-radius: 50%;
  border: none;
  background-color: #4CAF50;
  color: white;
  font-size: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s ease;
  flex-shrink: 0;
}

.quick-add-btn:hover {
  background-color: #45a049;
  transform: scale(1.1);
}

.inline-modal {
  margin-top: 10px;
  padding: 15px;
  border-radius: 6px;
  background-color: #f9f9f9;
  border: 1px solid #ddd;
  margin-bottom: 10px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.submit-btn {
  align-self: center;
  padding: 14px 30px;
  margin-top: 10px;
  font-size: 16px;
  font-weight: bold;
  border: none;
  border-radius: 30px;
  background-color: #4CAF50;
  color: white;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
  width: auto;
  min-width: 200px;
}

.submit-btn:hover {
  background-color: #45a049;
  transform: translateY(-2px);
  box-shadow: 0 6px 12px rgba(0,0,0,0.15);
}

.success {
  margin-top: 20px;
  padding: 15px;
  border-radius: 6px;
  background-color: #d4edda;
  color: #155724;
  border: 1px solid #c3e6cb;
  text-align: center;
}

.error {
  margin-top: 20px;
  padding: 15px;
  border-radius: 6px;
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
  text-align: center;
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .form-row {
    flex-direction: column;
    gap: 0;
  }
}
</style>