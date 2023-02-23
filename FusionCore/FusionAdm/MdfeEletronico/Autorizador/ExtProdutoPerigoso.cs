using MDFe.Classes.Informacoes;

namespace FusionCore.FusionAdm.MdfeEletronico.Autorizador
{
    public static class ExtProdutoPerigoso
    {
        public static MDFePeri ToZeus(this MDFeProdutoPerigoso pp)
        {
            var peri = new MDFePeri
            {
                NONU = pp.NumeroOnu,
                GrEmb = pp.GrupoEmbalagem,
                QTotProd = pp.QuantidadeTotalPorProduto,
                QVolTipo = pp.QuantidadeTipoVolume,
                XClaRisco = pp.ClasseRisco,
                XNomeAE = pp.NomeEmbarque
            };

            return peri;
        }
    }
}