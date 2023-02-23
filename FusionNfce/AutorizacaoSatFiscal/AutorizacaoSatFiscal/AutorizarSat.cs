using System.Linq;
using System.Text;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionAdm.Tef;
using FusionCore.FusionNfce.Autorizacao;
using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.FusionNfce.Empresa;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.SatFiscal;
using FusionCore.FusionNfce.Pagamento;
using FusionCore.FusionNfce.SatFiscal;
using FusionCore.FusionNfce.Servicos;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;
using FusionNfce.AutorizacaoSatFiscal.Criadores;
using FusionNfce.AutorizacaoSatFiscal.Ext.Extencoes;
using NHibernate;
using OpenAC.Net.DFe.Core.Common;
using OpenAC.Net.Sat;
using OpenAC.Net.Sat.Events;

namespace FusionNfce.AutorizacaoSatFiscal.AutorizacaoSatFiscal
{
    public class AutorizarSat
    {
        private readonly Nfce _nfce;

        public AutorizarSat(Nfce nfce)
        {
            _nfce = nfce;
            _emissorSat = SessaoSistemaNfce.Configuracao.EmissorFiscal.EmissorFiscalSat;
            _empresaSat = SessaoSistemaNfce.Configuracao.EmissorFiscal.Empresa;
        }

        public VendaSatResposta Transmitir()
        {
            Inicializar();

            using (_acbrSat)
            {
                new AtivarSat(_acbrSat).Ativar();

                GerarCfe();

                var historico = BuscarHistorico() ?? SalvarHistorico();

                var resposta = _acbrSat.EnviarDadosVenda(_cfeAtual);

                if (resposta.CodigoDeRetorno != 6000)
                {
                    AtualizarHistorico(historico, resposta);
                    return resposta;
                }

                _cfeAtual = resposta.Venda;

                FinalizarVendaSat(historico, resposta);

                return resposta;
            }
        }

        private HistoricoEnvioSat BuscarHistorico()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                return new RepositorioNfce(sessao).BuscarHistoricoMaisAtualSat(_nfce);
            }
        }

        private void AtualizarHistorico(HistoricoEnvioSat historico, VendaSatResposta resposta)
        {
            historico.CodigoErro = resposta.CodigoDeErro;
            historico.CodigoRetorno = resposta.CodigoDeRetorno;
            historico.CodigoSefaz = resposta.CodigoSEFAZ;
            historico.MensagemRetorno = resposta.MensagemRetorno ?? string.Empty;
            historico.MensagemSefaz = resposta.MensagemSEFAZ ?? string.Empty;
            historico.NumeroSessao = resposta.NumeroSessao.ToString();
            historico.Finalizou = true;

            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioNfce(sessao);

                repositorio.SalvarHistoricoSat(historico);

                transacao.Commit();
            }
        }

        private void FinalizarVendaSat(HistoricoEnvioSat historico, VendaSatResposta resposta)
        {
            var finalizacao = CriaFinalizacao(historico, resposta);

            using (var financeiro = new ServicoControleFinanceiroNfce(SessaoSistemaNfce.Usuario))
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                //Fluxo de emissão de nfc-e com sat
                financeiro.GerarFinanceiroParaNfce(_nfce);
                GeraRegistroCaixa(sessao);

                var repositorio = new RepositorioNfce(sessao);

                FinalizaHistorico(historico, repositorio, resposta);
                FinalizaNfce(repositorio);
                repositorio.SalvarFinalizacaoSat(finalizacao);

                _nfce.FinalizaEmissaoSat = finalizacao;

                financeiro.ComitarAlteracoes();
                transacao.Commit();
            }
        }

        private void GeraRegistroCaixa(ISession sessao)
        {
            new GeraRegistroCaixa(_nfce, sessao, SessaoSistemaNfce.Usuario).RegistrarCaixa();
        }

        private void FinalizaNfce(IRepositorioNfce repositorio)
        {
            _nfce.TransmitidaComSucesso();
            repositorio.SalvarESincronizar(_nfce);
        }

        private static void FinalizaHistorico(HistoricoEnvioSat historico, RepositorioNfce repositorio, VendaSatResposta resposta)
        {
            historico.CodigoErro = resposta.CodigoDeErro;
            historico.CodigoRetorno = resposta.CodigoDeRetorno;
            historico.CodigoSefaz = resposta.CodigoSEFAZ;
            historico.MensagemRetorno = resposta.MensagemRetorno ?? string.Empty;
            historico.MensagemSefaz = resposta.MensagemSEFAZ ?? string.Empty;
            historico.NumeroSessao = resposta.NumeroSessao.ToString();
            historico.Finalizou = true;
            repositorio.SalvarHistoricoSat(historico);
        }

        private FinalizaEmissaoSat CriaFinalizacao(HistoricoEnvioSat historico, VendaSatResposta resposta)
        {
            var chave = resposta.ChaveConsulta;

            if (chave.Substring(0, 3).ToUpper() == "CFE")
            {
                chave = chave.Substring(3);
            }

            var numero = int.Parse(chave.Substring(31, 6));

            var finalizacao = new FinalizaEmissaoSat
            {
                AmbienteSefaz = historico.AmbienteSefaz,
                NumeroCaixa = historico.NumeroCaixa,
                Nfce = historico.Nfce,
                Empresa = historico.Empresa,
                Chave = chave,
                NumeroDocumento = numero,
                CodigoErro = resposta.CodigoDeErro,
                CodigoRetorno = resposta.CodigoDeRetorno,
                CodigoSefaz = resposta.CodigoSEFAZ,
                MensagemRetorno = resposta.MensagemRetorno,
                MensagemSefaz = resposta.MensagemSEFAZ,
                NumeroSessao = resposta.NumeroSessao.ToString(),
                QrCode = resposta.QRCode,
                XmlRetorno = _cfeAtual.GetXml(encoding:Encoding.Unicode),
                EmissorFiscal = SessaoSistemaNfce.Configuracao.EmissorFiscal
            };

            return finalizacao;
        }

        private void Inicializar()
        {
            _acbrSat = CriaAcbrSat.Criar();
            _acbrSat.OnEnviarDadosVenda += AntesEnviarAcbr;
        }

        private void AntesEnviarAcbr(object sender, EventoDadosEventArgs e)
        {
            e.Dados = TrataIE.TrataXmlIE(e.Dados);
        }

        private OpenSat _acbrSat;
        private CFe _cfeAtual;
        private readonly NfceEmissorFiscalSat _emissorSat;
        private readonly EmpresaNfce _empresaSat;

        private void GerarCfe()
        {
            _cfeAtual = _acbrSat.NewCFe();
            _cfeAtual.InfCFe.Ide.NumeroCaixa = _emissorSat.NumeroCaixa;

            CfeDestinatario();

            Cfeitem();


            if (_nfce.TotalProdutosServicos > _nfce.TotalNfce)
            {
                _cfeAtual.InfCFe.Total.DescAcrEntr.VDescSubtot = _nfce.TotalProdutosServicos - _nfce.TotalNfce;
            }

            if (_nfce.TotalProdutosServicos < _nfce.TotalNfce)
            {
                _cfeAtual.InfCFe.Total.DescAcrEntr.VAcresSubtot =_nfce.TotalNfce - _nfce.TotalProdutosServicos;
            }

            _cfeAtual.InfCFe.Total.VCFeLei12741 = _nfce.ValorTributoAproximado;

            GerarFormasDePagamento();

            _cfeAtual.InfCFe.InfAdic.InfCpl = _nfce.Observacao.TrimSefazOrNull(5000);

            ComputaDeOlhoNoImpoto();
        }

        private void ComputaDeOlhoNoImpoto()
        {
            var totalEstadual = 0.0m;
            var totalFederal = 0.0m;
            var totalTodosItens = 0.0m;

            using (var sessao = new SessaoManagerNfce().CriaSessao())
            {
                var repositorio = new RepositorioIbpt(sessao);

                foreach (var item in _cfeAtual.InfCFe.Det)
                {
                    var ibpt = repositorio.GetPeloNcm(item.Prod.NCM);

                    if (ibpt == null)
                    {
                        item.Imposto.VItem12741 = 0;
                        continue;
                    }

                    var bc = new BaseCalculo(item.Prod.QCom * item.Prod.VUnCom);

                    var federal = ibpt.ImpostoFederalAproximado(bc);
                    var estadual = ibpt.ImpostoEstadualAproximado(bc);
                    var total = federal + estadual;

                    item.Imposto.VItem12741 = total;

                    totalEstadual += estadual;
                    totalFederal += federal;
                    totalTodosItens += total;
                }

                _cfeAtual.InfCFe.Total.VCFeLei12741 = totalTodosItens;
            }

            _cfeAtual.InfCFe.InfAdic.InfCpl += "; ";

            var valorRealProdutos = _cfeAtual.InfCFe.Det.Sum(d => ((d.Prod.QCom*d.Prod.VUnCom) - d.Imposto.VItem12741));

            _cfeAtual.InfCFe.InfAdic.InfCpl += "Você pagou aproximadamente:" +
                                               $" {totalFederal:C} de tributos federais," +
                                               $" {totalEstadual:C} de tributos estaduais e" +
                                               $" {valorRealProdutos:C} pelos produtos." +
                                               " Fonte: IBPT";
        }

        private void Cfeitem()
        {
            var numeroItem = 1;

            foreach (var item in _nfce.ObterOsItens())
            {
                var det = _cfeAtual.InfCFe.Det.AddNew();
                det.NItem = numeroItem;
                det.Prod.CProd = item.Produto.Id.ToString();
                det.Prod.XProd = item.Nome.TrimSefaz(120);
                det.Prod.NCM = item.CodigoNcm.TrimSefaz(8);
                det.Prod.CFOP = item.Cfop.Id;
                det.Prod.UCom = item.SiglaUnidade.TrimSefaz(6);
                det.Prod.QCom = item.Quantidade;
                det.Prod.VUnCom = item.ValorUnitario;
                det.Prod.IndRegra = IndRegra.Arredondamento;
                det.Prod.CEST = item.CodigoCest.TrimSefaz(7);

                det.Imposto.VItem12741 = item.ValorTributoAproximado;
                det.Imposto.Imposto = item.ImpostoIcms.ObterImpostoSAT(item);
                det.Imposto.Pis.Pis = item.ImpostoPis.GetPisSat(_nfce.RegimeTributario);
                det.Imposto.Cofins.Cofins = item.ImpostoCofins.GetCofinsSat(_nfce.RegimeTributario);

                numeroItem++;
            }
        }

        private void CfeDestinatario()
        {
            if (_nfce.Destinatario != null)
            {
                if (_nfce.Destinatario.DocumentoUnico?.Length == 14)
                    _cfeAtual.InfCFe.Dest.CNPJ = _nfce.Destinatario.DocumentoUnico.TrimSefaz(14);

                if (_nfce.Destinatario.DocumentoUnico?.Length == 11)
                    _cfeAtual.InfCFe.Dest.CPF = _nfce.Destinatario.DocumentoUnico.TrimSefaz(11);

                _cfeAtual.InfCFe.Dest.Nome = _nfce.Destinatario.Nome.TrimSefaz(60);
            }
        }

        private void GerarFormasDePagamento()
        {
            foreach (var formaPagamentoNfce in _nfce.ObterOsPagamentos())
            {
                if (formaPagamentoNfce.IdFormaPagamento.Equals("10")) continue;

                var pag = _cfeAtual.InfCFe.Pagto.Pagamentos.AddNew();

                switch (formaPagamentoNfce.IdFormaPagamento)
                {
                    case "01":
                        pag.CMp = CodigoMP.Dinheiro;
                        pag.VMp = formaPagamentoNfce.ValorPagamento;
                        break;
                    case "02":
                        pag.CMp = CodigoMP.CartaodeCredito;
                        pag.VMp = formaPagamentoNfce.ValorPagamento;

                        if (formaPagamentoNfce.IsMfe)
                        {
                            if (formaPagamentoNfce.TipoCartaoPos == TipoCartaoPos.Debito)
                                pag.CMp = CodigoMP.CartaodeDebito;

                            pag.CAdmC = formaPagamentoNfce.Credenciadora == null ? null : (int?)formaPagamentoNfce.Credenciadora.Value;
                        }

                        break;
                    case "03":
                        pag.CMp = CodigoMP.Outros;
                        pag.VMp = formaPagamentoNfce.ValorPagamento;
                        break;

                    case "11":
                        pag.CMp = CodigoMP.Outros;
                        pag.VMp = formaPagamentoNfce.ValorPagamento;
                        break;
                    case "99":
                        pag.CMp = CodigoMP.Outros;
                        pag.VMp = formaPagamentoNfce.ValorPagamento;
                        break;
                }
            }
        }

        private HistoricoEnvioSat SalvarHistorico()
        {
            var historico = CriaObjetoHistorico();

            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioNfce(sessao);

                repositorio.SalvarHistoricoSat(historico);

                transacao.Commit();
            }

            return historico;
        }

        private HistoricoEnvioSat CriaObjetoHistorico()
        {
            var xml =
                _cfeAtual.GetXml(encoding: Encoding.Unicode);

            var historico = new HistoricoEnvioSat
            {
                NumeroCaixa = short.Parse(_acbrSat.Configuracoes.IdeNumeroCaixa.ToString()),
                Nfce = _nfce,
                AmbienteSefaz = _acbrSat.Configuracoes.IdeTpAmb == DFeTipoAmbiente.Homologacao ? TipoAmbiente.Homologacao : TipoAmbiente.Producao,
                Empresa = _empresaSat,
                Finalizou = false,
                XmlEnvio = xml,
                MensagemRetorno = string.Empty,
                MensagemSefaz = string.Empty,
                NumeroSessao = string.Empty
            };

            return historico;
        }
    }
}