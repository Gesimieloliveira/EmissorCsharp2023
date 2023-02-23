using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteEmitente
    {
        public int CteId { get; set; }
        public Cte Cte { get; set; }
        public EmpresaDTO Emitente { get; set; }

        public static CteEmitente Cria(Cte cte, EmpresaDTO emitente)
        {
            var cteEmitente = new CteEmitente
            {
                CteId = cte.Id,
                Cte = cte,
                Emitente = emitente
            };

            return cteEmitente;
        }
    }
}