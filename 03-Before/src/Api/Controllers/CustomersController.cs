using Logic.Entities;
using Logic.Repositories;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly MovieRepository _movieRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly CustomerService _customerService;

    public CustomersController(MovieRepository movieRepository,
        CustomerRepository customerRepository, CustomerService customerService)
    {
        _movieRepository = movieRepository;
        _customerRepository = customerRepository;
        _customerService = customerService;
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        Customer? customer = _customerRepository.GetById(id);
        return customer is null
            ? NotFound()
            : Ok(customer);
    }

    [HttpGet]
    public IActionResult GetList()
    {
        IReadOnlyList<Customer> customers = _customerRepository.GetList();
        return Ok(customers);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Customer item)
    {
        if (_customerRepository.GetByEmail(item.Email) != null)
            return BadRequest("Email is already in use: " + item.Email);

        try
        {
            item.Id = 0;
            item.Status = CustomerStatus.Regular;
            _customerRepository.Add(item);
            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] Customer item)
    {
        Customer? customer = _customerRepository.GetById(id);
        if (customer is null)
            return BadRequest("Invalid customer id: " + id);

        try
        {
            customer.Name = item.Name;
            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("{id}/movies")]
    public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
    {
        try
        {
            Movie? movie = _movieRepository.GetById(movieId);
            if (movie is null)
                return BadRequest("Invalid movie id: " + movieId);

            Customer? customer = _customerRepository.GetById(id);
            if (customer is null)
                return BadRequest("Invalid customer id: " + id);

            if (customer.PurchasedMovies is not null && customer.PurchasedMovies.Any(x =>
                    x.MovieId == movie.Id && (x.ExpirationDate == null || x.ExpirationDate.Value >= DateTime.UtcNow)))
                return BadRequest("The movie is already purchased: " + movie.Name);

            _customerService.PurchaseMovie(customer, movie);

            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("{id}/promotion")]
    public IActionResult PromoteCustomer(long id)
    {
        try
        {
            Customer? customer = _customerRepository.GetById(id);
            if (customer is null)
                return BadRequest("Invalid customer id: " + id);

            if (customer.Status == CustomerStatus.Advanced &&
                    (customer.StatusExpirationDate == null || customer.StatusExpirationDate.Value > DateTime.UtcNow))
                return BadRequest("The customer is already has the Advanced status");

            bool success = _customerService.PromoteCustomer(customer);

            if (!success)
                return BadRequest("Cannot promote the customer");

            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
