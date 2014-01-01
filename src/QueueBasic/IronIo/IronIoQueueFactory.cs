using System;

namespace QueueBasic.IronIo
{
    public class IronIoQueueFactory : IQueueFactory
    {
        private const string _urlTemplate = "https://{0}/1/projects/{1}/queues/{2}";
        private readonly Uri _uri;
        private readonly string _host;
        private readonly string _projectId;
        private readonly string _authToken;

        public IronIoQueueFactory(string host, string projectId, string authToken)
        {
            if (string.IsNullOrWhiteSpace(projectId))
                throw new ArgumentNullException("projectId");

            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentNullException("host");

            if (string.IsNullOrWhiteSpace(authToken))
                throw new ArgumentNullException("authToken");

            Uri hostUri;
            if (!Uri.TryCreate("https://" + host, UriKind.Absolute, out hostUri))
                throw new ArgumentException("invalid", "host");

            _host = host;
            _projectId = projectId;
            _authToken = authToken;
        }

        public IQueue Create(string name)
        {
            var queueId = Uri.EscapeUriString(name);
            var queueUrl = string.Format(_urlTemplate, _host, _projectId, queueId);
            var queue = new IronIoQueue(queueUrl, _authToken);

            return queue;
        }

        public void Dispose()
        {
        }
    }
}
