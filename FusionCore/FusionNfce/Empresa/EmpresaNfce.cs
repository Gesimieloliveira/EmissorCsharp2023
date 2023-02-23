using System;
using FusionCore.CadastroEmpresa;
using FusionCore.FusionNfce.Cidade;
using FusionCore.FusionNfce.Uf;
using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Flags;

namespace FusionCore.FusionNfce.Empresa
{
    public class EmpresaNfce : Entidade, IEmpresa
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public DateTime? AtividadeIniciadaEm { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public UfNfce Estado { get; set; }
        public CidadeNfce Cidade { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Fone1 { get; set; }
        public string Fone2 { get; set; }
        public RegimeTributario RegimeTributario { get; set; }
        public DateTime? CadastradoEm { get; set; }
        public DateTime? AlteradoEm { get; set; }
        protected override int ReferenciaUnica => Id;
        public byte[] LogoMarca { get; set; }
    }
}