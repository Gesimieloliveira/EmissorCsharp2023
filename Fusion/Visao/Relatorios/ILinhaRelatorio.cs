namespace Fusion.Visao.Relatorios.ListagemLinha
{
    public interface ILinhaRelatorio
    {
        string Descricao { get; }
        string Grupo { get; }
        void Visualizar();
        void EditarDesenho();
    }
}