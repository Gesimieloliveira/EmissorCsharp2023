using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DFe.Utils;
using FusionCore.Core.Flags;
using FusionCore.DFe.XmlCte;
using FusionCore.DFe.XmlCte.XmlCte.Processada;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Inutilizacao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;
using Sped.Dominio;
using Sped.Repositorio;
using SpedBr.Common;
using SpedBr.SpedFiscal;

namespace Sped
{
    public class MainWindowModel : ViewModel
    {
        public MainWindowModel()
        {
            Empresas = new ObservableCollection<EmpresaComboBoxDTO>();
            Contador = new Contador();
            Mes = (Mes)DateTime.Now.Month;
        }

        private const string XmlContador = @"C:\SistemaFusion\Sped\Contador.xml";
        private const string XmlEmpresa = @"C:\SistemaFusion\Sped\Empresa.xml";

        public Contador Contador
        {
            get { return _contador; }
            set
            {
                if (Equals(value, _contador)) return;
                _contador = value;
                PropriedadeAlterada();
            }
        }

        private ObservableCollection<EmpresaComboBoxDTO> _empresas;
        private EmpresaComboBoxDTO _empresaSelecionada;
        private Contador _contador;
        private ObservableCollection<CidadeDTO> _cidades;
        private IDictionary<string, Participante> _participantes;
        public ICommand CommandGerar => GetSimpleCommand(GerarAction);

        public ObservableCollection<EmpresaComboBoxDTO> Empresas
        {
            get { return _empresas; }
            set
            {
                if (Equals(value, _empresas)) return;
                _empresas = value;
                PropriedadeAlterada();
            }
        }

        public EmpresaComboBoxDTO EmpresaSelecionada
        {
            get { return _empresaSelecionada; }
            set
            {
                if (Equals(value, _empresaSelecionada)) return;
                _empresaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<CidadeDTO> Cidades
        {
            get { return _cidades; }
            set
            {
                if (Equals(value, _cidades)) return;
                _cidades = value;
                PropriedadeAlterada();
            }
        }

        public DateTime FiltroDataInicio
        {
            get { return _filtroDataInicio; }
            set
            {
                if (value.Equals(_filtroDataInicio)) return;
                _filtroDataInicio = value;
                PropriedadeAlterada();
            }
        }

        public DateTime FiltroDataFinal
        {
            get { return _filtroDataFinal; }
            set
            {
                if (value.Equals(_filtroDataFinal)) return;
                _filtroDataFinal = value;
                PropriedadeAlterada();
            }
        }

        public Mes Mes
        {
            get { return _mes; }
            set
            {
                _mes = value;
                PropriedadeAlterada();
                var mes = (int)value;

                var dataPrimeiroDia = new DateTime(DateTime.Now.Year, mes, 1);

                switch (value)
                {
                    case Mes.Janeiro:
                        dataPrimeiroDia = new DateTime(DateTime.Now.Year-1, 12, 1);
                        break;
                    case Mes.Fevereiro:
                    case Mes.Marco:
                    case Mes.Abril:
                    case Mes.Maio:
                    case Mes.Junho:
                    case Mes.Julho:
                    case Mes.Agosto:
                    case Mes.Setembro:
                    case Mes.Outubro:
                    case Mes.Novembro:
                    case Mes.Dezembro:
                        dataPrimeiroDia = new DateTime(DateTime.Now.Year, mes-1, 1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }

                FiltroDataInicio = dataPrimeiroDia.PrimeiroDiaDoMesAtual();
                FiltroDataFinal = dataPrimeiroDia.UltimoDiaDoMesAtual();
            }
        }

        private int _quantidadeLinhasRegistroD100;
        private int _quantidadeLinhasRegistroD190;

        private void GerarAction(object obj)
        {
            try
            {
                Validacoes();

                PersisteDadosDefault();


                var empresa = BuscarEmpresaSelecionada();

                var reg0000 = GerarReg0000(empresa);
                var reg0001 = GerarReg0001();
                var reg0005 = GerarReg0005(empresa);
                var reg0100 = GerarReg0100();
                var reg0150 = GerarReg0150(empresa);
                var reg0990 = GerarReg0990(5 + reg0150.Count());
                var regC001 = GerarRegC001();
                var regC990 = GerarRegC990();
                var regD001 = GerarRegD001();
                var regD100 = GerarRegD100();
                var regD990 = GerarRegD990(2+ (_quantidadeLinhasRegistroD100 + _quantidadeLinhasRegistroD190));
                var regE001 = GerarRegE001();
                var regE100 = GerarRegE100();
                var regE110 = GerarRegE110();
                var regE116 = GerarRegE116(regE110.VlIcmsRecolher);
                var regE990 = GerarRegE990();
                var regG001 = GerarRegG001();
                var regG990 = GerarRegG990();
                var regH001 = GerarRegH001();
                var regH990 = GerarRegH990();
                var regK001 = GerarRegK001();
                var regK990 = GerarRegK990();
                var reg1001 = GerarReg1001();
                var reg1010 = GerarReg1010();
                var reg1990 = GerarReg1990();
                var reg9001 = GerarReg9001();
                var reg9900 = GerarReg9900();
                var reg9990 = GerarReg9990();

                var arquivoSped = new StringBuilder();

                GerarArquivoEmMemoria(arquivoSped, reg0000, 
                    reg0001, reg0005, reg0100, reg0150,
                    reg0990, regC001, regC990, regD001,
                    regD100, regD990, regE001, regE100,
                    regE110, regE116, regE990, regG001,
                    regG990, regH001, regH990, regK001,
                    regK990, reg1001, reg1010, reg1990,
                    reg9001, reg9900, reg9990);

                var manipulaPasta = CriaDiretorioDeSalvarSped();

                var arquivoCaminho = CaminhoArquivo(manipulaPasta, empresa);

                SalvarArquivoSped(arquivoCaminho, arquivoSped);

                DialogBox.MostraInformacao($"Gerou Sped no diretório\n{arquivoCaminho}");
                Process.Start(arquivoCaminho);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        private Bloco9.Registro9999 GerarReg9999()
        {
            return new Bloco9.Registro9999
            {
                QtdLin = (_quantidadeLinhasTotal + 1)
            };
        }

        private Bloco9.Registro9990 GerarReg9990()
        {
            return new Bloco9.Registro9990
            {
                QtdLin9 =_quantidadeRegistro9900 + 3
            };
        }

        private int _quantidadeRegistro9900;
        private List<Bloco9.Registro9900> GerarReg9900()
        {
            var lista = new List<Bloco9.Registro9900>();


            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "0000"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "0001"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "0005"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "0100"
            });
            _quantidadeRegistro9900++;

            if (_quantidadeRegistro0150 > 0)
            {
                lista.Add(new Bloco9.Registro9900
                {
                    QtdRegBlc = _quantidadeRegistro0150,
                    RegBlc = "0150"
                });
                _quantidadeRegistro9900++;
            }

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "0990"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "C001"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "C990"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "D001"
            });
            _quantidadeRegistro9900++;

            if (_quantidadeLinhasRegistroD100 > 0)
            {
                lista.Add(new Bloco9.Registro9900
                {
                    QtdRegBlc = _quantidadeLinhasRegistroD100,
                    RegBlc = "D100"
                });
                _quantidadeRegistro9900++;
            }

            if (_quantidadeLinhasRegistroD190 > 0)
            {
                lista.Add(new Bloco9.Registro9900
                {
                    QtdRegBlc = _quantidadeLinhasRegistroD190,
                    RegBlc = "D190"
                });
                _quantidadeRegistro9900++;
            }

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "D990"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "E001"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "E100"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "E110"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "E116"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "E990"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "G001"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "G990"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "H001"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "H990"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "K001"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "K990"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "1001"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "1010"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "1990"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "9001"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "9990"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = 1,
                RegBlc = "9999"
            });
            _quantidadeRegistro9900++;

            lista.Add(new Bloco9.Registro9900
            {
                QtdRegBlc = lista.Count +1,
                RegBlc = "9900"
            });
            _quantidadeRegistro9900++;

            return lista;
        }

        private Bloco9.Registro9001 GerarReg9001()
        {
            return new Bloco9.Registro9001
            {
                IndMov = IndMovimento.BlocoComDados
            };
        }

        private Bloco1.Registro1990 GerarReg1990()
        {
            return new Bloco1.Registro1990
            {
                QtdLin1 = 3
            };
        }

        private Bloco1.Registro1010 GerarReg1010()
        {
            return new Bloco1.Registro1010
            {
                IndExp = SimOuNao.N,
                IndCcrf = SimOuNao.N,
                IndComb = SimOuNao.N,
                IndUsina = SimOuNao.N,
                IndVa = SimOuNao.N,
                IndEe = SimOuNao.N,
                IndCart = SimOuNao.N,
                IndForm = SimOuNao.N,
                IndAer = SimOuNao.N
            };
        }

        private Bloco1.Registro1001 GerarReg1001()
        {
            return new Bloco1.Registro1001
            {
                IndMov = IndMovimento.BlocoComDados
            };
        }

        private BlocoK.RegistroK990 GerarRegK990()
        {
            return new BlocoK.RegistroK990
            {
                QtdLinK = 2
            };
        }

        private BlocoK.RegistroK001 GerarRegK001()
        {
            return new BlocoK.RegistroK001
            {
                IndMov = IndMovimento.BlocoSemDados
            };
        }

        private BlocoH.RegistroH990 GerarRegH990()
        {
            return new BlocoH.RegistroH990
            {
                QtdLinH = 2
            };
        }

        private BlocoH.RegistroH001 GerarRegH001()
        {
            return new BlocoH.RegistroH001
            {
                IndMov = IndMovimento.BlocoSemDados
            };
        }

        private BlocoG.RegistroG990 GerarRegG990()
        {
            return new BlocoG.RegistroG990
            {
                QtdLinG = 2
            };
        }

        private BlocoG.RegistroG001 GerarRegG001()
        {
            return new BlocoG.RegistroG001
            {
                IndMov = IndMovimento.BlocoSemDados
            };
        }

        private BlocoE.RegistroE990 GerarRegE990()
        {
            return new BlocoE.RegistroE990
            {
                QtdLinE = 5
            };
        }

        private BlocoE.RegistroE116 GerarRegE116(decimal regE110VlIcmsRecolher)
        {
            return new BlocoE.RegistroE116
            {
                CodOr = "000",
                VlOr = regE110VlIcmsRecolher,
                DtVcto = FiltroDataFinal.ToString("d").Replace("/", ""),
                CodRec = "108",
                MesRef = FiltroDataFinal.Month.ToString("D2")
                + FiltroDataFinal.ObterUltimoDiaMesAtual().Year
            };
        }

        private BlocoE.RegistroE110 GerarRegE110()
        {
            var registroE110 = new BlocoE.RegistroE110
            {
                VlTotDebitos = ObterValorTotalDebitos(),
                VlTotCreditos = ObterTotalCredito()
            };

            registroE110.VlSldApurado = registroE110.VlTotDebitos - registroE110.VlTotCreditos;
            registroE110.VlIcmsRecolher = registroE110.VlSldApurado;

            return registroE110;
        }

        private decimal ObterTotalCredito()
        {
            var total = 0.0m;

            _xmls.Where(
                x => x.SituacaoCodigo != 135 && x.SituacaoCodigo != 136 && x.SituacaoCodigo != 134).ForEach(x =>
            {
                total += ObterValorCredito(x.FusionCTe.CTe.InformacoesCTe.Imposto);
            });

            return total;
        }

        private decimal ObterValorCredito(FusionImpostoCTe imposto)
        {
            switch (imposto.FusionIcmsCTe.Icms)
            {
                case FusionIcms00CTe fusionIcms00CTe:
                    return 0.0m;
                case FusionIcms20CTe fusionIcms20CTe:
                    return 0.0m;
                case FusionIcms45CTe fusionIcms45CTe:
                    return 0.0m;
                case FusionIcms60CTe fusionIcms60CTe:
                    if (fusionIcms60CTe.vCred != null) return fusionIcms60CTe.vCred.Value;
                    return 0.0m;
                case FusionIcms90CTe fusionIcms90CTe:
                    if (fusionIcms90CTe.vCred != null) return fusionIcms90CTe.vCred.Value;
                    return 0.0m;
                case FusionIcmsSimplesNacionalCTe fusionIcmsSimplesNacionalCTe:
                    return 0.0m;
            }

            return 0.0m;
        }

        private decimal ObterValorTotalDebitos()
        {
            var total = 0.0m;

            _xmls.Where(
                x => x.SituacaoCodigo != 135 && x.SituacaoCodigo != 136 && x.SituacaoCodigo != 134).ForEach(x =>
            {
                total += ObterValorIcms(x.FusionCTe.CTe.InformacoesCTe.Imposto);
            });

            return total;
        }

        private BlocoE.RegistroE100 GerarRegE100()
        {
            return new BlocoE.RegistroE100
            {
                DtIni = FiltroDataInicio,
                DtFin = FiltroDataFinal
            };
        }

        private BlocoE.RegistroE001 GerarRegE001()
        {
            return new BlocoE.RegistroE001
            {
                IndMov = IndMovimento.BlocoComDados
            };
        }

        private BlocoD.RegistroD990 GerarRegD990(int quantidade)
        {
            return new BlocoD.RegistroD990 {QtdLinD = quantidade};
        }

        private List<RegistroCTe> GerarRegD100()
        {
            var registroCTe = new List<RegistroCTe>();

            _xmls.ForEach(xml =>
            {
                if (xml.IsCancelado())
                {
                    var registroCteCancelado = new RegistroCTe
                    {
                        RegistroD100 = new BlocoD.RegistroD100
                        {
                            IndOper = 1,
                            IndEmit = 0,
                            CodMod = "57",
                            CodSit = 02,
                            Ser = xml.FusionCTe.CTe.InformacoesCTe.Identificacao.Serie.ToString(),
                            Sub = xml.FusionCTe.CTe.InformacoesCTe.Identificacao.Serie.ToString(),
                            NumDoc = xml.FusionCTe.CTe.InformacoesCTe.Identificacao.NumeroDocumento.ToString(),
                            ChvCte = xml.FusionCTe.CTe.InformacoesCTe.Id.Substring(3, 44)
                        }
                    };


                    _quantidadeLinhasRegistroD100++;
                    registroCTe.Add(registroCteCancelado);
                    return;
                }


                CTeAutorizado(xml, registroCTe);
            });

            var xmlInutilizacao = BuscarXmlInutilizado();

            xmlInutilizacao.ForEach(xml =>
            {
                if (NumeroInicialIgualNumeroFinal(xml, registroCTe)) return;


                for (var i = xml.NumeroInicial; i <= xml.NumeroFinal; i++)
                {
                    var registroCteCancelado = new RegistroCTe
                    {
                        RegistroD100 = new BlocoD.RegistroD100
                        {
                            IndOper = 1,
                            IndEmit = 0,
                            CodMod = "57",
                            CodSit = 05,
                            Ser = xml.Serie.ToString(),
                            Sub = xml.Serie.ToString(),
                            NumDoc = i.ToString()
                        }
                    };

                    _quantidadeLinhasRegistroD100++;
                    registroCTe.Add(registroCteCancelado);
                }
            });

            return registroCTe;
        }

        private bool NumeroInicialIgualNumeroFinal(CteInutilizacao xml, List<RegistroCTe> registroCTe)
        {
            if (xml.NumeroInicial == xml.NumeroFinal)
            {
                var registroCteCancelado = new RegistroCTe
                {
                    RegistroD100 = new BlocoD.RegistroD100
                    {
                        IndOper = 1,
                        IndEmit = 0,
                        CodMod = "57",
                        CodSit = 05,
                        Ser = xml.Serie.ToString(),
                        Sub = xml.Serie.ToString(),
                        NumDoc = xml.NumeroFinal.ToString()
                    }
                };

                _quantidadeLinhasRegistroD100++;
                registroCTe.Add(registroCteCancelado);

                return true;
            }

            return false;
        }

        private void CTeAutorizado(XmlAutorizado xml, List<RegistroCTe> registroCTe)
        {
            var registroCte = new RegistroCTe();

            registroCte.RegistroD100 = new BlocoD.RegistroD100
            {
                IndOper = 1,
                IndEmit = 0,
                CodPart = ObterCodigoDoParticipante(xml.FusionCTe),
                CodMod = "57",
                CodSit = 00,
                Ser = xml.FusionCTe.CTe.InformacoesCTe.Identificacao.Serie.ToString(),
                Sub = xml.FusionCTe.CTe.InformacoesCTe.Identificacao.Serie.ToString(),
                NumDoc = xml.FusionCTe.CTe.InformacoesCTe.Identificacao.NumeroDocumento.ToString(),
                ChvCte = xml.FusionCTe.CTe.InformacoesCTe.Id.Substring(3, 44),
                DtDoc = xml.FusionCTe.CTe.InformacoesCTe.Identificacao.EmitidaEm.ToDateTime(),
                DtAP = xml.FusionCTe.CTe.InformacoesCTe.Identificacao.EmitidaEm.ToDateTime(),
                TpCte = (int) xml.FusionCTe.CTe.InformacoesCTe.Identificacao.TipoCTe,
                ChvCteRef = string.Empty,
                VlDoc = xml.FusionCTe.CTe.InformacoesCTe.ValoresPrestacaoServico.ValorTotal,
                VlDesc = 0.0m,
                IndFrt = 1,
                VlServ = xml.FusionCTe.CTe.InformacoesCTe.ValoresPrestacaoServico.ValorTotal,
                VlBcIcms = ObterBaseCalculoIcms(xml.FusionCTe.CTe.InformacoesCTe.Imposto),
                VlIcms = ObterValorIcms(xml.FusionCTe.CTe.InformacoesCTe.Imposto),
                VlNt = 0.0m,
                CodMunOrig = xml.FusionCTe.CTe.InformacoesCTe.Identificacao.CodigoIbgeMunicipioEnvio.ToString(),
                CodMunDest = xml.FusionCTe.CTe.InformacoesCTe.Identificacao.CodigoIbgeMunicipioFimOperacao.ToString()
            };

            _quantidadeLinhasRegistroD100++;

            registroCte.RegistroD190 = new BlocoD.RegistroD190
            {
                CstIcms = ObterCst(xml.FusionCTe.CTe.InformacoesCTe.Imposto),
                Cfop = Convert.ToInt32(xml.FusionCTe.CTe.InformacoesCTe.Identificacao.Cfop),
                AliqIcms = ObterAliquota(xml.FusionCTe.CTe.InformacoesCTe.Imposto),
                VlOpr = ObterBaseCalculoIcms(xml.FusionCTe.CTe.InformacoesCTe.Imposto),
                VlBcIcms = ObterBaseCalculoIcms(xml.FusionCTe.CTe.InformacoesCTe.Imposto),
                VlIcms = ObterValorIcms(xml.FusionCTe.CTe.InformacoesCTe.Imposto),
                VlRedBc = ObterValorIcmsReducao(xml.FusionCTe.CTe.InformacoesCTe.Imposto)
            };

            _quantidadeLinhasRegistroD190++;

            registroCTe.Add(registroCte);
        }

        // todo verificar redução
        private decimal ObterValorIcmsReducao(FusionImpostoCTe imposto)
        {
            switch (imposto.FusionIcmsCTe.Icms)
            {
                case FusionIcms00CTe fusionIcms00CTe:
                    return 0.0m;
                case FusionIcms20CTe fusionIcms20CTe:
                    return 0.0m; // todo fusionIcms20CTe.vICMS;
                case FusionIcms45CTe fusionIcms45CTe:
                    return 0.0m;
                case FusionIcms60CTe fusionIcms60CTe:
                    return 0.0m; // todo fusionIcms60CTe.vICMSSTRet;
                case FusionIcms90CTe fusionIcms90CTe:
                    return 0.0m;// todo fusionIcms90CTe.vICMS;
                case FusionIcmsSimplesNacionalCTe fusionIcmsSimplesNacionalCTe:
                    return 0.0m;
            }

            return 0.0m;
        }

        private decimal ObterAliquota(FusionImpostoCTe imposto)
        {
            switch (imposto.FusionIcmsCTe.Icms)
            {
                case FusionIcms00CTe fusionIcms00CTe:
                    return fusionIcms00CTe.pICMS;
                case FusionIcms20CTe fusionIcms20CTe:
                    return fusionIcms20CTe.pICMS;
                case FusionIcms45CTe fusionIcms45CTe:
                    return 0.0m;
                case FusionIcms60CTe fusionIcms60CTe:
                    return fusionIcms60CTe.pICMSSTRet;
                case FusionIcms90CTe fusionIcms90CTe:
                    return fusionIcms90CTe.pICMS;
                case FusionIcmsSimplesNacionalCTe fusionIcmsSimplesNacionalCTe:
                    return 0.0m;
            }

            return 0.0m;
        }

        private int ObterCst(FusionImpostoCTe imposto)
        {
            switch (imposto.FusionIcmsCTe.Icms)
            {
                case FusionIcms00CTe fusionIcms00CTe:
                    return Convert.ToInt32(fusionIcms00CTe.CST);
                case FusionIcms20CTe fusionIcms20CTe:
                    return fusionIcms20CTe.CST;
                case FusionIcms45CTe fusionIcms45CTe:
                    return fusionIcms45CTe.CST;
                case FusionIcms60CTe fusionIcms60CTe:
                    return fusionIcms60CTe.CST;
                case FusionIcms90CTe fusionIcms90CTe:
                    return fusionIcms90CTe.CST;
                case FusionIcmsSimplesNacionalCTe fusionIcmsSimplesNacionalCTe:
                    return fusionIcmsSimplesNacionalCTe.CST;
            }

            throw new InvalidOperationException("CST no sped incorreto");
        }

        private decimal ObterValorIcms(FusionImpostoCTe imposto)
        {
            switch (imposto.FusionIcmsCTe.Icms)
            {
                case FusionIcms00CTe fusionIcms00CTe:
                    return fusionIcms00CTe.vICMS;
                case FusionIcms20CTe fusionIcms20CTe:
                    return fusionIcms20CTe.vICMS;
                case FusionIcms45CTe fusionIcms45CTe:
                    return 0.0m;
                case FusionIcms60CTe fusionIcms60CTe:
                    return fusionIcms60CTe.vICMSSTRet;
                case FusionIcms90CTe fusionIcms90CTe:
                    return fusionIcms90CTe.vICMS;
                case FusionIcmsSimplesNacionalCTe fusionIcmsSimplesNacionalCTe:
                    return 0.0m;
            }

            return 0.0m;
        }

        private decimal ObterBaseCalculoIcms(FusionImpostoCTe imposto)
        {
            switch (imposto.FusionIcmsCTe.Icms)
            {
                case FusionIcms00CTe fusionIcms00CTe:
                    return fusionIcms00CTe.vBC;
                case FusionIcms20CTe fusionIcms20CTe:
                    return fusionIcms20CTe.vBC;
                case FusionIcms45CTe fusionIcms45CTe:
                    return 0.0m;
                case FusionIcms60CTe fusionIcms60CTe:
                    return fusionIcms60CTe.vBCSTRet;
                case FusionIcms90CTe fusionIcms90CTe:
                    return fusionIcms90CTe.vBC;
                case FusionIcmsSimplesNacionalCTe fusionIcmsSimplesNacionalCTe:
                    return 0.0m;
            }

            return 0.0m;
        }

        private string ObterCodigoDoParticipante(FusionCTeProcessamento xml)
        {
            return ObterCodigoDestinatario(xml);
        }

        private static string ObterCodigoDestinatario(FusionCTeProcessamento xml)
        {
            var destinatario = xml.CTe.InformacoesCTe.Destinatario;

            return destinatario.Cpf.IsNotNullOrEmpty() ? destinatario.Cpf : destinatario.Cnpj;
        }

        private BlocoD.RegistroD001 GerarRegD001()
        {
            return new BlocoD.RegistroD001 {IndMov = !_xmls.Any() ? IndMovimento.BlocoSemDados : IndMovimento.BlocoComDados};
        }

        private BlocoC.RegistroC990 GerarRegC990()
        {
            return new BlocoC.RegistroC990 {QtdLinC = 2};
        }

        private BlocoC.RegistroC001 GerarRegC001()
        {
            return new BlocoC.RegistroC001 {IndMov = IndMovimento.BlocoSemDados};
        }

        private Bloco0.Registro0990 GerarReg0990(int qtd)
        {
            return new Bloco0.Registro0990
            {
                QtdLin0 = qtd
            };
        }

        private int _quantidadeRegistro0150;
        private IEnumerable<Bloco0.Registro0150> GerarReg0150(EmpresaDTO empresa)
        {
            var reg0150 = new List<Bloco0.Registro0150>();

            _participantes = BuscarXmlsAutorizados(empresa);

            _participantes.ForEach(x =>
            {
                var participante = x.Value;

                var reg = new Bloco0.Registro0150
                {
                    Cnpj = participante.Cnpj,
                    Cpf = participante.Cpf,
                    Nome = participante.Nome,
                    Bairro = participante.Bairro,
                    Ie = participante.InscricaoEstadual,
                    Suframa = string.Empty,
                    CodMun = participante.CodigoMunicipio,
                    Num = participante.Numero,
                    Compl = participante.Complemento,
                    End = participante.Endereco,
                    CodPais = participante.Pais.ToString(),
                    CodPart = participante.Codigo
                };

                reg0150.Add(reg);

                _quantidadeRegistro0150++;
            });

            return reg0150;
        }

        private IDictionary<string, Participante> BuscarXmlsAutorizados(EmpresaDTO empresa)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioSpedCTe(sessao);

                var dataInicial = FiltroDataInicio;
                var dataFinal = FiltroDataFinal;

                _xmls = repositorio.BuscarXmlAutorizadosECancelados(empresa, dataInicial, dataFinal);


                var participantes = new Dictionary<string, Participante>();

                _xmls.ForEach(xml =>
                {
                    var inf = xml.FusionCTe.CTe.InformacoesCTe;

                    var existeParticipanteJa = inf.Destinatario.Cnpj.IsNotNullOrEmpty() ? participantes.ContainsKey(inf.Destinatario.Cnpj) : participantes.ContainsKey(inf.Destinatario.Cpf);

                    if (inf.Destinatario.Cnpj.IsNotNullOrEmpty())
                    {
                        if (existeParticipanteJa) return;

                        var dest = inf.Destinatario;

                        AdicionaParticipanteDestinatario(participantes, dest, dest.Cnpj);
                    }

                    if (inf.Destinatario.Cpf.IsNotNullOrEmpty())
                    {
                        if (existeParticipanteJa) return;

                        var dest = inf.Destinatario;

                        AdicionaParticipanteDestinatario(participantes, dest, dest.Cpf);
                    }
                });

                return participantes;
            }
        }

        private static void AdicionaParticipanteDestinatario(IDictionary<string, Participante> participantes, FusionDestinatarioCTe dest, string chave)
        {
            participantes.Add(chave,
                new Participante
                {
                    Cpf = dest.Cpf,
                    Cnpj = dest.Cnpj,
                    Nome = dest.Nome,
                    Complemento = dest.Endereco.Complemento,
                    Bairro = dest.Endereco.Bairro,
                    Endereco = dest.Endereco.Logradouro,
                    Numero = dest.Endereco.Numero,
                    InscricaoEstadual = dest.Ie,
                    Suframa = string.Empty,
                    Codigo = chave,
                    CodigoMunicipio = dest.Endereco.CodigoIbgeMunicipio.ToString()
                });
        }

        private static ManipulaPasta CriaDiretorioDeSalvarSped()
        {
            var manipulaPasta = new ManipulaPasta(@"C:\SistemaFusion\Sped");
            manipulaPasta.CriaPastaSeNaoExistir();
            return manipulaPasta;
        }

        private static void SalvarArquivoSped(string arquivoCaminho, StringBuilder arquivoSped)
        {
            File.WriteAllText(arquivoCaminho, arquivoSped.ToString());
        }

        private static string CaminhoArquivo(ManipulaPasta manipulaPasta, EmpresaDTO empresa)
        {
            return Path.Combine(manipulaPasta.Diretorio, empresa.NomeFantasia + " - " + DateTime.Now.ToString("D")) + ".txt";
        }

        private int _quantidadeLinhasTotal;
        private IEnumerable<XmlAutorizado> _xmls;
        private DateTime _filtroDataInicio;
        private DateTime _filtroDataFinal;
        private Mes _mes;

        private void GerarArquivoEmMemoria(StringBuilder arquivoSped,
            Bloco0.Registro0000 reg0000,
            Bloco0.Registro0001 reg0001,
            Bloco0.Registro0005 reg0005,
            Bloco0.Registro0100 reg0100,
            IEnumerable<Bloco0.Registro0150> reg0150,
            Bloco0.Registro0990 reg0990,
            BlocoC.RegistroC001 regC001,
            BlocoC.RegistroC990 regC990,
            BlocoD.RegistroD001 regD001,
            List<RegistroCTe> regD100,
            BlocoD.RegistroD990 regD990,
            BlocoE.RegistroE001 regE001,
            BlocoE.RegistroE100 regE100,
            BlocoE.RegistroE110 regE110,
            BlocoE.RegistroE116 regE116,
            BlocoE.RegistroE990 regE990,
            BlocoG.RegistroG001 regG001,
            BlocoG.RegistroG990 regG990,
            BlocoH.RegistroH001 regH001,
            BlocoH.RegistroH990 regH990,
            BlocoK.RegistroK001 regK001,
            BlocoK.RegistroK990 regK990,
            Bloco1.Registro1001 reg1001,
            Bloco1.Registro1010 reg1010,
            Bloco1.Registro1990 reg1990,
            Bloco9.Registro9001 reg9001,
            List<Bloco9.Registro9900> reg9900,
            Bloco9.Registro9990 reg9990)
        {
            string errosEncontrados0000 = string.Empty;
            var resultReg0000 = reg0000.EscreverCampos(out errosEncontrados0000);
            arquivoSped.Append(resultReg0000);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontrados0001 = string.Empty;
            var resultReg0001 = reg0001.EscreverCampos(out errosEncontrados0001);
            arquivoSped.Append(resultReg0001);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontrados0005 = string.Empty;
            var resultReg0005 = reg0005.EscreverCampos(out errosEncontrados0005);
            arquivoSped.Append(resultReg0005);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontrados0100 = string.Empty;
            var resultReg0100 = reg0100.EscreverCampos(out errosEncontrados0100);
            arquivoSped.Append(resultReg0100);
            QuantidadeLinhaTotalAdicionarMaisUm();

            reg0150.ForEach(reg =>
            {
                string errosEncontrados0150 = string.Empty;
                var resultReg0150 = reg.EscreverCampos(out errosEncontrados0150);
                arquivoSped.Append(resultReg0150);
                QuantidadeLinhaTotalAdicionarMaisUm();
            });

            string errosEncontrados0990 = string.Empty;
            var resultReg0990 = reg0990.EscreverCampos(out errosEncontrados0990);
            arquivoSped.Append(resultReg0990);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosC001 = string.Empty;
            var resultRegC001 = regC001.EscreverCampos(out errosEncontradosC001);
            arquivoSped.Append(resultRegC001);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosC990 = string.Empty;
            var resultRegC990 = regC990.EscreverCampos(out errosEncontradosC990);
            arquivoSped.Append(resultRegC990);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosD001 = string.Empty;
            var resultRegD001 = regD001.EscreverCampos(out errosEncontradosD001);
            arquivoSped.Append(resultRegD001);
            QuantidadeLinhaTotalAdicionarMaisUm();

            regD100.ForEach(reg =>
            {
                string errosEncontradosD100 = string.Empty;
                var resultRegD100 = reg.RegistroD100.EscreverCampos(out errosEncontradosD100);
                arquivoSped.Append(resultRegD100);
                QuantidadeLinhaTotalAdicionarMaisUm();

                if (reg.RegistroD100.CodSit == 02) return;
                if (reg.RegistroD100.CodSit == 05) return;

                string errosEncontradosD190 = string.Empty;
                var resultRegD190 = reg.RegistroD190.EscreverCampos(out errosEncontradosD190);
                arquivoSped.Append(resultRegD190);
                QuantidadeLinhaTotalAdicionarMaisUm();

            });

            string errosEncontradosD990 = string.Empty;
            var resultRegD990 = regD990.EscreverCampos(out errosEncontradosD990);
            arquivoSped.Append(resultRegD990);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosE001 = string.Empty;
            var resultRegE001 = regE001.EscreverCampos(out errosEncontradosE001);
            arquivoSped.Append(resultRegE001);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosE100 = string.Empty;
            var resultRegE100 = regE100.EscreverCampos(out errosEncontradosE100);
            arquivoSped.Append(resultRegE100);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosE110 = string.Empty;
            var resultRegE110 = regE110.EscreverCampos(out errosEncontradosE110);
            arquivoSped.Append(resultRegE110);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosE116 = string.Empty;
            var resultRegE116 = regE116.EscreverCampos(out errosEncontradosE116);
            arquivoSped.Append(resultRegE116);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosE990 = string.Empty;
            var resultRegE990 = regE990.EscreverCampos(out errosEncontradosE990);
            arquivoSped.Append(resultRegE990);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosG001 = string.Empty;
            var resultRegG001 = regG001.EscreverCampos(out errosEncontradosG001);
            arquivoSped.Append(resultRegG001);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosG990 = string.Empty;
            var resultRegG990 = regG990.EscreverCampos(out errosEncontradosG990);
            arquivoSped.Append(resultRegG990);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosH001 = string.Empty;
            var resultRegH001 = regH001.EscreverCampos(out errosEncontradosH001);
            arquivoSped.Append(resultRegH001);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosH990 = string.Empty;
            var resultRegH990 = regH990.EscreverCampos(out errosEncontradosH990);
            arquivoSped.Append(resultRegH990);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosK001 = string.Empty;
            var resultRegK001 = regK001.EscreverCampos(out errosEncontradosK001);
            arquivoSped.Append(resultRegK001);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontradosK990 = string.Empty;
            var resultRegK990 = regK990.EscreverCampos(out errosEncontradosK990);
            arquivoSped.Append(resultRegK990);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontrados1001 = string.Empty;
            var resultReg1001 = reg1001.EscreverCampos(out errosEncontrados1001);
            arquivoSped.Append(resultReg1001);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontrados1010 = string.Empty;
            var resultReg1010 = reg1010.EscreverCampos(out errosEncontrados1010);
            arquivoSped.Append(resultReg1010);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontrados1990 = string.Empty;
            var resultReg1990 = reg1990.EscreverCampos(out errosEncontrados1990);
            arquivoSped.Append(resultReg1990);
            QuantidadeLinhaTotalAdicionarMaisUm();

            string errosEncontrados9001 = string.Empty;
            var resultReg9001 = reg9001.EscreverCampos(out errosEncontrados9001);
            arquivoSped.Append(resultReg9001);
            QuantidadeLinhaTotalAdicionarMaisUm();

            reg9900.ForEach(x =>
            {
                string errosEncontrados9900 = string.Empty;
                var resultReg9900 = x.EscreverCampos(out errosEncontrados9900);
                arquivoSped.Append(resultReg9900);
                QuantidadeLinhaTotalAdicionarMaisUm();
            });

            string errosEncontrados9990 = string.Empty;
            var resultReg9990 = reg9990.EscreverCampos(out errosEncontrados9990);
            arquivoSped.Append(resultReg9990);
            QuantidadeLinhaTotalAdicionarMaisUm();


            var reg9999 = GerarReg9999();
            string errosEncontrados9999 = string.Empty;
            var resultReg9999 = reg9999.EscreverCampos(out errosEncontrados9999);
            arquivoSped.Append(resultReg9999);
        }

        private void QuantidadeLinhaTotalAdicionarMaisUm()
        {
            _quantidadeLinhasTotal++;
        }

        private Bloco0.Registro0100 GerarReg0100()
        {
            var reg0100 = new Bloco0.Registro0100();

            reg0100.Nome = Contador.Nome;
            reg0100.Cpf = Contador.Cpf;
            reg0100.Crc = Contador.Crc;
            reg0100.Cnpj = Contador.CnpjContabilidade;
            reg0100.Cep = Contador.Cep;
            reg0100.End = Contador.Logradouro;
            reg0100.Num = Contador.Numero;
            reg0100.Compl = Contador.Complemento;
            reg0100.Bairro = Contador.Bairro;
            reg0100.Fone = Contador.Telefone;
            reg0100.Fax = Contador.Fax;
            reg0100.Email = Contador.Email;
            reg0100.CodMun = Contador.Cidade.CodigoIbge.ToString();

            return reg0100;
        }

        private Bloco0.Registro0005 GerarReg0005(EmpresaDTO empresa)
        {
            var reg0005 = new Bloco0.Registro0005();

            reg0005.Fantasia = empresa.NomeFantasia;
            reg0005.Cep = empresa.Cep;
            reg0005.End = empresa.Logradouro;
            reg0005.Num = empresa.Numero;
            reg0005.Compl = empresa.Complemento;
            reg0005.Bairro = empresa.Bairro;
            reg0005.Fone = empresa.Fone1;
            reg0005.Fax = string.Empty;
            reg0005.Email = empresa.Email;

            return reg0005;
        }

        private Bloco0.Registro0001 GerarReg0001()
        {
            var reg0000 = new Bloco0.Registro0001();
            reg0000.IndMov = 0;
            return reg0000;
        }

        private Bloco0.Registro0000 GerarReg0000(EmpresaDTO empresa)
        {
            var reg0000 = new Bloco0.Registro0000();

            reg0000.CodVer = 12;
            reg0000.CodFin = IndCodFinalidadeArquivo.RemessaArquivoOriginal;
            reg0000.DtIni = FiltroDataInicio;
            reg0000.DtFin = FiltroDataFinal;
            reg0000.Nome = empresa.RazaoSocial;
            reg0000.Cnpj = empresa.Cnpj;
            reg0000.Uf = empresa.EstadoDTO.Sigla;
            reg0000.Ie = empresa.InscricaoEstadual;
            reg0000.CodMun = empresa.CidadeDTO.CodigoIbge.ToString();
            reg0000.Im = string.Empty;
            reg0000.Suframa = string.Empty;
            reg0000.IndPerfil = IndPerfilArquivo.A;
            reg0000.IndAtiv = IndTipoAtividade.Outros;
            return reg0000;
        }

        private EmpresaDTO BuscarEmpresaSelecionada()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmpresa(sessao);
                return repositorio.GetPeloId(EmpresaSelecionada.Id);
            }
        }

        public void Inicializa()
        {
            CarregarEmpresas();
            CarregarCidades();

            CarregarEmpresaSelecionada();
            CarregarContador();
        }

        private void CarregarEmpresaSelecionada()
        {
            var manipulaArquivo = new ManipulaArquivo(XmlEmpresa);

            if (manipulaArquivo.Existe())
            {
                EmpresaSelecionada = FuncoesXml.ArquivoXmlParaClasse<EmpresaComboBoxDTO>(XmlEmpresa);
            }
        }

        private void CarregarContador()
        {
            var manipulaArquivo = new ManipulaArquivo(XmlContador);

            if (manipulaArquivo.Existe())
            {
                Contador = FuncoesXml.ArquivoXmlParaClasse<Contador>(XmlContador);
            }
        }

        private void CarregarCidades()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCidade(sessao);
                var cidades = repositorio.BuscaTodos();
                Cidades = new ObservableCollection<CidadeDTO>(cidades);
            }
        }

        private void CarregarEmpresas()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmpresa(sessao);
                var empresas = repositorio.BuscarEmpresaComboBoxDtos();
                Empresas = new ObservableCollection<EmpresaComboBoxDTO>(empresas);
            }
        }

        private void PersisteDadosDefault()
        {
            FuncoesXml.ClasseParaArquivoXml(Contador, XmlContador);
            FuncoesXml.ClasseParaArquivoXml(EmpresaSelecionada, XmlEmpresa);
        }

        private void Validacoes()
        {
            if (EmpresaSelecionada == null)
            {
                throw new InvalidOperationException("Selecione uma empresa");
            }

            if (Contador.Nome.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Nome obrigatório");
            }

            if (new CpfRegra().NaoValido(Contador.Cpf.TrimOrEmpty()))
            {
                throw new InvalidOperationException("CPF inválido");
            }

            if (Contador.Crc.IsNullOrEmpty())
            {
                throw new InvalidOperationException("CRC obrigatório");
            }

            if (new EmailRegra().NaoValido(Contador.Email.TrimOrEmpty()))
            {
                throw new InvalidOperationException("E-mail inválido");
            }

            if (Contador.Cidade.IsNull())
            {
                throw new InvalidOperationException("Cidade obrigatório");
            }
        }

        private IEnumerable<CteInutilizacao> BuscarXmlInutilizado()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioSpedCTe(sessao).BuscarTodasInutilizacoesPorPeriodo(FiltroDataInicio, FiltroDataFinal);
            }
        }
    }
}