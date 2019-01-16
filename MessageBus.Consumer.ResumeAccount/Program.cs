using MassTransit;
using MassTransit.RabbitMqTransport;
using System;

namespace MessageBus.Consumer.ResumeAccount
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Consumer Resume Account";
            Console.WriteLine("Consumer Resume Account");

            RunMessageBuss();
        }

        private static void RunMessageBuss()
        {
            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost:5672/payment"), settings =>
                {
                    settings.Password("guest");
                    settings.Username("guest");
                });

                rabbit.ReceiveEndpoint(rabbitMqHost, "superdigital.payments.events.resumeaccount", conf =>
                {
                    conf.Consumer<PaymentCreatedConsumer>();
                });
            });
            rabbitBusControl.Start();
            Console.ReadKey();
            rabbitBusControl.Stop();
        }
    }
}
