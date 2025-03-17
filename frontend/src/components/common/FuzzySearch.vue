<template>
  <div class="fuzzy-search">
    <div class="fuzzy-search__container">
      <input 
        type="text" 
        :placeholder="placeholder"
        v-model="searchQuery" 
        class="fuzzy-search__input"
        @keydown.down.prevent="navigateDown"
        @keydown.up.prevent="navigateUp"
        @keydown.enter.prevent="selectHighlighted"
        @keydown.esc="clearSearch"
        ref="searchInput"
      />
      
      <div v-if="searchQuery && filteredItems.length > 0" class="fuzzy-search__results">
        <div 
          v-for="(item, index) in filteredItems" 
          :key="item.value"
          class="fuzzy-search__result-item"
          :class="{ 
            'fuzzy-search__result-item--best': index === 0,
            'fuzzy-search__result-item--highlighted': highlightedIndex === index
          }"
          @click="selectItem(item)"
          @mouseover="highlightedIndex = index"
          @mouseleave="mouseleaveHandler"
          ref="resultItems"
        >
          <div v-html="highlightMatches(item.text)" class="fuzzy-search__result-text"></div>
          <span v-if="index === 0" class="fuzzy-search__best-match">Best match</span>
        </div>
      </div>
      
      <select 
        :id="id" 
        v-model="selectedValue" 
        :required="required"
        class="fuzzy-search__select"
      >
        <option value="" disabled selected>{{ defaultOption }}</option>
        <option v-for="item in items" :key="item.value" :value="item.value">
          {{ item.text }}
        </option>
      </select>
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
    fuseOptions: {
      type: Object,
      default: () => ({})
    }
  },
  data() {
    return {
      searchQuery: '',
      fuse: null,
      selectedValue: this.value,
      highlightedIndex: -1,
      isKeyboardNavigation: false
    };
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
    },
    selectedValue(newValue) {
      this.$emit('input', newValue);
      if (newValue) {
        const selectedItem = this.items.find(item => item.value === newValue);
        if (selectedItem) {
          this.$emit('item-selected', selectedItem);
        }
      }
    },
    searchQuery() {
      // Reset highlighted index when search query changes
      this.highlightedIndex = -1;
    },
    filteredItems() {
      // Default highlight the first item when results change
      this.$nextTick(() => {
        if (this.filteredItems.length > 0 && this.isKeyboardNavigation) {
          this.highlightedIndex = 0;
        }
      });
    }
  },
  computed: {
    searchResults() {
      if (!this.searchQuery || !this.fuse) {
        return [];
      }
      
      return this.fuse.search(this.searchQuery);
    },
    filteredItems() {
      if (!this.searchQuery || !this.fuse) {
        return this.items;
      }
      
      // Use the searchResults computed property instead of modifying data
      return this.searchResults.map(result => result.item);
    }
  },
  methods: {
    initFuse(items) {
      const defaultOptions = {
        keys: ['text'],
        threshold: 0.4,
        includeScore: true,
        includeMatches: true, // Include match info for highlighting
        ignoreLocation: true
      };
      
      const options = { ...defaultOptions, ...this.fuseOptions };
      this.fuse = new Fuse(items || [], options);
    },
    
    navigateDown() {
      this.isKeyboardNavigation = true;
      
      if (this.filteredItems.length === 0) {
        return;
      }
      
      if (this.highlightedIndex < this.filteredItems.length - 1) {
        this.highlightedIndex++;
        this.scrollToHighlighted();
      } else {
        // Wrap around to the beginning
        this.highlightedIndex = 0;
        this.scrollToHighlighted();
      }
    },
    
    navigateUp() {
      this.isKeyboardNavigation = true;
      
      if (this.filteredItems.length === 0) {
        return;
      }
      
      if (this.highlightedIndex > 0) {
        this.highlightedIndex--;
        this.scrollToHighlighted();
      } else {
        // Wrap around to the end
        this.highlightedIndex = this.filteredItems.length - 1;
        this.scrollToHighlighted();
      }
    },
    
    selectHighlighted() {
      if (this.highlightedIndex >= 0 && this.highlightedIndex < this.filteredItems.length) {
        this.selectItem(this.filteredItems[this.highlightedIndex]);
        this.$refs.searchInput.blur(); // Remove focus from input after selection
      }
    },
    
    scrollToHighlighted() {
      this.$nextTick(() => {
        if (!this.$refs.resultItems) return;
        
        const highlighted = this.$refs.resultItems[this.highlightedIndex];
        if (highlighted && highlighted.scrollIntoView) {
          // Scroll the highlighted item into view if needed
          highlighted.scrollIntoView({ block: 'nearest', behavior: 'smooth' });
        }
      });
    },
    
    clearSearch() {
      this.searchQuery = '';
      this.$refs.searchInput.blur();
    },
    
    mouseleaveHandler() {
      // Only clear highlight if not using keyboard navigation
      if (!this.isKeyboardNavigation) {
        this.highlightedIndex = -1;
      }
    },
    
    highlightMatches(text) {
      if (!this.searchQuery || this.searchQuery.length < 2) {
        return text;
      }
      
      try {
        // Find the corresponding search result for this text using the computed property
        const result = this.searchResults.find(r => r.item.text === text);
        
        if (!result || !result.matches || !result.matches.length) {
          return text;
        }
        
        // Get matches for the 'text' key
        const match = result.matches.find(m => m.key === 'text');
        
        if (!match || !match.indices || !match.indices.length) {
          return text;
        }
        
        // Sort indices to process from end to start to avoid offset issues
        const indices = [...match.indices].sort((a, b) => b[0] - a[0]);
        
        let highlighted = text;
        
        // Replace each matched substring with a highlighted version
        indices.forEach(([start, end]) => {
          const matchedText = text.substring(start, end + 1);
          highlighted = highlighted.substring(0, start) + 
                      `<span class="fuzzy-search__highlight">${matchedText}</span>` + 
                      highlighted.substring(end + 1);
        });
        
        return highlighted;
      } catch (error) {
        console.error('Error highlighting matches:', error);
        return text;
      }
    },
    
    selectItem(item) {
      this.selectedValue = item.value;
      this.searchQuery = '';
      this.highlightedIndex = -1;
      this.isKeyboardNavigation = false;
      this.$emit('item-selected', item);
    }
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';
</style>