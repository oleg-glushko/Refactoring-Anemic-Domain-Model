using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Utils;

public class BaseController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public BaseController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected new IActionResult Ok()
    {
        _dbContext.SaveChanges();
        return base.Ok(Envelope.Ok());
    }

    protected IActionResult Ok<T>(T result)
    {
        _dbContext.SaveChanges();
        return base.Ok(Envelope.Ok(result));
    }

    protected IActionResult Error(string errorMessage)
    {
        return BadRequest(Envelope.Error(errorMessage));
    }
}
