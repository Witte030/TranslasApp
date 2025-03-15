<template>
  <div class="container">
    <h1 class="container__title">{{ $t('cards.title') }}</h1>
    
    <div class="filters">
      <div class="filters__group">
        <label class="filters__label" for="priorityFilter">{{ $t('cards.filters.priority') }}</label>
        <select class="filters__select" id="priorityFilter" v-model="priorityFilter">
          <option value="">{{ $t('cards.filters.allPriorities') }}</option>
          <option value="0">{{ $t('cards.filters.low') }}</option>
          <option value="1">{{ $t('cards.filters.medium') }}</option>
          <option value="2">{{ $t('cards.filters.high') }}</option>
        </select>
      </div>
      
      <div class="filters__group">
        <label class="filters__label" for="handledFilter">{{ $t('cards.filters.show') }}</label>
        <select class="filters__select" id="handledFilter" v-model="handledFilter">
          <option value="">{{ $t('cards.filters.allCards') }}</option>
          <option value="true">{{ $t('cards.filters.handledOnly') }}</option>
          <option value="false">{{ $t('cards.filters.unhandledOnly') }}</option>
        </select>
      </div>
      
      <button @click="loadCards" class="refresh-btn">
        <span>{{ $t('cards.actions.refresh') }}</span>
      </button>
    </div>
    
    <div v-if="loading" class="message">
      <p>{{ $t('cards.messages.loading') }}</p>
    </div>
    
    <div v-else-if="error" class="message message--error">
      <p>{{ error }}</p>
      <button @click="loadCards" class="button button--primary">{{ $t('cards.actions.tryAgain') }}</button>
    </div>
    
    <div v-else-if="filteredCards.length === 0" class="message">
      <p>{{ $t('cards.messages.noCards') }}</p>
      <router-link to="/create-card" class="create-btn">{{ $t('cards.actions.createNew') }}</router-link>
    </div>
    
    <div v-else class="cards-list">
      <div 
        v-for="card in filteredCards" 
        :key="card.id" 
        class="card"
        :class="{
          'card--high-priority': card.priority === 2,
          'card--medium-priority': card.priority === 1, 
          'card--handled': card.isHandled 
        }"
      >
        <div class="card__header">
          <div class="card__header-info">
            <span class="card__date">{{ formatDate(card.date) }}</span>
            <span 
              class="card__priority-badge"
              :class="`card__priority-badge--${getPriorityClass(card.priority)}`"
            >
              {{ getPriorityLabelI18n(card.priority) }}
            </span>
          </div>
          <div 
            class="card__status"
            :class="card.isHandled ? 'card__status--handled' : 'card__status--pending'"
          >
            {{ card.isHandled ? $t('cards.status.handled') : $t('cards.status.pending') }}
          </div>
        </div>
        
        <div class="card__body">
          <div class="card__main-info">
            <div class="card__info">
              <span class="card__label">{{ $t('cards.labels.receiver') }}</span>
              <span class="card__value">{{ card.receiver?.name || $t('cards.labels.unknown') }}</span>
            </div>
            <div class="card__info">
              <span class="card__label">{{ $t('cards.labels.supplier') }}</span>
              <span class="card__value">{{ card.supplier?.name || $t('cards.labels.unknown') }}</span>
            </div>
            <div class="card__info">
              <span class="card__label">{{ $t('cards.labels.carrier') }}</span>
              <span class="card__value">{{ card.carrier?.name || $t('cards.labels.unknown') }}</span>
            </div>
          </div>
          
          <div class="card__counts">
            <div class="card__count-item">
              <span class="card__count">{{ card.numberOfCollies }}</span>
              <span class="card__count-label">{{ $t('cards.labels.collies') }}</span>
            </div>
            <div class="card__count-item">
              <span class="card__count">{{ card.numberOfPallets }}</span>
              <span class="card__count-label">{{ $t('cards.labels.pallets') }}</span>
            </div>
            <div class="card__count-item">
              <span class="card__count">{{ card.numberOfBundels }}</span>
              <span class="card__count-label">{{ $t('cards.labels.bundels') }}</span>
            </div>
          </div>
        </div>
        
        <div class="card__footer">
          <button 
            @click="toggleHandled(card)" 
            class="card__button"
            :class="card.isHandled ? 'card__button--handled' : 'card__button--mark-handled'"
          >
            {{ card.isHandled ? $t('cards.actions.markUnhandled') : $t('cards.actions.markHandled') }}
          </button>
          <button class="card__button card__button--view" @click="viewCardDetails(card.id)">
            {{ $t('cards.actions.viewDetails') }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'CardList',
  data() {
    return {
      cards: [],
      loading: true,
      error: null,
      apiBaseUrl: 'http://localhost:5262',
      priorityFilter: '',
      handledFilter: '',
    };
  },
  computed: {
    filteredCards() {
      return this.cards.filter(card => {
        // Filter by priority
        if (this.priorityFilter !== '' && card.priority != this.priorityFilter) {
          return false;
        }
        
        // Filter by handled status
        if (this.handledFilter === 'true' && !card.isHandled) {
          return false;
        }
        if (this.handledFilter === 'false' && card.isHandled) {
          return false;
        }
        
        return true;
      });
    }
  },
  async mounted() {
    await this.loadCards();
  },
  methods: {
    async loadCards() {
      this.loading = true;
      this.error = null;
      
      try {
        const response = await fetch(`${this.apiBaseUrl}/api/card`);
        
        if (!response.ok) {
          throw new Error(`Error ${response.status}: ${response.statusText}`);
        }
        
        this.cards = await response.json();
        
        // Sort cards by priority (high to low) and then by date (newest first)
        this.cards.sort((a, b) => {
          if (b.priority !== a.priority) {
            return b.priority - a.priority;  // Sort by priority desc
          }
          return new Date(b.date) - new Date(a.date);  // Then by date desc
        });
      } catch (error) {
        console.error('Failed to load cards:', error);
        this.error = `${this.$t('cards.messages.failedLoad')} ${error.message}`;
      } finally {
        this.loading = false;
      }
    },
    
    formatDate(dateString) {
  if (!dateString) return this.$t('cards.labels.unknown');
  
  const date = new Date(dateString);
  
  // Options for date formatting with explicit timezone handling
  const options = {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    hour12: false, // Use 24-hour format
    timeZone: Intl.DateTimeFormat().resolvedOptions().timeZone // Use client's timezone
  };
  
  // Format the date according to the current locale
  return new Intl.DateTimeFormat(this.$i18n.locale === 'nl' ? 'nl-NL' : 'en-GB', options).format(date);
},
    
    getPriorityLabelI18n(priority) {
      switch (Number(priority)) {
        case 0: return this.$t('cards.filters.low');
        case 1: return this.$t('cards.filters.medium');
        case 2: return this.$t('cards.filters.high');
        default: return this.$t('cards.labels.unknown');
      }
    },
    
    getPriorityLabel(priority) {
      switch (Number(priority)) {
        case 0: return 'Low';
        case 1: return 'Medium';
        case 2: return 'High';
        default: return 'Unknown';
      }
    },
    
    getPriorityClass(priority) {
      switch (Number(priority)) {
        case 0: return 'low';
        case 1: return 'medium';
        case 2: return 'high';
        default: return 'unknown';
      }
    },
    
    async toggleHandled(card) {
      try {
        const response = await fetch(`${this.apiBaseUrl}/api/card/${card.id}/toggle-handled`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
          }
        });
        
        if (!response.ok) {
          throw new Error(`Error ${response.status}: ${response.statusText}`);
        }
        
        // Update the local card object
        card.isHandled = !card.isHandled;
        
      } catch (error) {
        console.error('Failed to toggle handled status:', error);
        alert(`${this.$t('cards.messages.failedUpdate')} ${error.message}`);
      }
    },
    
    viewCardDetails(cardId) {
      this.$router.push(`/cards/${cardId}`);
    }
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';
</style>