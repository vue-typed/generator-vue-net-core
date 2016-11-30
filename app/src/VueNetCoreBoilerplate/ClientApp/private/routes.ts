import { App } from './app'
import { Admin } from './admin'
import { Profile } from './profile'

const routes = [
	{ path: '/app', component: App, meta: { auth: true } },
	{ path: '/profile', component: Profile, meta: { auth: true } },
	{ path: '/admin', component: Admin, meta: { auth: true, roles: ['SUPER_ADMIN'] } }
]

export default routes