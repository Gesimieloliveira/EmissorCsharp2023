using System;
using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;
using FusionCore.Repositorio.Filtros;
using FusionWPF.Dialogos.Controls;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoRelatorioAniversariantes : OpcaoRelatorioFixo<RAniversariantes>
    {
        public override string Descricao { get; } = "Relatório de aniversariantes por periodo";
        public override string Grupo { get; } = "Clientes";

        protected override RAniversariantes CriaRelatorio()
        {
            return new RAniversariantes(SessaoManager);
        }

        public override void Visualizar()
        {
            SolicitarPeriodo(p =>
            {
                using (var r = CriaRelatorio())
                {
                    r.DefinirPeriodo(p);
                    r.Visualizar();
                }
            });
        }

        private static void SolicitarPeriodo(Action<FiltroPeriodoNascimento> sucesso)
        {
            var dialog = new FiltroAniversariantesDialog();

            if (dialog.ShowDialog() == true)
            {
                sucesso(dialog.Contexto.PeriodoEscolhido());
            }
        }

        public override void EditarDesenho()
        {
            SolicitarPeriodo(p =>
            {
                using (var r = CriaRelatorio())
                {
                    r.DefinirPeriodo(p);
                    r.EditarDesenho();
                }
            });
        }

        protected override void OnDevEditarDesenho(string arquivoFrx)
        {
            SolicitarPeriodo(p =>
            {
                using (var r = CriaRelatorio())
                {
                    r.DefinirPeriodo(p);
                    r.DevEditarDesenho(arquivoFrx);
                }
            });
        }
    }
}