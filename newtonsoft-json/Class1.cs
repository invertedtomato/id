namespace newtonsoft_json;

public class IdNewtonSoftJsonConverter : Newtonsoft.Json.JsonConverter
{
    public override bool CanConvert(Type objectType) => typeof(IId).IsAssignableFrom(objectType);

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (value == null) throw new ArgumentNullException();
        var id = (IId)value;
        serializer.Serialize(writer, id.ToString());
    }

    public override object? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        var underlying = serializer.Deserialize<string?>(reader);
        if (string.IsNullOrEmpty(underlying)) return null;
        return Activator.CreateInstance(objectType, underlying);
    }
}