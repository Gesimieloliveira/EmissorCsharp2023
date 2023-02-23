using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Fusion.FastReport.Relatorios.Fixos;
using FusionCore.CadastroEmpresa;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.EntradaOutras;
using FusionCore.FusionAdm.Sintegra;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.Sintegra.Dto;
using FusionCore.Sintegra.Registro61RNfce;
using FusionCore.Sintegra.Registros;
using FusionCore.Sintegra.Registros61Nfce;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;
using NHibernate.Util;
using SintegraBr.Classes;
using SintegraBr.Common;

namespace Fusion.Visao.Sintegra
{
    public class SintegraFormModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;

        public SintegraFormModel(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;

            IsRegistros50 = true;
            IsRegistros70 = true;
            IsRegistros61 = true;
        }

        public IList<EmpresaComboBoxDTO> EmpresasDisponiveis
        {
            get => GetValue<IList<EmpresaComboBoxDTO>>();
            set => SetValue(value);
        }

        public EmpresaComboBoxDTO EmpresaSelecionada
        {
            get => GetValue<EmpresaComboBoxDTO>();
            set => SetValue(value);
        }

        public DateTime FiltroDataInicio
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime FiltroDataFinal
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime DataIventario
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public FinalidadeArquivo FinalidadeArquivo
        {
            get => GetValue<FinalidadeArquivo>();
            set => SetValue(value);
        }

        public Mes Mes
        {
            get => GetValue<Mes>();
            set
            {
                SetValue(value);
                var mes = (int) value;
                var dataPrimeiroDia = new DateTime(DateTime.Now.Year, mes, 1);

                FiltroDataInicio = dataPrimeiroDia.PrimeiroDiaDoMesAtual();
                FiltroDataFinal = dataPrimeiroDia.UltimoDiaDoMesAtual();
            }
        }

        public bool IsRegistros50
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsRegistros61
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsRegistros70
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsRegistros74
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool NaoIncluirImpostosComprasRegistros50
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void Inicializar()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var empresas = new RepositorioEmpresa(sessao).BuscarEmpresaComboBoxDtos();

                EmpresasDisponiveis = new List<EmpresaComboBoxDTO>(empresas);
                EmpresaSelecionada = empresas.FirstOrDefault();
            }

            var today = DateTime.Today;

            var mesAnterior = today.AddMonths(-1);
            var ultimoDiaMesAnterior = DateTime.DaysInMonth(mesAnterior.Year, mesAnterior.Month);

            FinalidadeArquivo = FinalidadeArquivo.Normal;

            Mes = (Mes) mesAnterior.Month;
            FiltroDataInicio = new DateTime(mesAnterior.Year, mesAnterior.Month, 1);
            FiltroDataFinal = new DateTime(mesAnterior.Year, mesAnterior.Month, ultimoDiaMesAnterior);
            DataIventario = new DateTime(today.Year - 1, 12, 31);
        }

        public void GerarArquivoIventario(string outputFileName)
        {
            var empresa = EmpresaSelecionada.CarregaEmpresa(_sessaoManager);

            var sintegraRegistro10 = new SintegraRegistro10(FiltroDataInicio, FiltroDataFinal, empresa, FinalidadeArquivo);
            var sintegraRegistro11 = new SintegraRegistro11(empresa);

            var registros50Dto = BuscarRegistros50(empresa);
            var registros53Dto = BuscarRegistros53(empresa);
            var registros54Dto = BuscarRegistros54(empresa);
            var registros70Dto = BuscarRegistros70(empresa);
            var registros74Dto = BuscarRegistros74(empresa);
            var registros75Dto = BuscarRegistros75(empresa);

            var registro10 = sintegraRegistro10.MontaRegistro10();
            var registro11 = sintegraRegistro11.MontarRegistro11();

            var listaRegistros50 = MontaRegistro50(registros50Dto);
            var listaRegistro53 = MontaRegistro53(registros53Dto);
            var listaRegistro54 = MontaRegistro54(registros54Dto);
            var listaRegistro70 = MontaRegistro70(registros70Dto);
            var listaRegistro74 = MontaRegistro74(registros74Dto, empresa);

            var listaRegistro75 = MontaRegistro75(registros75Dto);

            var registrosSintegra = new List<string>();

            var linhaRegistro10 = registro10.EscreverCampos();
            var linhaRegistro11 = registro11.EscreverCampos();

            registrosSintegra.Add(linhaRegistro10);
            registrosSintegra.Add(linhaRegistro11);


            listaRegistros50.ForEach(reg50 => { registrosSintegra.Add(reg50.EscreverCampos()); });

            listaRegistro53.ForEach(reg53 => { registrosSintegra.Add(reg53.EscreverCampos()); });

            listaRegistro54.ForEach(reg54 => { registrosSintegra.Add(reg54.EscreverCampos()); });

            if (IsRegistros61)
            {
                new Registro61Servico(FiltroDataInicio, FiltroDataFinal, empresa).ObterRegistros61()
                    .ForEach(reg61 => { registrosSintegra.Add(reg61.EscreverCampos()); });

                new Registro61RServico(FiltroDataInicio, FiltroDataFinal, empresa).ObterRegistros61R()
                    .ForEach(reg61R => { registrosSintegra.Add(reg61R.EscreverCampos()); });
            }

            listaRegistro70.ForEach(reg70 => { registrosSintegra.Add(reg70.EscreverCampos()); });

            listaRegistro74.ForEach(reg74 => { registrosSintegra.Add(reg74.EscreverCampos()); });

            listaRegistro75.ForEach(reg75 => { registrosSintegra.Add(reg75.EscreverCampos()); });

            var registro90 = new Registro90(empresa.Cnpj, empresa.InscricaoEstadual, registrosSintegra);

            registrosSintegra.Add(registro90.EscreverRegistro90());

            var arquivoEmMemoria = GerarArquivoEmMemoria(registrosSintegra);

            GerarPacoteZip(empresa, outputFileName, arquivoEmMemoria);

            // File.WriteAllText(fileName, arquivoEmMemoria);
        }

        private static List<Registro50> MontaRegistro50(IList<IRegistro50Dto> registros50Dto)
        {
            var listaRegistros50 = new List<Registro50>();

            registros50Dto.ForEach(reg50Dto =>
            {
                listaRegistros50.Add(new SintegraRegistro50(reg50Dto).MontaRegistro50());
            });
            return listaRegistros50;
        }

        private static List<Registro53> MontaRegistro53(IList<IRegistro53Dto> registros53Dto)
        {
            var listaRegistros50 = new List<Registro53>();

            registros53Dto.ForEach(reg50Dto =>
            {
                listaRegistros50.Add(new SintegraRegistro53(reg50Dto).MontaRegistro53());
            });
            return listaRegistros50;
        }

        private static List<Registro54> MontaRegistro54(IList<IRegistro54Dto> registros54Dto)
        {
            var listaRegistros54 = new List<Registro54>();

            registros54Dto.ForEach(reg54 => { listaRegistros54.Add(new SintegraRegistro54(reg54).MontaRegistro54()); });

            return listaRegistros54;
        }

        private List<Registro70> MontaRegistro70(IList<IRegistro70Dto> registros70Dto)
        {
            var listaRegistros70 = new List<Registro70>();

            registros70Dto.ForEach(reg70 => { listaRegistros70.Add(new SintegraRegistro70(reg70).MontaRegistro70()); });

            return listaRegistros70;
        }

        private List<Registro74> MontaRegistro74(IList<IRegistro74Dto> registros74Dto, EmpresaDTO empresa)
        {
            var listaRegistros74 = new List<Registro74>();

            registros74Dto.ForEach(reg =>
            {
                listaRegistros74.Add(new SintegraRegistro74(reg, FiltroDataFinal, empresa).MontaRegistro74());
            });

            return listaRegistros74;
        }

        private List<Registro75> MontaRegistro75(IList<IRegistro75Dto> registros75Dto)
        {
            var listaRegistros75 = new List<Registro75>();

            registros75Dto.ForEach(reg75 =>
            {
                listaRegistros75.Add(new SintegraRegistro75(reg75, FiltroDataInicio, FiltroDataFinal)
                    .MontaRegistro75());
            });

            listaRegistros75 = new Registro75Agrupamento(listaRegistros75).Executar();

            return listaRegistros75;
        }

        private IList<IRegistro50Dto> BuscarRegistros50(EmpresaDTO empresa)
        {
            if (IsRegistros50 == false) return new List<IRegistro50Dto>();

            var listaRegistro50 = new List<IRegistro50Dto>();

            using (var sessao =_sessaoManager.CriaSessao())
            {
                var repositorioSintegra = new RepositorioSintegra(sessao);
                var repositorioNfe = new RepositorioNfe(sessao);
                var repositorioInutilizacao = new RepositorioInutilizacao(sessao);
                var repositorioNotaFiscalCompra = new RepositorioNotaFiscalCompra(sessao);

                var registrosNfOutros =
                    repositorioSintegra.BuscarRegistros50NfOutrosEntrada(FiltroDataInicio, FiltroDataFinal, empresa);
                var registrosNfe = repositorioNfe.BuscarRegistros50(FiltroDataInicio, FiltroDataFinal, empresa);
                var registrosInutilizacao =
                    repositorioInutilizacao.BuscaRegistro50(FiltroDataInicio, FiltroDataFinal, empresa);
                var registrosEntradas =
                    repositorioNotaFiscalCompra.BuscarRegistro50ComprasDtos(FiltroDataInicio, FiltroDataFinal, empresa);


                registrosEntradas.ForEach(item =>
                    {
                        item.BaseCalculoIcms = NaoIncluirImpostosComprasRegistros50 ? 0.0m : item.BaseCalculoIcms;
                        item.Aliquota = NaoIncluirImpostosComprasRegistros50 ? 0.0m : item.Aliquota;
                        item.ValorIcms = NaoIncluirImpostosComprasRegistros50 ? 0.0m : item.ValorIcms;
                    });


                var agrupamento = registrosEntradas.GroupBy(x => new
                    {
                        x.Numero,
                        x.Cfop,
                        x.Aliquota
                    })
                    .Select(x =>
                    {
                        var registro50 = x.First();

                        return new Registro50ComprasDto
                        {
                            Numero = registro50.Numero,
                            Aliquota = registro50.Aliquota,
                            SiglaUf = registro50.SiglaUf,
                            BaseCalculoIcms = x.Sum(soma => soma.BaseCalculoIcms),
                            Cfop = registro50.Cfop,
                            ChaveNfe = registro50.ChaveNfe,
                            Cnpj = registro50.Cnpj,
                            Cpf = registro50.Cpf,
                            Cst = registro50.Cst,
                            DocumentoUnico = registro50.DocumentoUnico,
                            InscricaoEstadual = registro50.InscricaoEstadual,
                            LancamentoEm = registro50.LancamentoEm,
                            Serie = registro50.Serie,
                            ValorIcms = x.Sum(soma => soma.ValorIcms),
                            ValorTotal = x.Sum(soma => soma.ValorTotal) + x.Sum(soma => soma.ValorSt)
                            + x.Sum(soma => soma.ValorIpi) + x.Sum(soma => soma.DespesaRateio)
                            + x.Sum(soma => soma.ValorFcpSt)
                        };
                    }).ToList();

                registrosEntradas = agrupamento;


                var geraRegistros50Nfe = new Registro50ComValorOutros(registrosNfe);
                var registro50Nfe = geraRegistros50Nfe.Executar();

                var geraRegistros50NotaCompra = new Registro50NotaCompra(registrosEntradas);
                var registro50Entradas = geraRegistros50NotaCompra.Executar();

                registrosNfOutros.ForEach(listaRegistro50.Add);
                registro50Nfe.ForEach(listaRegistro50.Add);
                registro50Entradas.ForEach(listaRegistro50.Add);

                foreach (var sintegraRegistro50InutilizacaoDto in registrosInutilizacao)
                {
                    var listaRegistro50InutilizacaoDtos = sintegraRegistro50InutilizacaoDto.ObterRegistros50();

                    listaRegistro50InutilizacaoDtos.ForEach(listaRegistro50.Add);
                }
            }

            return listaRegistro50;
        }

        private IList<IRegistro53Dto> BuscarRegistros53(EmpresaDTO empresa)
        {
            if (IsRegistros50 == false) return new List<IRegistro53Dto>();

            var listaRegistro50 = new List<IRegistro53Dto>();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioSintegra = new RepositorioSintegra(sessao);
                var repositorioNfe = new RepositorioNfe(sessao);
                var repositorioNotaFiscalCompra = new RepositorioNotaFiscalCompra(sessao);

                var registros53NfOutro =
                    repositorioSintegra.BuscarRegistros53NfOutrosEntrada(FiltroDataInicio, FiltroDataFinal, empresa);
                var registrosNfe = repositorioNfe.BuscarRegistros53(FiltroDataInicio, FiltroDataFinal, empresa);
                var registrosEntradas =
                    repositorioNotaFiscalCompra.BuscarRegistro53ComprasDtos(FiltroDataInicio, FiltroDataFinal, empresa);


                var geraRegistros53Nfe = new Registro53NfeAgrupamentoQuandoNaoTemIcms(registrosNfe);
                var registro53Nfe = geraRegistros53Nfe.Executar();

                var geraRegistros53Compra = new Registro53ComprasAgrupamentoQuandoNaoTemIcms(registrosEntradas);
                var registro53Compras = geraRegistros53Compra.Executar();

                registros53NfOutro.ForEach(listaRegistro50.Add);
                registro53Nfe.ForEach(listaRegistro50.Add);
                registro53Compras.ForEach(listaRegistro50.Add);
            }

            return listaRegistro50;
        }

        private IList<IRegistro54Dto> BuscarRegistros54(EmpresaDTO empresa)
        {
            if (IsRegistros50 == false) return new List<IRegistro54Dto>();

            var listaRegistro54 = new List<IRegistro54Dto>();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioNfe = new RepositorioNfe(sessao);
                var repositorioNotaFiscalCompra = new RepositorioNotaFiscalCompra(sessao);

                var registrosNfe = repositorioNfe.BuscarRegistros54(FiltroDataInicio, FiltroDataFinal, empresa);
                var registrosNFCompra =
                    repositorioNotaFiscalCompra.BuscarRegistro54ComprasDto(FiltroDataInicio, FiltroDataFinal, empresa);


                var registroCompraOrdenadoPorNumero = registrosNFCompra.OrderBy(x => x.Numero).ToList();
                var registroCompraAgrupado = registroCompraOrdenadoPorNumero.GroupBy(x => x.Numero);

                var registroCompraLista = new List<Registro54ComprasDto>();
                int numeroItem;
                registroCompraAgrupado.ForEach(x =>
                {
                    numeroItem = 0;
                    x.ForEach(reg =>
                    {
                        numeroItem++;
                        reg.NumeroItem = numeroItem;
                        registroCompraLista.Add(reg);
                    });
                });

                var registroCompraOrdenado =
                    registroCompraLista.OrderBy(x => x.Numero).ThenBy(x => x.NumeroItem).ToList();

                registroCompraOrdenado.ForEach(item =>
                    {
                        item.BaseCalculoIcms = NaoIncluirImpostosComprasRegistros50 ? 0.0m : item.BaseCalculoIcms;
                        item.AliquotaIcms = NaoIncluirImpostosComprasRegistros50 ? 0.0m : item.AliquotaIcms;
                    });

                var registrosNfeOrdenado = registrosNfe.OrderBy(x => x.Numero).ThenBy(x => x.NumeroItem).ToList();

                registrosNfeOrdenado.ForEach(listaRegistro54.Add);
                registroCompraOrdenado.ForEach(listaRegistro54.Add);
            }

            return listaRegistro54;
        }

        private IList<IRegistro70Dto> BuscarRegistros70(EmpresaDTO empresa)
        {
            if (IsRegistros70 == false) return new List<IRegistro70Dto>();

            var listaRegistro70 = new List<IRegistro70Dto>();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioCteEntrada = new RepositorioNfCteEntrada(sessao);
                var registrosCteEntradas =
                    repositorioCteEntrada.BuscarRegistro70CteEntradaDtos(FiltroDataInicio, FiltroDataFinal, empresa);

                var repositorioCte = new RepositorioCte(sessao);
                var registroCte = repositorioCte.BuscarRegistro70CteDtos(FiltroDataInicio, FiltroDataFinal, empresa);

                registrosCteEntradas.ForEach(listaRegistro70.Add);
                registroCte.ForEach(listaRegistro70.Add);
            }

            return listaRegistro70;
        }

        private IList<IRegistro74Dto> BuscarRegistros74(EmpresaDTO empresa)
        {
            if (IsRegistros74 == false) return new List<IRegistro74Dto>();

            var listaRegistro74 = new List<IRegistro74Dto>();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioProduto = new RepositorioProduto(sessao);

                var registros74 = repositorioProduto.BuscarRegistro74Sintegra(DataIventario);

                registros74.ForEach(listaRegistro74.Add);
            }

            return listaRegistro74;
        }

        private IList<IRegistro75Dto> BuscarRegistros75(EmpresaDTO empresa)
        {
            var listaRegistro75 = new List<IRegistro75Dto>();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioNfe = new RepositorioNfe(sessao);
                var repositorioNotaFiscalCompra = new RepositorioNotaFiscalCompra(sessao);

                var registrosNfe = repositorioNfe.BuscarRegistros75(FiltroDataInicio, FiltroDataFinal, empresa);
                var registrosCompra =
                    repositorioNotaFiscalCompra.BuscarRegistros75(FiltroDataInicio, FiltroDataFinal, empresa);

                registrosNfe.ForEach(listaRegistro75.Add);
                registrosCompra.ForEach(listaRegistro75.Add);

                if (IsRegistros74)
                {
                    var repositorioProduto = new RepositorioProduto(sessao);
                    var resposta = repositorioProduto.BuscarRegistro75Inventario(DataIventario);
                    resposta.ForEach(listaRegistro75.Add);
                }

                if (IsRegistros61)
                {
                    var resposta = new Registro75PorRegistro61RServico(FiltroDataInicio, FiltroDataFinal, empresa)
                        .ObterRegistros75();
                    resposta.ForEach(listaRegistro75.Add);
                }
            }

            return listaRegistro75;
        }

        private string GerarArquivoEmMemoria(List<string> registrosSintegra)
        {
            var arquivo = new StringBuilder();

            registrosSintegra.ForEach(reg => { arquivo.Append(reg); });

            var fileText = arquivo.ToString().RemoverAcentos();

            return fileText;
        }

        private void GerarPacoteZip(IEmpresa empresa, string outputFileName, string conteudoSintegra)
        {
            try
            {
                var fileName = Path.GetFileName(outputFileName);

                using (var memZip = new FileStream(outputFileName, FileMode.Create))
                using (var zip = new ZipArchive(memZip, ZipArchiveMode.Update))
                {
                    var nomeBase = fileName.Replace(".zip", "");
                    var sintegraArquivo = zip.CreateEntry($"{nomeBase}.txt", CompressionLevel.Optimal);

                    using (var entrada = sintegraArquivo.Open())
                    using (var streamWriter = new StreamWriter(entrada))
                    {
                        streamWriter.Write(conteudoSintegra);
                    }

                    AnexarRelatorioComprasPorOperacao(zip, empresa);
                    AnecarRealtorioComprasPorNotaEOperacao(zip, empresa);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao gerar arquivo compactado do Sintegra", e);
            }
        }

        private void AnexarRelatorioComprasPorOperacao(ZipArchive zip, IEmpresa empresa)
        {
            using (var r = new RClassificacaoFiscalDeComprasPorOperacao(_sessaoManager))
            {
                r.ComEmpresaId(empresa.Id);
                r.ComPeriodo(FiltroDataInicio.Date, FiltroDataFinal.Date);

                using (var pdf = new MemoryStream())
                {
                    r.ExportarPdf(pdf);

                    var entry = zip.CreateEntry("compras-por-operacao.pdf", CompressionLevel.Optimal);

                    using (var stream = entry.Open())
                    {
                        pdf.Seek(0, SeekOrigin.Begin);
                        pdf.CopyTo(stream);
                    }
                }
            }
        }

        private void AnecarRealtorioComprasPorNotaEOperacao(ZipArchive zip, IEmpresa empresa)
        {
            using (var r = new RClassificacaoFiscalDeComprasPorNotaEOperacao(_sessaoManager))
            {
                r.ComEmpresaId(empresa.Id);
                r.ComPeriodo(FiltroDataInicio.Date, FiltroDataFinal.Date);

                using (var pdf = new MemoryStream())
                {
                    r.ExportarPdf(pdf);

                    var entry = zip.CreateEntry("compras-por-nota-e-operacao.pdf", CompressionLevel.Optimal);

                    using (var stream = entry.Open())
                    {
                        pdf.Seek(0, SeekOrigin.Begin);
                        pdf.CopyTo(stream);
                    }
                }
            }
        }
    }
}