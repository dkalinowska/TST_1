using Microsoft.AspNetCore.Mvc;


public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _repository;

    public EmployeeController(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var employees = _repository.GetAll();

        if (employees == null || !employees.Any())
        {
            Console.WriteLine("Brak pracowników do wyświetlenia");
        }

        return View(employees);
    }

    public IActionResult GetEmployees()
    {
        var employees = _repository.GetAll();
        return Json(employees);
    }

    public IActionResult Details(int id)
    {
        var employee = _repository.GetById(id);
        if (employee == null)
            return NotFound();

        return View(employee);
    }

    public IActionResult Create()
    {
        ViewBag.Departments = Departments.List;
        return View();
    }

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        if (ModelState.IsValid)
        {
            employee.Id = GenerateId();
            _repository.Add(employee);
            return RedirectToAction("Index");
        }
        ViewBag.Departments = Departments.List;
        return View(employee);
    }

    public IActionResult Edit(int id)
    {
        var employee = _repository.GetById(id);
        if (employee == null)
            return NotFound();

        ViewBag.Departments = Departments.List;
        return View(employee);
    }

    [HttpPost]
    public IActionResult Edit(Employee employee)
    {
        if (ModelState.IsValid)
        {
            _repository.Update(employee);
            return RedirectToAction("Index");
        }
        ViewBag.Departments = Departments.List;
        return View(employee);
    }

    public IActionResult Delete(int id)
    {
        var employee = _repository.GetById(id);
        if (employee == null)
            return NotFound();

        return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        _repository.Delete(id);
        TempData["SuccessMessage"] = $"Pomyślnie usunięto pracownika o ID: {id}";
        return RedirectToAction("Index");
    }

    private int GenerateId()
    {
        var employees = _repository.GetAll();
        return employees.Any() ? employees.Max(e => e.Id) + 1 : 1;
    }

    [HttpGet]
    public IActionResult GetPositions(string department)
    {
        if (Positions.DepartmentPositions.ContainsKey(department))
        {
            var positions = Positions.DepartmentPositions[department];
            return Json(positions);
        }
        return Json(new List<string>());
    }

    public IActionResult Charts()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetDepartmentChartData()
    {
        var employees = _repository.GetAll();
        var data = employees.GroupBy(e => e.Department)
                            .Select(g => new { Department = g.Key, Count = g.Count() })
                            .ToList();
        return Json(data);
    }

    [HttpGet]
    public IActionResult GetEmploymentDateChartData()
    {
        var employees = _repository.GetAll();

        var data = employees
            .GroupBy(e => e.EmploymentDate.Year)
            .OrderBy(g => g.Key)
            .Select(g => new
            {
                Year = g.Key,
                Count = g.Count()
            })
            .ToList();

        return Json(data);
    }


    [HttpGet]
    public IActionResult GetAverageSalaryChartData()
    {
        var employees = _repository.GetAll();

        var data = employees
            .GroupBy(e => e.Position)
            .Select(g => new
            {
                Position = g.Key,
                AverageSalary = g.Average(e => e.Salary)
            })
            .OrderByDescending(g => g.AverageSalary)
            .ToList();

        return Json(data);
    }
}
