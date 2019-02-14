namespace Messaging
{
    public interface IPaymentCreated
    {
        int CreditAccount { get; }
        int DebitAccount { get; }
        decimal Value { get; }
        string PaymentType { get; set; }

        string CreatedBy { get; set; }

    }
}
