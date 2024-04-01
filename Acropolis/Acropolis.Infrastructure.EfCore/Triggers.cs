namespace Acropolis.Infrastructure.EfCore;
public static class Triggers
{
    public static string GetCreateRowVersionTriggerSql(string tableName, string rowVersionColumn = "RowVersion")
    {
        return $@"CREATE TRIGGER Increment{tableName}RowVersion
            AFTER UPDATE ON {tableName}
            BEGIN
                UPDATE {tableName}     
                SET {rowVersionColumn} = {rowVersionColumn} + 1
                WHERE rowid = NEW.rowid;
            END;";
    }
}
