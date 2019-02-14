using MassTransit;
using Messaging;
using System;
using System.Threading.Tasks;

namespace Messabus.Consumer 
{
    public class CreatePaymentConsumer : IConsumer<ICreatePayment>
    {
        public Task Consume(ConsumeContext<ICreatePayment> context)
        {
            ICreatePayment payment = context.Message;

            Console.WriteLine($"Payment Order: " +
                $"Account Credit {payment.CreditAccount}, " +
                $"Account Debit {payment.DebitAccount}, " +
                $"Value {payment.Value}, " +
                $"Payment Type {payment.PaymentType}");

            


            context.Publish<IPaymentCreated>(new
            {
                CreditAccount = payment.CreditAccount,
                DebitAccount = payment.DebitAccount,
                Value = payment.Value,
                PaymentType = payment.PaymentType,
                CreatedBy = "CreatePayment",
            });

            return Task.FromResult(context);
        }
    }
}
