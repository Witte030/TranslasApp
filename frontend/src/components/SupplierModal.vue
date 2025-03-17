<template>
  <EntityForm
    entityType="supplier"
    :title="$t('forms.supplier.title')"
    :nameLabel="$t('forms.supplier.name')"
    :addText="$t('forms.supplier.add')"
    :addingText="$t('forms.supplier.adding')"
    :successMessage="$t('forms.supplier.success')"
    :errorMessage="$t('forms.supplier.error')"
    :cancelText="$t('common.cancel')"
    apiEndpoint="/api/supplier"
    @supplier-added="$emit('supplier-added')"
    @close="$emit('close')"
  />
</template>

<script>
import EntityForm from './Entity-modal.vue';

export default {
  name: 'SupplierModal',
  components: {
    EntityForm
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';
</style>