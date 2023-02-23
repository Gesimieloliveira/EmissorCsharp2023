namespace Fusion.Conversor.Core.Map
{
    public sealed class PessoaCsv
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string CpfOuCnpj { get; set; }
        public string Rg { get; set; }
        public string OrgaoRg { get; set; }
        public string NascidoEm { get; set; }
        public string Sexo { get; set; }
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        public string NomeFantasia { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string InscricaoEstadual { get; set; }
        public string DocumentoExterior { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string NumeroEndereco { get; set; }
        public string ComplementoEndereco { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string IbgeCidade { get; set; }
        public string DescricaoTelefone { get; set; }
        public string Telefone { get; set; }
        public string DescricaoTelefone2 { get; set; }
        public string Telefone2 { get; set; }
        public string DescricaoTelefone3 { get; set; }
        public string Telefone3 { get; set; }
        public string Email { get; set; }
        public string EhFornecedor { get; set; }
        public string Observacao { get; set; }
        public string IndicadorIE { get; set; }

        public override string ToString()
        {
            return $"{Codigo} - {Nome}";
        }
    }
}