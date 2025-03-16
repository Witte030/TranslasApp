<template>
  <div class="entity-modal">
    <h3 class="entity-modal__title">{{ $t('forms.carrier.title') }}</h3>
    <form @submit.prevent="addCarrier" class="entity-modal__form">
      <div class="entity-modal__form-group">
        <label class="entity-modal__label" for="carrierName">{{ $t('forms.carrier.name') }}</label>
        <input class="entity-modal__input" type="text" id="carrierName" v-model="newCarrier.name" required />
      </div>
      <!-- Add more fields as needed -->
      <div class="entity-modal__buttons">
        <button type="submit" 
                class="entity-modal__button entity-modal__button--submit" 
                :disabled="loading">
          {{ loading ? $t('forms.carrier.adding') : $t('forms.carrier.add') }}
        </button>
        <button type="button" 
                class="entity-modal__button entity-modal__button--cancel" 
                @click="cancel">
          {{ $t('common.cancel') }}
        </button>
      </div>
    </form>
    <div v-if="message" 
         class="entity-modal__message"
         :class="{ 
           'entity-modal__message--success': success, 
           'entity-modal__message--error': !success 
         }">
      {{ message }}
    </div>
  </div>
</template>

<script>
export default {
  name: 'CarrierModal',
  data() {
    return {
      newCarrier: {
        name: ''
      },
      message: '',
      success: false,
      loading: false
    };
  },
  methods: {
    async addCarrier() {
      this.loading = true;
      try {
        const response = await fetch('/api/carrier', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
          },
          body: JSON.stringify(this.newCarrier)
        });

        if (!response.ok) {
          throw new Error(`${this.$t('common.error')} ${response.status}`);
        }

        const result = await response.json();
        console.log('Carrier added:', result);
        this.message = this.$t('forms.carrier.success');
        this.success = true;

        // Reset the form
        this.newCarrier = {
          name: ''
        };

        // Emit an event to notify the parent component to reload carriers
        this.$emit('carrier-added');

        // Close the modal
        this.$emit('close');

      } catch (error) {
        console.error('Error:', error);
        this.message = this.$t('forms.carrier.error') + error.message;
        this.success = false;
      } finally {
        this.loading = false;
      }
    },
    
    cancel() {
      // Simply emit the close event to close the modal without saving
      this.$emit('close');
    }
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';
</style>