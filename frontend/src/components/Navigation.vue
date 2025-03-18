<template>
  <nav class="nav">
    <div class="nav__container">
      <router-link :to="{ name: 'Home' }" class="nav__logo"> TranslasApp </router-link>

      <button
        class="nav__toggle"
        aria-label="Toggle menu"
        :aria-expanded="isMenuOpen"
        @click="toggleMenu"
      >
        <span class="nav__toggle-icon"></span>
      </button>

      <div class="nav__menu" :class="{ 'nav__menu--open': isMenuOpen }">
        <ul class="nav__list">
          <li class="nav__item">
            <router-link :to="{ name: 'Home' }" class="nav__link">{{
              $t("navigation.home")
            }}</router-link>
          </li>
          <li class="nav__item">
            <router-link :to="{ name: 'CreateCard' }" class="nav__link">{{
              $t("navigation.createCard")
            }}</router-link>
          </li>
          <li class="nav__item">
            <router-link :to="{ name: 'SupplierManagement' }" class="nav__link">{{
              $t("navigation.manageSuppliers")
            }}</router-link>
          </li>
          <li class="nav__item nav__item--language-dropdown">
            <div class="language-dropdown">
              <button
                class="language-dropdown__toggle"
                @click.stop="toggleLanguageDropdown"
                :class="{
                  'language-dropdown__toggle--active': isLanguageDropdownOpen,
                }"
              >
                <span>{{ currentLanguageLabel }}</span>
                <span class="language-dropdown__arrow"></span>
              </button>
              <div
                class="language-dropdown__menu"
                v-if="isLanguageDropdownOpen"
              >
                <button
                  v-for="lang in availableLanguages"
                  :key="lang.code"
                  @click="switchLanguage(lang.code)"
                  class="language-dropdown__item"
                  :class="{
                    'language-dropdown__item--active':
                      currentLocale === lang.code,
                  }"
                >
                  {{ lang.label }}
                </button>
              </div>
            </div>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script>
import { setLocaleInStorage, getLocale } from "../i18n";

export default {
  name: "AppNavigation",
  data() {
    return {
      isMenuOpen: false,
      isLanguageDropdownOpen: false,
      currentLocale: getLocale() || "nl",
      availableLanguages: [
        { code: "nl", label: "Nederlands" },
        { code: "en", label: "English" },
      ],
    };
  },
  computed: {
    currentLanguageLabel() {
      const lang = this.availableLanguages.find(
        (lang) => lang.code === this.currentLocale
      );
      return lang ? lang.label : "Nederlands";
    },
  },
  methods: {
    toggleMenu() {
      this.isMenuOpen = !this.isMenuOpen;
      if (this.isMenuOpen) {
        this.isLanguageDropdownOpen = false;
      }
    },
    closeMenu() {
      this.isMenuOpen = false;
    },
    toggleLanguageDropdown(event) {
      event.preventDefault();
      this.isLanguageDropdownOpen = !this.isLanguageDropdownOpen;
    },
    switchLanguage(locale) {
      try {
        // Update the locale in storage and i18n instance
        setLocaleInStorage(locale);
        // Update our component's state
        this.currentLocale = locale;
        this.isLanguageDropdownOpen = false; // Close dropdown after selection
        console.log(`Language switched to: ${locale}`);
      } catch (error) {
        console.error("Error switching language:", error);
      }
    },
  },
  watch: {
    $route() {
      this.closeMenu();
      this.isLanguageDropdownOpen = false;
    },
  },
  mounted() {
    // Store the handler as a property so we can remove it later
    this.handleClickOutside = (event) => {
      const dropdown = this.$el.querySelector(".language-dropdown");
      if (dropdown && !dropdown.contains(event.target)) {
        this.isLanguageDropdownOpen = false;
      }
    };

    // Add event listener
    document.addEventListener("click", this.handleClickOutside);
  },
  beforeUnmount() {
    // Use the stored handler to remove the listener
    document.removeEventListener("click", this.handleClickOutside);
  },
};
</script>

<style lang="scss">
@use "@/assets/scss/main";
</style>
