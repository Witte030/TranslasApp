<template>
  <div>
    <h1>{{ $t('home.welcome') }}</h1>
    <p>{{ $t('home.intro') }}</p>
    <button @click="navigateToCreateCard">{{ $t('home.createCard') }}</button>
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
  
<style scoped>
h1 {
  font-size: 2em;
  margin-bottom: 0.5em;
}
p {
  margin-bottom: 1em;
}
</style>