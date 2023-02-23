using System;
using FusionCore.Relatorios;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios
{
    public class RelatorioCustomizado : RelatorioBase
    {
        private readonly RelatorioProprio _relatorio;

        public RelatorioCustomizado(
            ISessaoManager sessaoManager,
            RelatorioProprio relatorio) : base(sessaoManager)
        {
            _relatorio = relatorio;
        }

        public string DescricaoRelatorio => _relatorio.Descricao;
        public string GrupoRelatorio => _relatorio.Grupo;

        protected override byte[] FornecerTemplate()
        {
            using (var sessao = SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioRelatorio(sessao);

                if (repositorio.TentaObterTemplate(_relatorio.Template.Id, out var template))
                {
                    return template.Dados;
                }
            }

            throw new InvalidOperationException("Não foi possível obter o template para este relatório");
        }

        protected override void PrepararDados()
        {
            RegistraParametro("DescricaoRelatorio", _relatorio.Descricao);
        }
    }
}