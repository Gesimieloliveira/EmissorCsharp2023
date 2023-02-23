using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Hql.Ast.ANTLR;

namespace FusionCore.FusionAdm.Importacao.Estrategia
{
    public class ImportacaoTabelaIbpt : IMecanismoImportacao
    {
        private string _caminhoArquivo;
        private ISession _sessao;

        public void AnexarSesao(ISession sessao)
        {
            _sessao = sessao;
        }

        public void InformarArquivo(string caminhoArquivo)
        {
            var fileInfo = new FileInfo(caminhoArquivo);

            if (!fileInfo.Exists)
            {
                throw new InvalidPathException(string.Concat("Arquivo não localizado: ", caminhoArquivo));
            }

            if (fileInfo.Extension != ".csv")
            {
                throw new InvalidPathException("Arquivo deve ser .csv");
            }

            _caminhoArquivo = caminhoArquivo;
        }

        public void Importar()
        {
            using (var reader = new StreamReader(_caminhoArquivo, Encoding.Default))
            {
                var cfg = new CsvConfiguration(CultureInfo.CurrentCulture) {Delimiter = ";"};
                var csv = new CsvReader(reader, cfg);

                var ibpts = new List<Ibpt>();

                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    ibpts.Add(CriaTributoIbpt(csv));
                }

                var repositorioNcm = new RepositorioNcm(_sessao);
                var repositorio = new RepositorioIbpt(_sessao);

                var ncmExistentes = repositorioNcm.TodosCodigosNcm();

                repositorio.DeletaTodos();

                ibpts.ForEach(ibpt =>
                {
                    repositorio.Persiste(ibpt);

                    if (ibpt.Tipo == TipoIbpt.Ncm && ncmExistentes.IndexOf(ibpt.Codigo) < 0)
                    {
                        var ncm = new NcmDTO
                        {
                            Id = ibpt.Codigo,
                            Descricao = ibpt.Descricao
                        };

                        repositorioNcm.Persiste(ncm);
                        ncmExistentes.Add(ncm.Id);
                    }
                });
            }
        }

        private static Ibpt CriaTributoIbpt(CsvReader csv)
        {
            var descricao = csv.GetField(3);
            var estadual = csv.GetField(6);
            var importado = csv.GetField(5);
            var excecaoFiscal = csv.GetField(1);
            var tipo = csv.GetField<TipoIbpt>(2);
            var nacional = csv.GetField(4);
            var chaveIbpt = csv.GetField(10);
            var codigo = csv.GetField(0);

            return new Ibpt(codigo, tipo, excecaoFiscal)
            {
                ChaveIbpt = chaveIbpt,
                Descricao = descricao,
                Estadual = decimal.Parse(estadual.Replace(".", ",")),
                Importado = decimal.Parse(importado.Replace(".", ",")),
                Nacional = decimal.Parse(nacional.Replace(".", ","))
            };
        }
    }
}