using BudgetPlanner.Data;
using BudgetPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.ViewModels
{
    public class CategorySummary
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class SummaryViewModel : ViewModelBase
    {
        private readonly BudgetPlannerContext _context;
        private decimal _totalIncome;
        private decimal _totalExpenses;
        private decimal _netBalance;
        private List<CategorySummary> _incomeSummary = new();
        private List<CategorySummary> _expenseSummary = new();

        public decimal TotalIncome
        {
            get => _totalIncome;
            set => SetProperty(ref _totalIncome, value);
        }

        public decimal TotalExpenses
        {
            get => _totalExpenses;
            set => SetProperty(ref _totalExpenses, value);
        }

        public decimal NetBalance
        {
            get => _netBalance;
            set => SetProperty(ref _netBalance, value);
        }

        public List<CategorySummary> IncomeSummary
        {
            get => _incomeSummary;
            set => SetProperty(ref _incomeSummary, value);
        }

        public List<CategorySummary> ExpenseSummary
        {
            get => _expenseSummary;
            set => SetProperty(ref _expenseSummary, value);
        }

        public SummaryViewModel(BudgetPlannerContext context)
        {
            _context = context;
            LoadSummary();
        }

        public void LoadSummary()
        {
            // Calculate total income (one-time + monthly)
            var incomes = _context.Incomes.ToList();
            TotalIncome = incomes.Where(i => i.Recurrence == RecurrenceType.OneTime).Sum(i => i.Amount) +
                          incomes.Where(i => i.Recurrence == RecurrenceType.Monthly).Sum(i => i.Amount);

            // Calculate total expenses (one-time + monthly)
            var expenses = _context.Expenses.ToList();
            TotalExpenses = expenses.Where(e => e.Recurrence == RecurrenceType.OneTime).Sum(e => e.Amount) +
                            expenses.Where(e => e.Recurrence == RecurrenceType.Monthly).Sum(e => e.Amount);

            NetBalance = TotalIncome - TotalExpenses;

            // Income by category
            IncomeSummary = incomes
                .GroupBy(i => i.Category)
                .Select(g => new CategorySummary
                {
                    Category = g.Key.ToString(),
                    Amount = g.Where(i => i.Recurrence == RecurrenceType.OneTime).Sum(i => i.Amount) +
                             g.Where(i => i.Recurrence == RecurrenceType.Monthly).Sum(i => i.Amount)
                })
                .OrderByDescending(cs => cs.Amount)
                .ToList();

            // Expenses by category
            ExpenseSummary = expenses
                .GroupBy(e => e.Category)
                .Select(g => new CategorySummary
                {
                    Category = g.Key.ToString(),
                    Amount = g.Where(e => e.Recurrence == RecurrenceType.OneTime).Sum(e => e.Amount) +
                             g.Where(e => e.Recurrence == RecurrenceType.Monthly).Sum(e => e.Amount)
                })
                .OrderByDescending(cs => cs.Amount)
                .ToList();
        }

    }
}
