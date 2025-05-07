using Foodtek.DTOs.AddItemToCart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Foodtek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> AddItemToCart(AddItemInput input)
        {
            try
            {


                throw new NotImplementedException("This method is not implemented yet.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new AddItemOutput
                    {
                        StatusCode = -1,
                        Message = $"Internal server error: {ex.Message}"
                    });
            }
        }
    }
}