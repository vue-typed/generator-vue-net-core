import { Component } from 'vue-typed'
import { FormComponent, Validate } from 'vue-typed-ui'
import * as Vue from 'vue'
import { AuthService } from '../../services/auth';

@FormComponent({
	template: require('./view.html'),
	validateInline: true,
	onSuccess: 'login'
})
export class Login extends Vue {

	@Validate({
		type: 'empty',
		prompt: 'User name can not be empty.'
	})
	userName: string = ""

	@Validate({
		type: 'empty',
		prompt: 'Password name can not be empty.'
	})
	password: string = ""

	errorMsg: string = ""

	login() {
		
		let self = this
		self.errorMsg = ""

		AuthService.signIn(self, {
			userName: self.userName, password: self.password
		})
			.then(()=>{
				self.$router.replace('./app')
			})
			.catch((errorMsg) => {
				self.errorMsg = errorMsg
			})

	}

}