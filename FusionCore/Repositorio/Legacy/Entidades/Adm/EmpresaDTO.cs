using System;
using FusionCore.CadastroEmpresa;
using FusionCore.Helpers.Hidratacao;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Tributacoes.Flags;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class EmpresaDTO : Entidade, ISincronizavelAdm, IEmpresa
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Cnpj { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public DateTime? AtividadeIniciadaEm { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public EstadoDTO EstadoDTO { get; set; }
        public CidadeDTO CidadeDTO { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Fone1 { get; set; }
        public string Fone2 { get; set; }
        public RegimeTributario RegimeTributario { get; set; } = RegimeTributario.SimplesNacional;
        public DateTime? CadastradoEm { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string Rntrc { get; set; }
        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel { get; set; } = EntidadeSincronizavel.Empresa;
        protected override int ReferenciaUnica => Id;
        public string Taf { get; set; }
        public string NumeroRegistroEstadual { get; set; }

        public string Endereco => $"{Logradouro}, {Numero} - {Cep} / {Bairro} / {CidadeDTO}";
        public byte[] LogoMarca { get; set; }
        public byte[] LogoMarcaNfce { get; set; }

        public EmpresaComboBoxDTO ToComboBox()
        {
            return new EmpresaComboBoxDTO
            {
                Id = Id,
                Nome = RazaoSocial
            };
        }

        public string DocumentoUnico => GetDocumentoUnico();
        public string DocumentoUnicoFormatado => GetDocumentoUnico().PadLeft(14, '0');

        private string GetDocumentoUnico()
        {
            return Cnpj.IsNotNullOrEmpty() ? Cnpj.TrimOrEmpty() : Cpf.TrimOrEmpty();
        }
    }
}