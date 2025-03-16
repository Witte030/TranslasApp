<template>
  <div class="home">
    <h1 class="home__title">{{ $t('home.welcome') }}</h1>
    <p class="home__intro">{{ $t('home.intro') }}</p>
    <button @click="navigateToCreateCard" class="home__button">{{ $t('home.createCard') }}</button>
  </div>
</template>
  
<script>
export default {
  name: 'HomePage',
  methods: {
    navigateToCreateCard() {
      this.$router.push('/create-card');
    }
  }
};
</script>
  
<style lang="scss">
@use '@/assets/scss/main';
</style>