<template>
  <div id="app">
    <header>
      <nav>
        <router-link to="/">{{ $t('navigation.home') }}</router-link> |
        <router-link to="/create-card">{{ $t('navigation.createCard') }}</router-link>
      </nav>
      <LanguageSwitcher class="language-switcher" />
    </header>
    
    <main>
      <router-view />
    </main>
  </div>
</template>

<script>
import LanguageSwitcher from './components/LanguageSwitcher.vue'

export default {
  components: {
    LanguageSwitcher
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  margin: 0;
  padding: 0;
}

header {
  background-color: #f8f9fa;
  padding: 15px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #eee;
}

nav {
  display: flex;
  gap: 20px;
}

nav a {
  text-decoration: none;
  color: #2c3e50;
  font-weight: 500;
}

nav a.router-link-exact-active {
  color: #007bff;
}

main {
  padding: 20px;
}

.language-switcher {
  margin-left: auto;
}
</style>

