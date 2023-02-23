using FusionCore.Core.Flags;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Fiscal.Flags;

// ReSharper disable InconsistentNaming

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class AbaPerfilNfeDTO
    {
        public short Id { get; set; }
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public string NaturezaOperacao { get; set; }
        public string Observacao { get; set; }
        public FinalidadeEmissao FinalidadeEmissao { get; set; }
        public byte EmissorFiscalId { get; set; }
        public int EmpresaId { get; set; }
        public int DestinatarioId { get; set; }
        public int TransportadoraId { get; set; }
        public string RazaoSocialEmpresa { get; set; }
        public string CnpjEmpresa { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public string NomeDestinatario { get; set; }
        public bool TemDestinatario => DestinatarioId > 0;
        public int VeiculoId { get; set; }
        public string CpfEmpresa { get; set; }

        public string DocumentoUnico => GetDocumentoUnico();

        private string GetDocumentoUnico()
        {
            return CnpjEmpresa.IsNotNullOrEmpty() ? CnpjEmpresa : CpfEmpresa;
        }
    }
}