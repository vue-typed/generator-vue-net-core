// adapt from https://github.com/DefinitelyTyped/DefinitelyTyped/blob/types-2.0/vue-resource/index.d.ts 

import * as _Vue from 'vue'


declare namespace VueResouce {

	interface HttpHeaders {
		put?: { [key: string]: string };
		post?: { [key: string]: string };
		patch?: { [key: string]: string };
		delete?: { [key: string]: string };
		common?: { [key: string]: string };
		custom?: { [key: string]: string };
		[key: string]: any;
	}

	interface HttpResponse {
		data: any;
		ok: boolean;
		status: number;
		statusText: string;
		headers: Function;
		text(): string;
		json(): any;
		blob(): Blob;
	}

	interface HttpOptions {
		url?: string;
		method?: string;
		body?: any;
		params?: any;
		headers?: any;
		before?(request: any): any;
		progress?(event: any): any;
		credentials?: boolean;
		emulateHTTP?: boolean;
		emulateJSON?: boolean;
	}

	interface $http {
		(url: string, data?: any, options?: HttpOptions): PromiseLike<HttpResponse>;
		(url: string, options?: HttpOptions): PromiseLike<HttpResponse>;
	}

	interface HttpInterceptor {
		request?(request: HttpOptions): HttpOptions;
		response?(response: HttpResponse): HttpResponse;
	}

	interface Http {
		options: HttpOptions & { root: string };
		headers: HttpHeaders;
		interceptors: (HttpInterceptor | (() => HttpInterceptor))[];
		get: $http;
		post: $http;
		put: $http;
		patch: $http;
		delete: $http;
		jsonp: $http;
	}

	interface ResourceActions {
		get: { method: string };
		save: { method: string };
		query: { method: string };
		update: { method: string };
		remove: { method: string };
		delete: { method: string };
	}

	interface ResourceMethod {
		(params: any, data?: any, success?: Function, error?: Function): PromiseLike<HttpResponse>;
		(params: any, success?: Function, error?: Function): PromiseLike<HttpResponse>;
		(success?: Function, error?: Function): PromiseLike<HttpResponse>;
	}

	interface ResourceMethods {
		get: ResourceMethod;
		save: ResourceMethod;
		query: ResourceMethod;
		update: ResourceMethod;
		remove: ResourceMethod;
		delete: ResourceMethod;
	}

	interface $resource {
		(url: string, params?: Object, actions?: any, options?: HttpOptions): ResourceMethods;
	}

	interface Resource extends $resource {
		actions: ResourceActions;
	}

	interface ComponentOption {
		http?: (HttpOptions & { headers?: HttpHeaders } & { [key: string]: any })
	}
}

declare module "vue/types/vue" {	
	interface Vue {		
		$http: {
			(options: VueResouce.HttpOptions): PromiseLike<VueResouce.HttpResponse>;
			get: VueResouce.$http;
			post: VueResouce.$http;
			put: VueResouce.$http;
			patch: VueResouce.$http;
			delete: VueResouce.$http;
			jsonp: VueResouce.$http;
		};
		$resource: VueResouce.$resource;
	}
}

// NOTES: Still can't resolve static members injected to Vue
// declare namespace VueResouceStatic {
// 	interface VueStatic {
// 		new (): VueStatic
// 		http: VueResouce.Http;
// 		resource: VueResouce.Resource;
// 	}
// 	var Vue: VueStatic
// }
// declare namespace Vue {
// 	export type http = VueResouce.Http;
// }

declare module "vue-resource" {
	const install: _Vue.PluginFunction<VueResouce.ComponentOption>;
	export = install;
}