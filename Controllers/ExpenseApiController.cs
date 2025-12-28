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


}