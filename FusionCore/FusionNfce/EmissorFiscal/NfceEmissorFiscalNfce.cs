using System;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.EmissorFiscal
{
    public class NfceEmissorFiscalNfce : Entidade
    {
        public short Serie { get; set; }
        public int NumeroAtual { get; set; }
        public short SerieContingencia { get; set; }
        public int NumeroAtualContingencia { get; set; }
        public byte EmissorFiscalId { private get; set; }
        public TipoAmbiente Ambiente { get; set; } = TipoAmbiente.Producao;

        public byte[] ArquivoLogo { get; set; }
        public ModeloDocumento Modelo { get; set; } = ModeloDocumento.NFCe;
        public int IdToken { get; set; }
        public string Csc { get; set; }
        public bool IsIntegradorCeara { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public NfceEmissorFiscal EmissorFiscal { get; set; }
        protected override int ReferenciaUnica => EmissorFiscalId;
        public bool UsaNumeracaoDiferenteContigencia { get; set; }

        public NfceEmissorFiscalNfce()
        {
            UsaNumeracaoDiferenteContigencia = false;
        }

        public EmissorFiscalNFCE ToAdm()
        {
            return new EmissorFiscalNFCE
            {
                EmissorFiscal = EmissorFiscal.ToAdm(),
                Csc = Csc,
                Ambiente = Ambiente,
                IdToken = IdToken,
                AlteradoEm = AlteradoEm,
                Modelo = Modelo,
                EmissorFiscalId = EmissorFiscalId,
                Serie = Serie,
                ArquivoLogo = ArquivoLogo,
                NumeroAtual = NumeroAtual,
                NumeroAtualContingencia = NumeroAtualContingencia,
                SerieContingencia = SerieContingencia,
                UsaNumeracaoDiferenteContigencia = UsaNumeracaoDiferenteContigencia,
                IsIntegradorCeara = IsIntegradorCeara
            };
        }

        public void SetSequenciaNormal(short serie, int numeroAtual)
        {
            Serie = serie;
            NumeroAtual = numeroAtual;
        }

        public void SetSequenciaOffline(short serie, int numero)
        {
            SerieContingencia = serie;
            NumeroAtualContingencia = numero;
        }
    }
}