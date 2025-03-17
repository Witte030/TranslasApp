<template>
  <EntityForm
    entityType="receiver"
    :title="$t('forms.receiver.title')"
    :nameLabel="$t('forms.receiver.name')"
    :addText="$t('forms.receiver.add')"
    :addingText="$t('forms.receiver.adding')"
    :successMessage="$t('forms.receiver.success')"
    :errorMessage="$t('forms.receiver.error')"
    :cancelText="$t('common.cancel')"
    apiEndpoint="/api/receiver"
    @receiver-added="$emit('receiver-added')"
    @close="$emit('close')"
  />
</template>

<script>
import EntityForm from './Entity-modal.vue';

export default {
  name: 'ReceiverModal',
  components: {
    EntityForm
  }
};
</script>

<style lang="scss">
@use '@/assets/scss/main';
</style>