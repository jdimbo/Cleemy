using Cleemy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleemy.Application
{
    public interface IExpenseService
    {
        Task<List<Expense>> GetExpensesAsync(int? userId, string? sortColumn);

        Task<(string, Expense?)> AddAsync(ExpenseCreateModel entity);
    }
}
