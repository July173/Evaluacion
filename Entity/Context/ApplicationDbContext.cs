using Dapper;
using Entity.Model.Base;
using Entity.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public ApplicationDbContext() { } // Necesario para las migraciones

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relación 1:N entre Author y Book
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId);

        // Configuración de entidades base
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(t => t.ClrType != null && t.ClrType.IsSubclassOf(typeof(BaseModel))))
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property("Name")
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity(entityType.ClrType)
                .Property("Description")
                .HasMaxLength(500);
        }

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }

    public override int SaveChanges()
    {
        EnsureAudit();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        EnsureAudit();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void EnsureAudit()
    {
        ChangeTracker.DetectChanges();
    }

    // Métodos Dapper
    public async Task<IEnumerable<T>> QueryAsync<T>(string text, object? parameters = null, int? timeout = null, CommandType? type = null)
    {
        using var command = new DapperEFCoreCommand(this, text, parameters ?? new { }, timeout, type, CancellationToken.None);
        var connection = this.Database.GetDbConnection();
        return await connection.QueryAsync<T>(command.Definition);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string text, object? parameters = null, int? timeout = null, CommandType? type = null)
    {
        using var command = new DapperEFCoreCommand(this, text, parameters ?? new { }, timeout, type, CancellationToken.None);
        var connection = this.Database.GetDbConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
    }


    /// <summary>
    /// Estructura para ejecutar comandos SQL con Dapper en Entity Framework Core.
    /// </summary>
    public readonly struct DapperEFCoreCommand : IDisposable
    {
        /// <summary>
        /// Constructor del comando Dapper.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        /// <param name="text">Consulta SQL.</param>
        /// <param name="parameters">Parámetros opcionales.</param>
        /// <param name="timeout">Tiempo de espera opcional.</param>
        /// <param name="type">Tipo de comando SQL opcional.</param>
        /// <param name="ct">Token de cancelación.</param>
        public DapperEFCoreCommand(DbContext context, string text, object parameters, int? timeout, CommandType? type, CancellationToken ct)
        {
            var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
            var commandType = type ?? CommandType.Text;
            var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;

            Definition = new CommandDefinition(
                text,
                parameters,
                transaction,
                commandTimeout,
                commandType,
                cancellationToken: ct
            );
        }

        /// <summary>
        /// Define los parámetros del comando SQL.
        /// </summary>
        public CommandDefinition Definition { get; }

        /// <summary>
        /// Método para liberar los recursos.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
