using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Ecf;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate.Util;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberEcf : SincronizacaoBase
    {
        public override string Tag { get; } = "receber-ecf";

        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var emissores = ObterEmissores();
            if (emissores.Count == 0)
                return;

            var repositorio = new EcfRepositorio(SessaoPdv);
            var emissoresDoPdv = repositorio.BuscaTodos();

            SessaoPdv.Clear();

            emissores.ForEach(emissor =>
            {
                var emissorDoPdv = (EcfDt) emissoresDoPdv.Where(e => e.Id == emissor.Id).FirstOrNull();
                var ecf = CriarEcf(emissor, emissorDoPdv);
                repositorio.Salvar(ecf);
            });

            RegistraEvento = false;
        }

        private static EcfDt CriarEcf(PdvEcfDTO emissor, EcfDt emissorDoPdv)
        {
            return new EcfDt
            {
                AlteradoEm = emissor.AlteradoEm,
                Ativo = emissor.Ativo ? IntBinario.Sim : IntBinario.Nao,
                Modelo = emissor.Modelo,
                NumeroEcf = emissor.NumeroEcf,
                Id = emissor.Id,
                Serie = emissor.Serie,
                ModeloAcbr = emissorDoPdv?.ModeloAcbr ?? string.Empty,
                Porta = emissorDoPdv?.Porta ?? string.Empty,
                Velocidade = emissorDoPdv?.Velocidade ?? string.Empty,
                EmUso = emissorDoPdv?.EmUso ?? 0,
                ControlePorta = emissorDoPdv?.ControlePorta ?? false
            };
        }

        private IList<PdvEcfDTO> ObterEmissores()
        {
            var repositorio = new RepositorioComun<PdvEcfDTO>(SessaoAdm);
            return repositorio.Busca(new TodosEcf());
        }
    }
}