Console.OutputEncoding = System.Text.Encoding.UTF8;

// Просимо користувача ввести назву файлу
Console.WriteLine("Шлях до папки з файлами: " + Directory.GetCurrentDirectory());
Console.WriteLine("Введіть назву файлу (Приклад: thermo.wav):");
string fileName = Console.ReadLine();
Console.Clear();

try
{
    // Отримуємо шлях до папки проекту
    string projectDirectory = Directory.GetCurrentDirectory();

    // Об'єднуємо шлях до папки проекту з назвою файлу
    string filePath = Path.Combine(projectDirectory, fileName);

    // Читаємо бінарний файл та робимо підрахунок кількості кожного байта
    var byteCounts = CountBytes(filePath);

    // Виводимо результат
    Console.WriteLine("Байт - Кількість:");
    foreach (var byteCount in byteCounts.OrderBy(pair => pair.Key))
    {
        Console.WriteLine($"0x{byteCount.Key:X2} - {byteCount.Value}");
    }
}
catch (Exception ex)
{
    // Обробка помилок
    Console.WriteLine($"Виникла помилка: {ex.Message}");
}

static IDictionary<byte, int> CountBytes(string filePath)
{
    // Читаємо бінарний файл
    byte[] bytes = File.ReadAllBytes(filePath);

    // Використовуємо LINQ для підрахунку кількості кожного байта
    var byteCounts = bytes.GroupBy(b => b)
                          .ToDictionary(g => g.Key, g => g.Count());

    return byteCounts;
}