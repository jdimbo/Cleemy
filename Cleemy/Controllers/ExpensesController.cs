using Cleemy.Api.Request;
using Cleemy.Api.Response;
using Cleemy.Application;
using Cleemy.Domain;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cleemy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : CleemyControllerBase
    {

        private readonly IExpenseService expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            this.expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
        }

        

        [HttpGet()]
        [ProducesResponseType(typeof(IList<ExpenseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IEnumerable<ExpenseResponse>> Get(int? userId, string? sortColumn)
        {

            List<Expense> expenses = await expenseService.GetExpensesAsync(userId, sortColumn);

            List<ExpenseResponse > expensesResponse = new List<ExpenseResponse>();
            foreach (var expenseLoop in expenses)
            {
                ExpenseResponse expenseResponse = new ExpenseResponse { 
                    Description = expenseLoop.Description,
                    Date = expenseLoop.Date,
                    Amount = expenseLoop.Amount,
                    Type = expenseLoop.ExpenseType.ToString(),
                    User = String.Format("{0} {1}", expenseLoop.User.FirstName, expenseLoop.User.LastName),
                    Currency = expenseLoop.Currency.Symbol
                };

                expensesResponse.Add(expenseResponse);
            }

            return expensesResponse;
        }

        // POST api/<UsersController>
        [HttpPost]
        [ProducesResponseType(typeof(ExpenseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ExpenseRequest expenseRequest)
        {
            ExpenseTypeEnum expenseTypeEnum;
            if (!Enum.TryParse(expenseRequest.Type, out expenseTypeEnum))
            {
                return BadRequest("Type is invalid : values possible : Restaurant, Hotel or Misc");
            }

            ExpenseCreateModel expenseCreateModel = new ExpenseCreateModel
            {
                Description = expenseRequest.Description,
                Date = expenseRequest.Date,
                Amount = expenseRequest.Amount,
                Type = expenseTypeEnum,
                UserId = expenseRequest.UserId,
                Currency = expenseRequest.Currency
            };


            var resultExpense = await expenseService.AddAsync(expenseCreateModel);

            if(resultExpense.Item1 != null)
            {
                return BadRequest(resultExpense.Item1);
            }

            return Ok(resultExpense.Item2);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
