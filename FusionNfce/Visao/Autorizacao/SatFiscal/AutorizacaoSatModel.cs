using System;
using System.Net;
using System.Threading.Tasks;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.Servicos;
using FusionLibrary.VisaoModel;
using FusionNfce.AutorizacaoSatFiscal.AutorizacaoSatFiscal;
using FusionNfce.AutorizacaoSatFiscal.Ext;
using OpenAC.Net.Sat;

namespace FusionNfce.Visao.Autorizacao.SatFiscal
{
    public class AutorizacaoModelResposta
    {
        public bool Sucesso { get; private set; }
        public string Mensagem { get; private set; }

        public AutorizacaoModelResposta(bool sucesso, string mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
    }

    public sealed class AutorizacaoSatModel : ModelBase
    {
        private readonly Nfce _nfce;
        private bool _emProcessamento;
        private TipoNotificacao _tipoNotificacao;
        private string _textoNotificacao;

        public bool EmProcessamento
        {
            get { return _emProcessamento; }
            set
            {
                if (value == _emProcessamento) return;
                _emProcessamento = value;
                PropriedadeAlterada();
            }
        }

        public TipoNotificacao TipoNotificacao
        {
            get { return _tipoNotificacao; }
            set
            {
                if (value == _tipoNotificacao) return;
                _tipoNotificacao = value;
                PropriedadeAlterada();
            }
        }

        public string TextoNotificacao
        {
            get { return _textoNotificacao; }
            set
            {
                if (value == _textoNotificacao) return;
                _textoNotificacao = value;
                PropriedadeAlterada();
            }
        }

        public AutorizacaoSatModel(Nfce nfce)
        {
            _nfce = nfce;
        }

        private void MostraNotificacao(string texto, TipoNotificacao tipo = TipoNotificacao.Informativo)
        {
            TextoNotificacao = texto;
            TipoNotificacao = tipo;
        }

        public async Task<AutorizacaoModelResposta> EmiteNotaFiscalAsync()
        {
            return await Task.Run(() => EmiteNotaFiscal());
        }

        private AutorizacaoModelResposta EmiteNotaFiscal()
        {
            // ReSharper disable once RedundantAssignment
            AutorizacaoModelResposta resposta = null;

            try
            {
                EmProcessamento = true;
                MostraNotificacao("Obtendo informações para emissão...");
                resposta = AutorizaNaSefaz();
            }
            catch (WebException)
            {
                MostraNotificacao("Ouve um erro de conexão com a sefaz ou internet", TipoNotificacao.Erro);
                resposta = new AutorizacaoModelResposta(false, "Ouve um erro de conexão com a sefaz ou internet");
            }
            catch (Exception e)
            {
                MostraNotificacao(e.Message, TipoNotificacao.Erro);
                resposta = new AutorizacaoModelResposta(false, e.Message);
            }
            finally
            {
                EmProcessamento = false;
            }

            return resposta;
        }

        private AutorizacaoModelResposta AutorizaNaSefaz()
        {
            MostraNotificacao("Autorizando CF-e...");

            new CalculaImpostosNfce(_nfce).Calcular();

            var autorizarSat = new AutorizarSat(_nfce);

            var retorno = autorizarSat.Transmitir();

            return ProcessaResposta(retorno);
        }

        private AutorizacaoModelResposta ProcessaResposta(VendaSatResposta resposta)
        {
            if (resposta.CodigoDeRetorno != 6000)
            {
                MostraNotificacao(resposta.MensagemDoCodigoDeRetorno()?.Mensagem + "\n" + resposta.MensagemDoCodigoDeErro()?.Descricao, TipoNotificacao.Erro);
                return new AutorizacaoModelResposta(false, resposta.MensagemDoCodigoDeRetorno()?.Mensagem + "\n" + resposta.MensagemDoCodigoDeErro()?.Descricao);
            }


            MostraNotificacao(resposta.MensagemDoCodigoDeRetorno()?.Mensagem + "\n" + resposta.MensagemDoCodigoDeErro()?.Descricao, TipoNotificacao.Sucesso);
            return new AutorizacaoModelResposta(true, resposta.MensagemDoCodigoDeRetorno()?.Mensagem + "\n" + resposta.MensagemDoCodigoDeErro()?.Descricao);
        }
    }
}