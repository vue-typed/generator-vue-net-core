import * as VueRouter from 'vue-router'
import { default as publicRoutes } from '../public/routes'
import { default as privateRoutes } from '../private/routes'
import { AuthService } from '../services/auth';

const routes = [].concat(publicRoutes, privateRoutes)

export default {

	 create: () => {

		var router = new VueRouter({
			routes: routes,
			linkActiveClass: 'active',
			mode: 'history'
		});

		router.beforeEach((to, from, next) => {
			let user = AuthService.user

			if (to.matched.some(r => r.meta.auth === true)) {
				let inRole = true

				if (to.matched.some(r => r.meta.roles)) {					
					let roles = to.meta.roles as string[]
					let matchRoles = roles.filter((r)=>{
						return user.isInRole(r);
					})
					inRole = matchRoles.length > 0
				}

				if (!user.authenticated || !inRole) {
					next('/login')
				} else {
					next()
				}
				
			} else if (['/login', '/register'].indexOf(to.path) > -1) {

				if (user.authenticated) {
					next('/')
				} else {
					next()
				}
			
			} else {
				next()
			}
		})

		return router;

	 }

}
