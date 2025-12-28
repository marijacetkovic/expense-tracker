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
        // GET: /Expense/Index
        public async Task<IActionResult> Index()
        {   
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "User");
            }
            var domainExpenses = await _expenseService.GetAllUserExpensesAsync(HttpContext.Session.GetInt32("UserId").Value);

            var viewModels = domainExpenses.Select(e => new ExpenseViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                Amount = e.TotalAmount,
                CreatedByUserId = e.CreatedByUserId,
                ParticipantCount = e.Participants.Count
            }).ToList();

            return View(viewModels);
        }
        // GET: /Expense/ExpenseForm
        public IActionResult ExpenseForm() {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "User");
            }
            var model = new AddExpenseViewModel();
            return View(model);
        }

        // GET: /Expense/Details/{id}
        public async Task<IActionResult> ExpenseDetails(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "User");
            }
            var expense = await _expenseService.GetExpenseWithParticipantsAsync(id);
            
            if (expense == null) return NotFound();

            var currentUserId = HttpContext.Session.GetInt32("UserId");

            var viewModel = new ExpenseDetailsViewModel
            {
                Id = expense.Id,
                Name = expense.Name,
                Date = expense.Date,
                TotalAmount = expense.Participants.Sum(p => p.ShareAmount),
                CreatedByUserId = expense.CreatedByUserId,
                UserShare = expense.Participants.FirstOrDefault(p => p.UserId == currentUserId)?.ShareAmount ?? 0,
                Participants = expense.Participants.Select(p => new ParticipantViewModel
                {
                    Username = p.User.Username,
                    ShareAmount = p.ShareAmount,
                    IsCurrentUser = p.UserId == currentUserId
                    }).ToList()
                };

            return View(viewModel);
        }
 }
}
