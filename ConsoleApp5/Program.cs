using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


public interface IInventoryEntity
{
    int Id { get; }
}


public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

public class InventoryLogger<T> where T : IInventoryEntity
{
    private List<T> _log = new List<T>();
    private readonly string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(T item)
    {
        _log.Add(item);
    }
    public List<T> GetAll()
    {
        return _log;
    }

    public void SaveToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(_log, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
            Console.WriteLine("Data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }
    public void LoadFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("File not found. No data loaded.");
                return;
            }

            string json = File.ReadAllText(_filePath);
            _log = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            Console.WriteLine("Data loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
        }
    }

}

public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger = new InventoryLogger<InventoryItem>(filePath);
    }

    public void SeedSampleData()
    {
        _logger.Add(new InventoryItem(1, "iPhone 15 Pro Max", 40, DateTime.Now));
        _logger.Add(new InventoryItem(2, "iPad Mini", 30, DateTime.Now));
        _logger.Add(new InventoryItem(3, "Macbook Pro 2024", 10, DateTime.Now));
        _logger.Add(new InventoryItem(4, "Honda Accord", 5, DateTime.Now));
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        var items = _logger.GetAll();
        foreach (var item in items)
        {
            Console.WriteLine($"{item.Name} (ID: {item.Id}) - Quantity: {item.Quantity}, Added: {item.DateAdded}");
        }
    }
}

class Program
{
    static void Main()
    {
        string filePath = "inventory.json";

        
        InventoryApp app = new InventoryApp(filePath);
        app.SeedSampleData();
        app.SaveData();

        
        Console.WriteLine("\n****** New Session *******");
        InventoryApp newApp = new InventoryApp(filePath);
        newApp.LoadData();
        newApp.PrintAllItems();
    }
}