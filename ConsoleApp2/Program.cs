using System;
using System.Collections.Generic;
using System.Linq;

public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
        return items;
    }
    public T? GetById(Func<T, bool> predicate) 
    { 
        return items.FirstOrDefault(predicate);
    }
    public bool Remove(Func<T, bool> predicate)
    {
        var item = items.FirstOrDefault(predicate);
        if (item != null)
        {
            items.Remove(item);
            return true;
        }
            return false;
    }
}

public class Patient(int id, string name, int age, string gender)
{
    public int Id { get; } = id;
    public int Age { get; } = age;
    public string Name { get; } = name;
    public string Gender { get; } = gender;

}

public class Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
{
    public int Id { get; } = id;
    public int PatientId { get; } = patientId;
    public string MedicationName { get; } = medicationName;
    public DateTime DateIssued { get; } = dateIssued;   
 
}

public class HealthSystemApp {
    private Repository<Patient> _patientRepo = new Repository<Patient>();
    private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
    private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

    public void SeedData()
    {
        _patientRepo.Add(new Patient(1, "Kofi Amponsah", 20, "Male"));
        _patientRepo.Add(new Patient(2, "Ama Boatemaa", 17, "Female"));
        _patientRepo.Add(new Patient(3, "Nii Lante", 22, "Male"));
        _patientRepo.Add(new Patient(4, "Cynthia Ofori", 25, "Female"));

        _prescriptionRepo.Add(new Prescription(1, 1, "Amoxicillin", DateTime.Now.AddDays(-10)));
        _prescriptionRepo.Add(new Prescription(2, 4, "Ibuprofen", DateTime.Now.AddDays(-5)));
        _prescriptionRepo.Add(new Prescription(3, 2, "Paracetamol", DateTime.Now.AddDays(-3)));
        _prescriptionRepo.Add(new Prescription(4, 3, "Lisinopril", DateTime.Now.AddDays(-7)));
        _prescriptionRepo.Add(new Prescription(5, 2, "Cetirizine", DateTime.Now.AddDays(-1)));
    }

    public void BuildPrescriptionMap()
    {
        _prescriptionMap.Clear();
        foreach (var prescription in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] = new List<Prescription>();
            }
            _prescriptionMap[prescription.PatientId].Add(prescription);
        }
    }

    public void PrintAllPatients()
    {
        Console.WriteLine("************* All Patients ****************");
        foreach (var patient in _patientRepo.GetAll())
        {
            Console.WriteLine($"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");
        }
    }

    public void PrintPrescriptionsForPatient(int id)
    {
        Console.WriteLine($"\n******** Prescriptions for Patient ID {id} *********");

        if (_prescriptionMap.ContainsKey(id))
        {
            foreach (var prescription in _prescriptionMap[id])
            {
                Console.WriteLine($"Prescription ID: {prescription.Id}, Medication: {prescription.MedicationName}, Date Issued: {prescription.DateIssued.ToShortDateString()}");
            }
        }
        else
        {
            Console.WriteLine("No prescriptions found for this patient.");
        }
    }
}

public class Program
{
    public static void Main()
    {
        var app = new HealthSystemApp();    

        app.SeedData();
        app.BuildPrescriptionMap();
        app.PrintAllPatients();

        Console.WriteLine("Enter patient ID:");
        if (int.TryParse(Console.ReadLine(), out int patientId))
        {
            app.PrintPrescriptionsForPatient(patientId);
        }
        else
        {
            Console.WriteLine("Input a valid number.");
        }

        
    }
}