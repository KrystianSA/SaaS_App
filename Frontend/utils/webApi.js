import { hash } from 'ohash'

export const useWebApiFetch = function (request, opts) {
    const config = useRuntimeConfig()
    const { getErrorMessage } = useWebApiResponseParser();
    const globalMessageStore = useGlobalMessageStore();
    const antiForgeryStore = useAntiForgeryStore();

    opts = opts || {};
    opts.headers = opts.headers || {};
    opts.headers['X-XSRF-TOKEN'] = antiForgeryStore.$state.token

    return useFetch(request, {
        baseURL: config.public.BASE_URL,
        onRequest({ request, options }) {
            // Set the request headers
        },
        onRequestError(context) {

        },
        onResponse({ request, response, options }) {

        },
        onResponseError({ request, response, options }) {
            const message = getErrorMessage(response, {});
            globalMessageStore.showErrorMessage(message);
        },
        credentials: 'include',
        key: hash(['webapi-fetch',request, opts?.headers, opts?.body, opts?.params, opts?.method, opts?.query]),
        ...opts
    });
}