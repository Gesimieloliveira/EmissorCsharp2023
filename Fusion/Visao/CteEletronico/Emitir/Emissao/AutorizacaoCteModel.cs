using System;
using FusionCore.DFe.XmlCte.XmlCte.RetornoRecepcao;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronico.Emitir.Emissao
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

    public sealed class AutorizacaoCteModel : ModelBase
    {
        private readonly Cte _cte;
        private bool _emProcessamento;
        private TipoNotificacao _tipoNotificacao;
        private string _textoNotificacao;
        private bool _habilitaCorrecaoDuplicidade;
        private bool _forcaUtilizacaoProximoNumero;
        private EmissorModel EmissorModel { get; }

        public CteEmissaoStatus GetStatusEmissao()
        {
            if (_cte.CteEmissao == null)
            {
                return CteEmissaoStatus.Vazio;
            }

            return _cte.CteEmissao.StatusConsultaRecibo;
        }

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

        public bool HabilitaCorrecaoDuplicidade
        {
            get { return _habilitaCorrecaoDuplicidade; }
            set
            {
                if (value == _habilitaCorrecaoDuplicidade) return;
                _habilitaCorrecaoDuplicidade = value;
                PropriedadeAlterada();
            }
        }

        public AutorizacaoCteModel(Cte cte)
        {
            _cte = cte;
            EmissorModel = new EmissorModel();
        }

        private void MostraNotificacao(string texto, TipoNotificacao tipo = TipoNotificacao.Informativo)
        {
            TextoNotificacao = texto;
            TipoNotificacao = tipo;
        }

        public AutorizacaoModelResposta EmiteNotaFiscal(bool forcaProximoNumero = false)
        {
            AutorizacaoModelResposta resposta = null;
            _forcaUtilizacaoProximoNumero = forcaProximoNumero;

            try
            {
                EmProcessamento = true;
                HabilitaCorrecaoDuplicidade = false;
                MostraNotificacao("Obtendo informações para emissão...");
                EmissorModel.CarregaEmissorPeloPefil(_cte.PerfilCte.Id);
                resposta = AutorizaNaSefaz();
            }
            catch (Exception e)
            {
                MostraNotificacao(e.Message, TipoNotificacao.Erro);
            }
            finally
            {
                EmProcessamento = false;
                _forcaUtilizacaoProximoNumero = false;
            }

            return resposta;
        }

        private AutorizacaoModelResposta AutorizaNaSefaz()
        {
            /**MostraNotificacao("Atualizando dados da CT-e");
            MostraNotificacao("Preparando para emissão...");

            var certificado = CertificadoDigitalFactory.Cria(EmissorModel.Emissor, true);

            MostraNotificacao("Emitindo Conhecimento de Transporte Eletrônica...");

            var emitirCte = new EmitirCte(EmissorModel.Emissor, certificado);
            _cte.CteEmissao = emitirCte.Emite(_cte, _forcaUtilizacaoProximoNumero);

            MostraNotificacao("Autorizando Conhecimento de Transporte Eletrônica na SEFAZ...");

            var autorizaNaSefaz = new AutorizaNaSefazCte(certificado);
            var resposta = autorizaNaSefaz.AutorizaNaSefaz(_cte.CteEmissao);

            return ProcessaResposta(resposta);**/
            return null;
        }

        private AutorizacaoModelResposta ProcessaResposta(FusionInformacaoProcessadaCTe resposta)
        {
            HabilitaCorrecaoDuplicidade = false;

            if (resposta.CodigoStatusResposta == 100)
            {
                MostraNotificacao(resposta.Motivo, TipoNotificacao.Sucesso);
                return new AutorizacaoModelResposta(true, resposta.Motivo);
            }

            HabilitaCorrecaoDuplicidade = PodeHabilitarCorrecaoDuplicidate(resposta);
            MostraNotificacao(resposta.Motivo, TipoNotificacao.Erro);
            return new AutorizacaoModelResposta(false, resposta.Motivo);
        }

        private bool PodeHabilitarCorrecaoDuplicidate(FusionInformacaoProcessadaCTe resposta)
        {
            return resposta.CodigoStatusResposta == 539
                   || resposta.CodigoStatusResposta == 206
                   || resposta.CodigoStatusResposta == 204;
        }
    }
}