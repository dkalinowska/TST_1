using Newtonsoft.Json;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "employees.json");

    public EmployeeRepository()
    {
        if (!File.Exists(_filePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            File.WriteAllText(_filePath, "[]");
        }
    }

    public List<Employee> GetAll()
    {
        var jsonData = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<List<Employee>>(jsonData);
    }

    public Employee GetById(int id)
    {
        return GetAll().FirstOrDefault(e => e.Id == id);
    }

    public void Add(Employee employee)
    {
        var employees = GetAll();
        employees.Add(employee);
        SaveData(employees);
    }

    public void Update(Employee employee)
    {
        var employees = GetAll();
        var index = employees.FindIndex(e => e.Id == employee.Id);
        if (index != -1)
        {
            employees[index] = employee;
            SaveData(employees);
        }
    }

    public void Delete(int id)
    {
        var employees = GetAll();
        var employee = employees.FirstOrDefault(e => e.Id == id);
        if (employee != null)
        {
            employees.Remove(employee);
            SaveData(employees);
        }
    }

    private void SaveData(List<Employee> employees)
    {
        var jsonData = JsonConvert.SerializeObject(employees, Formatting.Indented);
        File.WriteAllText(_filePath, jsonData);
    }
}
