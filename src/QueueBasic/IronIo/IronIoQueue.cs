using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QueueBasic.IronIo
{
    internal class IronIoQueue : IQueue
    {
        private readonly Uri _baseUri;
        private readonly HttpClient _httpClient;

        public string Address { get; private set; }

        public IronIoQueue(string queueUrl, string authToken)
        {
            if (string.IsNullOrWhiteSpace(queueUrl))
                throw new ArgumentNullException("queueUrl");

            if (string.IsNullOrWhiteSpace(authToken))
                throw new ArgumentNullException("authToken");

            if (!Uri.TryCreate(queueUrl.TrimEnd('/'), UriKind.Absolute, out _baseUri))
                throw new ArgumentException("queueUrl");

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Address = queueUrl;
        }

        public void Push(string data)
        {
            //iron.io expected object format
            var messages = new QueueMessages
            {
                Messages = new QueueMessage[]
                {
                    new QueueMessage
                    {
                        Body = data
                    }
                }
            };

            var json = JsonConvert.SerializeObject(messages);
            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var uri = string.Format("{0}/messages", _baseUri);
            var httpResponse = _httpClient.PostAsync(uri, httpContent).Result;
        }

        public IQueueMessage Peek()
        {
            var uri = string.Format("{0}/messages?n=1&timeout=60", _baseUri);
            var httpContent = _httpClient.GetAsync(uri).Result;
            var json = httpContent.Content.ReadAsStringAsync().Result;
            var messages = JsonConvert.DeserializeObject<QueueMessages>(json) ?? new QueueMessages();
            var message = messages.Messages.FirstOrDefault();

            if (message == null)
                return null;

            return new IronIoQueueMessage
            {
                Id = message.Id,
                Data = message.Body
            };
        }

        public void Pop(IQueueMessage queueMessage)
        {
            var uri = string.Format("{0}/messages/{1}", _baseUri, queueMessage.Id);
            var httpResponse = _httpClient.DeleteAsync(uri).Result;
        }

        public long Count()
        {
            var httpContent = _httpClient.GetAsync(_baseUri).Result;
            var json = httpContent.Content.ReadAsStringAsync().Result;
            var information = JsonConvert.DeserializeObject<QueueInformation>(json);

            return information.Size;
        }

        #region IDisposable
        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _httpClient.Dispose();
            }

            _disposed = true;
        }
        #endregion IDisposable

        #region Internal Classes
        private class QueueInformation
        {
            public long Size { get; set; }
        }

        private class QueueMessages
        {
            public QueueMessage[] Messages { get; set; }

            public QueueMessages()
            {
                Messages = new QueueMessage[] { };
            }
        }

        private class QueueMessage
        {
            public string Id { get; set; }
            public string Body { get; set; }
        }
        #endregion Internal Classes
    }
}
