using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionPdv.Ecf;
using FusionPdv.Servicos.ArquivoAuxiliar;
using FusionPdv.Servicos.ValidacaoInicial;
using NHibernate.Util;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfVerificaSerie
    {
        private readonly string _serie;

        public EcfVerificaSerie()
        {
            _serie = new BuscarSerie().Executar();
        }

        public void Existe()
        {
            var serieEcf = SessaoEcf.EcfFiscal.Serie();

            var existe = serieEcf == _serie;
            var resultadoComparacaoBancoDados = ComparacaoEcfBancoDados(serieEcf);

            if (!existe || !resultadoComparacaoBancoDados)
            {
                throw new ExceptionSerieEcf("Número de serie da ecf não correponde com o registro na base de dados.");
            }
        }

        private bool ComparacaoEcfBancoDados(string serieEcf)
        {
            bool resultado;

            using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
            {
                var ecf = (EcfDt) new EcfRepositorio(sessao).BuscarEcfEmUso().FirstOrNull();

                resultado = ecf.Serie == serieEcf;
            }

            return resultado;
        }
    }
}
