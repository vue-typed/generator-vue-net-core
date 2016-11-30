import store from '../store';
import mutations from '../store/mutations'
import { Identity } from '../store/identity'

export class User {

	set authenticated(val: boolean) {
		store.commit(mutations.AUTHORIZE, val)
	}
	get authenticated(): boolean {
		return store.state.authorized;
	}

	set identity(identity: Identity) {
		store.commit(mutations.IDENTITY, identity);
	}
	get identity(): Identity {		
		return store.state.identity;
	}

	isInRole(role: string): boolean {
		return this.identity.roles.indexOf(role) >= 0;
	}

}