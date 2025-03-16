<template>
  <div class="language-switcher">
    <button 
      @click="switchLanguage('en')" 
      class="language-switcher__button language-switcher__button--en"
      :class="{ 'language-switcher__button--active': currentLocale === 'en' }"
    >
      <span>English</span>
    </button>
    <button 
      @click="switchLanguage('nl')" 
      class="language-switcher__button language-switcher__button--nl"
      :class="{ 'language-switcher__button--active': currentLocale === 'nl' }"
    >
      <span>Nederlands</span>
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

<style lang="scss">
@use '@/assets/scss/main';
</style>