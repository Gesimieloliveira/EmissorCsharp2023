using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ACBrFramework;
using FusionCore.FusionPdv.Ecf;
using FusionLibrary.VisaoModel;
using FusionPdv.Servicos.Ecf;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace FusionPdv.Visao.MapearFormasDePagamentos
{
    public class AdicionarMeioPagamentoModel : ModelBase
    {
        private readonly IList<FormaPagamento> _formaPagamentosEcf;
        private string _formaPagamentoFinalizador;
        private bool _permiteVinculado;

        public string FormaPagamentoFinalizador
        {
            get { return _formaPagamentoFinalizador; }
            set
            {
                if (value == _formaPagamentoFinalizador) return;
                _formaPagamentoFinalizador = value;
                PropriedadeAlterada();
            }
        }

        public bool PermiteVinculado
        {
            get { return _permiteVinculado; }
            set
            {
                if (value == _permiteVinculado) return;
                _permiteVinculado = value;
                PropriedadeAlterada();
            }
        }

        public AdicionarMeioPagamentoModel(IList<FormaPagamento> formaPagamentosEcf)
        {
            _formaPagamentosEcf = formaPagamentosEcf;
        }

        public void SalvarMeioDePagamento()
        {
            if (string.IsNullOrEmpty(FormaPagamentoFinalizador))
            {
                DialogBox.MostraInformacao("Selecione uma forma de pagamento.");
                return;
            }

            var formaPagamentoFinalizador = FormaPagamentoFinalizador.Trim();

            try
            {
                var formaPagamento =
                    _formaPagamentosEcf.Where(p => p.Descricao.Equals(formaPagamentoFinalizador)).FirstOrNull();

                if (formaPagamento != null)
                {
                    throw new InvalidOperationException("Já está cadastrado na ecf");
                }

                DialogBox.MostraAviso("Você esta prestes a programar uma forma de pagamento. DESCRIÇÃO: " +
                                      formaPagamentoFinalizador);

                var messageBoxResult =
                    DialogBox.MostraConfirmacao("A Forma de Pagamento: " + formaPagamentoFinalizador +
                                                " será programada. \nCuidado !! A programação de Formas de Pagamento é irreversivel\nConfirma a operação ?");

                if (messageBoxResult == MessageBoxResult.Yes)

                    new EcfAddFormaPagamento().Add(formaPagamentoFinalizador, PermiteVinculado);

                DialogBox.MostraAviso("Forma de pagamento: " + FormaPagamentoFinalizador +
                                      " adicionada com sucesso.");

                FormaPagamentoFinalizador = string.Empty;
                PermiteVinculado = false;
            }
            catch (ACBrException ex)
            {
                throw new ACBrException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }
    }
}