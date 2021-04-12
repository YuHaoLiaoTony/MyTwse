using MyTwse.Extensions;
using RestSharp;
using RestSharp.Authenticators;
using System;

namespace MyTwse.Helpers
{
    public class RestRequestHelper
    {
        RestClient client = null;
        RestRequest http = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="authenticator">HttpBasicAuthenticator</param>
        public RestRequestHelper(string url, IAuthenticator authenticator = null)
        {
            client = new RestClient(url);
            if (authenticator != null)
            {
                client.Authenticator = authenticator;
            }
        }
        public static RestRequestHelper Request(string url)
        {
            RestRequestHelper helper = new RestRequestHelper(url);
            return helper;
        }
        public static RestRequestHelper Request(string url, string username, string password)
        {
            RestRequestHelper helper = new RestRequestHelper(url, new HttpBasicAuthenticator(username, password));
            return helper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="apiUrl"></param>
        /// <param name="authenticator">HttpBasicAuthenticator</param>
        /// <returns></returns>
        public static RestRequestHelper Request(string baseUrl, string apiUrl)
        {
            RestRequestHelper helper = new RestRequestHelper($"{baseUrl}{apiUrl}", null);
            return helper;
        }

        /// <summary>
        /// Get方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public RestRequestHelper Get(Action<RestRequestParameterHelper> action = null)
        {
            SetHttpRequest(Method.GET, action);
            return this;
        }
        /// <summary>
        /// Put方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public RestRequestHelper Put(Action<RestRequestParameterHelper> action = null)
        {
            SetHttpRequest(Method.PUT, action);
            return this;
        }
        public RestRequestHelper Post(Action<RestRequestParameterHelper> action = null)
        {
            SetHttpRequest(Method.POST, action);
            return this;
        }
        private void SetHttpRequest(Method method, Action<RestRequestParameterHelper> action = null)
        {
            this.http = new RestRequest(method);

            if (action != null)
            {
                action(new RestRequestParameterHelper(http));
            }
        }

        /// <summary>
        /// 自訂回傳類型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isErrorStop">錯誤是否拋例外</param>
        /// <returns></returns>
        public T Response<T>(bool isErrorStop = true)
        {
            IRestResponse response = client.Execute(http);

            T content = default(T);

            if (typeof(T) == typeof(string))
            {
                content = (T)(object)response.Content;
            }
            else
            {
                content = response.Content.JsonConvertToModel<T>();
            }
            return content;
        }
        public string Response()
        {
            return Response<string>();
        }
    }
}
