using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Dominio;

namespace FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Aplicacao
{
    public interface ITodasContingenciasNfce
    {
        void Salvar(ContingenciaNfce contingencia);

        bool ExisteContingenciaEmAberto();
        ContingenciaNfce BuscarContingenciaAberta();
    }
}