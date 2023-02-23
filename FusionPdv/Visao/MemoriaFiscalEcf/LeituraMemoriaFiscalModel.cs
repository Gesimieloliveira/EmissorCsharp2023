using System;
using FusionLibrary.VisaoModel;
using FusionPdv.Ecf;

namespace FusionPdv.Visao.MemoriaFiscalEcf
{
    public class LeituraMemoriaFiscalModel : ModelBase
    {
        private DateTime _dataFinal = DateTime.Now;
        private DateTime _dataInicial = DateTime.Now;
        private string _tipoLeitura = "S";

        public DateTime DataInicial
        {
            get { return _dataInicial; }
            set
            {
                _dataInicial = value;
                PropriedadeAlterada();
            }
        }

        public DateTime DataFinal
        {
            get { return _dataFinal; }
            set
            {
                _dataFinal = value;
                PropriedadeAlterada();
            }
        }

        public string TipoLeitura
        {
            get { return _tipoLeitura; }
            set
            {
                _tipoLeitura = value;
                PropriedadeAlterada();
            }
        }

        public void TirarLeituraMemoriaFiscal()
        {
            SessaoEcf.EcfFiscal.LeituraMemoriaFiscal(_dataInicial, _dataFinal, ESimplificada());
        }

        public bool ESimplificada()
        {
            return "S".Equals(_tipoLeitura);
        }
    }
}