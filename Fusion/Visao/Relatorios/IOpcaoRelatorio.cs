using System;

namespace Fusion.Visao.Relatorios
{
    public interface IOpcaoRelatorio
    {
        Guid TemplateId { get; }
        string Descricao { get; }
        string Grupo { get; }
        void Visualizar();
        void EditarDesenho();
        void ExportarTemplate();
        void ImportarTemplate();
        void ExcluirRelatorio();
        void EditarInforamacoesRelatorio();
        void DevEditarDesenho();
    }
}