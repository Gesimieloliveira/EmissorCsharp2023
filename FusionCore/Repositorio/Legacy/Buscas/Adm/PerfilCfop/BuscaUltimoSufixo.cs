using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.PerfilCfop
{
    public class BuscaUltimoSufixo : IBuscaUnico<UltimoSufixo>
    {
        private readonly CfopDTO _cfop;

        public BuscaUltimoSufixo(CfopDTO cfop)
        {
            _cfop = cfop;
        }

        public UltimoSufixo Busca(ISession sessao)
        {
            var listaDePerfilCfop = sessao.Query<PerfilCfopDTO>().Where(perfilCfop => perfilCfop.Cfop.Id.Equals(_cfop.Id)).ToList();

            byte maiorNumero = 0;

            listaDePerfilCfop.ForEach(p =>
            {
                if (maiorNumero < p.Sufixo)
                    maiorNumero = p.Sufixo;
            });


            return new UltimoSufixo(maiorNumero);
        }
    }
}
