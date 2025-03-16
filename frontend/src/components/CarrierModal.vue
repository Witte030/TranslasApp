<template>
  <EntityForm
    entityType="carrier"
    :title="$t('forms.carrier.title')"
    :nameLabel="$t('forms.carrier.name')"
    :addText="$t('forms.carrier.add')"
    :addingText="$t('forms.carrier.adding')"
    :successMessage="$t('forms.carrier.success')"
    :errorMessage="$t('forms.carrier.error')"
    :cancelText="$t('common.cancel')"
    apiEndpoint="/api/carrier"
    @carrier-added="$emit('carrier-added')"
    @close="$emit('close')"
  />
</template>

<script>
import EntityForm from './EntityForm.vue';

export default {
  name: 'CarrierModal',
  components: {
    EntityForm
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';
</style>