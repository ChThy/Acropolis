using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Acropolis.Infrastructure.EfCore.Converters;

public class DateTimeOffsetToDateTimeConverter() : ValueConverter<DateTimeOffset, DateTime>(
    v => v.UtcDateTime,
    v => new(v));