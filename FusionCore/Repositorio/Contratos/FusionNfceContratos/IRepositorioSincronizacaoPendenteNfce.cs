using System.Collections.Generic;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioSincronizacaoPendenteNfce : IRepositorio<SincronizacaoPendenteNfce, SincronizacaoPendenteNfce>
    {
        void Salvar(SincronizacaoPendenteNfce sincronizacaoPendente);

        void Deletar(SincronizacaoPendenteNfce sincronizacaoPendente);

        IList<SincronizacaoPendenteNfce> BuscaTodosParaSincronizacao(EntidadeSincronizavel sincronizavel);
    }
}