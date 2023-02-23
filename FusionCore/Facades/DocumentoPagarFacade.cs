using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;

namespace FusionCore.Facades
{
    public class DocumentoPagarFacade
    {
        private readonly ISessaoManager _sessaoManager;

        public DocumentoPagarFacade(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public void EstornarDocumentos(IList<DocumentoPagar> documentos, UsuarioDTO usuario, string motivo)
        {
            if (documentos.Any(doc => doc.PossuiLancamentoNaoEstornado()))
            {
                throw new InvalidOperationException("Existem documentos com quitação. Necessário estorna-los antes!");
            }

            var repostorio = new RepositorioDocumentoPagar(_sessaoManager.GetSessaoAberta());

            foreach (var doc in documentos)
            {
                doc.Estornar();
                repostorio.Salvar(doc);
            }
        }
    }
}