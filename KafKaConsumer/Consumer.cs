using Confluent.Kafka;
using KafKaConsumer.Service.IService;

namespace KafKaConsumer
{
    public class Consumer : IHostedService
    
    {
        IConfiguration _Configuration;
        ISendEmail _sendEmail;
        public Consumer(IConfiguration Configuration, ISendEmail sendEmail )
        {
             _Configuration = Configuration;
             _sendEmail = sendEmail;
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
                        string pwd = "";
                        string email = "";
                        string name = "";
                        int indexEmail = consumer.Message.Value.IndexOf("♥");
                        int indexName= consumer.Message.Value.IndexOf(":");
                     
                        if (indexEmail != -1)
                        {
                             pwd = consumer.Message.Value.Substring(indexEmail + 1);
                            
                             email = consumer.Message.Value.Substring(indexName +1, (indexEmail-1) - (indexName ));

                             name = consumer.Message.Value.Substring(0, indexName);

                            //  _sendEmail.SendEmailToAll("a.it", "m@hotmail.com","ciao", "test invio email", true, "m@hotmail.com", ".,"smtp.office365.com",587);
                            _sendEmail.SendEmailToAll(email, email, "Access BanK", $"Gentile {name} , ti informiamo che è stato rilevato l'accesso al tuo conto corrente in data:  {DateTime.Now}", true, email, pwd, "smtp.libero.it", 587);

                        }
                        else
                        {
                            Console.WriteLine("Not found");
                        }
                   
                        Console.WriteLine($"Message: {name+ " " + DateTime.Now} received from {consumer.TopicPartitionOffset}");
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

