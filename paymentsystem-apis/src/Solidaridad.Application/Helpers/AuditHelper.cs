using Newtonsoft.Json;
using Solidaridad.Application.Models.AuditLog;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Solidaridad.Application.Helpers;

public static class AuditHelper
{
    private static readonly HashSet<string> ExcludedFields = new()
    {
        "ChangedOn", "UpdatedOn", "CreatedOn" // Add more fields if needed
    };

    public static string SerializeFiltered<T>(T obj)
    {
        if (obj == null) return null;

        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false
        };

        var dictionary = obj.GetType()
                            .GetProperties()
                            .Where(p => !ExcludedFields.Contains(p.Name))
                            .ToDictionary(p => p.Name, p => p.GetValue(obj));

        return JsonSerializer.Serialize(dictionary, options);
    }

    public static List<AuditChange> GetChangedFields<T>(string oldValuesJson, string newValuesJson)
    {
        // Deserialize JSON into objects of type T
        var oldValues = JsonConvert.DeserializeObject<T>(oldValuesJson);
        var newValues = JsonConvert.DeserializeObject<T>(newValuesJson);

        // Ensure both objects are not null
        if (oldValues == null || newValues == null)
            return new List<AuditChange>();

        var changes = new List<AuditChange>();

        // Get all properties of the entity
        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            // Read old and new values of the property
            var oldValue = prop.GetValue(oldValues)?.ToString();
            var newValue = prop.GetValue(newValues)?.ToString();

            // Check if values are different
            if (oldValue != newValue)
            {
                changes.Add(new AuditChange
                {
                    Field = prop.Name,
                    OldValue = oldValue,
                    NewValue = newValue
                });
            }
        }

        return changes;
    }

}
