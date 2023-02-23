using System;
using FusionLibrary.VisaoModel;

namespace FusionWPF.FusionAdm.CteEletronico
{
    public sealed class NfePickerFiltro : ModelBase
    {
        private DateTime? _dataEmissaoFinal;
        private DateTime? _dataEmissaoInicial;
        private string _textoSearch;

        public string TextoSearch
        {
            get { return _textoSearch; }
            set
            {
                if (value == _textoSearch) return;
                _textoSearch = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? DataEmissaoFinal
        {
            get { return _dataEmissaoFinal; }
            set
            {
                _dataEmissaoFinal = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? DataEmissaoInicial
        {
            get { return _dataEmissaoInicial; }
            set
            {
                _dataEmissaoInicial = value;
                PropriedadeAlterada();
            }
        }
    }
}