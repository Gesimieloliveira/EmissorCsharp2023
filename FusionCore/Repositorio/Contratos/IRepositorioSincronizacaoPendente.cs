using System.Collections.Generic;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioSincronizacaoPendente : IRepositorio<SincronizacaoPendente, SincronizacaoPendente>
    {
        void Salvar(SincronizacaoPendente sincronizacaoPendente);

        void Deletar(SincronizacaoPendente sincronizacaoPendente);

        void AdicionaTodosUsuariosNaPrimeiraSync(byte idTerminal);

        IList<SincronizacaoPendente> BuscaTodosParaSincronizacao(EntidadeSincronizavel sincronizavel, byte terminalOfflineId);
    }
}