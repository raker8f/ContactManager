using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class JsonService
{
    public static void SaveToJson<T>(List<T> data, string filePath)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
            Console.WriteLine("資料已儲存到 " + filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("儲存失敗：" + ex.Message);
        }
    }

    public static List<T> LoadFromJson<T>(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("找不到檔案：" + filePath);
                return new List<T>();
            }

            string readText = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<List<T>>(readText);
            return data ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("讀取失敗：" + ex.Message);
            return new List<T>();
        }
    }
}
