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
export default {
  name: 'EntityModal',
  props: {
    // Type of entity (receiver, supplier, carrier)
    entityType: {
      type: String,
      required: true,
      validator: value => ['receiver', 'supplier', 'carrier'].includes(value)
    }
  },
  data() {
    return {
      entityData: {
        name: ''  // Initialize with empty string instead of undefined
      },
      loading: false,
      message: '',
      isSuccess: false
    };
  },
  computed: {
    title() {
      return this.$t(`forms.${this.entityType}.title`);
    },
    labelText() {
      return this.$t(`forms.${this.entityType}.name`);
    },
    submitText() {
      return this.$t(`forms.${this.entityType}.add`);
    },
    loadingText() {
      return this.$t(`forms.${this.entityType}.adding`);
    },
    emitEvent() {
      return `${this.entityType}-added`;
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
        const response = await fetch(`/api/${this.entityType}`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            name: this.entityData.name
          })
        });

        const data = await response.json();

        if (!response.ok) {
          throw new Error(data.message || 'Unknown error occurred');
        }

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
  
  <style lang="scss">
  @use '@/assets/scss/main';
  </style>