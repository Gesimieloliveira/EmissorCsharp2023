using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.GerenciarManifestacoesEletronicas.EstrategiasProcessamento;
using FusionCore.Helpers.Basico;

// ReSharper disable ConvertToUsingDeclaration

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class DistribuidorItemDfe
    {
        private readonly IList<string> _logs;

        public DistribuidorItemDfe(IList<string> logs = null)
        {
            _logs = logs;
        }

        public void Processar(IEstrategia estrategia)
        {
            var docsPendentes = BuscarItensDaEstrategia(estrategia);

            if (docsPendentes == null || docsPendentes.Any() == false)
            {
                return;
            }

            foreach (var item in docsPendentes)
            {
                try
                {
                    using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                    using (var transaction = sessao.BeginTransaction())
                    {
                        try
                        {
                            var repositorio = new RepositorioDistribuicaoDFe(sessao);

                            if (!estrategia.Criar(item, repositorio))
                            {
                                continue;
                            }

                            item.MarcarComoProcessado();
                            repositorio.Update(item);

                            transaction.Commit();

                            _logs.Add($"OK - Nsu: {item.Nsu} - {item.TipoDfe.GetDescription()} - Processado com sucesso!");
                        }
                        catch (Exception e)
                        {
                            _logs.Add($"ERRO - Nsu: {item.Nsu} - {item.TipoDfe.GetDescription()} - {e.Message}");
                        }
                    }
                }
                catch (Exception e)
                {
                    _logs.Add($"Falha ao conectar no banco de dados para processar os itens: {e.Message}");
                    break;
                }
            }

            
        }

        private IList<ItemDistribuicaoDFe> BuscarItensDaEstrategia(IEstrategia estrategia)
        {
            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioDistribuicaoDFe(sessao);
                    return repositorio.BuscarNaoProcessados(estrategia.ETipoDfe, (int?) estrategia.ETipoEvento);
                }
            }
            catch (Exception e)
            {
                _logs.Add($"Erro ao obter itens do tipo: {estrategia.ETipoDfe} - {e.Message}");
            }

            return null;
        }
    }
}