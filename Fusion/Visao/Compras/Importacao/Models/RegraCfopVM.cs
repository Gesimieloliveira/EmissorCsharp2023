using Fusion.Sessao;
using Fusion.Visao.Cfop;
using FusionCore.Core.Flags;
using FusionCore.Core.Tributario;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Compras.Importacao.Models
{
    public class RegraCfopVM : ViewModel
    {
        private readonly CfopFacade _cfopFacade;

        public RegraCfopVM(short cfop)
        {
            _cfopFacade = new CfopFacade(SessaoSistema.Instancia.SessaoManager);

            Origem = cfop;
        }

        private OrigemOperacao OrigemCfopXml => CodigoCfopHelper.ObtemOrigem(Origem.ToString());

        public short Origem
        {
            get => GetValue<short>();
            set => SetValue(value);
        }

        public CfopDTO CodigoCfop
        {
            get => GetValue<CfopDTO>();
            set
            {
                SetValue(value);
                SetValue(value?.Id, nameof(Destino));
            }
        }

        public string Destino
        {
            get => GetValue<string>();
            set
            {
                SetValue(value);
                CarregarCfop();
            }
        }

        public object EscolherCfopCommand => GetSimpleCommand(obj =>
        {
            var view = new CfopPickerView();

            view.Contexto.MostrarApenasOsDeOperacao(TipoOperacao.Entrada);
            view.Contexto.MostrarApenasOsDeOrigem(OrigemCfopXml);

            view.Contexto.CfopFoiSelecionado += (o, cfop) =>
            {
                CodigoCfop = cfop;
                view.Close();
            };

            view.ShowDialog();
        });

        public void CarregarCfop()
        {
            if (Destino == CodigoCfop?.Id)
            {
                return;
            }

            if (Destino?.Length != 4)
            {
                SetValue<CfopDTO>(null, nameof(CodigoCfop));
                return;
            }

            var cfop = _cfopFacade.BuscarPeloCodigo(Destino);

            if (cfop == null || 
                cfop.TipoOperacao != TipoOperacao.Entrada || 
                cfop.OrigemOperacao != OrigemCfopXml
            ) {
                SetValue<CfopDTO>(null, nameof(CodigoCfop));
                return;
            }

            CodigoCfop = cfop;
        }
    }
}