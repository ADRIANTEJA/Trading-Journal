using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace MainModule.Common.Utils;

public class JsonFileUtils
{
    private static readonly JsonSerializerSettings jsonSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        Converters = { new StringEnumConverter() },    
    };
    /// <summary>
    /// serializes an object into a json file, if the files does not exists
    /// it is created on the specified path, if it exists its contents are overwritten
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="filePath"></param>
    public static void SerializeJsonFile(object obj, string filePath)
    {
        using var streamWriter = File.CreateText(filePath);
        using var jsonWriter = new JsonTextWriter(streamWriter);

        JsonSerializer.CreateDefault(jsonSettings).Serialize(jsonWriter, obj);
    }
    /// <summary>
    /// deserializes a json file into an object
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="filePath"></param>
    /// <returns>returns an object with the values of the json file
    /// if the file does not exist null is returned</returns>
    public static object? DeserializeJsonFile<T>(string filePath)
    {
        if (File.Exists(filePath))
        {
            using var streamReader = File.OpenText(filePath);
            using var jsonReader = new JsonTextReader(streamReader);

            return JsonSerializer.CreateDefault(jsonSettings).Deserialize<T>(jsonReader);
        }
        else return null;
    }
    /// <summary>
    /// executes JsonFileUtils.SerializeJsonFile method asynchronously
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static async Task SerializeJsonFileAsync(object settingsObj, string filePath)
    {
        await Task.Run(() => SerializeJsonFile(settingsObj, filePath));
    }
    /// <summary>
    /// executes JsonFileUtils.DeserializeJsonFile method asynchronously
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>returns an object with the values of the json file
    /// if the file does not exist null is returned</returns>
    public static async Task<object?>DeserializeJsonFileAsync<T>(string filePath)
    {
       return await Task.Run(() => DeserializeJsonFile<T>(filePath));
    }
}
