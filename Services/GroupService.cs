using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GroupService
{
    private readonly string groupFolder;
    private List<Group> groups;

    public GroupService(string folderPath)
    {
        groupFolder = folderPath;
        groups = new List<Group>();
        LoadAllGroups();
    }

    public void AddGroup(string name)
    {
        if (groups.Any(g => g.Name == name))
        {
            Console.WriteLine("群組名稱已存在");
            return;
        }
        groups.Add(new Group(name));
        Console.WriteLine($"已建立群組：{name}");
    }

    public List<Group> GetAllGroups() => groups;

    public Group FindGroup(string name)
    {
        return groups.Find(g => g.Name == name);
    }

    public void AddContactToGroup(string groupName, Contact contact)
    {
        var group = FindGroup(groupName);
        if (group != null)
        {
            group.AddMember(contact);
            Console.WriteLine($"已加入 {contact.Name} 至群組 {groupName}");
        }
        else
        {
            Console.WriteLine("找不到群組");
        }
    }

    public void SaveAllGroups()
    {
        if (!Directory.Exists(groupFolder))
            Directory.CreateDirectory(groupFolder);

        foreach (var group in groups)
        {
            string filePath = Path.Combine(groupFolder, $"{group.Name}.json");
            JsonService.SaveToJson<Contact>(group.Members, filePath);
        }
    }

    private void LoadAllGroups()
    {
        if (!Directory.Exists(groupFolder)) return;

        string[] files = Directory.GetFiles(groupFolder, "*.json");
        foreach (var file in files)
        {
            string groupName = Path.GetFileNameWithoutExtension(file);
            var members = JsonService.LoadFromJson<Contact>(file);
            var group = new Group(groupName);
            group.Members = members;
            groups.Add(group);
        }
    }
}
