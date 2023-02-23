using DFe.Classes.Entidades;
using DFe.Classes.Extensoes;
using DFe.Ext;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.MdfeEletronico.Extencoes;
using MDFe.Classes.Informacoes;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FusionCore.FusionAdm.Automoveis
{
    public sealed class ProprietarioVeiculo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Cpf { get; set; }
        public bool IsTransportadora { get; set; }
        public string InscricaoEstadual { get; set; }
        public TipoProprietario Tipo { get; set; }
        public string Rntrc { get; set; }
        public string Taf { get; set; }
        public string NumeroRegistroEstadual { get; set; }
        public string DocumentoUnico => GetDocumentoUnico();

        public bool PossuiCpf() => Cpf?.Length == 11;
        public bool PossuiCnpj() => Cnpj?.Length == 14;

        private string GetDocumentoUnico()
        {
            if (PossuiCnpj())
            {
                return Cnpj;
            }

            return PossuiCpf() ? Cpf : string.Empty;
        }

        public MDFeProp ToMdfe(Veiculo veiculo)
        {
            var ie = string.IsNullOrWhiteSpace(InscricaoEstadual) ? "ISENTO" : InscricaoEstadual.TrimSefaz();
            var rntrc = string.IsNullOrWhiteSpace(Rntrc) ? null : Rntrc.TrimSefaz();

            return new MDFeProp
            {
                CNPJ = PossuiCnpj() ? Cnpj : null,
                CPF = PossuiCpf() ? Cpf : null,
                IE = ie,
                MDFeTpProp = Tipo.ToZeusMdfe(),
                UF = Estado.GO.SiglaParaEstado(veiculo.SiglaUf),
                RNTRC = rntrc,
                XNome = Nome?.RemoverAcentos()
            };
        }
    }
}