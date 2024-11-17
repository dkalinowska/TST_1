using System.ComponentModel.DataAnnotations;

public class Employee
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Imię jest wymagane")]
    public required string FirstName { get; set; }

    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "PESEL jest wymagany")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL musi składać się z 11 cyfr")]
    public required string PESEL { get; set; }

    [Required(ErrorMessage = "Data zatrudnienia jest wymagana")]
    public DateTime EmploymentDate { get; set; }

    [Required(ErrorMessage = "Departament jest wymagany")]
    public required string Department { get; set; }

    [Required(ErrorMessage = "Stanowisko jest wymagane")]
    public required string Position { get; set; }

    [Required(ErrorMessage = "Wynagrodzenie jest wymagane")]
    [Range(0, 1000000, ErrorMessage = "Wynagrodzenie musi być z przedziału 0-1000000")]
    public decimal Salary { get; set; }
}
