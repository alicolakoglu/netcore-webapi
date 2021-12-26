using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleProject.Data.Context
{
    internal class CustomSqlServerMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
    {
        internal const string DatabaseCollationName = "Turkish_CI_AS";

        public CustomSqlServerMigrationsSqlGenerator(
            MigrationsSqlGeneratorDependencies dependencies,
            IRelationalAnnotationProvider migrationsAnnotations)
        : base(dependencies, migrationsAnnotations)
        {
        }

        protected override void Generate(
            SqlServerCreateDatabaseOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);

            if (DatabaseCollationName != null)
            {
                builder
                    .Append("ALTER DATABASE ")
                    .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
                    .Append(" COLLATE ")
                    .Append(DatabaseCollationName)
                    .AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator)
                    .EndCommand(suppressTransaction: true);
            }
        }
    }
}
