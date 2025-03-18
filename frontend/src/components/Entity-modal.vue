<template>
  <div class="entity-modal">
    <h2 class="entity-modal__title">{{ title }}</h2>
    
    <form @submit.prevent="submitForm" class="entity-modal__form">
      <div class="entity-modal__form-group">
        <label class="entity-modal__label" for="entityName">{{ labelText }}</label>
        <input 
          type="text" 
          id="entityName" 
          v-model="entityData.name" 
          required
          class="entity-modal__input"
          :disabled="loading"
          ref="nameInput"
        />
      </div>
      
      <div class="entity-modal__buttons">
        <button 
          type="button" 
          class="entity-modal__button entity-modal__button--cancel" 
          @click="$emit('close')"
          :disabled="loading"
        >
          {{ $t('common.cancel') }}
        </button>
        
        <button 
          type="submit" 
          class="entity-modal__button entity-modal__button--submit" 
          :disabled="loading || !entityData.name"
        >
          {{ loading ? loadingText : submitText }}
          {{ $t('common.submit') }}
        </button>
      </div>
      
      <div 
        v-if="message" 
        class="entity-modal__message" 
        :class="{
          'entity-modal__message--success': isSuccess,
          'entity-modal__message--error': !isSuccess
        }"
      >
        {{ message }}
      </div>
    </form>
  </div>
</template>

<script>
import { getEntityService } from '@/services/api/entityService';

export default {
  name: 'EntityModal',
  props: {
    entityType: {
      type: String,
      required: true,
      validator: value => ['receiver', 'supplier', 'carrier'].includes(value)
    }
  },
  data() {
    return {
      entityData: {
        name: ''
      },
      loading: false,
      message: '',
      isSuccess: false
    };
  },
  computed: {
    // Existing computed properties...
    
    // Get the appropriate service for this entity type
    entityService() {
      return getEntityService(this.entityType);
    }
  },
  mounted() {
    // Focus the name input when the modal opens
    this.$nextTick(() => {
      if (this.$refs.nameInput) {
        this.$refs.nameInput.focus();
      }
    });
  },
  methods: {
    async submitForm() {
      if (!this.entityData.name) {
        return;
      }

      this.loading = true;
      this.message = '';

      try {
        // Use the entity service instead of direct fetch
        const data = await this.entityService.create({ 
          name: this.entityData.name 
        });
        
        // Show success message
        this.message = this.$t(`forms.${this.entityType}.success`);
        this.isSuccess = true;

        // Emit event to refresh entity list
        this.$emit(this.emitEvent, data);

        // Reset form after success
        setTimeout(() => {
          this.entityData.name = '';
          this.$emit('close');
        }, 1500);
      } catch (error) {
        // Show error message
        this.message = `${this.$t(`forms.${this.entityType}.error`)} ${error.message}`;
        this.isSuccess = false;
        console.error(`Error adding ${this.entityType}:`, error);
      } finally {
        this.loading = false;
      }
    }
  }
};

</script>

<style scoped lang="scss">
@use '@/assets/scss/abstracts' as *;
@use '@/assets/scss/components/_entity-modal';
</style>