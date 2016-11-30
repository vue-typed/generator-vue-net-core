import * as Vue from 'vue'
import store from '../store';
import mutations from '../store/mutations'
import * as _ from 'lodash';
import { Identity } from '../store/identity'
import { User } from './user';
import * as VueRouter from 'vue-router'

export class AuthService {

	static initialized: boolean = false
	static router: VueRouter
	static user = new User()

	static init(router: VueRouter): void {
		if (this.initialized) return

		this.router = router

		Vue['http'].interceptors.push((request, next) => {

			$('#loading').addClass('active')

			// add auth header
			if (localStorage.getItem('token'))
				request.headers.append('Authorization', 'Bearer ' + localStorage.getItem('token'))

			next((response) => {

				$('#loading').removeClass('active')
				
				// sign out if unauthorized
				if (response.status === 401) {
					this.signOut()
					if (this.router.currentRoute.matched.some(r => r.meta.auth === true))
						this.router.replace('/login')
				}

			});
		});

		if (localStorage.getItem('token'))
			Vue['http'].get('/api/account/refresh-token').then((response) => {
				localStorage.setItem('token', response.data.access_token);
				this.checkAuth();
			});

		this.checkAuth()

		this.initialized = true
	}

	static signUp(context: Vue, data: SignUpDto): Promise<string> {
		var self = this

		return new Promise((resolve, reject) => {
			context.$http.post('/api/account/signup', data).then(function (response) {
				localStorage.setItem('token', response.data.access_token)
				self.checkAuth()
				resolve();
			}, function (err) {
				reject(err.data)
			})
		});
	}

	static signIn(context: Vue, data: SignInDto): Promise<{}> {
		var self = this

		return new Promise((resolve, reject) => {
			context.$http.post('/api/account/signin', {
				userName: data.userName, password: data.password
			}).then(function (response) {
				localStorage.setItem('token', response.data.access_token)
				self.checkAuth()
				resolve();
			}, function (err) {
				reject(err.data)
			})
		});

	}

	static signOut(): void {
		this.user.authenticated = false
		localStorage.removeItem('token')
	}

	static checkAuth(): void {
		var jwt = localStorage.getItem('token')
		if (jwt) {

			let payload = require('jwt-decode')(jwt)
			let res: any = _.mapKeys(payload, (v, k: string) => {
				return k.replace(/http:\/\/schemas(.*)\/claims\//g, '');
			})

			this.user.authenticated = true
			this.user.identity = <Identity>{
				name: res.name,
				fullName: res.fullName,
				roles: _.isArray(res.role) ? res.role : [res.role]
			}

		}
		else {
			this.user.authenticated = false
			this.user.identity = null
		}
	}

}