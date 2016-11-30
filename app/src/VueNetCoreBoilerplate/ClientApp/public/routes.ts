import { Home } from './home'
import { Register } from './register'
import { Login } from './login'

const routes = [
	{ path: '/', component: Home },
	{ path: '/register', component: Register },
	{ path: '/login', component: Login },
]

export default routes