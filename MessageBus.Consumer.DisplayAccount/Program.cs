using MassTransit;
using MessageBus.Consumer.ResumeAccount;
using System;
using GreenPipes;
using MassTransit.RabbitMqTransport;
using Messaging;
using System.Threading.Tasks;

namespace MessageBus.Consumer.DisplayAccount
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Consumer Display Account";
            Console.WriteLine("Consumer Display Account");

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

                rabbit.ReceiveEndpoint(rabbitMqHost, "superdigital.payments.events.displayAccount", conf =>
                {
                    conf.PrefetchCount = 1;
                    conf.Consumer<PaymentCreatedConsumer>(paymentCreatedConfig =>
                    {
                        paymentCreatedConfig.UseContextFilter((context) =>
                        {
                            bool valid = context.TryGetMessage<IPaymentCreated>(out var message);
                            if (valid)
                            {
                                valid = message.Message?.CreatedBy == "CreatePayment";
                            }

                            return Task.FromResult(valid);
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
