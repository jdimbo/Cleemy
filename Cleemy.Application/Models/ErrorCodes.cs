using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleemy.Application
{
    public static class ErrorCodes
    {
        public static readonly string EXPENSE_ENTITY_IS_NULL = $"EXPENSE_ENTITY_IS_NULL";
        public static readonly string EXPENSE_CURRENCY_NOT_FOUND = $"EXPENSE_CURRENCY_NOT_FOUND";
        public static readonly string EXPENSE_USER_NOT_FOUND = $"EXPENSE_USER_NOT_FOUND";
        public static readonly string EXPENSE_DATE_FUTURE = $"EXPENSE_DATE_FUTURE";
        public static readonly string EXPENSE_DATE_PAST_3_MONTH = $"EXPENSE_DATE_PAST_3_MONTH";
        public static readonly string EXPENSE_DESCRITPTION_IS_MANDATORY = $"EXPENSE_DESCRITPTION_IS_MANDATORY";
        public static readonly string EXPENSE_CURRENCY_NOT_USER_CURRENCY = $"EXPENSE_CURRENCY_NOT_USER_CURRENCY";
        public static readonly string EXPENSE_ALREADY_EXISTS_SAME_DATE = $"EXPENSE_ALREADY_EXISTS_SAME_DATE";

    }
}
