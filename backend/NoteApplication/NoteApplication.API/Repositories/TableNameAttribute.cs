namespace NoteApplication.API.Repositories;

[AttributeUsage(AttributeTargets.Class)]
public class TableNameAttribute : Attribute
{
    public string TableName { get; }
    public TableNameAttribute(string name) => TableName = name;
}

[AttributeUsage(AttributeTargets.Property)]
public class KeyColumnAttribute : Attribute
{
    
}