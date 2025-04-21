using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ContactService
{
    private List<Contact> contacts = new List<Contact>();
    private readonly string filePath;

    public ContactService(string path)
    {
        filePath = path;
        LoadContacts();
    }

    public void AddContact(Contact c)
    {
        contacts.Add(c);
        Console.WriteLine("已新增聯絡人");
    }

    public List<Contact> GetAllContacts()
    {
        return contacts;
    }

    public List<Contact> Search(string keyword)
    {
        return contacts.FindAll(c => c.Name.ToLower().Contains(keyword.ToLower()));
    }

    public int Delete(string keyword)
    {
        return contacts.RemoveAll(c => c.Name.ToLower().Contains(keyword.ToLower()));
    }

    public void SaveContacts()
    {
        JsonService.SaveToJson<Contact>(contacts, filePath);
    }

    private void LoadContacts()
    {
        if (File.Exists(filePath))
        {
            contacts = JsonService.LoadFromJson<Contact>(filePath);
        }
    }
}