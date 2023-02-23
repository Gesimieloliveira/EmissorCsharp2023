using System;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionLibrary.VisaoModel;

namespace FusionPdv.Visao.Produto
{
    public class ConsultarPrecoProdutoModel : ModelBase
    {
        private string _codigoBarra;

        private ProdutoDt _produtoDt;


        public string CodigoBarra
        {
            get { return _codigoBarra; }
            set
            {
                _codigoBarra = value;
                PropriedadeAlterada();
            }
        }

        public ProdutoDt Produto
        {
            get { return _produtoDt; }
            set
            {
                _produtoDt = value;
                PropriedadeAlterada();
            }
        }

        public void BuscarPorCodigoBarras()
        {

            if(string.IsNullOrEmpty(_codigoBarra)) throw new InvalidOperationException("Por favor digite um código de barras.");

            try
            {
                using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
                {
                    Produto = new ProdutoRepositorio(sessao).BuscarPorCodigoBarraOuCodigo(_codigoBarra);
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }

            
        }
    }
}
