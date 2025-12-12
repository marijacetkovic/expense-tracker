using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Data;
using ExpenseTracker.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ExpenseService _expenseService;
        public ExpenseController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public async Task<IActionResult> Index()
        {
            var domainExpenses = await _expenseService.GetAllUserExpensesAsync();

            var viewModels = domainExpenses.Select(e => new ExpenseViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Amount = e.Amount,
                Date = e.Date,
            }).ToList();

            return View(viewModels);
        }

        public IActionResult ExpenseForm() {
            var model = new AddExpenseViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ExpenseForm(AddExpenseViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool success = await _expenseService.AddExpenseAsync(
                model.Title,
                model.Amount,
                model.Date,
                model.Participants.Select(p => p.Username).ToList()
            );

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to create expense. Check participant usernames.");
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}
