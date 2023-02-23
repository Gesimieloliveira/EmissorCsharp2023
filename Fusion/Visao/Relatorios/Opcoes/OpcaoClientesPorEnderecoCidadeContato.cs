using Fusion.FastReport.Relatorios;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoClientesPorEnderecoCidadeContato : OpcaoRelatorioFixo<RelatorioFixo>
    {
        public override string Descricao { get; } = "Relatório de clientes por endereço, cidade e contato";
        public override string Grupo { get; } = "Clientes";

        protected override RelatorioFixo CriaRelatorio()
        {
            return new RelatorioFixo(
                SessaoManager,
                "FrClientesPorEndereçoCidadeContato.frx",
                Descricao
            );
        }
    }
}