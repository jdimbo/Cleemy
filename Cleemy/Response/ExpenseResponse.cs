namespace Cleemy.Api.Response
{
    public class ExpenseResponse
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public Decimal Amount { get; set; }

        public string Currency { get; set; }

        public string User { get; set; }
    }
}
