<template>
  <div class="card-list-container">
    <h1>{{ $t('cards.title') }}</h1>
    
    <div class="filters">
      <div class="filter-group">
        <label for="priorityFilter">{{ $t('cards.filters.priority') }}</label>
        <select id="priorityFilter" v-model="priorityFilter">
          <option value="">{{ $t('cards.filters.allPriorities') }}</option>
          <option value="0">{{ $t('cards.filters.low') }}</option>
          <option value="1">{{ $t('cards.filters.medium') }}</option>
          <option value="2">{{ $t('cards.filters.high') }}</option>
        </select>
      </div>
      
      <div class="filter-group">
        <label for="handledFilter">{{ $t('cards.filters.show') }}</label>
        <select id="handledFilter" v-model="handledFilter">
          <option value="">{{ $t('cards.filters.allCards') }}</option>
          <option value="true">{{ $t('cards.filters.handledOnly') }}</option>
          <option value="false">{{ $t('cards.filters.unhandledOnly') }}</option>
        </select>
      </div>
      
      <button @click="loadCards" class="refresh-btn">
        <span>{{ $t('cards.actions.refresh') }}</span>
      </button>
    </div>
    
    <div v-if="loading" class="loading">
      <p>{{ $t('cards.messages.loading') }}</p>
    </div>
    
    <div v-else-if="error" class="error">
      <p>{{ error }}</p>
      <button @click="loadCards">{{ $t('cards.actions.tryAgain') }}</button>
    </div>
    
    <div v-else-if="filteredCards.length === 0" class="no-cards">
      <p>{{ $t('cards.messages.noCards') }}</p>
      <router-link to="/create-card" class="create-btn">{{ $t('cards.actions.createNew') }}</router-link>
    </div>
    
    <div v-else class="cards-list-view">
      <div 
        v-for="card in filteredCards" 
        :key="card.id" 
        class="card"
        :class="{ 'high-priority': card.priority === 2, 'medium-priority': card.priority === 1, 'handled': card.isHandled }"
      >
        <div class="card-header">
          <div class="card-header-info">
            <span class="card-date">{{ formatDate(card.date) }}</span>
            <span :class="'priority-badge priority-' + getPriorityClass(card.priority)">
              {{ getPriorityLabelI18n(card.priority) }}
            </span>
          </div>
          <div class="card-status" :class="card.isHandled ? 'status-handled' : 'status-pending'">
            {{ card.isHandled ? $t('cards.status.handled') : $t('cards.status.pending') }}
          </div>
        </div>
        
        <div class="card-body">
          <div class="card-main-info">
            <div class="card-info">
              <span class="label">{{ $t('cards.labels.receiver') }}</span>
              <span class="value">{{ card.receiver?.name || $t('cards.labels.unknown') }}</span>
            </div>
            <div class="card-info">
              <span class="label">{{ $t('cards.labels.supplier') }}</span>
              <span class="value">{{ card.supplier?.name || $t('cards.labels.unknown') }}</span>
            </div>
            <div class="card-info">
              <span class="label">{{ $t('cards.labels.carrier') }}</span>
              <span class="value">{{ card.carrier?.name || $t('cards.labels.unknown') }}</span>
            </div>
          </div>
          
          <div class="card-counts">
            <div class="count-item">
              <span class="count">{{ card.numberOfCollies }}</span>
              <span class="count-label">{{ $t('cards.labels.collies') }}</span>
            </div>
            <div class="count-item">
              <span class="count">{{ card.numberOfPallets }}</span>
              <span class="count-label">{{ $t('cards.labels.pallets') }}</span>
            </div>
            <div class="count-item">
              <span class="count">{{ card.numberOfBundels }}</span>
              <span class="count-label">{{ $t('cards.labels.bundels') }}</span>
            </div>
          </div>
        </div>
        
        <div class="card-footer">
          <button 
            @click="toggleHandled(card)" 
            :class="card.isHandled ? 'btn-handled' : 'btn-mark-handled'"
          >
            {{ card.isHandled ? $t('cards.actions.markUnhandled') : $t('cards.actions.markHandled') }}
          </button>
          <button class="btn-view" @click="viewCardDetails(card.id)">
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

<style scoped>
.card-list-container {
  max-width: 900px; /* Reduced from 1200px for a narrower column */
  margin: 0 auto;
  padding: 20px;
}

h1 {
  color: #333;
  margin-bottom: 20px;
  text-align: center;
}

.filters {
  display: flex;
  justify-content: space-between;
  margin-bottom: 20px;
  padding: 15px;
  background-color: #f8f9fa;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
}

.filter-group {
  display: flex;
  align-items: center;
  gap: 10px;
}

.filter-group label {
  font-weight: 500;
  color: #555;
}

.filter-group select {
  padding: 8px 12px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
}

.refresh-btn {
  padding: 8px 16px;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 5px;
}

.refresh-btn:hover {
  background-color: #45a049;
}

.loading, .error, .no-cards {
  text-align: center;
  padding: 40px;
  background-color: #f8f9fa;
  border-radius: 8px;
  margin-top: 20px;
}

.error {
  color: #dc3545;
}

.create-btn {
  display: inline-block;
  margin-top: 15px;
  padding: 10px 20px;
  background-color: #007bff;
  color: white;
  text-decoration: none;
  border-radius: 4px;
  font-weight: 500;
}

.create-btn:hover {
  background-color: #0069d9;
}

/* Changed from grid to list view */
.cards-list-view {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.card {
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 5px rgba(0,0,0,0.1);
  overflow: hidden;
  transition: transform 0.2s, box-shadow 0.2s;
  border-left: 4px solid #ccc;
}

.card:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(0,0,0,0.1);
}

.high-priority {
  border-left-color: #dc3545;
}

.medium-priority {
  border-left-color: #ffc107;
}

.handled {
  background-color: #f8f9fa;
  opacity: 0.85;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 15px;
  background-color: #f8f9fa;
  border-bottom: 1px solid #eee;
}

.card-header-info {
  display: flex;
  align-items: center;
  gap: 10px;
}

.card-date {
  font-size: 14px;
  color: #777;
}

.card-status {
  font-size: 12px;
  font-weight: 500;
  padding: 4px 8px;
  border-radius: 12px;
}

.status-pending {
  background-color: #ffc107;
  color: #333;
}

.status-handled {
  background-color: #28a745;
  color: white;
}

.priority-badge {
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: 500;
}

.priority-high {
  background-color: #dc3545;
  color: white;
}

.priority-medium {
  background-color: #ffc107;
  color: #333;
}

.priority-low {
  background-color: #6c757d;
  color: white;
}

.card-body {
  padding: 15px;
  display: flex;
  justify-content: space-between;
}

.card-main-info {
  flex: 3;
}

.card-info {
  margin-bottom: 8px;
  display: flex;
  align-items: flex-start;
}

.label {
  font-weight: 500;
  min-width: 80px;
  color: #555;
}

.value {
  color: #333;
  font-weight: 600;
}

.card-counts {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding-left: 15px;
  border-left: 1px dashed #eee;
}

.count-item {
  display: flex;
  align-items: center;
  gap: 10px;
}

.count {
  font-size: 18px;
  font-weight: bold;
  color: #343a40;
}

.count-label {
  font-size: 14px;
  color: #6c757d;
}

.card-footer {
  display: flex;
  padding: 12px 15px;
  background-color: #f8f9fa;
  border-top: 1px solid #eee;
  gap: 10px;
}

.card-footer button {
  flex: 1;
  padding: 8px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 500;
  font-size: 13px;
}

.btn-mark-handled {
  background-color: #28a745;
  color: white;
}

.btn-handled {
  background-color: #17a2b8;
  color: white;
}

.btn-view {
  background-color: #007bff;
  color: white;
}

.btn-mark-handled:hover {
  background-color: #218838;
}

.btn-handled:hover {
  background-color: #138496;
}

.btn-view:hover {
  background-color: #0069d9;
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .filters {
    flex-direction: column;
    gap: 10px;
  }
  
  .filter-group {
    width: 100%;
  }
  
  .filter-group select {
    flex-grow: 1;
  }
  
  .card-body {
    flex-direction: column;
  }
  
  .card-counts {
    flex-direction: row;
    justify-content: space-around;
    padding-left: 0;
    padding-top: 15px;
    margin-top: 15px;
    border-left: 0;
    border-top: 1px dashed #eee;
  }
}
</style>