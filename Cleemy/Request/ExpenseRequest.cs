namespace Cleemy.Api.Request
{
    public class ExpenseRequest
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public Decimal Amount { get; set; }

        public string Currency { get; set; }

        public int UserId { get; set; }
    }
}
