import { createI18n } from 'vue-i18n';
import messages from '../locales';

// Get the stored locale from localStorage or use default
const storedLocale = localStorage.getItem('locale') || 'nl';

// Create the i18n instance
const i18n = createI18n({
  legacy: false,
  globalInjection: true,
  locale: storedLocale,
  fallbackLocale: 'en',
  messages
});

// Function to set locale in storage
export function setLocaleInStorage(locale) {
  localStorage.setItem('locale', locale);
  i18n.global.locale.value = locale;
}

// Helper function to get current locale
export function getLocale() {
  return i18n.global.locale.value;
}

export default i18n;