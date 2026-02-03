using BudgetPlanner.Commands;
using BudgetPlanner.Data;
using BudgetPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BudgetPlanner.ViewModels
{
    public class EntryViewModel : ViewModelBase
    {
        private readonly BudgetPlannerContext _context;

        // Income properties
        private string _incomeDescription = string.Empty;
        private decimal _incomeAmount;
        private IncomeCategory _selectedIncomeCategory;
        private RecurrenceType _selectedIncomeRecurrence;
        private int _incomeRecurrenceMonth = 1;
        private Income? _selectedIncome;

        // Expense properties
        private string _expenseDescription = string.Empty;
        private decimal _expenseAmount;
        private ExpenseCategory _selectedExpenseCategory;
        private RecurrenceType _selectedExpenseRecurrence;
        private int _expenseRecurrenceMonth = 1;
        private Expense? _selectedExpense;

        public ObservableCollection<Income> Incomes { get; set; }
        public ObservableCollection<Expense> Expenses { get; set; }

        // Income properties
        public string IncomeDescription
        {
            get => _incomeDescription;
            set => SetProperty(ref _incomeDescription, value);
        }

        public decimal IncomeAmount
        {
            get => _incomeAmount;
            set => SetProperty(ref _incomeAmount, value);
        }

        public IncomeCategory SelectedIncomeCategory
        {
            get => _selectedIncomeCategory;
            set => SetProperty(ref _selectedIncomeCategory, value);
        }

        public RecurrenceType SelectedIncomeRecurrence
        {
            get => _selectedIncomeRecurrence;
            set => SetProperty(ref _selectedIncomeRecurrence, value);
        }

        public int IncomeRecurrenceMonth
        {
            get => _incomeRecurrenceMonth;
            set => SetProperty(ref _incomeRecurrenceMonth, value);
        }

        public Income? SelectedIncome
        {
            get => _selectedIncome;
            set
            {
                SetProperty(ref _selectedIncome, value);
                if (value != null)
                {
                    IncomeDescription = value.Description;
                    IncomeAmount = value.Amount;
                    SelectedIncomeCategory = value.Category;
                    SelectedIncomeRecurrence = value.Recurrence;
                    IncomeRecurrenceMonth = value.RecurrenceMonth ?? 1;
                }
            }
        }

        // Expense properties
        public string ExpenseDescription
        {
            get => _expenseDescription;
            set => SetProperty(ref _expenseDescription, value);
        }

        public decimal ExpenseAmount
        {
            get => _expenseAmount;
            set => SetProperty(ref _expenseAmount, value);
        }

        public ExpenseCategory SelectedExpenseCategory
        {
            get => _selectedExpenseCategory;
            set => SetProperty(ref _selectedExpenseCategory, value);
        }

        public RecurrenceType SelectedExpenseRecurrence
        {
            get => _selectedExpenseRecurrence;
            set => SetProperty(ref _selectedExpenseRecurrence, value);
        }

        public int ExpenseRecurrenceMonth
        {
            get => _expenseRecurrenceMonth;
            set => SetProperty(ref _expenseRecurrenceMonth, value);
        }

        public Expense? SelectedExpense
        {
            get => _selectedExpense;
            set
            {
                SetProperty(ref _selectedExpense, value);
                if (value != null)
                {
                    ExpenseDescription = value.Description;
                    ExpenseAmount = value.Amount;
                    SelectedExpenseCategory = value.Category;
                    SelectedExpenseRecurrence = value.Recurrence;
                    ExpenseRecurrenceMonth = value.RecurrenceMonth ?? 1;
                }
            }
        }

        // Commands
        public ICommand AddIncomeCommand { get; }
        public ICommand UpdateIncomeCommand { get; }
        public ICommand DeleteIncomeCommand { get; }
        public ICommand ClearIncomeFormCommand { get; }

        public ICommand AddExpenseCommand { get; }
        public ICommand UpdateExpenseCommand { get; }
        public ICommand DeleteExpenseCommand { get; }
        public ICommand ClearExpenseFormCommand { get; }

        public EntryViewModel(BudgetPlannerContext context)
        {
            _context = context;
            Incomes = new ObservableCollection<Income>();
            Expenses = new ObservableCollection<Expense>();

            // Initialize commands
            AddIncomeCommand = new RelayCommand(_ => AddIncome());
            UpdateIncomeCommand = new RelayCommand(_ => UpdateIncome(), _ => SelectedIncome != null);
            DeleteIncomeCommand = new RelayCommand(_ => DeleteIncome(), _ => SelectedIncome != null);
            ClearIncomeFormCommand = new RelayCommand(_ => ClearIncomeForm());

            AddExpenseCommand = new RelayCommand(_ => AddExpense());
            UpdateExpenseCommand = new RelayCommand(_ => UpdateExpense(), _ => SelectedExpense != null);
            DeleteExpenseCommand = new RelayCommand(_ => DeleteExpense(), _ => SelectedExpense != null);
            ClearExpenseFormCommand = new RelayCommand(_ => ClearExpenseForm());

            LoadData();
        }

        public void LoadData()
        {
            Incomes.Clear();
            Expenses.Clear();

            var incomes = _context.Incomes.ToList();
            var expenses = _context.Expenses.ToList();

            foreach (var income in incomes)
                Incomes.Add(income);

            foreach (var expense in expenses)
                Expenses.Add(expense);
        }

        private void AddIncome()
        {
            if (string.IsNullOrWhiteSpace(IncomeDescription) || IncomeAmount <= 0)
            {
                MessageBox.Show("Please enter a valid description and amount.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var income = new Income
            {
                Description = IncomeDescription,
                Amount = IncomeAmount,
                Category = SelectedIncomeCategory,
                Recurrence = SelectedIncomeRecurrence,
                RecurrenceMonth = SelectedIncomeRecurrence == RecurrenceType.Yearly ? IncomeRecurrenceMonth : null,
                CreatedDate = DateTime.Now
            };

            _context.Incomes.Add(income);
            _context.SaveChanges();
            Incomes.Add(income);
            ClearIncomeForm();
            MessageBox.Show("Income added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateIncome()
        {
            if (SelectedIncome == null) return;

            if (string.IsNullOrWhiteSpace(IncomeDescription) || IncomeAmount <= 0)
            {
                MessageBox.Show("Please enter a valid description and amount.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedIncome.Description = IncomeDescription;
            SelectedIncome.Amount = IncomeAmount;
            SelectedIncome.Category = SelectedIncomeCategory;
            SelectedIncome.Recurrence = SelectedIncomeRecurrence;
            SelectedIncome.RecurrenceMonth = SelectedIncomeRecurrence == RecurrenceType.Yearly ? IncomeRecurrenceMonth : null;

            _context.SaveChanges();
            LoadData();
            ClearIncomeForm();
            MessageBox.Show("Income updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteIncome()
        {
            if (SelectedIncome == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete '{SelectedIncome.Description}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _context.Incomes.Remove(SelectedIncome);
                _context.SaveChanges();
                Incomes.Remove(SelectedIncome);
                ClearIncomeForm();
                MessageBox.Show("Income deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ClearIncomeForm()
        {
            IncomeDescription = string.Empty;
            IncomeAmount = 0;
            SelectedIncomeCategory = IncomeCategory.Salary;
            SelectedIncomeRecurrence = RecurrenceType.OneTime;
            IncomeRecurrenceMonth = 1;
            SelectedIncome = null;
        }

        private void AddExpense()
        {
            if (string.IsNullOrWhiteSpace(ExpenseDescription) || ExpenseAmount <= 0)
            {
                MessageBox.Show("Please enter a valid description and amount.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var expense = new Expense
            {
                Description = ExpenseDescription,
                Amount = ExpenseAmount,
                Category = SelectedExpenseCategory,
                Recurrence = SelectedExpenseRecurrence,
                RecurrenceMonth = SelectedExpenseRecurrence == RecurrenceType.Yearly ? ExpenseRecurrenceMonth : null,
                CreatedDate = DateTime.Now
            };

            _context.Expenses.Add(expense);
            _context.SaveChanges();
            Expenses.Add(expense);
            ClearExpenseForm();
            MessageBox.Show("Expense added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateExpense()
        {
            if (SelectedExpense == null) return;

            if (string.IsNullOrWhiteSpace(ExpenseDescription) || ExpenseAmount <= 0)
            {
                MessageBox.Show("Please enter a valid description and amount.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedExpense.Description = ExpenseDescription;
            SelectedExpense.Amount = ExpenseAmount;
            SelectedExpense.Category = SelectedExpenseCategory;
            SelectedExpense.Recurrence = SelectedExpenseRecurrence;
            SelectedExpense.RecurrenceMonth = SelectedExpenseRecurrence == RecurrenceType.Yearly ? ExpenseRecurrenceMonth : null;

            _context.SaveChanges();
            LoadData();
            ClearExpenseForm();
            MessageBox.Show("Expense updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteExpense()
        {
            if (SelectedExpense == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete '{SelectedExpense.Description}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _context.Expenses.Remove(SelectedExpense);
                _context.SaveChanges();
                Expenses.Remove(SelectedExpense);
                ClearExpenseForm();
                MessageBox.Show("Expense deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ClearExpenseForm()
        {
            ExpenseDescription = string.Empty;
            ExpenseAmount = 0;
            SelectedExpenseCategory = ExpenseCategory.Food;
            SelectedExpenseRecurrence = RecurrenceType.OneTime;
            ExpenseRecurrenceMonth = 1;
            SelectedExpense = null;
        }
    }

}
