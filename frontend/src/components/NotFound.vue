<template>
    <div class="not-found">
      <h1>404 - {{ $t('errors.pageNotFound.title') }}</h1>
      <p>{{ $t('errors.pageNotFound.message') }}</p>
      <router-link :to="{ name: 'Home' }" class="not-found__link">
        {{ $t('errors.pageNotFound.goHome') }}
      </router-link>
    </div>
  </template>
  
  <script>
  export default {
    name: 'NotFound'
  }
  </script>
  
  <style lang="scss">
  .not-found {
    text-align: center;
    padding: 2rem;
    max-width: 600px;
    margin: 0 auto;
    
    h1 {
      margin-bottom: 1rem;
      color: var(--color-primary);
    }
    
    &__link {
      display: inline-block;
      margin-top: 1.5rem;
      padding: 0.5rem 1rem;
      background-color: var(--color-primary);
      color: white;
      text-decoration: none;
      border-radius: 4px;
      
      &:hover {
        background-color: var(--color-primary-dark);
      }
    }
  }
  </style>