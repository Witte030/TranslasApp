<template>
  <transition name="modal-fade">
    <div class="modal" v-if="isVisible" @click.self="closeOnClickOutside ? close() : null">
      <div class="modal-content" role="dialog" :aria-labelledby="title ? 'modal-title' : undefined">
        <button v-if="showCloseButton" class="modal__close-button" @click="close" aria-label="Close" type="button">×</button>
        <h2 v-if="title" id="modal-title" class="modal__title">{{ title }}</h2>
        <div class="modal__body">
          <slot></slot>
        </div>
        <div class="modal__footer" v-if="$slots.footer">
          <slot name="footer"></slot>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
export default {
  name: 'ModalDialog',
  props: {
    /**
     * Controls whether the modal is visible or not
     */
    isVisible: {
      type: Boolean,
      default: false
    },
    /**
     * Determines if clicking outside the modal should close it
     */
    closeOnClickOutside: {
      type: Boolean,
      default: true
    },
    /**
     * Whether to show a close button in the top-right corner
     */
    showCloseButton: {
      type: Boolean,
      default: false
    },
    /**
     * Optional title for accessibility
     */
    title: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      // Store the event handler reference
      escapeHandler: null
    };
  },
  watch: {
    isVisible(newVal) {
      // When modal becomes visible
      if (newVal) {
        // Prevent scrolling on the body when modal is open
        document.body.style.overflow = 'hidden';
      } else {
        // Re-enable scrolling when modal is closed
        document.body.style.overflow = '';
      }
    }
  },
  methods: {
    /**
     * Emits close event to parent component
     */
    close() {
      this.$emit('close');
    }
  },
  mounted() {
    // Create and store the escape key handler
    this.escapeHandler = (e) => {
      if (e.key === 'Escape' && this.isVisible) {
        this.close();
      }
    };
    
    // Add escape key listener to close modal
    document.addEventListener('keydown', this.escapeHandler);
  },
  // Changed from beforeDestroy to beforeUnmount for Vue 3 compatibility
  beforeUnmount() {
    // Remove the event listener using the stored handler reference
    if (this.escapeHandler) {
      document.removeEventListener('keydown', this.escapeHandler);
    }
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';

</style>