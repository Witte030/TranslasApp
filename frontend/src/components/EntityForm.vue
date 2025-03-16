<template>
    <div class="entity-modal">
      <h3 class="entity-modal__title">{{ title }}</h3>
      <form @submit.prevent="submitForm" class="entity-modal__form">
        <div class="entity-modal__form-group">
          <label class="entity-modal__label" :for="entityType + 'Name'">{{ nameLabel }}</label>
          <input 
            class="entity-modal__input" 
            type="text" 
            :id="entityType + 'Name'" 
            v-model="entityData.name" 
            required 
          />
        </div>
        <!-- Slot for additional fields -->
        <slot name="additional-fields"></slot>
        
        <div class="entity-modal__buttons">
          <button 
            type="submit" 
            class="entity-modal__button entity-modal__button--submit" 
            :disabled="loading"
          >
            {{ loading ? addingText : addText }}
          </button>
          <button 
            type="button" 
            class="entity-modal__button entity-modal__button--cancel" 
            @click="cancel"
          >
            {{ cancelText }}
          </button>
        </div>
      </form>
      <div 
        v-if="message" 
        class="entity-modal__message"
        :class="{ 
          'entity-modal__message--success': success, 
          'entity-modal__message--error': !success 
        }"
      >
        {{ message }}
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'EntityForm',
    props: {
      entityType: {
        type: String,
        required: true
      },
      title: {
        type: String,
        required: true
      },
      nameLabel: {
        type: String,
        required: true
      },
      addText: {
        type: String,
        required: true
      },
      addingText: {
        type: String,
        required: true
      },
      successMessage: {
        type: String,
        required: true
      },
      errorMessage: {
        type: String,
        required: true
      },
      cancelText: {
        type: String,
        default: 'Cancel'
      },
      apiEndpoint: {
        type: String,
        required: true
      }
    },
    data() {
      return {
        entityData: {
          name: ''
        },
        message: '',
        success: false,
        loading: false
      };
    },
    methods: {
      async submitForm() {
        this.loading = true;
        try {
          const response = await fetch(this.apiEndpoint, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
              'Accept': 'application/json'
            },
            body: JSON.stringify(this.entityData)
          });
  
          if (!response.ok) {
            throw new Error(`${this.$t('common.error')} ${response.status}`);
          }
  
          const result = await response.json();
          console.log(`${this.entityType} added:`, result);
          this.message = this.successMessage;
          this.success = true;
  
          // Reset the form
          this.entityData = {
            name: ''
          };
  
          // Emit an event to notify the parent component
          this.$emit(`${this.entityType.toLowerCase()}-added`);
  
          // Close the modal
          this.$emit('close');
  
        } catch (error) {
          console.error('Error:', error);
          this.message = this.errorMessage + error.message;
          this.success = false;
        } finally {
          this.loading = false;
        }
      },
      
      cancel() {
        this.$emit('close');
      }
    }
  };
  </script>
  
  <style lang="scss">
  @use '@/assets/scss/main';
  </style>