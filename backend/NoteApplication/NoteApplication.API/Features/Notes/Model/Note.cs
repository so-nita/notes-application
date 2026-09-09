using System.ComponentModel.DataAnnotations.Schema;
using NoteApplication.API.Contracts;
using NoteApplication.API.Features.Auth.Model;
using NoteApplication.API.Repositories;

namespace NoteApplication.API.Features.Notes.Model;

public abstract class NoteBase
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; } = string.Empty;
}

[TableName("Notes")]
public class Note : NoteBase, IBaseEntity<String>
{
    [KeyColumn]
    public string Id { get; set; } = string.Empty;
    
    public bool IsDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class NoteResponse : NoteBase
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class NoteCreateReq : NoteBase
{
    
}

public class NoteUpdateReq
{
    public string? Title { get; set; } = string.Empty;

    public string? Content { get; set; }
}