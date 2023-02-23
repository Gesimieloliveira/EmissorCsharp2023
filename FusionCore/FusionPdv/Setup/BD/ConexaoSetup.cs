using System;
using System.IO;
using FusionCore.Helpers.Ambiente;
using Newtonsoft.Json;

namespace FusionCore.FusionPdv.Setup.BD
{
    public class ConexaoSetup
    {
        private readonly string _pathArquivo;

        public ConexaoSetup()
        {
            _pathArquivo = Path.Combine(DiretorioAssembly.GetPastaConfig(), "conexao.agil4");
        }

        public bool ExisteArquivoConexao => File.Exists(_pathArquivo);

        public void CriarArquivoConexao()
        {
            try
            {
                var conexaoAdm = new ConexaoCfg("FusionAdm");
                var conexaoPdv = new ConexaoCfg("FusionPdv");

                var container = new ContainerCfg(conexaoAdm, conexaoPdv);

                var json = JsonConvert.SerializeObject(container);

                using (var stream = File.Create(_pathArquivo))
                using (var writer = new StreamWriter(stream))
                {
                    stream.Position = 0;
                    writer.Write(json);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Erro ao criar arquivo conexão: " + e.Message);
            }
        }

        public ContainerCfg LerArquivoConexao()
        {
            try
            {
                var content = File.ReadAllText(_pathArquivo);
                return JsonConvert.DeserializeObject<ContainerCfg>(content);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Erro ao ler arquivo conexão: " + e.Message);
            }
        }

        public void Armazenar(ContainerCfg container)
        {
            try
            {
                var json = JsonConvert.SerializeObject(container);
                File.WriteAllText(_pathArquivo, json);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Erro ao gravar arquivo conexao: " + e.Message);
            }
        }
    }
}