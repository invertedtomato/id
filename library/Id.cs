namespace InvertedTomato.Ids;
// Inspired by https://github.com/andrewlock/StronglyTypedId/blob/master/test/StronglyTypedIds.Tests/Snapshots/SourceGenerationHelperSnapshotTests.GeneratesFullIdCorrectly_type%3DGuid.verified.txt

[System.Text.Json.Serialization.JsonConverter(typeof(IdSystemTextJsonConverterFactory))]
public readonly struct Id<T> : IId, IComparable<Id<T>>, IEquatable<Id<T>> where T : new()
{
    private readonly Guid _underlying;

    public Id(Guid underlying)
    {
        _underlying = underlying;
    }

    public Id(String underlying)
    {
        _underlying = Guid.Parse(underlying);
    }

    public static Id<T> New() => new(Guid.NewGuid());

    public Boolean Equals(Id<T> other) => _underlying.Equals(other._underlying);

    public override Boolean Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is Id<T> other && Equals(other);
    }

    public override Int32 GetHashCode() => _underlying.GetHashCode();

    public override String ToString() => _underlying.ToString();
    public Guid ToGuid() => _underlying;
    public Int32 CompareTo(Id<T> other) => _underlying.CompareTo(other._underlying);

    public static Boolean operator ==(Id<T> a, Id<T> b) => a.Equals(b);
    public static Boolean operator !=(Id<T> a, Id<T> b) => !(a == b);
}

public interface IId
{
    string ToString();
    Guid ToGuid();
}

public class IdSystemTextJsonConverterFactory : System.Text.Json.Serialization.JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeof(IId).IsAssignableFrom(typeToConvert);

    public override System.Text.Json.Serialization.JsonConverter CreateConverter(Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
    {
        return new IdSystemTextJsonConverter();
    }
}

public class IdSystemTextJsonConverter : System.Text.Json.Serialization.JsonConverter<IId?>
{
    public override void Write(System.Text.Json.Utf8JsonWriter writer, IId? value, System.Text.Json.JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    public override IId? Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
    {
        var underlying = reader.GetString();
        if (string.IsNullOrEmpty(underlying)) return null;
        return (IId)Activator.CreateInstance(typeToConvert, underlying)!;
    }
}