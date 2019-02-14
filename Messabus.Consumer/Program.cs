using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using System;

namespace Messabus.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Consumer Payment";
            Console.WriteLine("Consumer Payment");

            RunMessageBus();
        }

        private static void RunMessageBus()
        {
            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost:5672/payment"), settings =>
                {
                    settings.Password("guest");
                    settings.Username("guest");
                });

                rabbit.ReceiveEndpoint(rabbitMqHost, "superdigital.payments.orderpayments", conf =>
                {
                    conf.UseRetry(retryConfig =>
                    {
                        retryConfig.Immediate(5);
                        retryConfig.Handle<Exception>();
                    });

                    conf.Consumer<CreatePaymentConsumer>(consumerConfig =>
                    {

                        consumerConfig.UseRetry(retryConfig =>
                        {
                            retryConfig.Immediate(5);
                            retryConfig.Ignore<ArgumentNullException>();
                        });

                    });
                });
            });



            rabbitBusControl.Start();
            Console.ReadKey();

            rabbitBusControl.Stop();
        }
    }
}
