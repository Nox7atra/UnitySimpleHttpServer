using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using UnityEngine;

namespace HTTPServer
{
    public class HttpServer : MonoBehaviour
    {
        [SerializeField] private int _Port = 10021;
        private HttpListener _httpListener;
        private List<HTTPServerHandler> _httpServerHandlers;
        private void Start()
        {
            _httpServerHandlers = new List<HTTPServerHandler>();
            var subclassTypes = Assembly
                .Load("Assembly-CSharp")
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(HTTPServerHandler)));
            foreach (var subclassType in subclassTypes)
            {
                _httpServerHandlers.Add(Activator.CreateInstance(subclassType) as HTTPServerHandler);
            }
            
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add($"http://*:{_Port}/");
            _httpListener.Start();
            _httpListener.BeginGetContext(OnGetCallback, null);
        }

        private void OnGetCallback (IAsyncResult result)
        {
            
            HttpListenerContext context = _httpListener.EndGetContext(result);
            var response = context.Response;
            var request = context.Request;
            context.Response.Headers.Clear();

            
            response.AppendHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
            response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            response.AddHeader("Access-Control-Max-Age", "1728000");
            if (request.HttpMethod == "OPTIONS")
            {
                CreateResponse(response, new NetworkAnswer(){ status = 200});
                if (_httpListener.IsListening)
                {
                    _httpListener.BeginGetContext(OnGetCallback, null);
                }
                return;
            }
           
            try
            {
                HandleListenerContext(context, response);
            }
            catch (Exception e)
            {
                CreateErrorResponse(response, e.Message);
            }
            if (_httpListener.IsListening)
            {
                _httpListener.BeginGetContext(OnGetCallback, null);
            }
        }

        private async void CreateResponse(HttpListenerResponse response, NetworkAnswer data = default)
        {
            response.SendChunked = false;
            response.StatusCode = data.status;
            response.StatusDescription = data.status == 200 ? "OK" : "Internal Server Error";
            using (var writer = new StreamWriter(response.OutputStream, response.ContentEncoding))
            {
                await writer.WriteAsync(JsonUtility.ToJson(data));
            }
            response.Close();
        }
        private async void CreateErrorResponse(HttpListenerResponse response, string error)
        {
            response.SendChunked = false;
            response.StatusCode = 500;
            response.StatusDescription = "Internal Server Error";
            using (var writer = new StreamWriter(response.OutputStream, response.ContentEncoding))
            {
                await writer.WriteAsync(JsonUtility.ToJson(new NetworkAnswer()
                {
                    status = 500,
                    errorMessage =  error
                }));
            }
            response.Close();
        }
        private void HandleListenerContext (HttpListenerContext context, HttpListenerResponse response)
        {
            var url = context.Request.RawUrl;
            var handler = _httpServerHandlers.FirstOrDefault(x => x.IsThisRoute(url));
            if (handler != null)
            {
                handler.ProcessParams(url);
                CreateResponse(response, handler.GetAnswerData());
            } else
            {
                CreateErrorResponse(response, $"There are no handler for url {url}");
            }
        }

        private void OnDestroy ()
        {
            _httpListener.Stop();
            _httpListener.Close();
        }
    }
        
    public class NetworkAnswer
    {
        public int status;
        public string errorMessage;
        public object data;
    }
    public abstract class HTTPServerHandler
    {
        protected abstract string _route { get; }
        protected string[] _params;
        public bool IsThisRoute (string url)
        {
            return url.ToLower().Contains(_route.ToLower());
        }
        private void ParseParams (string url)
        {
            _params = url.Replace(_route, string.Empty).Split('/');
        }
        public abstract NetworkAnswer GetAnswerData ();
        public virtual void ProcessParams (string url)
        {
            ParseParams(url);   
        }
    }
}
