using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteDocumentoAnterior
    {
        public CteDocumentoAnterior()
        {
            Documentos = new List<CteDocumentoTransporte>();
        }

        public int Id { get; set; }

        public Cte Cte { get; set; }

        public string DocumentoUnico { get; set; }
        public string InscricaoEstadual { get; set; }
        public EstadoDTO EstadoUf { get; set; }
        public string NomeOuRazaoSocial { get; set; }

        public IList<CteDocumentoTransporte> Documentos { get; set; }
    }
}