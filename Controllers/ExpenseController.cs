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
        [HttpGet("/Expense/Index")]
        public IActionResult Index() => View();

        [HttpGet("/Expense/ExpenseForm")]
        public IActionResult ExpenseForm() => View();

        [HttpGet("/Expense/ExpenseDetails/{id}")]
        public IActionResult ExpenseDetails(int id) => View(id);
    }
}