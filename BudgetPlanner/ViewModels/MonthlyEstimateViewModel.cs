using BudgetPlanner.Data;
using BudgetPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.ViewModels
{
    public class MonthlyEstimate
    {
        public string Month { get; set; } = string.Empty;
        public int MonthNumber { get; set; }
        public decimal EstimatedIncome { get; set; }
        public decimal EstimatedExpenses { get; set; }
        public decimal EstimatedBalance { get; set; }

    }


    public class MonthlyEstimateViewModel : ViewModelBase
    {
        private readonly BudgetPlannerContext _context;
        private List<MonthlyEstimate> _monthlyEstimates = new();
        private int _selectedYear;

        public List<MonthlyEstimate> MonthlyEstimates
        {
            get => _monthlyEstimates;
            set => SetProperty(ref _monthlyEstimates, value);
        }

        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (SetProperty(ref _selectedYear, value))
                {
                    LoadEstimates();
                }
            }
        }

        public MonthlyEstimateViewModel(BudgetPlannerContext context)
        {
            _context = context;
            _selectedYear = DateTime.Now.Year;
            LoadEstimates();
        }

        public void LoadEstimates()
        {
            var estimates = new List<MonthlyEstimate>();

            var incomes = _context.Incomes.ToList();
            var expenses = _context.Expenses.ToList();

            for (int month = 1; month <= 12; month++)
            {
                // Calculate income for this month
                decimal monthlyIncome = 0;

                // Add all monthly recurring income
                monthlyIncome += incomes
                    .Where(i => i.Recurrence == RecurrenceType.Monthly)
                    .Sum(i => i.Amount);

                // Add yearly recurring income that occurs in this month
                monthlyIncome += incomes
                    .Where(i => i.Recurrence == RecurrenceType.Yearly && i.RecurrenceMonth == month)
                    .Sum(i => i.Amount);

                // Calculate expenses for this month
                decimal monthlyExpenses = 0;

                // Add all monthly recurring expenses
                monthlyExpenses += expenses
                    .Where(e => e.Recurrence == RecurrenceType.Monthly)
                    .Sum(e => e.Amount);

                // Add yearly recurring expenses that occur in this month
                monthlyExpenses += expenses
                    .Where(e => e.Recurrence == RecurrenceType.Yearly && e.RecurrenceMonth == month)
                    .Sum(e => e.Amount);

                estimates.Add(new MonthlyEstimate
                {
                    Month = new DateTime(SelectedYear, month, 1).ToString("MMMM"),
                    MonthNumber = month,
                    EstimatedIncome = monthlyIncome,
                    EstimatedExpenses = monthlyExpenses,
                    EstimatedBalance = monthlyIncome - monthlyExpenses
                });
            }

            MonthlyEstimates = estimates;
        }

    }
}
