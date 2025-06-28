using System.Data;
using Dapper;

namespace IMDBApi_Assignment4.Helpers
{
    public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        // Converts DateOnly to DateTime for sending to DB
        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        }

        // Converts DateTime from DB to DateOnly
        public override DateOnly Parse(object value)
        {
            return value switch
            {
                DateTime dateTime => DateOnly.FromDateTime(dateTime),
                _ => throw new DataException($"Cannot convert {value.GetType()} to DateOnly")
            };
        }
    }
}
