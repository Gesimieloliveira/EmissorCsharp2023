using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;

namespace FusionCore.FusionAdm.Fiscal.ChaveEletronica
{
    public class ComponentesChaveNfe : IChaveComponentes
    {
        private readonly Nfeletronica _nfe;
        private readonly long _numeroDocumento;
        private readonly TipoEmissao _tipoEmissao;

        public ComponentesChaveNfe(Nfeletronica nfe, TipoEmissao tipoEmissao)
        {
            _nfe = nfe;
            _numeroDocumento = _nfe.NumeroDocumento;
            _tipoEmissao = tipoEmissao;
        }

        public ModeloDocumento GetModelo()
        {
            return ModeloDocumento.NFe;
        }

        public string GetVersao()
        {
            return "3.10";
        }

        public short GetSerie()
        {
            return _nfe.SerieEmissao;
        }

        public string GetCodigoNumerico()
        {
            var random = new Random().Next(1, 99999999);

            if (random == _numeroDocumento)
            {
                return GetCodigoNumerico();
            }

            return random.ToString("D8");
        }

        public byte GetCodigoUf()
        {
            return _nfe.Emitente.Empresa.EstadoDTO.CodigoIbge;
        }

        public long GetNumeroDocumento()
        {
            return _numeroDocumento;
        }

        public TipoEmissao GetTipoEmissao()
        {
            return _tipoEmissao;
        }

        public string GetDocumentoUnico()
        {
            return _nfe.Emitente.DocumentoUnico.PadLeft(14, '0');
        }

        public DateTime GetDhEmissao()
        {
            return _nfe.EmitidaEm;
        }
    }
}