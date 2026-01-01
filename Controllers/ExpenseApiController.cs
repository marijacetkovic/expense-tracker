using ExpenseTracker.Models;
using ExpenseTracker.Strategies;
using Microsoft.AspNetCore.Mvc;

[ApiController]

[Route("api/[controller]")]
public class ExpenseApiController : ControllerBase
{
    private readonly ExpenseService _expenseService;

    public ExpenseApiController(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    // GET: api/expenseapi 
        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var domainExpenses = await _expenseService.GetAllUserExpensesAsync(userId.Value);

            // Mapping to the ViewModel structure your JavaScript expects
            var result = domainExpenses.Select(e => new 
            {
                id = e.Id,
                name = e.Name,
                timestamp = e.Timestamp,
                amount = e.TotalAmount,
                createdByUserId = e.CreatedByUserId,
                participantCount = e.Participants.Count
            });

            return Ok(result);
        }

        // GET: api/expenseapi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null) return Unauthorized();

            var expense = await _expenseService.GetExpenseWithParticipantsAsync(id);
            if (expense == null) return NotFound();

            // Mapping to JSON structure
            var result = new
            {
                id = expense.Id,
                name = expense.Name,
                timestamp = expense.Timestamp,
                totalAmount = expense.Participants.Sum(p => p.ShareAmount),
                createdByUserId = expense.CreatedByUserId,
                userShare = expense.Participants.FirstOrDefault(p => p.UserId == currentUserId)?.ShareAmount ?? 0,
                participants = expense.Participants.Select(p => new
                {
                    userId = p.UserId,
                    username = p.User.Username,
                    shareAmount = p.ShareAmount,
                    isCurrentUser = p.UserId == currentUserId
                }).ToList()
            };

            return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddExpenseViewModel model)
    {
        var currentUserId = HttpContext.Session.GetInt32("UserId");

        if (currentUserId == null)
        {
            return Unauthorized(new { message = "Session expired. Please login again." });
        }

        var strategy = SplitStrategyFactory.Create(model.SplitType);
        bool success = await _expenseService.AddExpenseAsync(model, strategy, currentUserId.Value);

        if (!success) return UnprocessableEntity();
        return StatusCode(201); // Created
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "User");

        var success = await _expenseService.DeleteExpenseAsync(id, userId.Value);
        
        if (!success) return Forbid(); //403

        return NoContent(); // 204
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, [FromBody] AddExpenseViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();//401
        var success = await _expenseService.UpdateExpenseAsync(id, model, SplitStrategyFactory.Create(model.SplitType));
        if (!success) return Forbid(); //403

        return NoContent(); // 204 means "I did it, nothing more to show"
        }

}