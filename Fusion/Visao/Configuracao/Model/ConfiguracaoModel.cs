using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Configuracao.Model
{
    public class ConfiguracaoModel : ViewModel
    {
        public ConfiguracaoModel()
        {
            Email = new ConfiguracaoEmailModel();
            Estoque = new ConfiguracaoEstoqueModel();
            ConfiguracaoEstoqueFaturamento = new ConfiguracaoEstoqueFaturamentoModel();
            Balanca = new ConfiguracaoBalancaModel();
            Financeiro = new ConfiguracaoFinanceiroModel();
            FrenteCaixa = new ConfiguracaoFrenteCaixaModel();
            FusionServico = new ConfiguracaoServicoModel();
            ControleCaixa = new ConfiguracaoControleCaixaModel();
            ControleVendedor = new ConfiguracaoVendedorModel();
        }

        public ConfiguracaoEmailModel Email
        {
            get => GetValue<ConfiguracaoEmailModel>();
            set => SetValue(value);
        }

        public ConfiguracaoEstoqueModel Estoque
        {
            get => GetValue<ConfiguracaoEstoqueModel>();
            set => SetValue(value);
        }

        public ConfiguracaoEstoqueFaturamentoModel ConfiguracaoEstoqueFaturamento
        {
            get => GetValue<ConfiguracaoEstoqueFaturamentoModel>();
            set => SetValue(value);
        }

        public ConfiguracaoBalancaModel Balanca
        {
            get => GetValue<ConfiguracaoBalancaModel>();
            set => SetValue(value);
        }

        public ConfiguracaoFinanceiroModel Financeiro
        {
            get => GetValue<ConfiguracaoFinanceiroModel>();
            set => SetValue(value);
        }

        public ConfiguracaoFrenteCaixaModel FrenteCaixa
        {
            get => GetValue<ConfiguracaoFrenteCaixaModel>();
            set => SetValue(value);
        }

        public ConfiguracaoServicoModel FusionServico
        {
            get => GetValue<ConfiguracaoServicoModel>();
            set => SetValue(value);
        }

        public ConfiguracaoControleCaixaModel ControleCaixa
        {
            get => GetValue<ConfiguracaoControleCaixaModel>();
            set => SetValue(value);
        }

        public ConfiguracaoVendedorModel ControleVendedor
        {
            get => GetValue<ConfiguracaoVendedorModel>();
            set => SetValue(value);
        }

        public void Inicializa()
        {
            Email.Inicializa();
            Estoque.Inicializa();
            ConfiguracaoEstoqueFaturamento.Inicializa();
            Balanca.Inicializa();
            Financeiro.Inicializa();
            FrenteCaixa.Inicializa();
            FusionServico.Inicializa();
            ControleCaixa.Inicializa();
            ControleVendedor.Inicializa();
        }
    }
}