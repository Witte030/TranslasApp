<template>
  <div class="language-switcher">
    <button 
      @click="switchLanguage('nl')"
      :class="{ 'language-switcher__button--active': currentLocale === 'nl' }"
      class="language-switcher__button">
      NL
    </button>
    <button 
      @click="switchLanguage('en')"
      :class="{ 'language-switcher__button--active': currentLocale === 'en' }"
      class="language-switcher__button">
      EN
    </button>
  </div>
</template>

<script>
import { setLocaleInStorage, getLocale } from '../i18n';

export default {
  name: 'LanguageSwitcher',
  
  data() {
    return {
      currentLocale: getLocale() || 'nl'
    };
  },

  methods: {
    switchLanguage(locale) {
      try {
        // Update the locale in storage and i18n instance
        setLocaleInStorage(locale);
        // Update our component's state
        this.currentLocale = locale;
        console.log(`Language switched to: ${locale}`);
      } catch (error) {
        console.error('Error switching language:', error);
      }
    }
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';
</style>