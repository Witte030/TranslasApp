<template>
  <div id="app" :key="$i18n.locale">
    <AppNavigation />
    <main class="main-content">
      <router-view />
    </main>
  </div>
</template>

<script>
import AppNavigation from './components/Navigation.vue';

export default {
  name: 'App',
  components: {
    AppNavigation
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';

#app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.main-content {
  flex: 1;
  padding: 1.5rem;
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
  
  @media (max-width: 768px) {
    padding: 1rem;
  }
}
</style>

