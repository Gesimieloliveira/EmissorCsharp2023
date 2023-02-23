using System;
using System.Collections.Generic;
using FusionCore.FusionPdv.Ecf;
using FusionLibrary.VisaoModel;
using FusionPdv.Servicos.Ecf;
using FusionPdv.Servicos.ValidacaoInicial;

namespace FusionPdv.Visao.AdicionarImposto
{
    public class ImpostoModel : ModelBase
    {
        private IList<Aliquota> _listaDeAliquota;


        public ImpostoModel()
        {
            AtualizaListaAliquota();
        }

        public IList<Aliquota> ListaDeAliquota
        {
            get { return _listaDeAliquota; }
            set
            {
                _listaDeAliquota = value; 
                PropriedadeAlterada();
            }
        }

        public void ValidarSeExisteImposto()
        {
            try
            {
                new ExisteAliquota().Executar();
            }
            catch (Exception ex)
            {
                throw new ExceptionExisteAliquota(ex.Message, ex);
            }
            
        }

        public void AtualizaListaAliquota()
        {
            try
            {
                ListaDeAliquota = new EcfPegarAliquotas().Aliquotas();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Erro ao tentar listar alíquotas da ecf.", ex);
            }
            
        }
    }
}
