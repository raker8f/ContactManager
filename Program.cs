using System;
using System.Collections.Generic;
using System.Text.Json;

class Program
{
    static List<Contact> contacts = new List<Contact>();

    static void Main(string[] args)
    {
        if (File.Exists("contacts.json"))
        {
            string readText = File.ReadAllText("contacts.json");  // 讀 JSON 字串
            var loadedContacts = JsonSerializer.Deserialize<List<Contact>>(readText);

            if (loadedContacts != null)
            {
                contacts = loadedContacts; 
                Console.WriteLine("讀取資料如下：");
                foreach (var s in contacts)
                {
                    Console.WriteLine($"Name: {s.Name}, Phone: {s.Phone}");
                }
            }
        }
        else
        {
            Console.WriteLine("找不到 contacts.json 檔案！");
        }


        while (true)
        {
            Console.WriteLine("\n--- Contact Manager ---");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. View All Contacts");
            Console.WriteLine("3. Search Contact by Name");
            Console.WriteLine("4. Delete Contact by Name");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddContact();
                    break;
                case "2":
                    ViewContacts();
                    break;
                case "3":
                    SearchContact();
                    break;
                case "4":
                    DeleteContact();
                    break;
                case "5":
                    Storejson();
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void AddContact()
    {
        Console.Write("Enter name: ");
        string name = Console.ReadLine();
        Console.Write("Enter phone: ");
        string phone = Console.ReadLine();

        contacts.Add(new Contact(name, phone));
        Console.WriteLine("Contact added.");
    }

    static void ViewContacts()
    {
        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts.");
            return;
        }

        foreach (var contact in contacts)
        {
            contact.Display();
        }
    }

    static void SearchContact()
    {
        Console.Write("Enter name to search: ");
        string name = Console.ReadLine();
        var found = contacts.FindAll(c => c.Name.ToLower().Contains(name.ToLower()));

        if (found.Count == 0)
        {
            Console.WriteLine("No contact found.");
        }
        else
        {
            foreach (var c in found)
                c.Display();
        }
    }

    static void DeleteContact()
    {
        Console.Write("Enter name to delete: ");
        string name = Console.ReadLine();
        int removed = contacts.RemoveAll(c => c.Name.ToLower().Contains(name.ToLower()));
        Console.WriteLine($"{removed} contact(s) deleted.");
    }

    static void Storejson()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(contacts,options); // 轉成 JSON 字串

        try
        {
            File.WriteAllText("contacts.json", json);
            Console.WriteLine("聯絡人已儲存到 contacts.json");
        }
        catch (Exception ex)
        {
            Console.WriteLine("儲存失敗：" + ex.Message);
        }

    }
}
