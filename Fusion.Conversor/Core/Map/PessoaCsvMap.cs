namespace Fusion.Conversor.Core.Map
{
    public sealed class PessoaCsvMap : MapBase<PessoaCsv>
    {
        public PessoaCsvMap()
        {
            Map(m => m.Codigo).Name("codigo");
            Map(m => m.Cnpj).Name("cnpj");
            Map(m => m.Cpf).Name("cpf");
            Map(m => m.CpfOuCnpj).Name("cpf_cnpj");
            Map(m => m.DocumentoExterior).Name("doc_exterior");
            Map(m => m.InscricaoEstadual).Name("inscricao_estadual");
            Map(m => m.NascidoEm).Name("data_nascimento");
            Map(m => m.InscricaoMunicipal).Name("inscricao_municipal");
            Map(m => m.Nome).Name("nome");
            Map(m => m.NomeFantasia).Name("nome_fantasia");
            Map(m => m.NomeMae).Name("nome_mae");
            Map(m => m.NomePai).Name("nome_pai");
            Map(m => m.OrgaoRg).Name("orgao_rg");
            Map(m => m.Rg).Name("rg");
            Map(m => m.Sexo).Name("sexo");
            Map(m => m.Cep).Name("cep");
            Map(m => m.Logradouro).Name("logradouro");
            Map(m => m.Bairro).Name("bairro");
            Map(m => m.NumeroEndereco).Name("numero");
            Map(m => m.ComplementoEndereco).Name("complemento");
            Map(m => m.Cidade).Name("cidade");
            Map(m => m.Uf).Name("uf");
            Map(m => m.IbgeCidade).Name("ibge_cidade");
            Map(m => m.DescricaoTelefone).Name("descricao_telefone");
            Map(m => m.Telefone).Name("telefone");
            Map(m => m.DescricaoTelefone2).Name("descricao_telefone2");
            Map(m => m.Telefone2).Name("telefone2");
            Map(m => m.DescricaoTelefone3).Name("descricao_telefone3");
            Map(m => m.Telefone3).Name("telefone3");
            Map(m => m.Observacao).Name("observacao");
            Map(m => m.Email).Name("email");
            Map(m => m.EhFornecedor).Name("eh_fornecedor");
            Map(m => m.IndicadorIE).Name("indicador_ie");

            ColunasObrigatorias = "PESSOA: nome, (cpf, cnpj ou cpf_cnpj); ENDEREÇO: cep, logradouro, bairro, numero, (cidade e uf ou ibge_cidade)";
        }
    }
}