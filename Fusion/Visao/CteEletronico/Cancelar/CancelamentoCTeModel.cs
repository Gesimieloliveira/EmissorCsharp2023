using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FusionCore.FusionAdm.CteEletronico.Cancelar;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronicoOs.Cancelamento;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.CteEletronicoOs.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronico.Cancelar
{
    public class CancelamentoCTeModel : ViewModel
    {
        private string _chave;
        private string _numeroDocumento;
        public CancelarRn CancelarRn { get; }

        [Required(ErrorMessage = @"Porfavor digitar uma justificativa com no mínimo 15 caracteres.")]
        [MinLength(15, ErrorMessage = @"Porfavor digitar uma justificativa com no mínimo 15 caracteres.")]
        public string Justificativa
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Chave
        {
            get { return _chave; }
            set
            {
                if (value == _chave) return;
                _chave = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroDocumento
        {
            get { return _numeroDocumento; }
            set
            {
                if (value == _numeroDocumento) return;
                _numeroDocumento = value;
                PropriedadeAlterada();
            }
        }

        public CancelamentoCTeModel(CancelarRn cancelarRn)
        {
            CancelarRn = cancelarRn;
            Chave = cancelarRn.Cte.Chave;
            NumeroDocumento = cancelarRn.Cte.NumeroFiscal;
        }

        public void CancelarAsync()
        {
            Task.Run(() => Cancelar());
        }

        private void Cancelar()
        {
            CancelarRn.AdicionarJustificativa(Justificativa);
            CancelarRn.Cancela();
        }

        public void SalvarCancelamento()
        {
            var cteCancelamentoDados = CancelarRn.ObterCTeCancelamento();

            switch (CancelarRn.Cte.Documento)
            {
                case Documento.CTe:
                    CancelarCte(cteCancelamentoDados);
                    break;
                case Documento.CTeOs:
                    CancelarCteOs(cteCancelamentoDados);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CancelarCteOs(CancelamentoCteDados dados)
        {
            var cancelamento = new CteOsCancelado();
            cancelamento.XmlEnvio = dados.XmlEnvio;
            cancelamento.XmlRetorno = dados.XmlRetorno;
            cancelamento.CteOs = (CteOs)CancelarRn.Cte.GetCte();
            cancelamento.Ambiente = dados.TipoAmbiente;
            cancelamento.StatusResposta = dados.StatusResposta;
            cancelamento.Justificativa = dados.Justificativa;
            cancelamento.OcorreuEm = dados.OcorreuEm;
            cancelamento.Motivo = dados.Motivo;


            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCteOs(sessao);

                cancelamento.CteOs.Status = Status.Cancelada;
                repositorio.Salvar(cancelamento.CteOs);
                repositorio.Salvar(cancelamento);

                transacao.Commit();
            }
        }

        private void CancelarCte(CancelamentoCteDados dados)
        {
            var cancelamento = new CteCancelamento();
            cancelamento.XmlEnvio = dados.XmlEnvio;
            cancelamento.XmlRetorno = dados.XmlRetorno;
            cancelamento.Cte = (Cte)CancelarRn.Cte.GetCte();
            cancelamento.Ambiente = dados.TipoAmbiente;
            cancelamento.StatusResposta = dados.StatusResposta;
            cancelamento.Justificativa = dados.Justificativa;
            cancelamento.OcorreuEm = dados.OcorreuEm;
            cancelamento.Motivo = dados.Motivo;


            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCte(sessao);
                repositorio.SalvarCancelamento(cancelamento);

                transacao.Commit();
            }
        }
    }
}