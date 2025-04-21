using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

class Program
{
    //static List<Contact> contacts = new List<Contact>();
    static ContactService contactService;
    static GroupService groupService;
    //static readonly string contactFilePath = Path.Combine(Directory.GetCurrentDirectory(), "contacts.json");
    //static readonly string groupFolder = Path.Combine(Directory.GetCurrentDirectory(), "Groups");

    static List<Group> groups = new List<Group>();



    static void Main(string[] args)
    {
        string contactFilePath = Path.Combine(Directory.GetCurrentDirectory(), "contacts.json");
        contactService = new ContactService(contactFilePath);
        string groupFolder = Path.Combine(Directory.GetCurrentDirectory(), "Groups");
        groupService = new GroupService(groupFolder);

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
                    groupService.SaveAllGroups();
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
        Console.Write("Enter name: ");
        string name = Console.ReadLine();
        Console.Write("Enter phone: ");
        string phone = Console.ReadLine();

        contactService.AddContact(new Contact(name, phone));
    }

    static void ViewContacts()
    {
         var all = contactService.GetAllContacts();
        if (all.Count == 0)
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
                    result = all.ToList();
                    break;
                case "2":
                    result = all.OrderBy(c => c.Name).ToList();
                    break;
                case "3":
                    result = all.OrderByDescending(c => c.Name).ToList();
                    break;
                case "4":
                    Console.Write("輸入搜尋關鍵字：");
                    string keyword = Console.ReadLine();
                    result = contactService.Search(keyword);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("無效選項");
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
        var found = contactService.Search(name);

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
        int removed = contactService.Delete(name);
        Console.WriteLine($"{removed} contact(s) deleted.");
    }

    static void Storejson()
    {
        contactService.SaveContacts();
    }

    static void AddContactToGroup()
    {
        Console.Write("輸入群組名稱：");
        string groupName = Console.ReadLine();

        Console.Write("聯絡人姓名：");
        string name = Console.ReadLine();
        Console.Write("電話：");
        string phone = Console.ReadLine();

        var contact = new Contact(name, phone);
        groupService.AddContactToGroup(groupName, contact);
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
                    groupService.AddGroup(name);
                    break;
                case "2":
                    var groups = groupService.GetAllGroups();
                    foreach (var g in groups)
                        Console.WriteLine($"群組：{g.Name}（{g.Members.Count} 人）");
                    break;
                case "3":
                    AddContactToGroup();
                    break;
                case "4":
                    Console.Write("輸入要查看的群組名稱：");
                    string gname = Console.ReadLine();
                    var target = groupService.FindGroup(gname);
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
}

