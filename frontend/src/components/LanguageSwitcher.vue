<template>
    <div class="language-switcher">
      <button 
        @click="switchLanguage('en')" 
        :class="{ active: currentLocale === 'en' }"
      >
        🇬🇧 English
      </button>
      <button 
        @click="switchLanguage('nl')" 
        :class="{ active: currentLocale === 'nl' }"
      >
        🇳🇱 Nederlands
      </button>
    </div>
  </template>
  
  <script>
  import { setLocaleInStorage } from '../i18n'
  
  export default {
    name: 'LanguageSwitcher',
    computed: {
      currentLocale() {
        return this.$i18n.locale
      }
    },
    methods: {
      switchLanguage(locale) {
        this.$i18n.locale = locale
        setLocaleInStorage(locale)
        document.documentElement.lang = locale
      }
    }
  }
  </script>
  
  <style scoped>
  .language-switcher {
    display: flex;
    gap: 10px;
  }
  
  .language-switcher button {
    background: none;
    border: 1px solid #ddd;
    border-radius: 4px;
    padding: 5px 10px;
    cursor: pointer;
    font-size: 14px;
    transition: all 0.2s ease;
  }
  
  .language-switcher button:hover {
    background-color: #f8f9fa;
  }
  
  .language-switcher button.active {
    background-color: #007bff;
    color: white;
    border-color: #007bff;
  }
  </style>