using System;
using  sample_api.Models;
using  sample_api.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]

public class MenuController : ControllerBase
{
    private readonly MenuService _menuService;
    public  MenuController(MenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpPost("resource")]
    public async Task<IActionResult> GetResourcesByRole([FromBody] RoleRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Role))
        {
            return BadRequest("Role is required.");
        }
        var resources = await _menuService.GetResourcesByRole(request.Role);
        Console.WriteLine(resources.Count);
        if (resources == null || resources.Count == 0)
        {
            return NotFound("No resources found for the specified role.");
        }
        return Ok(resources);
    }
}