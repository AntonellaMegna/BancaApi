using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Kafka_Producer.Service.KafKa
{
    public class KafKaProducers :IKafKaProducers
    {
        IConfiguration _configuration;
        
        public KafKaProducers(IConfiguration configuration )
        {
            _configuration = configuration;
             
        }

        public Object SendToKafka( string message, string topic)
        {
            ProducerConfig config = new()
            {
                BootstrapServers = _configuration.GetSection("Kafka:BootstrapServers").Value
            };
            using (var producer = new ProducerBuilder<Null, string>(config).Build())

            {
                try
                {
                    //    return producer.ProduceAsync(_configuration.GetSection("Kafka:Topic").Value!.Trim(), new Message<Null, string> { Value = message })
                    //        .GetAwaiter()
                    //        .GetResult();
                     
                    return producer.ProduceAsync(topic.Trim(), new Message<Null, string> { Value = message })
                         .GetAwaiter()
                        .GetResult();

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went wrong: {e}");
                }
            }
            return null;
        }

     
    }
}
