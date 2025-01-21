using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class JsonDataSerializer
{
    public static string Serialize(object data, string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new System.ArgumentNullException("path", "path cannot be empty");

        var json = JsonConvert.SerializeObject(data);

        try
        {
            File.WriteAllText(path, json);
            return json;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to write file {path} with Exception {ex}");
            return string.Empty;
        }
    }

    public static T Deserialize<T>(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new System.ArgumentNullException("path", "path cannot be empty");
        }
        if (!File.Exists(path))
        {
            Debug.LogError($"Failed to load file at {path} because it does not exist");
            return default;
        }
        string json = File.ReadAllText(path);

        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load file {path} with Exception {ex}");
            return default;
        }
    }
}