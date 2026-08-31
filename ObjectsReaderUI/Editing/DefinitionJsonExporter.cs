using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using RenData.Definitions;

namespace ObjectsReaderUI.Editing;

/// <summary>
/// Serializes a definition to indented JSON. Mirrors what the property grid shows: every
/// instance field (public and protected) plus public readable properties, recursed into nested
/// objects and collections. Enums are written by name, byte[] as base64.
/// </summary>
public static class DefinitionJsonExporter
{
    private static readonly JsonSerializerOptions WriteOptions = new() { WriteIndented = true };

    // Guards against runaway recursion on deep/self-referential graphs.
    private const int MaxDepth = 12;

    public static string ToJson(DefinitionClass definition)
    {
        var root = BuildNode(definition, new HashSet<object>(ReferenceEqualityComparer.Instance), 0);
        return root?.ToJsonString(WriteOptions) ?? "null";
    }

    private static JsonNode? BuildNode(object? value, HashSet<object> visited, int depth)
    {
        if (value is null) return null;

        var type = value.GetType();

        if (type.IsEnum) return JsonValue.Create(value.ToString());
        if (value is string || type.IsPrimitive) return JsonSerializer.SerializeToNode(value, type);
        if (value is byte[] bytes) return JsonValue.Create(Convert.ToBase64String(bytes));

        if (depth >= MaxDepth) return JsonValue.Create($"({type.Name})");

        // Reference cycles would otherwise loop forever; drop the object from the visited set on
        // the way out so the same instance can still appear in independent (diamond) branches.
        if (!visited.Add(value)) return JsonValue.Create("(circular reference)");
        try
        {
            if (value is IEnumerable sequence)
            {
                var array = new JsonArray();
                foreach (var item in sequence)
                    array.Add(BuildNode(item, visited, depth + 1));
                return array;
            }

            var obj = new JsonObject();

            foreach (var field in DefinitionReflection.GetAllInstanceFields(type))
            {
                object? member;
                try { member = field.GetValue(value); }
                catch { continue; }
                obj[field.Name] = BuildNode(member, visited, depth + 1);
            }

            foreach (var prop in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                     .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                                     .OrderBy(p => p.Name))
            {
                if (obj.ContainsKey(prop.Name)) continue;
                object? member;
                try { member = prop.GetValue(value); }
                catch { continue; }
                obj[prop.Name] = BuildNode(member, visited, depth + 1);
            }

            return obj;
        }
        finally
        {
            visited.Remove(value);
        }
    }
}
