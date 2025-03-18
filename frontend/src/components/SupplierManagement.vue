<template>
  <div class="supplier-management">
    <h1 class="supplier-management__title">{{ $t('suppliers.title') }}</h1>

    <!-- Supplier List -->
    <div class="supplier-management__list-container">
      <div v-if="loading" class="supplier-management__loading">
        {{ $t('common.loading') }}
      </div>
      <div v-else-if="error" class="supplier-management__error">
        {{ error }}
        <button @click="loadSuppliers" class="supplier-management__retry-button">
          {{ $t('cards.actions.tryAgain') }}
        </button>
      </div>
      <div v-else-if="suppliers.length === 0" class="supplier-management__empty">
        {{ $t('suppliers.noSuppliers') }}
      </div>
      <table v-else class="supplier-management__table">
        <thead>
          <tr>
            <th>{{ $t('suppliers.labels.name') }}</th>
            <th>{{ $t('suppliers.labels.priority') }}</th>
            <th>{{ $t('suppliers.labels.additionalInfo') }}</th>
            <th>{{ $t('suppliers.labels.actions') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="supplier in suppliers" :key="supplier.id"
              :class="{'supplier-management__row--editing': editingSupplier && editingSupplier.id === supplier.id}">
            <td>{{ supplier.name }}</td>
            <td>
              <div v-if="editingSupplier && editingSupplier.id === supplier.id">
                <select v-model="editingSupplier.priority" class="supplier-management__select">
                  <option :value="0">{{ $t('cards.filters.low') }}</option>
                  <option :value="1">{{ $t('cards.filters.medium') }}</option>
                  <option :value="2">{{ $t('cards.filters.high') }}</option>
                </select>
              </div>
              <div v-else :class="getPriorityClass(supplier.priority)">
                {{ getPriorityLabel(supplier.priority) }}
              </div>
            </td>
            <td>
              <div v-if="editingSupplier && editingSupplier.id === supplier.id">
                <textarea v-model="editingSupplier.additionalInfo" 
                          class="supplier-management__textarea"
                          :placeholder="$t('suppliers.placeholders.additionalInfo')"
                ></textarea>
              </div>
              <div v-else class="supplier-management__info-text">
                {{ supplier.additionalInfo || $t('suppliers.noAdditionalInfo') }}
              </div>
            </td>
            <td>
              <div v-if="editingSupplier && editingSupplier.id === supplier.id">
                <button @click="saveSupplier" class="supplier-management__save-button" :disabled="saving">
                  {{ saving ? $t('common.processing') : $t('suppliers.actions.save') }}
                </button>
                <button @click="cancelEdit" class="supplier-management__cancel-button">
                  {{ $t('common.cancel') }}
                </button>
              </div>
              <div v-else>
                <button @click="editSupplier(supplier)" class="supplier-management__edit-button">
                  {{ $t('suppliers.actions.edit') }}
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Status Message -->
    <div v-if="message" 
         class="supplier-management__message"
         :class="{ 
           'supplier-management__message--success': isSuccess, 
           'supplier-management__message--error': !isSuccess 
         }">
      {{ message }}
    </div>
  </div>
</template>
  
<script>
export default {
  name: 'SupplierManagement',
  data() {
    return {
      suppliers: [],
      loading: true,
      error: null,
      editingSupplier: null,
      saving: false,
      message: '',
      isSuccess: false
    };
  },
  mounted() {
    this.loadSuppliers();
  },
    methods: {
      async loadSuppliers() {
        this.loading = true;
        this.error = null;
        
        try {
          const response = await fetch('/api/supplier');
          
          if (!response.ok) {
            throw new Error(`${response.status}: ${response.statusText}`);
          }
          
          const data = await response.json();
          this.suppliers = data;
        } catch (error) {
          this.error = `${this.$t('suppliers.errors.loadFailed')} ${error.message}`;
          console.error('Error loading suppliers:', error);
        } finally {
          this.loading = false;
        }
      },
  
      getPriorityLabel(priority) {
        const labels = {
          0: this.$t('cards.filters.low'),
          1: this.$t('cards.filters.medium'),
          2: this.$t('cards.filters.high')
        };
        return labels[priority] || this.$t('cards.filters.low');
      },
  
      getPriorityClass(priority) {
        return {
          'supplier-management__priority': true,
          'supplier-management__priority--low': priority === 0,
          'supplier-management__priority--medium': priority === 1,
          'supplier-management__priority--high': priority === 2
        };
      },
  
      editSupplier(supplier) {
        // Create a copy to avoid directly modifying the original object
        this.editingSupplier = { 
          ...supplier,
          // Ensure additionalInfo exists
          additionalInfo: supplier.additionalInfo || ''
        };
      },
  
      cancelEdit() {
        this.editingSupplier = null;
        this.message = '';
      },
  
      async saveSupplier() {
        if (!this.editingSupplier) return;
        
        this.saving = true;
        this.message = '';
        
        try {
          const response = await fetch(`/api/supplier/${this.editingSupplier.id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(this.editingSupplier)
          });
  
          // Read the response as text first
          const responseText = await response.text();
          
          // If the response is not ok, handle the error
          if (!response.ok) {
            let errorMsg = `Server error: ${response.status}`;
            
            try {
              const errorData = JSON.parse(responseText);
              if (errorData?.message) {
                errorMsg = errorData.message;
              } else if (errorData?.errors) {
                errorMsg = Object.values(errorData.errors).join(', ');
              }
            } catch (e) {
              if (responseText) {
                errorMsg = `${response.status}: ${responseText.substring(0, 100)}...`;
              }
            }
            
            throw new Error(errorMsg);
          }
          
          // Update the supplier in the list
          const updatedSupplier = this.editingSupplier;
          const index = this.suppliers.findIndex(s => s.id === updatedSupplier.id);
          if (index !== -1) {
            this.suppliers.splice(index, 1, updatedSupplier);
          }
          
          this.message = this.$t('suppliers.success.updated');
          this.isSuccess = true;
          this.editingSupplier = null;
        } catch (error) {
          this.message = `${this.$t('suppliers.errors.updateFailed')} ${error.message}`;
          this.isSuccess = false;
          console.error('Error updating supplier:', error);
        } finally {
          this.saving = false;
        }
      }
    }
  };
  </script>
  
  <style lang="scss">
@use '@/assets/scss/main';
// TODO: Implement backend for saving additional info
</style>