<template>
  <div>
    <h3>{{ $t('forms.receiver.title') }}</h3>
    <form @submit.prevent="addReceiver" class="add-receiver-form">
      <div>
        <label for="receiverName">{{ $t('forms.receiver.name') }}</label>
        <input type="text" id="receiverName" v-model="newReceiver.name" required />
      </div>
      <!-- Add more fields as needed -->
      <div class="form-buttons">
        <button type="submit" class="submit-btn" :disabled="loading">
          {{ loading ? $t('forms.receiver.adding') : $t('forms.receiver.add') }}
        </button>
        <button type="button" class="cancel-btn" @click="cancel">
          {{ $t('common.cancel') }}
        </button>
      </div>
    </form>
    <div v-if="message" :class="{ 'success': success, 'error': !success }">
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

<style scoped>
/* Styles for the modal */
.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {
  background-color: white;
  padding: 20px;
  border-radius: 5px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
  width: 400px;
}

.add-receiver-form {
  display: flex;
  flex-direction: column;
}

.add-receiver-form div {
  margin-bottom: 10px;
}

.add-receiver-form label {
  margin-bottom: 5px;
}

.add-receiver-form input {
  padding: 8px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.form-buttons {
  display: flex;
  justify-content: space-between;
  gap: 10px;
  margin-top: 15px;
}

.form-buttons button {
  padding: 10px;
  font-size: 16px;
  cursor: pointer;
  border: none;
  border-radius: 4px;
  flex: 1;
}

.submit-btn {
  background-color: #42b983;
  color: white;
}

.submit-btn:hover {
  background-color: #3aa876;
}

.cancel-btn {
  background-color: #f3f3f3;
  color: #333;
  border: 1px solid #ddd;
}

.cancel-btn:hover {
  background-color: #e7e7e7;
}

.success {
  color: green;
  margin-top: 10px;
}

.error {
  color: red;
  margin-top: 10px;
}
</style>