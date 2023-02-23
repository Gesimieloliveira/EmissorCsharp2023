using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.FusionAdm.ConfiguracoesDfe
{
    public class RepositorioConfiguracaoDfe
    {
        private readonly ISession _sessao;

        public RepositorioConfiguracaoDfe(ISession sessao)
        {
            _sessao = sessao;
        }

        public void DeletaTodos()
        {
            _sessao.Delete($"from {nameof(ConfiguracaoDfe)} c");
            _sessao.Flush();
        }

        public void Inserir(IEnumerable<ConfiguracaoDfe> listaConfiguracaoDfe)
        {
            foreach (var configuracaoDfe in listaConfiguracaoDfe)
            {
                _sessao.Save(configuracaoDfe);
            }
        }

        private IList<ConfiguracaoDfe> BuscarPor(EstadoDTO uf, TipoAmbiente tipoAmbiente, TipoDocumentoFiscalEletronico tipoDocumentoFiscalEletronico)
        {
            ConfiguracaoDfe configuracaoDfeAlias = null;
            EstadoDTO estadoAlias = null;

            var queryOver = _sessao.QueryOver(() => configuracaoDfeAlias).JoinAlias(() => configuracaoDfeAlias.Uf, () => estadoAlias)
                .Where(() => configuracaoDfeAlias.AmbienteSefaz == tipoAmbiente && estadoAlias.Id == uf.Id);

            switch (tipoDocumentoFiscalEletronico)
            {
                case TipoDocumentoFiscalEletronico.NFe:
                    throw new ArgumentOutOfRangeException(nameof(tipoDocumentoFiscalEletronico),
                        tipoDocumentoFiscalEletronico,
                        null);
                    break;
                case TipoDocumentoFiscalEletronico.NFCe:
                    throw new ArgumentOutOfRangeException(nameof(tipoDocumentoFiscalEletronico),
                        tipoDocumentoFiscalEletronico,
                        null);
                case TipoDocumentoFiscalEletronico.CTe:
                    queryOver.Where(() => configuracaoDfeAlias.IsQrCodeCte == true);
                    break;
                case TipoDocumentoFiscalEletronico.CTeOs:
                    queryOver.Where(() => configuracaoDfeAlias.IsQrCodeCteOs == true);
                    break;
                case TipoDocumentoFiscalEletronico.MDFe:
                    queryOver.Where(() => configuracaoDfeAlias.IsQrCodeMdfe == true);
                    break;
                case TipoDocumentoFiscalEletronico.SAT:
                    throw new ArgumentOutOfRangeException(nameof(tipoDocumentoFiscalEletronico),
                        tipoDocumentoFiscalEletronico,
                        null);
                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoDocumentoFiscalEletronico),
                        tipoDocumentoFiscalEletronico,
                        null);
            }

            return queryOver.List<ConfiguracaoDfe>();
        }

        public bool AdicionarQrCode(EstadoDTO uf,
            TipoAmbiente tipoAmbiente,
            TipoDocumentoFiscalEletronico tipoDocumentoFiscalEletronico)
        {
            var lista = BuscarPor(uf, tipoAmbiente, tipoDocumentoFiscalEletronico);

            return lista != null && lista.Count != 0;
        }
    }
}