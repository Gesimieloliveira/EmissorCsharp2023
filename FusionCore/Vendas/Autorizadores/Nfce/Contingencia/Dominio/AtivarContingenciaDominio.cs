namespace FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Dominio
{
    public class AtivarContingenciaDominio : IAtivarContingenciaDominio
    {
        public virtual ContingenciaNfce Ativar()
        {
            var contingencia = new ContingenciaNfce();
            contingencia.Ativar();

            return contingencia;
        }
    }
}