#nullable disable

namespace EvolutionaryArchitecture.Fitnet.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using EvolutionaryArchitecture.Fitnet.Passes.Data.Database;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
[DbContext(typeof(PassesPersistence))]
[Migration("20260228000100_Add_Outbox_And_Saga_Tables")]
public partial class AddOutboxAndSagaTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "Passes");

        migrationBuilder.CreateTable(
            name: "OutboxMessages",
            schema: "Passes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Payload = table.Column<string>(type: "text", nullable: false),
                CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                ProcessedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OutboxMessages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SagaStates",
            schema: "Passes",
            columns: table => new
            {
                SagaId = table.Column<Guid>(type: "uuid", nullable: false),
                SagaType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SagaStates", x => x.SagaId);
            });

        migrationBuilder.CreateIndex(
            name: "IX_OutboxMessages_ProcessedAt",
            schema: "Passes",
            table: "OutboxMessages",
            column: "ProcessedAt");

        migrationBuilder.CreateIndex(
            name: "IX_OutboxMessages_Type_CorrelationId",
            schema: "Passes",
            table: "OutboxMessages",
            columns: new[] { "Type", "CorrelationId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_SagaStates_SagaType_CorrelationId",
            schema: "Passes",
            table: "SagaStates",
            columns: new[] { "SagaType", "CorrelationId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OutboxMessages",
            schema: "Passes");

        migrationBuilder.DropTable(
            name: "SagaStates",
            schema: "Passes");
    }
}
