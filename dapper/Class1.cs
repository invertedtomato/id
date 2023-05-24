namespace dapper;


public class DapperTypeHandler : Dapper.SqlMapper.TypeHandler<Id<T>>
{
    public override void SetValue(System.Data.IDbDataParameter parameter, Id<T> value)
    {
        parameter.Value = value.ToGuid();
    }

    public override Id<T> Parse(object value)
    {
        return value switch
        {
            Guid guidValue => new(guidValue),
            string stringValue when !string.IsNullOrEmpty(stringValue) && Guid.TryParse(stringValue, out var result) => new(result),
            _ => throw new InvalidCastException($"Unable to cast object of type {value.GetType()} to {nameof(Id<T>)}"),
        };
    }
}