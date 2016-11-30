import { Component } from 'vue-typed'
import { FormComponent, Validate } from 'vue-typed-ui'
import * as Vue from 'vue'
import { AuthService } from '../../services/auth';

@FormComponent({
	template: require('./view.html'),
	validateInline: true,
	onSuccess: 'register'
})
export class Register extends Vue {

	@Validate({
		type: 'empty',
		prompt: 'Full name can not be empty.'
	})
	fullName: string = ""

	@Validate({
		type: 'empty',
		prompt: 'User name can not be empty.'
	})
	userName: string = ""

	@Validate({
		type: 'empty',
		prompt: 'Password can not be empty.'
	})
	password: string = ""

	@Validate({
		type: 'match[password]',
		prompt: 'Password confirmation can not be empty.'
	})
	passwordConfirm: string = ""

	errorMsg: string = ""

	register() {

		let self = this
		self.errorMsg = ""

		AuthService.signUp(self, {
			fullName: self.fullName,
			userName: self.userName,
			password: self.password,
			passwordConfirm: self.passwordConfirm
		})
			.then(() => {
				self.$router.replace('./app')
			})
			.catch((errorMsg) => {
				self.errorMsg = errorMsg
			})
	}

}