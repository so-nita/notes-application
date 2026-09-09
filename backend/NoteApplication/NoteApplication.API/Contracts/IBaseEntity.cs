namespace NoteApplication.API.Contracts;

public interface IBaseEntity<TK> where TK : notnull
{
    TK? CreatedBy { get; set; }

    bool IsDeleted { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
}