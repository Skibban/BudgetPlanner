using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlanner.Data;

namespace BudgetPlanner.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly BudgetPlannerContext _context;

        public EntryViewModel EntryViewModel { get; }
        public SummaryViewModel SummaryViewModel { get; }
        public MonthlyEstimateViewModel MonthlyEstimateViewModel { get; }
        public MainViewModel(BudgetPlannerContext context)
        {
            _context = context;
            EntryViewModel = new EntryViewModel(_context);
            SummaryViewModel = new SummaryViewModel(_context);
            MonthlyEstimateViewModel = new MonthlyEstimateViewModel(_context);
        }

        public void RefreshAllData()
        {
            EntryViewModel.LoadData();
            SummaryViewModel.LoadSummary();
            MonthlyEstimateViewModel.LoadEstimates();
        }
    }
}
