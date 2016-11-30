import * as Vue from "vue"
import { Component } from 'vue-typed'
import { AuthService } from '../services/auth'
import store from '../store';

import '../styles/index.less'


@Component({
	template: require('./view.html')
})
export class MainApp extends Vue {

	get fullName() {
		return AuthService.user.identity.fullName;
	}

	get authorized(): boolean {
		return AuthService.user.authenticated;
	}

	logout() {
		AuthService.signOut()
		this.$router.replace('/')
	}

	isInRole(role: string) : boolean {
		return AuthService.user.isInRole(role);
	}

	mounted() {

		// temporary solution for: https://github.com/vue-typed/vue-typed-ui/issues/4
		$('#main-sidebar').on('click', '.item', () => {
			$('#main-sidebar').sidebar('hide')
		})
	}
}