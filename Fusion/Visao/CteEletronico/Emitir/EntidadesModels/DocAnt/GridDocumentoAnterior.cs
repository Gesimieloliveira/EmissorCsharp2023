using System.Collections.Generic;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.CteEletronico.Emitir.EntidadesModels.DocAnt
{
    public class GridDocumentoAnterior
    {
        public GridDocumentoAnterior()
        {
            DocumentosEmPapels = new List<GridDocumentoTransporte>();
        }

        public string DocumentoUnico { get; set; }
        public string InscricaoEstadual { get; set; }
        public EstadoDTO EstadoUf { get; set; }
        public string NomeOuRazaoSocial { get; set; }

        public List<GridDocumentoTransporte> DocumentosEmPapels { get; set; }

        public CteDocumentoAnterior CteDocumentoAnterior { get; set; }

        public static GridDocumentoAnterior Cria(CteDocumentoAnterior ant)
        {
            var docAnt = new GridDocumentoAnterior
            {
                CteDocumentoAnterior = ant,
                DocumentoUnico = ant.DocumentoUnico,
                InscricaoEstadual = ant.InscricaoEstadual,
                NomeOuRazaoSocial = ant.NomeOuRazaoSocial,
                EstadoUf = ant.EstadoUf
            };

            return docAnt;
        }
    }
}