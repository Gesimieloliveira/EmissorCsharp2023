using System;
using System.Collections.Generic;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos
{
    public interface IRepositorioExportacaoXml
    {
        IEnumerable<IEnvelope> BuscarXmlExportacao(DateTime inicio, DateTime fim, EmpresaDTO empresa);
    }
}