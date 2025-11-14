using System.Runtime.Serialization;
using System.Text.Json;

namespace Formster.Application.Utils;

public static class FormSubmissionJsonDataSerializer
{
    public static Dictionary<string, object> DeserializeJsonData(string jsonData) 
        => JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData) ??
           throw new SerializationException("Json data cannot being deserialized.");
}
