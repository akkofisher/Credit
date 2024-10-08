using Credit.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Credit.API.Controllers;

public class ApiController : ControllerBase
{

    protected new IActionResult Response(object? resultData = null)
    {
       
            return Ok(
                new ResponseMessage<object>
                {
                    Success = true,
                    Data = resultData
                });
       
    }
}