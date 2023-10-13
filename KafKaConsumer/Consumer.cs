using Confluent.Kafka;
using KafKaConsumer.Service.IService;
using System;
using static Confluent.Kafka.ConfigPropertyNames;

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
                       int indiceLettera = consumer.Message.Value.IndexOf("♥");
                        
                        if (indiceLettera != -1)
                        {

                             pwd = consumer.Message.Value.Substring(indiceLettera + 1);

                             email = consumer.Message.Value.Substring(0, indiceLettera);

                            //  _sendEmail.SendEmailToAll("a.it", "m@hotmail.com","ciao", "test invio email", true, "m@hotmail.com", ".,"smtp.office365.com",587);
                            _sendEmail.SendEmailToAll(email, email, "Access BanK", "Dear customer, we inform you that access to your current account has been detected  " + DateTime.Now, true, email, pwd, "smtp.libero.it", 587);


                        }
                        else
                        {
                            Console.WriteLine("Not found");
                        }

                        

                       
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
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

