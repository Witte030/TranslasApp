<template>
  <div class="entity-modal">
    <h3 class="entity-modal__title">{{ $t('forms.receiver.title') }}</h3>
    <form @submit.prevent="addReceiver" class="entity-modal__form">
      <div class="entity-modal__form-group">
        <label class="entity-modal__label" for="receiverName">{{ $t('forms.receiver.name') }}</label>
        <input class="entity-modal__input" type="text" id="receiverName" v-model="newReceiver.name" required />
      </div>
      <!-- Add more fields as needed -->
      <div class="entity-modal__buttons">
        <button type="submit" 
                class="entity-modal__button entity-modal__button--submit" 
                :disabled="loading">
          {{ loading ? $t('forms.receiver.adding') : $t('forms.receiver.add') }}
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
  name: 'ReceiverModal',
  data() {
    return {
      newReceiver: {
        name: ''
      },
      message: '',
      success: false,
      loading: false
    };
  },
  methods: {
    async addReceiver() {
      this.loading = true;
      try {
        const response = await fetch('/api/receiver', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
          },
          body: JSON.stringify(this.newReceiver)
        });

        if (!response.ok) {
          throw new Error(`${this.$t('common.error')} ${response.status}`);
        }

        const result = await response.json();
        console.log('Receiver added:', result);
        this.message = this.$t('forms.receiver.success');
        this.success = true;

        // Reset the form
        this.newReceiver = {
          name: ''
        };

        // Emit an event to notify the parent component to reload receivers
        this.$emit('receiver-added');

        // Close the modal
        this.$emit('close');

      } catch (error) {
        console.error('Error:', error);
        this.message = this.$t('forms.receiver.error') + error.message;
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