using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace MyTwse
{
    public class StockInfoService : IStockInfoService
    {
        public void GetStockInfoByRecent(string stockCode, int days)
        {
            string url = "https://www.twse.com.tw/exchangeReport/BWIBBU_d?response=json&selectType=ALL";
            DateTime now = DateTime.UtcNow.AddHours(8).Date;
            for (DateTime i = now; i > now.AddDays(-days); i = i.AddDays(-1))
            {
                var result = RestRequestHelper.Request(url)
                    .Get(e => e
                        .AddParameter("date", now.ToString("yyyyMMdd"))
                        ).Response();

            }


        }

    }
    public interface IStockInfoService
    {
        void GetStockInfoByRecent(string stockCode, int days);
    }
    public class RestRequestHelper
    {
        RestClient client = null;
        RestRequest http = null;
        public RestRequestHelper(string url)
        {
            client = new RestClient(url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="authenticator">HttpBasicAuthenticator</param>
        public RestRequestHelper(string url, IAuthenticator authenticator)
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
    public static class ExtensionOfObject
    {
        public static T JsonConvertToModel<T>(this string jsonText)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                return default(T);

            return JsonSerializer.Deserialize<T>(jsonText);
        }
    }


    public class StockInfoModel
    {
        /// <summary>
        /// 狀態 EX. OK
        /// </summary>
        public string Stat { get; set; }
        /// <summary>
        /// EX. 20210407
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// EX. 110年04月07日 個股日本益比、殖利率及股價淨值比
        /// </summary>
        public string Title { get; set; }
        public string[] Fields { get; set; }
        public object[][] Data { get; set; }
        public string SelectType { get; set; }
        public string[] Notes { get; set; }
    }

}
