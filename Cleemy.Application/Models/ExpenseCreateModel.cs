using Cleemy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleemy.Application
{
    public class ExpenseCreateModel
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public ExpenseTypeEnum Type { get; set; }
        public Decimal Amount { get; set; }

        public string Currency { get; set; }

        public int UserId { get; set; }
    }
}
