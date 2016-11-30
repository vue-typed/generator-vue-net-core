import * as Vue from "vue"
import { Component } from 'vue-typed'
import { FormComponent, Validate } from 'vue-typed-ui';
import * as _ from 'lodash'
import store from '../../store';
import mutations from '../../store/mutations';

@FormComponent({
	template: require('./view.html'),
	onSuccess: 'save',
	validateInline: true
})
export class Profile extends Vue implements UserProfileDto {

	@Validate({ type: 'empty' })	
	fullName = ''

	@Validate({ type: 'email' })
	@Validate({ type: 'empty' })
	email = ''
	dob = undefined	
	gender = 0


	created() {
		this.$http.get('/api/account/profile').then((e)=>{				
			_.each(e.data, (d, k)=> {	
				if (k === 'dob' && d) 
					d = new Date(d)
				Vue.set(this, k, d)
			})
		})
	}

	save() {
	
		this.$http.post('/api/account/profile', <UserProfileDto>{
			fullName: this.fullName,
			email: this.email,
			dob: this.dob,
			gender: this.gender
		}).then((e)=>{
			store.commit(mutations.IDENTITY, {
				fullName: this.fullName
			})			
			this.$ui.toast.success('Your profile was successfully updated!')
		}, (e) => {
			this.$ui.alert('Oops!', e.data, "error")
		})
		
	}

}