using System.Reflection;
using Dapper;
using NoteApplication.API.Contexts;

namespace NoteApplication.API.Repositories;

public interface IRepository<TE> where TE : class
{
    Task<TE?> GetByIdAsync(string id);
    Task<IEnumerable<TE?>> GetAllAsync();
    Task<IEnumerable<TE?>> FindAsync(string whereClause, object? parameters = null);
    Task<TE?> FindOneAsync(string whereClause, object? parameters = null);
    Task<bool> CreateAsync(TE entity);
    Task<bool> UpdateAsync(TE entity);
    Task<bool> DeleteAsync(string id);
}

public class Repository<TE> : IRepository<TE> where TE : class
{
    private readonly IAppDataContext _context;
    private readonly string _tableName;
    private readonly string _keyColumn;
    private static readonly PropertyInfo[] _properties = typeof(TE).GetProperties();

    public Repository(IAppDataContext dataContext)
    {
        _context = dataContext;
        var tableAtt = typeof(TE).GetCustomAttribute<TableNameAttribute>();
        _tableName = tableAtt?.TableName ?? typeof(TE).Name + "s";
        var keyProp = _properties.FirstOrDefault(p => p.GetCustomAttribute<KeyColumnAttribute>() != null)
                      ?? _properties.FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));

        _keyColumn = keyProp?.Name!;
    }

    public async Task<TE?> GetByIdAsync(string id)
    {
        using var connection = _context.CreateConnection();
        var sql = $"SELECT * FROM {_tableName} WHERE {_keyColumn} = @Id";
        return await connection.QueryFirstOrDefaultAsync<TE>(sql, new { Id = id });
    }

    public async Task<IEnumerable<TE?>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = "SELECT * FROM " + _tableName;
        return await connection.QueryAsync<TE>(sql);
    }

    public async Task<IEnumerable<TE?>> FindAsync(string whereClause, object? parameters = null)
    {
        using var connection = _context.CreateConnection();
        var sql = $"SELECT * FROM {_tableName} WHERE {whereClause} ";
        return await connection.QueryAsync<TE>(sql, parameters);
    }
    
    public async Task<TE?> FindOneAsync(string whereClause, object? parameters = null)
    {
        using var connection = _context.CreateConnection();
        var sql = $"SELECT TOP 1 * FROM {_tableName} WHERE {whereClause}";
        return await connection.QueryFirstOrDefaultAsync<TE>(sql, parameters);
    }

    public async Task<bool> CreateAsync(TE entity)
    {
        var columns = string.Join(", ", _properties.Select(p => p.Name));
        var parameters = string.Join(", ", _properties.Select(p => "@" + p.Name));
        var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters});";

        using var connection = _context.CreateConnection();
        var result = await connection.ExecuteAsync(sql, entity);
        return result > 0;
    }

    public async Task<bool> UpdateAsync(TE entity)
    {
        var updateColumns = _properties.Where(p => !p.Name.Equals(_keyColumn, StringComparison.OrdinalIgnoreCase));
        var setClause = string.Join(", ", updateColumns.Select(p => $"{p.Name} = @{p.Name}"));
        
        var sql = $"UPDATE {_tableName} SET {setClause} WHERE {_keyColumn} = @{_keyColumn}";

        using var connection = _context.CreateConnection();
        var result = await connection.ExecuteAsync(sql, entity);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var sql = $"DELETE FROM {_tableName} WHERE {_keyColumn} = @Id";
        using var connection = _context.CreateConnection();
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}