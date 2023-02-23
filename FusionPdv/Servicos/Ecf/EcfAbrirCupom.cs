using System;
using System.Windows;
using ACBrFramework;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using FusionPdv.Ecf;
using FusionPdv.Servicos.ValidacaoInicial.AbrirVenda;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfAbrirCupom
    {
        private static ValidacaoAbrirVenda _validacaoAbrirVenda;
        private readonly VendaEcfDt _vendaEcf;

        public EcfAbrirCupom(VendaEcfDt vendaEcf)
        {
            _vendaEcf = vendaEcf;
        }

        private static void ValidaDisponibilidade(EstadoEcfFiscal estado)
        {
            if (_validacaoAbrirVenda != null)
            {
                _validacaoAbrirVenda.Executar(estado);
                return;
            }

            _validacaoAbrirVenda = new ValidacaoAbrirVenda();
            _validacaoAbrirVenda.Executar(estado);
        }

        public void IniciarVenda()
        {
            if (ValidaEstado()) return;

            SalvaVenda();
        }

        private bool ValidaEstado()
        {
            var estadoEcf = SessaoEcf.EcfFiscal.Estado();

            if (estadoEcf != EstadoEcfFiscal.Livre) return true;

            ValidaDisponibilidade(estadoEcf);

            return false;
        }

        private void SalvaVenda()
        {
            if (_vendaEcf.Id != 0) return;

            if (SessaoSistema.AcessoConcedido == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DialogBox.MostraAviso("Licenciamento: " + SessaoSistema.MensagemErroAcesso);
                    Application.Current.Shutdown();
                });
            }

            _vendaEcf.Ccf = int.Parse(SessaoEcf.EcfFiscal.Ccf());
            _vendaEcf.Coo = int.Parse(SessaoEcf.EcfFiscal.Coo()) + 1;
            _vendaEcf.SerieEcf = SessaoEcf.EcfFiscal.Serie();
            _vendaEcf.Status = VendaStatus.Aberta;
            _vendaEcf.IndicadorPagamento = IndicadorPagamento.Avista;

            var sessao = GerenciaSessao.ObterSessao(SessaoPdv.FabricaDois).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            try
            {
                _vendaEcf.EcfDt = (EcfDt) new EcfRepositorio(sessao).BuscarEcfEmUso().FirstOrNull();
                _vendaEcf.VendidoEm = DateTime.Now;
                _vendaEcf.AlteradoEm = DateTime.Now;
                sessao.Clear();
                new VendaEcfRepositorio(sessao).Salvar(_vendaEcf);

                SessaoEcf.EcfFiscal.AbreCupom(_vendaEcf.DocumentoCliente,
                    _vendaEcf.NomeCliente,
                    _vendaEcf.EnderecoCliente);

                transacao.Commit();
            }
            catch (ACBrException)
            {
                transacao?.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                transacao?.Rollback();
                throw new Exception("Falha ao salvar no banco de dados", ex);
            }
            finally
            {
                sessao.Close();
            }
        }
    }
}