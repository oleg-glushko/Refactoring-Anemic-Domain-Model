﻿using Logic.Common;
using Logic.Utils;
using Microsoft.EntityFrameworkCore;

namespace Logic.Customers;

public class CustomerRepository : Repository<Customer>
{
    public CustomerRepository(AppDbContext context) : base(context)
    {
    }

    public IReadOnlyList<Customer> GetList()
    {
        _context.ChangeTracker.LazyLoadingEnabled = false;
        return _context.Customers
            .AsNoTracking()
            .ToList();
    }

    public Customer? GetByEmail(Email email)
    {
        return _context.Customers
            .SingleOrDefault(x => x.Email == email);
    }
}
