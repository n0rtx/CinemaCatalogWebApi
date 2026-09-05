using System.ComponentModel;
using CinemaCatalog.Domain.ModelBinding;

namespace CinemaCatalog.Domain.ValueObjects;

[TypeConverter(typeof(YearTypeConverter))]
public class Year
{
    public int Start { get; set; }

    public int? End { get; set; }
    
    public static Year Parse(string input)
    {
        var parts = input.Split('–', '-');
        var start = Convert.ToInt32(parts[0]);
        int? end = parts.Length > 1 && int.TryParse(parts[1], out var e) ? e : null;

        return new Year
        {
            Start = start,
            End = end,
        };
    }

    public override string ToString() => End.HasValue ? $"{Start} - {End}" : Start.ToString();
}