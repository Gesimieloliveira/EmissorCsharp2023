using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion.Sessao;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Controles.Utilitarios.ComboBox.Dados
{
    internal class DadosTiposDocumento : IDadosCombobox
    {
        public string NomeMembroExibicao { get; } = nameof(TipoDocumento.Descricao);

        public async Task<IEnumerable<object>> DadosAsync()
        {
            using (var sessao = SessaoSistema.Instancia.SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioTipoDocumento(sessao);
                return await Task.Run(() => repositorio.BuscaTodos());
            }
        }
    }
}