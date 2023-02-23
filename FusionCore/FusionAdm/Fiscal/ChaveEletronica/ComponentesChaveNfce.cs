using System;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.TyneTypes;

namespace FusionCore.FusionAdm.Fiscal.ChaveEletronica
{
    public class ComponentesChaveNfce : IChaveComponentes
    {
        private readonly Chave _chave;

        public ComponentesChaveNfce(Chave chave)
        {
            _chave = chave;
        }

        public string GetDocumentoUnico()
        {
            return _chave.Cnpj.Valor;
        }

        public byte GetCodigoUf()
        {
            return _chave.CodigoIbgeUf;
        }

        public DateTime GetDhEmissao()
        {
            return _chave.AnoMes;
        }

        public ModeloDocumento GetModelo()
        {
            return _chave.ModeloDocumento;
        }

        public long GetNumeroDocumento()
        {
            return _chave.NumeroFiscal.Valor;
        }

        public short GetSerie()
        {
            return _chave.Serie.Valor;
        }

        public string GetCodigoNumerico()
        {
            return _chave.CodigoNumerico.Valor.ToString("D8");
        }

        public TipoEmissao GetTipoEmissao()
        {
            return _chave.FormaEmissao;
        }

        public string GetVersao()
        {
            return Versao.V400.GetString();
        }
    }
}