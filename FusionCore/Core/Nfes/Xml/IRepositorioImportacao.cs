using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Federal;

namespace FusionCore.Core.Nfes.Xml
{
    public interface IRepositorioImportacao
    {
        IEnumerable<ProdutoUnidadeDTO> GetUnidades(string sigla);
        CidadeDTO GetCidadePeloIbge(int codigo);
        ProdutoGrupoDTO GetGrupo();
        TributacaoIcms GetIcms(string cst);
        TributacaoPis GetPis(string cst);
        TributacaoCofins GetCofins(string cst);
        TributacaoIpi GetIpi(string cst);
    }
}
