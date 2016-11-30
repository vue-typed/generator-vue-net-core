import * as Vue from "vue"
import * as VueRouter from 'vue-router'
import * as VueTypedUI from 'vue-typed-ui'
import * as Vuex from 'vuex'
var VueResource = require('vue-resource')


// init vue plugins
Vue.use(Vuex)
Vue.use(VueRouter)
Vue.use(VueTypedUI, <VueTypedUI.Options>{
	toastr: {
		closeButton: true,
		timeOut: 2000,
		hideDuration: 500,
		showDuration: 300		
	}
})
Vue.use(VueResource)


// init routes
import route from './infrastructure/routes'
var router = route.create()

// init services
import { AuthService } from './services/auth';
AuthService.init(router)


// build main app
import { MainApp } from './main/index';
const Vm = {
	router,
	render: h => h(MainApp),
	mounted() {
		$('#pre-loading').transition('fade').remove()

	}
}

export default Vm