using System;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NFe.Servicos;
using NFe.Servicos.Retorno;

namespace Fusion.Visao.Produto
{
    public sealed class FlyoutCodigoBarraModel : ViewModel
    {
        private ProdutoAlias _produtoAlias;
        private ObservableCollection<EmissorFiscalComboBox> _listaEmissorFiscal;

        public FlyoutCodigoBarraModel()
        {
            ModoEdicao = false;
            IsCodigoBarras = true;
            IsGTIN = false;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var emissores = new RepositorioEmissorFiscal(sessao).BuscaTodosQueSejamNfeParaComboBox();
                ListaEmissorFiscal = new ObservableCollection<EmissorFiscalComboBox>(emissores);
            }

            if (ListaEmissorFiscal.Any())
            {
                EmissorSelecionado = ListaEmissorFiscal.First();
            }
        }

        public bool ModoEdicao
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsOpen
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string Alias
        {
            get => GetValue();
            set => SetValue(value);
        }

        public bool IsCodigoBarras
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsGTIN
        {
            get => GetValue<bool>();
            set
            {
                PermiteEditarFlagCodigoBarras = !value;
                SetValue(value);

                if (value is true)
                {
                    IsCodigoBarras = true;
                }
            }
        }

        public bool PermiteEditarFlagCodigoBarras
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

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

        public EmissorFiscalComboBox EmissorSelecionado
        {
            get => GetValue<EmissorFiscalComboBox>();
            set => SetValue(value);
        }

        public event EventHandler<ProdutoAlias> Confirmou;

        public void Edicao(ProdutoAlias produtoAlias)
        {
            IsCodigoBarras = produtoAlias.IsCodigoBarras;
            IsGTIN = produtoAlias.IsGtin;
            Alias = produtoAlias.Alias;

            ModoEdicao = true;

            _produtoAlias = produtoAlias;
        }

        public void SalvarAlteracao()
        {
            if (ValidaGTINComSefaz()) return;

            if (_produtoAlias == null)
            {
                var novo = new ProdutoAlias(Alias, IsCodigoBarras, IsGTIN);
                Confirmou?.Invoke(this, novo);

                return;
            }

            _produtoAlias.Update(IsCodigoBarras, IsGTIN);

            Confirmou?.Invoke(this, _produtoAlias);
        }

        private bool ValidaGTINComSefaz()
        {
            if (EmissorSelecionado == null && IsGTIN)
            {
                DialogBox.MostraAviso("Cadastre um Emissor NF-e para validar o GTIN");
                return true;
            }

            if (IsGTIN)
            {
                EmissorFiscal emissorFiscal;
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                    emissorFiscal = new RepositorioEmissorFiscal(sessao).GetPeloId(EmissorSelecionado.Id);

                var cfg = new ConfiguracaoZeusBuilder(emissorFiscal.EmissorFiscalNfe, TipoEmissao.Normal).GetConfiguracao();

                var servicos = new ServicosNFe(cfg, CertificadoDigitalFactory.Cria(emissorFiscal));

                var resposta = servicos.ConsultaGtin(Alias);
                if (GTINInvalido(resposta))
                {
                    DialogBox.MostraAviso(
                        $"{resposta.retConsGtin.xMotivo} \n Desmarcar esse é um GTIN valido para não valido ");
                    return true;
                }
            }

            return false;
        }

        private bool GTINInvalido(RetornoConsultaGtin resposta)
        {
            if (resposta?.retConsGtin == null) return true;

            if (resposta.retConsGtin.cStat == 9491 ||
                resposta.retConsGtin.cStat == 9492 ||
                resposta.retConsGtin.cStat == 9493 ||
                resposta.retConsGtin.cStat == 9494 ||
                resposta.retConsGtin.cStat == 9495) return true;

            return false;
        }
    }
}