using CSharpFunctionalExtensions;
using Logic.Dtos;
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
        if (customer is null)
            return NotFound();

        var dto = new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name.Value,
            Email = customer.Email.Value,
            MoneySpent = customer.MoneySpent,
            Status = customer.Status.Type.ToString(),
            StatusExpirationDate = customer.Status.ExpirationDate,
            PurchasedMovies = customer.PurchasedMovies?.Select(x => new PurchasedMovieDto
            {
                Price = x.Price,
                ExpirationDate = x.ExpirationDate,
                PurchaseDate = x.PurchaseDate,
                Movie = new MovieDto
                {
                    Id = x.Movie.Id,
                    Name = x.Movie.Name
                }
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpGet]
    public IActionResult GetList()
    {
        IReadOnlyList<Customer> customers = _customerRepository.GetList();

        List<CustomerInListDto> dtos = customers.Select(x => new CustomerInListDto
        {
            Id = x.Id,
            Name = x.Name.Value,
            Email = x.Email.Value,
            MoneySpent = x.MoneySpent,
            Status = x.Status.Type.ToString(),
            StatusExpirationDate = x.Status.ExpirationDate
        }).ToList();
        
        return Ok(dtos);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateCustomerDto item)
    {
        Result<CustomerName> customerNameOrError = CustomerName.Create(item.Name);
        Result<Email> emailOrError = Email.Create(item.Email);

        Result result = Result.Combine(customerNameOrError, emailOrError);
        if (result.IsFailure)
            return BadRequest(result);

        if (_customerRepository.GetByEmail(emailOrError.Value) != null)
            return BadRequest("Email is already in use: " + item.Email);

        try
        {
            var customer = new Customer(customerNameOrError.Value, emailOrError.Value);
            _customerRepository.Add(customer);
            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] UpdateCustomerDto item)
    {
        Result<CustomerName> customerNameOrError = CustomerName.Create(item.Name);
        if (customerNameOrError.IsFailure)
            return BadRequest(customerNameOrError.Error);

        Customer? customer = _customerRepository.GetById(id);
        if (customer is null)
            return BadRequest("Invalid customer id: " + id);

        try
        {
            customer.Name = customerNameOrError.Value;
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
                    x.Movie.Id == movie.Id && !x.ExpirationDate.IsExpired))
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

            if (customer.Status.IsAdvanced)
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
