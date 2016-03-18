using System;
using System.Collections.Generic;

namespace Metrics.Kafka
{
    public interface IKafkaDocument
    {
        string Type { get; set; }
        string Name { get; set; }
        DateTime Timestamp { get; set; }
    }

    public interface IKafkaDocument<T> : IKafkaDocument
    {
        T Value { get; set; }
    }
    public class KafkaDocument<T> : IKafkaDocument<T>
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public string[] Tags { get; set; }
        public T Value { get; set; }
    }

    public static class KafkaDocumentExtensions
    {
        public static void AddTo(this IKafkaDocument document, ICollection<IKafkaDocument> collection)
        {
            if (document != null) collection.Add(document);
        }
    }
}