using FluentMigrator.Runner.VersionTableInfo;

namespace FusionCore.MigracaoFluente.Tabela
{
    [VersionTableMetaData]
    public class TabelaVersao : IVersionTableMetaData
    {
        public object ApplicationContext { get; set; }
        public bool OwnsSchema { get; }
        public string SchemaName => string.Empty;
        public string TableName => "versao_informacao";
        public string DescriptionColumnName => "descricao";
        public string UniqueIndexName => "UC_Version";
        public string AppliedOnColumnName => "aplicadoEm";
        public string ColumnName => "versao";
    }
}