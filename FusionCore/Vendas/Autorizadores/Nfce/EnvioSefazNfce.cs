using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Servicos;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class EnvioSefazNfce : IEnvioSefaz
    {
        private readonly FaturamentoVenda _venda;
        private readonly UsuarioDTO _usuarioLogado;

        public EnvioSefazNfce(FaturamentoVenda venda, UsuarioDTO usuarioLogado)
        {
            _venda = venda;
            _usuarioLogado = usuarioLogado;
        }

        public IEnvioSefaz CriaCupomFiscal()
        {
            new CriarCupomFiscal(_venda, _usuarioLogado).Cria();
            return this;
        }

        public IEnvioSefaz AlocarNumeracaoFiscal()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                if (new RepositorioCupomFiscal(sessao).AlocarNumeroFiscal(_venda))
                    new AlocarNumeracaoCupomFiscal(_venda).Aloca(sessao);

                transacao.Commit();
            }

            return this;
        }

        public IEnvioSefaz CriaHistorico()
        {
            new CupomFiscalCriaHistorico().Criar(_venda);

            new AdicionarContingenciaNaVenda(_venda).Adicionar();

            if (ContingenciaAtiva.EstaAtiva())
            {
                ConverteNfceEmContingencia.Converter(_venda);
            }

            return this;
        }

        public IEnvioSefaz Autorizar()
        {
            new AutorizaNfce(_venda).AutorizarNaSefaz();

            return this;
        }
    }
}