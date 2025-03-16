<template>
    <transition name="modal-fade">
      <div class="modal" v-if="isVisible" @click.self="closeOnClickOutside ? close() : null">
        <div class="modal-content" role="dialog" aria-labelledby="modal-title">
          <div v-if="showCloseButton" class="modal__close-button" @click="close">×</div>
          <slot></slot>
        </div>
      </div>
    </transition>
  </template>
  
  <script>
  export default {
    name: 'Modal',
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
      }
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
      // Add escape key listener to close modal
      const handleEscape = (e) => {
        if (e.key === 'Escape' && this.isVisible) {
          this.close();
        }
      };
      
      document.addEventListener('keydown', handleEscape);
      
      // Cleanup listener when component is destroyed
      this.$once('hook:beforeDestroy', () => {
        document.removeEventListener('keydown', handleEscape);
      });
    }
  };
  </script>
  
  <style lang="scss">
  @use '@/assets/scss/main';
  
  /* Additional modal-specific styles */
  .modal-fade-enter-active, .modal-fade-leave-active {
    transition: opacity 0.3s;
  }
  
  .modal-fade-enter, .modal-fade-leave-to {
    opacity: 0;
  }
  
  .modal__close-button {
    position: absolute;
    top: 10px;
    right: 15px;
    font-size: 24px;
    font-weight: bold;
    cursor: pointer;
    color: var(--color-gray);
    line-height: 1;
    
    &:hover {
      color: var(--color-gray-dark);
    }
  }
  </style>