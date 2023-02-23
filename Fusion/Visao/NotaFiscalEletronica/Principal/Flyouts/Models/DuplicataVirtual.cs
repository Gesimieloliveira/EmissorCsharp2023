using System;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models
{
    public class DuplicataVirtual : ModelBase
    {
        private int _id;
        private int _numeroParcela;
        private DateTime? _venceEm;
        private int _dias;
        private decimal _valor;
        private TipoDocumento _tipoDocumento;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime? VenceEm
        {
            get { return _venceEm; }
            set
            {
                _venceEm = value; 
                PropriedadeAlterada();
            }
        }

        public int Dias
        {
            get { return _dias; }
            set
            {
                _dias = value;
                PropriedadeAlterada();
            }
        }

        public decimal Valor
        {
            get { return _valor; }
            set
            {
                _valor = value;
                PropriedadeAlterada();
            }
        }

        public TipoDocumento TipoDocumento
        {
            get { return _tipoDocumento; }
            set
            {
                _tipoDocumento = value;
                PropriedadeAlterada();
            }
        }

        public int NumeroParcela
        {
            get { return _numeroParcela; }
            set { _numeroParcela = value; }
        }
    }
}
