using System;
using System.Collections.Generic;

public class Group
{
    public string Name { get; set; }
    public List<Contact> Members { get; set; }

    public Group() 
    {
        Members = new List<Contact>();
    }

    public Group(string name)
    {
        Name = name;
        Members = new List<Contact>();
    }

    public void AddMember(Contact c)
    {
        Members.Add(c);
    }

    public void Display()
    {
        Console.WriteLine($"\n 群組：{Name}（共 {Members.Count} 人）");
        foreach (var m in Members)
        {
            m.Display();
        }
    }
}
