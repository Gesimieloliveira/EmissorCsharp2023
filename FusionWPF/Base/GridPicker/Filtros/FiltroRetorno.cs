namespace FusionWPF.Base.GridPicker.Filtros
{
    public class FiltroRetorno
    {
        public FiltroRetorno(object filtroModel, bool isFecharJanela)
        {
            FiltroModel = filtroModel;
            IsFecharJanela = isFecharJanela;
        }

        public object FiltroModel { get; private set; }
        public bool IsFecharJanela { get; set; }

        public void FecharJanela(bool fechar = true)
        {
            IsFecharJanela = fechar;
        }
    }
}