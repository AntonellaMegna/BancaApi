using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace BancaApi.Controllers
{
    [Route("api/kafkacontroller")]
    [ApiController]
    public class KafkaController:ControllerBase
    {
        private readonly ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "127.0.0.1:9092"
        };
        private readonly string topic = "Banca_Topic";

        [HttpPost]
        public IActionResult Post([FromQuery] string message)
        {
            return Created(string.Empty, SendToKafka(topic, message));
        }
        private Object SendToKafka(string topic, string message)
        {
            using (var producer =
                 new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    return producer.ProduceAsync(topic, new Message<Null, string> { Value = message })
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
