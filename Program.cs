using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

class Program
{
    static List<Contact> contacts = new List<Contact>();
    static readonly string contactFilePath = Path.Combine(Directory.GetCurrentDirectory(), "contacts.json");


    static void Main(string[] args)
    {
        if (File.Exists(contactFilePath))
        {
            string readText = File.ReadAllText(contactFilePath); 
            Console.WriteLine($"使用資料檔路徑：{contactFilePath}");
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
        Console.WriteLine("Choose contact type:");
        Console.WriteLine("1. General Contact");
        Console.WriteLine("2. Friend Contact");
        Console.WriteLine("3. Business Contact");
        Console.Write("Type: ");
        string type = Console.ReadLine();

        Console.Write("Enter name: ");
        string name = Console.ReadLine();
        Console.Write("Enter phone: ");
        string phone = Console.ReadLine();

        switch (type)
        {
            case "1":
                contacts.Add(new Contact(name, phone));
                break;
            case "2":
                Console.Write("Enter nickname: ");
                string nickname = Console.ReadLine();
                contacts.Add(new FriendContact(name, phone, nickname));
                break;
            case "3":
                Console.Write("Enter company: ");
                string company = Console.ReadLine();
                contacts.Add(new BusinessContact(name, phone, company));
                break;
            default:
                Console.WriteLine("Unknown type.");
                break;
        }

        Console.WriteLine("Contact added.");
    }

    static void ViewContacts()
    {
        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts.");
            return;
        }

        while (true)
        {
            Console.WriteLine("\n--- View Contacts ---");
            Console.WriteLine("1. 顯示所有聯絡人");
            Console.WriteLine("2. 依姓名升冪排序");
            Console.WriteLine("3. 依姓名降冪排序");
            Console.WriteLine("4. 搜尋姓名關鍵字");
            Console.WriteLine("5. 返回主選單");
            Console.Write("請選擇：");

            string option = Console.ReadLine();
            List<Contact> result = new List<Contact>();

            switch (option)
            {
                case "1":
                    result = contacts.ToList();
                    break;
                case "2":
                    result = contacts.OrderBy(c => c.Name).ToList();
                    break;
                case "3":
                    result = contacts.OrderByDescending(c => c.Name).ToList();
                    break;
                case "4":
                    Console.Write("輸入搜尋關鍵字：");
                    string keyword = Console.ReadLine();
                    result = contacts
                        .Where(c => c.Name.ToLower().Contains(keyword.ToLower()))
                        .ToList();
                    break;
                case "5":
                    return; // 回主選單
                default:
                    Console.WriteLine("無效選項，請重試");
                    continue;
            }

            Console.WriteLine($"\n共 {result.Count} 筆聯絡人：");
            foreach (var c in result)
                c.Display();
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
            File.WriteAllText(contactFilePath, json);
            Console.WriteLine("聯絡人已儲存到 contacts.json");
        }
        catch (Exception ex)
        {
            Console.WriteLine("儲存失敗：" + ex.Message);
        }

    }
}
