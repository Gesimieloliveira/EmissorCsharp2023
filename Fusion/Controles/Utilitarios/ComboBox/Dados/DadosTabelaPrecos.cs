using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;

namespace Fusion.Controles.Utilitarios.ComboBox.Dados
{
    internal class DadosTabelaPrecos : IDadosCombobox
    {
        public string NomeMembroExibicao { get; } = null;

        public async Task<IEnumerable<object>> DadosAsync()
        {
            using (var sessao = SessaoSistema.Instancia.SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioTabelaPreco(sessao);
                var dados = await Task.Run(() => repositorio.BuscarTodasTabelasDto());

                return dados;
            }
        }
    }
}