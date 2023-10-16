using Confluent.Kafka;

namespace KafkaConsumer2
{
    public class Consumer2 : IHostedService
    {
        IConfiguration _Configuration;
       
        public Consumer2(IConfiguration Configuration )
        {
            _Configuration = Configuration;
   
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _Configuration.GetSection("Kafka:GroupId").Value,
                BootstrapServers = _Configuration.GetSection("Kafka:BootstrapServers").Value,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var consumerBuilder = new ConsumerBuilder
               <Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(_Configuration.GetSection("Kafka:Topic").Value!.Trim());
                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume(cancelToken.Token);
                        string name = "";
                        int indexName = consumer.Message.Value.IndexOf(":");
                        name = consumer.Message.Value.Substring(0, indexName);

                        Console.WriteLine($"Message: {name} received from {consumer.TopicPartitionOffset}");
                    }
                }
                catch (Exception)
                {
                    consumerBuilder.Close();
                }
            }

            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
