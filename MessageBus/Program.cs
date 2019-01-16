using MassTransit;
using Messaging;
using System;
using System.Threading.Tasks;

namespace MessageBus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Producer Payment";
            Console.WriteLine("Producer Payment");
            RunMessageBus();
        }

        private static void RunMessageBus()
        {
            string rabbitMqAddress = "rabbitmq://localhost:5672/payment";
            string rabbitMqQueue = "superdigital.payments.orderpayments";
            Uri rabbitMqRootUri = new Uri(rabbitMqAddress);

            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                rabbit.Host(rabbitMqRootUri, settings =>
                {
                    settings.Password("guest");
                    settings.Username("guest");
                });
            });

            Task<ISendEndpoint> sendEndpointTask = rabbitBusControl.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            Task sendTask = sendEndpoint.Send<ICreatePayment>(new
            {
                CreditAccount = 5587,
                DebitAccount = 6698,
                Value = 580.75,
                PaymentType = "Salary"
            });

            Console.ReadKey();
        }
    }
}
