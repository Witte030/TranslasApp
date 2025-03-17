<template>
  <div class="fuzzy-search" :class="{ 'fuzzy-search--error': showError }">
    <div class="fuzzy-search__container">
      <input 
        type="text" 
        :placeholder="isFocused || !displayValue ? placeholder : ''"
        v-model="searchQuery" 
        class="fuzzy-search__input"
        @keydown.down.prevent="navigateDown"
        @keydown.up.prevent="navigateUp"
        @keydown.enter.prevent="selectHighlighted"
        @keydown.esc="clearSearch"
        @focus="handleFocus"
        @blur="handleBlur"
        @click="handleInputClick"
        @input="handleInputChange"
        ref="searchInput"
        :class="{ 
          'fuzzy-search__input--has-value': displayValue && !isFocused,
          'fuzzy-search__input--error': showError 
        }"
      />
      
      <div 
        v-if="displayValue && !isFocused" 
        class="fuzzy-search__selected-value"
        @click="handleSelectedValueClick"
      >
        {{ displayValue }}
        <button 
          type="button" 
          class="fuzzy-search__clear-button" 
          @click.stop="clearSelection"
          aria-label="Clear selection"
        >×</button>
      </div>
      
      <!-- Single dropdown for both all items and search results -->
      <div v-if="(showDropdown && isFocused)" class="fuzzy-search__results">
        <div v-if="displayedItems.length === 0" class="fuzzy-search__no-results">
          No results found
        </div>
        <div 
          v-for="(item, index) in displayedItems" 
          :key="item.value"
          class="fuzzy-search__result-item"
          :class="{ 
            'fuzzy-search__result-item--best': searchQuery && index === 0,
            'fuzzy-search__result-item--highlighted': highlightedIndex === index,
            'fuzzy-search__result-item--selected': selectedValue === item.value
          }"
          @click="selectItem(item)"
          @mouseover="highlightedIndex = index"
          @mouseleave="mouseleaveHandler"
          ref="resultItems"
        >
          <div v-html="searchQuery ? highlightMatches(item.text) : item.text" class="fuzzy-search__result-text"></div>
          <span v-if="searchQuery && index === 0" class="fuzzy-search__best-match">Best match</span>
        </div>
      </div>
      
      <!-- Hidden input for form validation -->
      <input 
        type="hidden" 
        :id="id" 
        :name="id" 
        :required="required" 
        :value="selectedValue"
      />
      
      <!-- Error message -->
      <div v-if="showError && errorMessage" class="fuzzy-search__error-message">
        {{ errorMessage }}
      </div>
    </div>
  </div>
</template>

<script>
import Fuse from 'fuse.js';

export default {
  name: 'FuzzySearch',
  props: {
    id: {
      type: String,
      required: true
    },
    items: {
      type: Array,
      required: true
    },
    value: {
      type: String,
      default: ''
    },
    placeholder: {
      type: String,
      default: 'Search...'
    },
    defaultOption: {
      type: String,
      default: 'Select an option'
    },
    required: {
      type: Boolean,
      default: true
    },
    errorMessage: {
      type: String,
      default: 'This field is required'
    },
    fuseOptions: {
      type: Object,
      default: () => ({})
    },
    showValidationErrors: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      searchQuery: '',
      fuse: null,
      selectedValue: this.value,
      highlightedIndex: -1,
      isKeyboardNavigation: false,
      isFocused: false,
      blurTimer: null,
      showDropdown: false,
      isDirty: false
    };
  },
  created() {
    // Initialize Fuse right away
    this.initFuse(this.items);
  },
  mounted() {
    // If there's an initial value, make sure to display it
    this.initializeSelectedValue();
  },
  watch: {
    items: {
      immediate: true,
      handler(newItems) {
        this.initFuse(newItems);
      }
    },
    value(newValue) {
      this.selectedValue = newValue;
      
      // Update displayed text when value changes externally
      if (newValue) {
        const selectedItem = this.items.find(item => item.value === newValue);
        if (selectedItem) {
          this.searchQuery = selectedItem.text;
        }
      } else {
        // Clear if value is empty
        this.searchQuery = '';
      }
    },
    showValidationErrors(val) {
      // Mark as dirty when validation is shown
      if (val) {
        this.isDirty = true;
      }
    }
  },
  computed: {
    displayValue() {
      if (this.selectedValue) {
        const selectedItem = this.items.find(item => item.value === this.selectedValue);
        return selectedItem ? selectedItem.text : '';
      }
      return '';
    },
    // Get items to display based on search query
    displayedItems() {
      if (!this.searchQuery || !this.fuse) {
        // If no search query, show all items
        return this.items;
      }
      
      try {
        // Search using Fuse.js
        const results = this.fuse.search(this.searchQuery);
        return results.map(result => result.item);
      } catch (error) {
        console.error('Error searching with Fuse.js:', error);
        return this.items; // Fallback to all items on error
      }
    },
    // Determine if error should be shown
    showError() {
      return this.isDirty && this.required && !this.selectedValue && this.showValidationErrors;
    }
  },
  methods: {
    // Initialize Fuse.js with items
    initFuse(items) {
      if (!items || items.length === 0) return;
      
      try {
        const options = {
          keys: ['text'],
          threshold: 0.4,
          includeScore: true,
          includeMatches: true,
          ignoreLocation: true,
          ...this.fuseOptions
        };
        
        this.fuse = new Fuse(items, options);
      } catch (error) {
        console.error('Error initializing Fuse.js:', error);
      }
    },
    
    initializeSelectedValue() {
      if (this.selectedValue) {
        const selectedItem = this.items.find(item => item.value === this.selectedValue);
        if (selectedItem) {
          this.searchQuery = selectedItem.text;
        }
      }
    },
    
    handleFocus() {
      this.isFocused = true;
      this.isDirty = true; // Mark as dirty on focus
      
      // Always show dropdown on focus
      this.showDropdown = true;
      
      // Clear search if needed
      if (this.selectedValue) {
        const selectedItem = this.items.find(item => item.value === this.selectedValue);
        if (selectedItem && this.searchQuery === selectedItem.text) {
          // Clear search to allow new search
          this.searchQuery = '';
        }
      }
      
      // Clear any pending blur timer
      if (this.blurTimer) {
        clearTimeout(this.blurTimer);
        this.blurTimer = null;
      }
    },
    
    handleBlur() {
      // Use a small delay to allow click events on results to complete
      this.blurTimer = setTimeout(() => {
        this.isFocused = false;
        this.showDropdown = false;
        this.isDirty = true; // Mark as dirty on blur
        
        // If there's a selected value but no search query, show the selected item's text
        if (this.selectedValue && !this.searchQuery) {
          const selectedItem = this.items.find(item => item.value === this.selectedValue);
          if (selectedItem) {
            this.searchQuery = selectedItem.text;
          }
        }
      }, 200); // Slightly longer timeout for reliable clicks
    },
    
    handleInputClick() {
      if (!this.isFocused) {
        this.$refs.searchInput.focus();
      }
      
      // Always show dropdown when clicking input
      this.showDropdown = true;
    },
    
    handleInputChange() {
      // Ensure dropdown is visible when typing
      this.showDropdown = true;
      
      // Reset highlighted index when search changes
      this.highlightedIndex = -1;
    },
    
    handleSelectedValueClick() {
      this.$refs.searchInput.focus();
      this.searchQuery = '';
      this.showDropdown = true;
    },
    
    selectHighlighted() {
      const items = this.displayedItems;
      
      if (items.length === 0) return;
      
      // If an item is highlighted, select that item
      if (this.highlightedIndex >= 0 && this.highlightedIndex < items.length) {
        this.selectItem(items[this.highlightedIndex]);
      } 
      // Otherwise, select the best match (first item in results)
      else if (items.length > 0) {
        this.selectItem(items[0]);
      }
    },
    
    navigateDown() {
      this.isKeyboardNavigation = true;
      const items = this.displayedItems;
      
      if (items.length === 0) return;
      
      if (this.highlightedIndex < items.length - 1) {
        this.highlightedIndex++;
      } else {
        this.highlightedIndex = 0; // Wrap around
      }
    },
    
    navigateUp() {
      this.isKeyboardNavigation = true;
      const items = this.displayedItems;
      
      if (items.length === 0) return;
      
      if (this.highlightedIndex > 0) {
        this.highlightedIndex--;
      } else {
        this.highlightedIndex = items.length - 1; // Wrap around
      }
    },
    
    clearSearch() {
      this.searchQuery = '';
      this.showDropdown = false;
      this.$refs.searchInput.blur();
    },
    
    clearSelection() {
      this.selectedValue = '';
      this.searchQuery = '';
      this.$emit('input', '');
      this.$emit('item-selected', null);
      this.$refs.searchInput.focus();
      this.showDropdown = true;
    },
    
    mouseleaveHandler() {
      if (!this.isKeyboardNavigation) {
        this.highlightedIndex = -1;
      }
    },
    
    selectItem(item) {
      // Clear any pending blur timer
      if (this.blurTimer) {
        clearTimeout(this.blurTimer);
        this.blurTimer = null;
      }
      
      this.selectedValue = item.value;
      this.searchQuery = item.text; // Set input value to selected item's text
      this.highlightedIndex = -1;
      this.isKeyboardNavigation = false;
      this.isFocused = false;
      this.showDropdown = false;
      this.isDirty = true; // Mark as dirty when selecting an item
      this.$emit('input', item.value);
      this.$emit('item-selected', item);
      
      // After selecting, blur the input
      this.$refs.searchInput.blur();
    },
    
    // Method to highlight search query in text
    highlightMatches(text) {
      if (!this.searchQuery || this.searchQuery.length < 1) {
        return text;
      }
      
      try {
        // Simple highlight implementation
        const regex = new RegExp(this.searchQuery.replace(/[-/\\^$*+?.()|[\]{}]/g, '\\$&'), 'gi');
        return text.replace(regex, match => `<span class="fuzzy-search__highlight">${match}</span>`);
      } catch (error) {
        return text;
      }
    }
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';
</style>