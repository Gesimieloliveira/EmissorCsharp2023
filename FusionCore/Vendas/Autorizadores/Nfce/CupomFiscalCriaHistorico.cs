using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Faturamentos;
using NFe.Utils.NFe;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class CupomFiscalCriaHistorico
    {
        private CupomFiscal _cupomFiscal;

        public void Criar(FaturamentoVenda venda)
        {
            BuscarCupomFiscal(venda);

            if (VerificaSeTemHistoricoEmAberto.Verifica(venda)) return;

            AlteraDataEmitirEm(venda);
            BuscarCupomFiscal(venda);

            var nfe = new ConstruirXmlNfceDeUmaVenda(venda, _cupomFiscal).Construir();

            var cupomFiscalHistorico = new CupomFiscalHistorico(
                _cupomFiscal, _cupomFiscal.NumeroFiscal
                , _cupomFiscal.Serie, _cupomFiscal.CodigoNumerico
                , nfe.ObterChaveNfeZeus(), _cupomFiscal.AmbienteSefaz
            );

            cupomFiscalHistorico.TentativaEm(nfe.infNFe.ide.dhEmi.DateTime);
            cupomFiscalHistorico.ComXmlEnvio(nfe.ObterXmlString().RemoverAcentos());

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                repositorioCupomFiscal.SalvarOuAlterarHistorico(cupomFiscalHistorico);

                transacao.Commit();
            }
        }
        
        private void BuscarCupomFiscal(FaturamentoVenda venda)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _cupomFiscal = new RepositorioCupomFiscal(sessao).ObterApenasCupomFiscal(venda);
            }
        }

        private void AlteraDataEmitirEm(FaturamentoVenda venda)
        {
            if (_cupomFiscal.TipoEmissao == TipoEmissao.Normal)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorio = new RepositorioCupomFiscal(sessao);

                    _cupomFiscal = repositorio.ObterCupomFiscal(venda);

                    _cupomFiscal.AlterarEmitirEm(DateTime.Now);
                    repositorio.SalvarOuAlterar(_cupomFiscal);

                    sessao.Flush();
                    sessao.Clear();
                    transacao.Commit();
                }
            }
        }
    }
}