using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyTwse.Helpers
{
    public class RestRequestParameterHelper
    {
        RestRequest http = null;
        public RestRequestParameterHelper(RestRequest http)
        {
            this.http = http;
        }
        #region Header設定

        public RestRequestParameterHelper AddHeader(string key, string value)
        {
            this.http.AddHeader(key, value);

            return this;
        }
        public RestRequestParameterHelper AddHeader(Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                this.http.AddHeaders(headers);
            }

            return this;
        }
        #endregion
        #region Parameter設定

        /// <summary>
        /// key,value設定參數
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RestRequestParameterHelper AddParameter(string key, object value)
        {
            switch (http.Method)
            {
                case Method.GET:
                    {
                        this.http.AddQueryParameter(key, value.ToString());
                    }
                    break;
                default:
                    {
                        this.http.AddParameter(key, value);
                    }
                    break;
            }

            return this;
        }
        /// <summary>
        /// TextPlain
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public RestRequestParameterHelper AddTextPlainBody(string body)
        {
            this.http.AddHeader("Content-Type", "text/plain");
            this.http.AddParameter("text/plain", body, ParameterType.RequestBody);
            return this;
        }
        public RestRequestParameterHelper AddParameter<T>(T body) where T : new()
        {
            switch (http.Method)
            {
                case Method.GET:
                    {
                        foreach (PropertyInfo item in typeof(T).GetProperties())
                        {
                            var value = item.GetValue(body);
                            this.http.AddQueryParameter(item.Name, value.ToString());
                        }
                    }
                    break;
                default:
                    {
                        this.http.AddJsonBody(body);
                    }
                    break;
            }

            return this;
        }

        /// <summary>
        /// key,value設定參數
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RestRequestParameterHelper AddParameter(Dictionary<string, string> parameters = null)
        {
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    AddParameter(item.Key, item.Value);
                }
            }

            return this;
        }

        #endregion
    }
}
