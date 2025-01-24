import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import HomeView from '../views/HomeView.vue'
import PlayView from '@/views/PlayView.vue'
import ProfileView from '@/views/ProfileView.vue'

const routes = [
  {
    path: '/home',
    name: 'home',
    component: HomeView
  },
  {
    path: '/play',
    name: 'play',
    component: PlayView
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView,
  },
  {
    path: '/register',
    name: 'register',
    component: RegisterView
  },
  {
    path: '/profile',
    name: 'profile',
    component: ProfileView
  },
  {
    path: '/logout',
    name: 'logout',
    beforeEnter: (to, from, next) => {
      localStorage.removeItem('jwtToken')
      window.dispatchEvent(new CustomEvent('login', {
        detail: {
          storage: localStorage.getItem('jwtToken')
        }
      }));
      next({ name: 'login' })
    },
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

// router.beforeEach((to, from, next) => {
//   const isAuthenticated = localStorage.getItem('jwtToken') !== null
//   // if (to.name === 'login' && isAuthenticated) {
//   //   next({ name: 'home' });
//   // } else {
//   //   next();
//   // }
// });

export default router
