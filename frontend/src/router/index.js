import { createRouter, createWebHistory } from 'vue-router'
import CreateCard from '../components/CreateCard.vue'
import CardList from '../components/CardList.vue'
import SupplierManagement from '../components/SupplierManagement.vue';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: CardList // Using CardList as the home component
  },
  {
    path: '/create-card',
    name: 'CreateCard',
    component: CreateCard
  },
  {
    path: '/cards',
    name: 'CardList',
    component: CardList
  },
  {
    path: '/supplier-management',
    name: 'SupplierManagement',
    component: SupplierManagement
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: () => import('../components/NotFound.vue') // Create this component
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router