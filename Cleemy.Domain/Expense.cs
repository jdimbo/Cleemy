namespace Cleemy.Domain
{
    public class Expense
    {
        public Expense()
        {
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public DateOnly Date { get; set; }
        public ExpenseTypeEnum ExpenseType { get; set; }
        public Decimal Amount { get; set; }
       
        public int CurrencyId { get; set; }
        public int UserId { get; set; }


        public virtual Currency Currency { get; set; }
        public virtual User User { get; set; }



    }
}