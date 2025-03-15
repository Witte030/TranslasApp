import { createI18n } from 'vue-i18n'
import en from '../locales/en.json'
import nl from '../locales/nl.json'

// Get browser language or use stored preference
const getBrowserLocale = () => {
  // Try to get from localStorage first
  const storedLocale = localStorage.getItem('user-locale')
  if (storedLocale) {
    return storedLocale
  }
  
  // Otherwise detect from browser
  const navigatorLocale = navigator.language.split('-')[0]
  return navigatorLocale === 'nl' ? 'nl' : 'en' // Default to English if not Dutch
}

export const i18n = createI18n({
  legacy: false, // Set to false to use Composition API
  globalInjection: true, // Make $t available in templates
  locale: getBrowserLocale(),
  fallbackLocale: 'en',
  messages: {
    en,
    nl
  }
})

// Helper to store user language preference
export const setLocaleInStorage = (locale) => {
  localStorage.setItem('user-locale', locale)
}