using System;

namespace Messaging
{
    public interface ICreatePayment
    {
        int CreditAccount { get; }
        int DebitAccount { get; }
        decimal Value { get; }
        string PaymentType { get; set; }
    }
}
