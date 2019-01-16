using MassTransit;
using Messaging;
using System;
using System.Threading.Tasks;

namespace MessageBus.Consumer.ResumeAccount
{
    public class PaymentCreatedConsumer : IConsumer<IPaymentCreated>
    {
        public Task Consume(ConsumeContext<IPaymentCreated> context)
        {
            IPaymentCreated payment = context.Message;

            Console.WriteLine("New payment registered");

            Console.WriteLine($"Payment Created Receive: " +
               $"Account Credit {payment.CreditAccount}, " +
               $"Account Debit {payment.DebitAccount}, " +
               $"Value {payment.Value}, " +
               $"Payment Type {payment.PaymentType}");

            return Task.FromResult(context.Message);
        }
    }
}
