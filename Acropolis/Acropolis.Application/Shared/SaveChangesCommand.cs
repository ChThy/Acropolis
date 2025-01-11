using Acropolis.Shared.Commands;

namespace Acropolis.Application.Shared;

public record SaveChangesCommand(IEnumerable<object>? Add = null, IEnumerable<object>? Remove = null) : ICommand
{
    public static readonly SaveChangesCommand Instance = new();

    public static SaveChangesCommand AddSave(IEnumerable<object> add) => new(add);
    public static SaveChangesCommand AddSave(object add) => AddSave([add]);
    public static SaveChangesCommand RemoveSave(IEnumerable<object> remove) => new(null, remove);
    public static SaveChangesCommand RemoveSave(object remove) => RemoveSave([remove]);
}