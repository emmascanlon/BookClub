class RequestError extends Error {
    constructor(message, responseData, statusCode) {
      super(message);
      this.response = {
        data: responseData
      };
      this.statusCode = statusCode;
    }
  }

  const showToast = (message) => {
    console.log(message)
  }
  
  async function request(requestMethod, url, data) {
    const method = requestMethod.toUpperCase();
    const response = await fetch(url, {
      method,
      cache: 'no-cache',
      headers: {
        'Content-Type': 'application/json',
        'X-Requested-With': 'XMLHttpRequest'
      },
      redirect: 'follow',
      body: ['POST', 'PUT', 'DELETE'].includes(method) ? JSON.stringify(data) : undefined
    });
  
    const successful = response.status >= 200 && response.status < 300;
    const responseData =
      (response.headers.get('content-type') ?? '').indexOf('json') > -1
        ? await response.json()
        : undefined;
  
    if (!successful) throw new RequestError(response.statusText, responseData, response.status);
  
    return responseData;
  }
  
  const add = queryParams => ({
    to: url => {
      const queryParamsString = Object.keys(queryParams ?? {})
        .filter(key => queryParams[key] !== undefined && queryParams[key] !== null)
        .map(key => `${key}=${queryParams[key]}`)
        .join('&');
      const urlIncludesQueryParams = url.indexOf('?') > -1;
      const prefixedQueryString = queryParamsString
        ? `${urlIncludesQueryParams ? '&' : '?'}${queryParamsString}`
        : '';
  
      return `${url}${prefixedQueryString}`;
    }
  });
  
  const get = (url, options) => request('GET', add(options?.params).to(url));
  const post = (url, data, options) => request('POST', add(options?.params).to(url), data);
  const put = (url, data, options) => request('PUT', add(options?.params).to(url), data);
  const del = (url, options) => request('DELETE', add(options?.params).to(url), options?.data);
  
  const basePath = 'http://localhost:5065/api/';
  
  const errorHandled = (method, options) => async (...args) => {
    console.log(method, options);
    try {
      const [url, ...otherArgs] = args;
      console.log(url)
      var response = await method(`${url.indexOf('://') === -1 ? basePath : ''}${url}`, ...otherArgs);
      return response;
    } catch (caughtError) {
      const errorDetails = caughtError?.response?.data;
      const subErrors = errorDetails?.errors;
      const formattedSubErrors =
        subErrors instanceof Object
          ? Object.keys(subErrors)
              .map(key => subErrors[key])
              .flat()
          : subErrors;
      const isToastsEnabled = options === undefined || options.isToastable !== false;
      const isToastable =
        isToastsEnabled && (options?.noToastWhen === undefined || !options.noToastWhen(caughtError));
      isToastable &&
        showToast(
          errorDetails?.title || caughtError?.message || "We're sorry, an unknown error occurred.",
          formattedSubErrors ?? []
        );
      throw caughtError;
    }
  };
  
  const is409StatusCode = error => {
    return error.statusCode === 409 ? true : false;
  };
  
  export const Http = {
    get: errorHandled(get),
    post: errorHandled(post),
    put: errorHandled(put),
    delete: errorHandled(del, { noToastWhen: is409StatusCode })
  };
  
  export const NonToastedHttp = {
    get: errorHandled(get, { isToastable: false }),
    post: errorHandled(post, { isToastable: false }),
    put: errorHandled(put, { isToastable: false }),
    delete: errorHandled(del, { isToastable: false })
  };
  
  export const createCustomHttp = options => ({
    get: errorHandled(get, options),
    post: errorHandled(post, options),
    put: errorHandled(put, options),
    delete: errorHandled(del, options)
  });
  