using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

class Program
{
    static List<Contact> contacts = new List<Contact>();
    static readonly string contactFilePath = Path.Combine(Directory.GetCurrentDirectory(), "contacts.json");
    static readonly string groupFolder = Path.Combine(Directory.GetCurrentDirectory(), "Groups");

    static List<Group> groups = new List<Group>();



    static void Main(string[] args)
    {
        contacts = JsonService.LoadFromJson<Contact>(contactFilePath);
        LoadAllGroups();

        while (true)
        {
            Console.WriteLine("\n--- Contact Manager ---");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. View All Contacts");
            Console.WriteLine("3. Search Contact by Name");
            Console.WriteLine("4. Delete Contact by Name");
            Console.WriteLine("5. Exit");
            Console.WriteLine("6. 群組功能");
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
                    SaveAllGroups();
                    return;
                case "6":
                    GroupMenu();
                    break;

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
        JsonService.SaveToJson<Contact>(contacts, contactFilePath);
    }

    static void AddContactToGroup()
    {
        Console.Write("輸入群組名稱：");
        string groupName = Console.ReadLine();
        var group = groups.Find(g => g.Name == groupName);
        if (group == null)
        {
            Console.WriteLine("找不到群組！");
            return;
        }

        Console.Write("聯絡人姓名：");
        string name = Console.ReadLine();
        Console.Write("電話：");
        string phone = Console.ReadLine();

        var contact = new Contact(name, phone);
        group.AddMember(contact);
        Console.WriteLine($"已加入 {name} 至群組 {groupName}");
    }


    static void GroupMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- 群組功能 ---");
            Console.WriteLine("1. 建立群組");
            Console.WriteLine("2. 顯示所有群組");
            Console.WriteLine("3. 加聯絡人到群組");
            Console.WriteLine("4. 顯示某群組成員");
            Console.WriteLine("5. 返回主選單");
            Console.Write("請選擇：");

            string opt = Console.ReadLine();
            switch (opt)
            {
                case "1":
                    Console.Write("請輸入群組名稱：");
                    string name = Console.ReadLine();
                    groups.Add(new Group(name));
                    Console.WriteLine($"已建立群組：{name}");
                    break;
                case "2":
                    foreach (var g in groups)
                        Console.WriteLine($"📁 {g.Name}（{g.Members.Count} 人）");
                    break;
                case "3":
                    AddContactToGroup();
                    break;
                case "4":
                    Console.Write("輸入要查看的群組名稱：");
                    string gname = Console.ReadLine();
                    var target = groups.Find(g => g.Name == gname);
                    if (target != null)
                        target.Display();
                    else
                        Console.WriteLine("群組不存在。");
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("無效選項");
                    break;
            }
        }
    }
    static void SaveAllGroups()
    {
        if (!Directory.Exists(groupFolder))
            Directory.CreateDirectory(groupFolder);

        foreach (var group in groups)
        {
            string filePath = Path.Combine(groupFolder, $"{group.Name}.json");
            JsonService.SaveToJson<Contact>(group.Members, filePath);
        }
        Console.WriteLine("所有群組已儲存");
    }
    static void LoadAllGroups()
    {
        if (!Directory.Exists(groupFolder))
            return;

        string[] files = Directory.GetFiles(groupFolder, "*.json");

        foreach (var file in files)
        {
            string groupName = Path.GetFileNameWithoutExtension(file);
            var members = JsonService.LoadFromJson<Contact>(file);

            var group = new Group(groupName);
            group.Members = members;
            groups.Add(group);
        }

        Console.WriteLine($"已載入 {groups.Count} 個群組");
    }

}

