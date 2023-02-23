using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Fusion.Sessao;
using Fusion.Visao.EmissorFiscalEletronico;
using Fusion.Visao.PerfilCfop;
using FusionCore.FusionAdm.CteEletronico;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Perfil
{
    public sealed class CtePerfilFormModel : ViewModel
    {
        private PerfilCte _perfil;
        private ObservableCollection<EmissorFiscalComboBox> _listaEmissorFiscal;
        private PerfilCfopDTO _perfilCfop;
        private TipoCte _tipoCte;
        private TipoServico _tipoServico;
        private string _observacao;
        private string _descricaoPerfilCfop;
        private ICommand _commandBuscaPerfilCfop;
        private bool _isUsarRemetente;
        private bool _isUsarDocumentoPadrao;
        private bool _isGerenciarEmissorFiscal;

        public CtePerfilFormModel(PerfilCte perfil)
        {
            _perfil = perfil;

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            IsGerenciarEmissorFiscal = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_EMISSOR_FISCAL_ELETRONICO);
        }

        public ICommand CommandBuscaPerfilCfop
            => _commandBuscaPerfilCfop ?? (_commandBuscaPerfilCfop = GetSimpleCommand(BuscaPerfilCfop));

        public ObservableCollection<EmissorFiscalComboBox> ListaEmissorFiscal
        {
            get => _listaEmissorFiscal;
            set
            {
                if (Equals(value, _listaEmissorFiscal)) return;
                _listaEmissorFiscal = value;
                PropriedadeAlterada();
            }
        }

        [Required(ErrorMessage = @"Descrição é obrigatória")]
        public string Descricao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool IsUsarRemetente
        {
            get => _isUsarRemetente;
            set
            {
                if (value == _isUsarRemetente) return;
                _isUsarRemetente = value;
                PropriedadeAlterada();
            }
        }

        public bool IsUsarDocumentoPadrao
        {
            get => _isUsarDocumentoPadrao;
            set
            {
                if (value == _isUsarDocumentoPadrao) return;
                _isUsarDocumentoPadrao = value;
                PropriedadeAlterada();
            }
        }

        [Required(ErrorMessage = @"Emissor Fiscal é obritório")]
        public EmissorFiscalComboBox EmissorSelecionado
        {
            get => GetValue<EmissorFiscalComboBox>();
            set => SetValue(value);
        }

        public PerfilCfopDTO PerfilCfop
        {
            get => _perfilCfop;
            set
            {
                if (Equals(value, _perfilCfop)) return;
                _perfilCfop = value;
                CodigoPerfilCfop = _perfilCfop?.Codigo;
                DescricaoPerfilCfop = PerfilCfop?.Descricao;
            }
        }

        [Required(ErrorMessage = @"Natureza da Operação é obrigatória")]
        public string NaturezaOperacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public TipoCte TipoCte
        {
            get => _tipoCte;
            set
            {
                if (value == _tipoCte) return;
                _tipoCte = value;
                PropriedadeAlterada();
            }
        }

        public TipoServico TipoServico
        {
            get => _tipoServico;
            set
            {
                if (value == _tipoServico) return;
                _tipoServico = value;
                PropriedadeAlterada();
            }
        }

        public string Observacao
        {
            get => _observacao;
            set
            {
                if (value == _observacao) return;
                _observacao = value;
                PropriedadeAlterada();
            }
        }

        [Required(ErrorMessage = @"CFOP é obrigatório")]
        public string CodigoPerfilCfop
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string DescricaoPerfilCfop
        {
            get => _descricaoPerfilCfop;
            set
            {
                if (value == _descricaoPerfilCfop) return;
                _descricaoPerfilCfop = value;
                PropriedadeAlterada();
            }
        }

        public string ProdutoPredominante
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public Ibpt Nbs
        {
            get => GetValue<Ibpt>();
            set
            {
                SetValue(value);
                SetValue(value != null, nameof(PossuiNbs));
            }
        }

        public bool PossuiNbs
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public bool CargaPredefinida
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public UnidadeMedida CargaUnidade
        {
            get => GetValue<UnidadeMedida>();
            set => SetValue(value);
        }

        public string CargaTipoMedida
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal CargaQuantidade
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool IsGerenciarEmissorFiscal
        {
            get => _isGerenciarEmissorFiscal;
            set
            {
                if (value == _isGerenciarEmissorFiscal) return;
                _isGerenciarEmissorFiscal = value;
                PropriedadeAlterada();
            }
        }

        private void BuscaPerfilCfop(object obj)
        {
            var model = new PerfilCfopPickerModel();
            model.GetPickerView().ShowDialog();

            if (model.ItemSelecionado == null)
                return;

            var perfilCfopSelecionado = model.ItemSelecionado.ItemReal as PerfilCfopDTO;

            PerfilCfop = perfilCfopSelecionado;
        }

        public void Deletar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPerfilCte(sessao);

                repositorio.Deletar(_perfil);

                transacao.Commit();
            }
        }

        public void Salvar()
        {
            try
            {
                ThrowExceptionSeExistirErros();
                AtualizarObjetoPerfil();

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorio = new RepositorioPerfilCte(sessao);

                    repositorio.Salvar(_perfil);
                    transacao.Commit();
                }

                DialogBox.MostraMensagemSalvouComSucesso();
                OnFechar();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void AtualizarObjetoPerfil()
        {
            if (_perfil == null)
            {
                _perfil = new PerfilCte();
            }

            _perfil.Descricao = Descricao;
            _perfil.Ativo = true;
            _perfil.NaturezaOperacao = NaturezaOperacao;
            _perfil.Observacao = Observacao ?? string.Empty;
            _perfil.TipoCte = TipoCte;
            _perfil.TipoServico = TipoServico;
            _perfil.PerfilCfop = PerfilCfop;
            _perfil.EmissorFiscal = BuscarEmissorFiscalCte();
            _perfil.RemetentePadrao = IsUsarRemetente;
            _perfil.DocumentoPadrao = IsUsarDocumentoPadrao;
            _perfil.ProdutoPredominante = ProdutoPredominante ?? string.Empty;
            _perfil.CodigoIbpt = Nbs?.Codigo;

            _perfil.Carga = new PerfilCteCarga(CargaPredefinida, CargaUnidade, CargaTipoMedida, CargaQuantidade);
        }

        private EmissorFiscal BuscarEmissorFiscalCte()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                return repositorio.GetPeloId(EmissorSelecionado.Id);
            }
        }

        public void Inicializa()
        {
            CarregaEmissorFiscalComboBox();
            PreencherModel();
        }

        private void CarregaEmissorFiscalComboBox()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                var emissores = repositorio.BuscaTodosQueSejamCteParaComboBox();
                ListaEmissorFiscal = new ObservableCollection<EmissorFiscalComboBox>(emissores);
            }
        }

        private void PreencherModel()
        {
            Descricao = _perfil.Descricao;
            EmissorSelecionado = ConverteParaEmissorComboBox(_perfil.EmissorFiscal);
            NaturezaOperacao = _perfil.NaturezaOperacao;
            Observacao = _perfil.Observacao ?? string.Empty;
            PerfilCfop = _perfil.PerfilCfop;
            TipoCte = _perfil.TipoCte;
            TipoServico = _perfil.TipoServico;
            IsUsarRemetente = _perfil.RemetentePadrao;
            IsUsarDocumentoPadrao = _perfil.DocumentoPadrao;
            ProdutoPredominante = _perfil.ProdutoPredominante;
            Nbs = _perfil.FetchIbpt();

            CargaPredefinida = _perfil.Carga?.Ativo ?? false;
            CargaUnidade = _perfil.Carga?.Unidade ?? UnidadeMedida.Unidade;
            CargaTipoMedida = _perfil.Carga?.TipoMedida ?? string.Empty;
            CargaQuantidade = _perfil.Carga?.Quantidade ?? 0;
        }

        private EmissorFiscalComboBox ConverteParaEmissorComboBox(EmissorFiscal emissorFiscal)
        {
            if (emissorFiscal == null) return null;

            return new EmissorFiscalComboBox
            {
                Descricao = emissorFiscal.Descricao,
                Id = emissorFiscal.Id
            };
        }

        public void AdicionarEmissorFiscal()
        {
            new EmissorFiscalForm(EmissorFiscalForm.CriaModel()).ShowDialog();

            CarregaEmissorFiscalComboBox();
        }
    }
}