public static class Departments
{
    public static readonly List<string> List = ["IT", "HR", "Marketing"];
}

public static class Positions
{
    public static Dictionary<string, List<string>> DepartmentPositions = new()
    {
        { "IT", new List<string> { "Deweloper", "Tester", "Lider", "Analityk" } },
        { "HR", new List<string> { "Specjalista ds. wynagrodzeń", "Specjalista ds. szkoleń", "Specjalista ds. rekrutacji" } },
        { "Marketing", new List<string> { "Doradca klienta", "Przedstawiciel handlowy", "Doradca handlowy" } }
    };
}
