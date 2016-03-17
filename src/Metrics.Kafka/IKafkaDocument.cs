using System;
using System.Collections.Generic;
using Kafka.Basic;

namespace Metrics.Kafka
{
    public interface IKafkaDocument
    {
        Message ToMessage(string contextName);
    }

    public interface IKafkaDocument<T> : IKafkaDocument
    {
        string Type { get; set; }
        string Name { get; set; }
        DateTime Timestamp { get; set; }
        T Value { get; set; }
    }

    public static class KafkaDocumentExtensions
    {
        public static void AddTo(this IKafkaDocument document, ICollection<IKafkaDocument> collection)
        {
            if (document != null) collection.Add(document);
        }
    }
}