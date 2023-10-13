namespace KafkaProducer.Service.KafKa
{
    public interface IKafKaProducers
    {
        Object SendToKafka(string _message, string topic);
    }
}