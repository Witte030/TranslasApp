import { createRouter, createWebHistory } from 'vue-router'
import CreateCard from '../components/CreateCard.vue'
import CardList from '../components/CardList.vue'

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
  // Optional route for card details - commented out until implemented
  // {
  //   path: '/cards/:id',
  //   name: 'CardDetail',
  //   component: () => import('../components/CardDetail.vue')
  // }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router