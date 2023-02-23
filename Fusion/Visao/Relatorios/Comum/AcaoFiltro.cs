using System;
using FusionCore.Repositorio.Filtros;
using FusionWPF.Dialogos.Controls;

namespace Fusion.Visao.Relatorios.Comum
{
    public static class AcaoFiltro
    {
        public static void SolicitarPeriodo(Action<FiltroPeriodo> sucesso)
        {
            var dialog = new FiltroPeriodoDialog();
            dialog.Contexto.Confirmou += (sender, periodo) => { sucesso(periodo); };

            dialog.ShowDialog();
        }
    }
}