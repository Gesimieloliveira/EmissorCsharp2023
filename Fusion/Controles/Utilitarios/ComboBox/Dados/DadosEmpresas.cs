using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Controles.Utilitarios.ComboBox.Dados
{
    internal class DadosEmpresas : IDadosCombobox
    {
        public string NomeMembroExibicao { get; } = null;

        public async Task<IEnumerable<object>> DadosAsync()
        {
            using (var sessao = SessaoSistema.Instancia.SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioEmpresa(sessao);
                var empresas = await Task.Run(() => repositorio.BuscarEmpresaComboBoxDtos());

                return empresas;
            }
        }
    }
}