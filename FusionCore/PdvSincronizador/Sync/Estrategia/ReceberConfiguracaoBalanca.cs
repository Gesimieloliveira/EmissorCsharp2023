using System;
using FusionCore.FusionPdv.Configuracoes;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionPdv;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberConfiguracaoBalanca : SincronizacaoBase
    {
        public override string Tag => @"receber-balanca";

        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var balanca = new RepositorioBalanca(SessaoAdm).BuscarUnicaBalanca();

            if (balanca.AlteradoEm <= ultimaSincronizacao) return;

            var balancaPdv = new BalancaPdv
            {
                Ativo = balanca.Ativo,
                DigitoVerificador = balanca.DigitoVerificador,
                ModoDeOperacao = balanca.ModoDeOperacao,
                TamanhoCodigo = balanca.TamanhoCodigo,
                CasasDecimais = balanca.CasasDecimais
            };

            using (var transacao = SessaoPdv.BeginTransaction())
            {
                var repositorioPdv = new RepositorioBalancaPdv(SessaoPdv);
                repositorioPdv.Salvar(balancaPdv);

                transacao.Commit();
            }

            RegistraEvento = true;
        }
    }
}