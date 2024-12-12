using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeContext _context;
    public EmployeeRepository(EmployeeContext context)
    {
        _context = context;
    }

    public List<Employee> GetAll()
    {
        return _context.Employees.ToList();
    }

    public Employee GetById(int id)
    {
        return _context.Employees.Find(id);
    }

    public void Add(Employee employee)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Employees ON");
            _context.Employees.Add(employee);
            _context.SaveChanges();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Employees OFF");
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }


    public void Update(Employee employee)
    {
        _context.Employees.Update(employee);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var employee = GetById(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}
