using System.IO;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionServico;
using FusionCore.Servicos.Core.Exportacao.Estrategias;
using FusionCore.Sessao;

namespace FusionCore.Servicos.Core.Exportacao
{
    public class Exportador
    {
        private readonly ISessaoManager _sessaoManager;

        public Exportador(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public void Exportar(IExportacao exportacao, ConfiguracaoExportacao configuracao)
        {
            var sessao = _sessaoManager.CriaStatelessSession();

            try
            {
                var repositorio = exportacao.CriaRepositorio(sessao);
                var docs = repositorio.ListarDocumentosNaoExportados();

                if (docs.Any() == false)
                {
                    return;
                }

                foreach (var doc in docs)
                {
                    var ambiente = doc.Ambiente == TipoAmbiente.Producao ? "Produção" : "Homologação";

                    var dir = Path.Combine(
                        configuracao.DiretorioExportacao, 
                        ambiente, 
                        doc.GetCnpjEmitente(), 
                        doc.GetMesAnoAutorizacao(), 
                        doc.GetModelo()
                    );

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using (var file = File.CreateText(Path.Combine(dir, $"{doc.Chave}.xml")))
                    {
                        file.Write(doc.Xml);
                        file.Flush();
                    }
                }

                repositorio.SalvarExportadas(docs);
            }
            finally
            {
                sessao.Dispose();
            }
        }
    }
}