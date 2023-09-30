using Api.Utils;
using CSharpFunctionalExtensions;
using Logic.Customers;
using Logic.Movies;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Customers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : BaseController
{
    private readonly MovieRepository _movieRepository;
    private readonly CustomerRepository _customerRepository;

    public CustomersController(AppDbContext dbContext, MovieRepository movieRepository,
        CustomerRepository customerRepository) : base(dbContext)
    {
        _movieRepository = movieRepository;
        _customerRepository = customerRepository;
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        //throw new Exception("test");

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
            return Error(result.Error);

        if (_customerRepository.GetByEmail(emailOrError.Value) != null)
            return Error("Email is already in use: " + item.Email);

        var customer = new Customer(customerNameOrError.Value, emailOrError.Value);
        _customerRepository.Add(customer);

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] UpdateCustomerDto item)
    {
        Result<CustomerName> customerNameOrError = CustomerName.Create(item.Name);
        if (customerNameOrError.IsFailure)
            return Error(customerNameOrError.Error);

        Customer? customer = _customerRepository.GetById(id);
        if (customer is null)
            return Error("Invalid customer id: " + id);

        customer.Name = customerNameOrError.Value;

        return Ok();
    }

    [HttpPost("{id}/movies")]
    public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
    {
        Movie? movie = _movieRepository.GetById(movieId);
        if (movie is null)
            return Error("Invalid movie id: " + movieId);

        Customer? customer = _customerRepository.GetById(id);
        if (customer is null)
            return Error("Invalid customer id: " + id);

        if (customer.HasPurchasedMovie(movie))
            return Error("The movie is already purchased: " + movie.Name);

        customer.PurchasedMovie(movie);

        return Ok();
    }

    [HttpPost("{id}/promotion")]
    public IActionResult PromoteCustomer(long id)
    {
        Customer? customer = _customerRepository.GetById(id);
        if (customer is null)
            return Error("Invalid customer id: " + id);

        Result promotionCheck = customer.CanPromote();
        if (promotionCheck.IsFailure)
            return Error(promotionCheck.Error);

        customer.Promote();

        return Ok();
    }
}
