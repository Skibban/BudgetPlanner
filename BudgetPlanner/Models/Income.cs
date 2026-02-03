using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Models
{
    public class Income
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public IncomeCategory Category { get; set; }
        public RecurrenceType Recurrence { get; set; }

        public int? RecurrenceMonth { get; set; } // For yearly recurrence, indicates the month (1-12)
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
