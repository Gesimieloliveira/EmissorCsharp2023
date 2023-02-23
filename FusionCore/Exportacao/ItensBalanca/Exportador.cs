using System;
using System.Collections.Generic;
using System.IO;
using FusionCore.Sessao;

namespace FusionCore.Exportacao.ItensBalanca
{
    public class Exportador
    {
        private readonly ISessaoManager _sessaoManager;

        public Exportador(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public IList<string> Avisos { get; } = new List<string>();

        public void Exportar(ILayouotBalanca layout, IEnumerable<string> arquivosDestino)
        {
            var itens = BuscaItensExportacao();
            var tempName = Path.GetTempFileName();

            Avisos.Clear();

            using (var stream = new FileStream(tempName, FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(stream))
            {
                try
                {
                    foreach (var item in itens)
                    {
                        writer.WriteLine(layout.ConverteLinha(item));
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Avisos.Add(ex.Message);
                }
            }

            foreach (var s in arquivosDestino)
            {
                File.Copy(tempName, s, true);
            }
        }

        private IEnumerable<ModeloItem> BuscaItensExportacao()
        {
            using (var sessao = _sessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioExportacaoBalanca(sessao);
                return repositorio.BuscaTodos();
            }
        }
    }
}