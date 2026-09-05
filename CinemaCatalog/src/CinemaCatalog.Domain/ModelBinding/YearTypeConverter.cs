using System.ComponentModel;
using System.Globalization;
using CinemaCatalog.Domain.ValueObjects;

namespace CinemaCatalog.Domain.ModelBinding;

public class YearTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value is string str ? Year.Parse(str) : base.ConvertFrom(context, culture, value);

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => destinationType == typeof(string) && value is Year year
            ? year.ToString()
            : base.ConvertTo(context, culture, value, destinationType);
}