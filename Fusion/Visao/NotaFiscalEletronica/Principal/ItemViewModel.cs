using Fusion.Visao.NotaFiscalEletronica.Principal.Controles;
using FusionCore.FusionAdm.Compras;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public class ItemViewModel : ViewModel, IOutrasOpcoesContexto
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private FusionCore.FusionAdm.Fiscal.NF.Perfil.PerfilNfe _perfil;
        private Nfeletronica _nfe;
        private ItemNfe _item;
        private bool _movimentarEstoqueConfiguracao;
        private bool _naoEditar;
        private bool _usarIpiTagPropria;

        public ItemViewModel()
        {
            _naoEditar = true;
        }

        public ItemContexto ItemContexto
        {
            get => GetValue<ItemContexto>();
            set => SetValue(value);
        }

        public IpiContexto IpiContexto
        {
            get => GetValue<IpiContexto>();
            set => SetValue(value);
        }

        public IcmsContexto IcmsContexto
        {
            get => GetValue<IcmsContexto>();
            set => SetValue(value);
        }

        public PisCofinsContexto PisCofinsContexto
        {
            get => GetValue<PisCofinsContexto>();
            set => SetValue(value);
        }

        public bool AutoAjustarImposto
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);

                IpiContexto.AutoAjusteImposto = AutoAjustarImposto;
                IcmsContexto.AutoAjusteImposto = AutoAjustarImposto;
                PisCofinsContexto.AutoAjusteImposto = AutoAjustarImposto;
            }
        }

        public bool AutoCalcularTotaisItem
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);

                ItemContexto.AutoCalcularTotaisItem = value;
            }
        }

        public bool GeraIcmsInterstadual
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool MovimentarEstoqueConfiguracao
        {
            get => _movimentarEstoqueConfiguracao;
            set
            {
                if (value == _movimentarEstoqueConfiguracao) return;
                _movimentarEstoqueConfiguracao = value;
                PropriedadeAlterada();
            }
        }

        public bool EnviarInformacoesCreditoNaObsItem
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool UsarIpiTagPropria
        {
            get => _usarIpiTagPropria;
            set
            {
                _usarIpiTagPropria = value;
                PropriedadeAlterada();
            }
        }

        public bool NaoEditar
        {
            get => _naoEditar;
            set
            {
                if (value == _naoEditar) return;
                _naoEditar = value;
                PropriedadeAlterada();
            }
        }

        public void PrepararCom(Nfeletronica nfe)
        {
            _nfe = nfe;

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioPerfilNfe(sessao);
                var perfil = repositorio.GetPeloId(_nfe.PerfilId);

                _perfil = perfil;
            }

            if (_perfil.AutoAtivarPartilhaIcms)
            {
                GeraIcmsInterstadual = _nfe.SujeitoIcmsInterstadual;
            }

            MovimentarEstoqueConfiguracao = _perfil.MovimentarEstoqueProduto;
        }

        public void ItemEdicao(ItemNfe item)
        {
            _item = item;

            if (_item.Id != 0)
            {
                NaoEditar = false;
            }
        }

        public void Inicializar()
        {
            AutoAjustarImposto = true;
            AutoCalcularTotaisItem = true;

            if (_perfil.DesativarInfoCreditoItem == true)
            {
                EnviarInformacoesCreditoNaObsItem = false;
            }
            else
            {
                EnviarInformacoesCreditoNaObsItem = true;
            }

            UsarIpiTagPropria = _perfil.UsarIpiTagPropria;

            ItemContexto.PriorizarCfopDoPerfil(_perfil.Cfop);
            ItemContexto.TipoOperacao = _nfe.TipoOperacao;
            ItemContexto.DestinoOperacao = _nfe.Destinatario.IndicadorDestinoOperacao ?? DestinoOperacao.Interna;
            ItemContexto.FinalidadeEmissao = _nfe.FinalidadeEmissao;
            ItemContexto.AtualizarTabelaPreco(_nfe.TabelaPreco);

            IcmsContexto.ConfigurarOpcoesCst(_nfe.Emitente.RegimeTributario);
            IcmsContexto.PriorizarCsosn(_perfil.SimplesNacional);

            if (_item != null)
            {
                CarregarEdicao();
            }
        }

        private void CarregarEdicao()
        {
            SetValue(_item.AutoCalcularTotaisItem, nameof(AutoCalcularTotaisItem));
            SetValue(_item.AutoAjustarImposto, nameof(AutoAjustarImposto));
            SetValue(_item.PartilharIcms, nameof(GeraIcmsInterstadual));
            SetValue(_item.MovimentarEstoqueConfiguracao, nameof(MovimentarEstoqueConfiguracao));
            SetValue(_item.AutoAtivarCreditoItem, nameof(EnviarInformacoesCreditoNaObsItem));
            UsarIpiTagPropria = _item.UsarIpiTagPropria;

            ItemContexto.Com(_item);
            IpiContexto.Com(_item);
            IcmsContexto.Com(_item);
            PisCofinsContexto.Com(_item);
        }

        public void SalvarAlteracoes()
        {
            ItemContexto.ThrowExceptionSeInvalido();
            IpiContexto.ThrowExceptionSeInvalido();
            IcmsContexto.ThrowExceptionSeInvalido();
            PisCofinsContexto.ThrowExceptionSeInvalido();

            ChecadorEstoqueNegativoNfe.ThrowExceptionSeQuantidadeNegativarEstoque(
                new ChecadorEstoqueNegativoNfe.NovoMovimento(
                    ItemContexto.ProdutoSelecionado.Produto.ProdutoId,
                    ItemContexto.Quantidade,
                    _nfe.TipoOperacao,
                    _item?.Id)
            );

            if (_item == null)
            {
                _item = new ItemNfe(_nfe);
            }

            _item.AutoAjustarImposto = AutoAjustarImposto;
            _item.PartilharIcms = GeraIcmsInterstadual;
            _item.MovimentarEstoqueConfiguracao = MovimentarEstoqueConfiguracao;
            _item.AutoAtivarCreditoItem = EnviarInformacoesCreditoNaObsItem;
            _item.AutoCalcularTotaisItem = AutoCalcularTotaisItem;
            _item.UsarIpiTagPropria = UsarIpiTagPropria;

            ItemContexto.AplicarAlteracoesEm(_item);
            IpiContexto.AplicarAlteracoesEm(_item);
            IcmsContexto.AplicarAlteracoesEm(_item);
            PisCofinsContexto.AplicarAlteracoesEm(_item);

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var tabelaPreco = _nfe.TabelaPreco;

                if (tabelaPreco != null)
                {
                    AtualizaPrecosComTabelaPreco.AjusteTabelaPreco(tabelaPreco,
                        new RepositorioTabelaPreco(sessao),
                        _item.Produto,
                        new AtualizaPrecosCalculadosPorTabelaPreco(_item, _item));
                }

                _nfe.AdicionarItem(_item);
                _nfe.CalcularItens();

                var repo = new RepositorioNfe(sessao);
                repo.SalvarAlteracoes(_nfe);

                transacao.Commit();
            }
        }
    }
}