using System;
using System.Collections.Generic;

class Program
{
    static List<Contact> contacts = new List<Contact>();

    static void Main(string[] args)
    {
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
}
