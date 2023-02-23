using System;
using System.ComponentModel.DataAnnotations;
using Fusion.Sessao;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.TipoDocumentoFinanceiro
{
    public class TipoDocumentoFinanceiroFormModel : ViewModel
    {
        private readonly bool _possuiGestor;
        private readonly TipoDocumento _tipoDocumento;
        private FFormaPagamento _formaPagamento;

        public TipoDocumentoFinanceiroFormModel(TipoDocumento tipoDocumento)
        {
            _tipoDocumento = tipoDocumento;
            _possuiGestor = SessaoSistema.Instancia.AcessoConcedido.PossuiFusionGestor;

            AtualizaModel();
        }

        [Required(ErrorMessage = @"Preciso de uma descrição para o documento")]
        public string Descricao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool EstaAtivo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool CheckAtivoIsVisible
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool BotaoDeleteIsVisible
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool RegistraFinanceiro
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public FFormaPagamento FormaPagamento
        {
            get => _formaPagamento;
            set
            {
                if (value == _formaPagamento) return;
                _formaPagamento = value;
                PropriedadeAlterada();
            }
        }

        private void AtualizaModel()
        {
            Descricao = _tipoDocumento.Descricao;
            EstaAtivo = _tipoDocumento.EstaAtivo;
            FormaPagamento = _tipoDocumento.FormaPagamento;
            CheckAtivoIsVisible = _tipoDocumento.Id > 0;
            BotaoDeleteIsVisible = _tipoDocumento.Id > 0;
            RegistraFinanceiro = _tipoDocumento.RegistraFinanceiro;
        }

        public void Deletar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioTipoDocumento(sessao);
                repositorio.Deletar(_tipoDocumento);

                transacao.Commit();
            }
        }

        public void Salvar()
        {
            ThrowExceptionSeExistirErros();

            if (RegistraFinanceiro && _possuiGestor == false)
            {
                throw new InvalidOperationException("Recurso de financeiro não está habilitado");
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _tipoDocumento.Descricao = Descricao;
                _tipoDocumento.EstaAtivo = EstaAtivo;
                _tipoDocumento.FormaPagamento = FormaPagamento;
                _tipoDocumento.AlteradoEm = DateTime.Now;
                _tipoDocumento.RegistraFinanceiro = RegistraFinanceiro;

                var repositorio = new RepositorioTipoDocumento(sessao);
                repositorio.Salvar(_tipoDocumento);

                transacao.Commit();
            }
        }
    }
}