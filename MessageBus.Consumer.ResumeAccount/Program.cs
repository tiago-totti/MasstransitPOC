﻿using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Messaging;
using System;
using System.Threading.Tasks;

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
                    conf.PrefetchCount = 1;
                    conf.Consumer<PaymentCreatedConsumer>(paymentCreatedConfig =>
                    {
                        paymentCreatedConfig.UseContextFilter((context) =>
                       {
                           bool valid = context.TryGetMessage<IPaymentCreated>(out var message);
                           if (valid)
                           {
                               valid = message.Message?.CreatedBy == "wololo";
                               Console.WriteLine("Skipped message " + context.MessageId);
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
