using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Models
{
    public enum CategoryType
    {
        Income,
        Expense
    }
    
    public enum IncomeCategory
    {
        Salary,
        Dividend,
        Grant,
    }

    public enum ExpenseCategory
    {
        Food,
        Housing,
        Transport,
        Leisure,
        Subscriptions,
        Savings,

    }

    public enum RecurrenceType
    {
        OneTime,
        Monthly,
        Yearly
    }
}
