namespace Kafka_Producer.Service.KafKa
{
    public interface IKafKaProducers
    {
        Object SendToKafka(string _message, string topic);
    }
}