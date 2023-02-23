using System;
using FusionLibrary.VisaoModel;
using FusionPdv.Ecf;

namespace FusionPdv.Visao.EspelhoMfd
{
    public class EspelhoMfdFormModel : ModelBase
    {
        private DateTime _dataInicial;
        private DateTime _dataFinal;

        public EspelhoMfdFormModel()
        {
            _dataInicial = DateTime.Now;
            _dataFinal = DateTime.Now;
        }

        public DateTime DataInicial
        {
            get { return _dataInicial; }
            set
            {
                if (value.Equals(_dataInicial)) return;
                _dataInicial = value;
                PropriedadeAlterada();
            }
        }

        public DateTime DataFinal
        {
            get { return _dataFinal; }
            set
            {
                if (value.Equals(_dataFinal)) return;
                _dataFinal = value;
                PropriedadeAlterada();
            }
        }


        public void TirarEspelho()
        {
            SessaoEcf.EcfFiscal.PafEspelhoMfd(DataInicial, DataFinal, @"C:\Users\Roberto\Desktop");
        }
    }
}