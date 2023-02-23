using System;
using System.Collections.Generic;
using ACBrFramework.ECF;
using FusionCore.FusionPdv.Sessao;
using FusionPdv.Acbr.Ecf;
using FusionPdv.ManipulaValor;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;
using Aliquota = FusionCore.FusionPdv.Ecf.Aliquota;
using FormaPagamento = FusionCore.FusionPdv.Ecf.FormaPagamento;

namespace FusionPdv.Ecf.Implementacao
{
    public class Acbr : EcfFiscal
    {
        public override bool Ativo => AcbrEcf.Ativo;
        public override bool PoucoPapel => AcbrEcf.PoucoPapel;
        public override bool HorarioVerao => AcbrEcf.HorarioVerao;
        private bool _primeiroAvisoPoucoPapel;
        public override int LinhasEntreCupons => AcbrEcf.LinhasEntreCupons;

        public override decimal SubTotal => AcbrEcf.SubTotal;
        public override decimal TotalPago => AcbrEcf.TotalPago;
        public override bool GavetaAberta => AcbrEcf.GavetaAberta;

        public override void Configurar()
        {
            AcbrEcf.Modelo = new BuscarModeloAcbr(Dispositivo.Modelo).Buscar();
            AcbrEcf.Device.Porta = Dispositivo.Porta;
            AcbrEcf.ControlePorta = Dispositivo.ControlePorta;
            AcbrEcf.Device.Baud = Dispositivo.Velocidade;
            AcbrEcf.Retentar = false;
            AcbrEcf.MaxLinhasBuffer = 3;
            AcbrEcf.OnMsgPoucoPapel += _acbrEcf_OnMsgPoucoPapel;
            AcbrEcf.OnDepoisFechaCupom += OnDepoisFecharCupom;
            AcbrEcf.Device.ProcessMessages = false;
            AcbrEcf.Device.TimeOut = 50;
        }

        private static void OnDepoisFecharCupom(object sender, FechaCupomEventArgs e)
        {
            AcbrEcf.AbreGaveta();
        }

        private void _acbrEcf_OnMsgPoucoPapel(object sender, EventArgs e)
        {
            var dataOnPoucoPapel = SessaoSistema.OnPoucoPapel;

            var dataValida = dataOnPoucoPapel.Subtract(DateTime.Now);

            if (!(dataValida.TotalMinutes < -40.00) && _primeiroAvisoPoucoPapel) return;

            DialogBox.MostraInformacao("Impressora com pouco papel.");
            SessaoSistema.OnPoucoPapel = DateTime.Now;
            _primeiroAvisoPoucoPapel = true;
        }

        public override void ManipulaArquivo()
        {
            
        }

        public override void Ativar()
        {
            AcbrEcf.Ativar();
        }

        public override void IdentificaPaf(string nomeVersao, string md5)
        {
            AcbrEcf.IdentificaPAF(nomeVersao, md5);
        }

        public override EstadoEcfFiscal Estado()
        {
            var estado = AcbrEcf.Estado;
            var retorno = EstadoEcfFiscal.Livre;
            
            switch (estado)
            {
                case EstadoECF.NaoInicializada:
                    retorno = EstadoEcfFiscal.NaoInicializada;
                break;

                case EstadoECF.Desconhecido:
                    retorno = EstadoEcfFiscal.Desconhecido;
                break;

                case EstadoECF.Livre:
                    retorno = EstadoEcfFiscal.Livre;
                break;

                case EstadoECF.Venda:
                    retorno = EstadoEcfFiscal.Venda;
                break;

                case EstadoECF.Pagamento:
                    retorno = EstadoEcfFiscal.Pagamento;
                break;

                case EstadoECF.Relatorio:
                    retorno = EstadoEcfFiscal.Relatorio;
                break;

                case EstadoECF.Bloqueada:
                    retorno = EstadoEcfFiscal.Bloqueada;
                break;

                case EstadoECF.RequerZ:
                    retorno = EstadoEcfFiscal.RequerZ;
                break;

                case EstadoECF.RequerX:
                    retorno = EstadoEcfFiscal.RequerX;
                break;
            }

            SessaoSistema.EstadoEcf = retorno;
            return retorno;
        }

        public override DateTime DataEHora()
        {
            return AcbrEcf.DataHora;
        }

        public override DateTime DataMovimento()
        {
            return AcbrEcf.DataMovimento;
        }

        public override decimal GrandeTotal()
        {
            return AcbrEcf.GrandeTotal;
        }

        public override string Serie()
        {
            return AcbrEcf.NumSerie;
        }

        public override string NumeroEcf()
        {
            return AcbrEcf.NumECF;
        }

        public override string Ccf()
        {
            return AcbrEcf.NumCCF;
        }

        public override string Coo()
        {
            return AcbrEcf.NumCOO;
        }

        public override IList<FormaPagamento> FormasPagamentos()
        {
            IList<FormaPagamento> formasPagamentos = new List<FormaPagamento>();

            AcbrEcf.CarregaFormasPagamento();
            AcbrEcf.FormasPagamento.ForEach(fp =>
            {
                formasPagamentos.Add(new FormaPagamento
                {
                    Indice = fp.Indice,
                    Descricao = fp.Descricao,
                    PermiteVinculado = fp.PermiteVinculado
                });
            });

            return formasPagamentos;
        }

        public override IList<Aliquota> Aliquotas()
        {
            IList<Aliquota> aliquotas = new List<Aliquota>();

            AcbrEcf.CarregaAliquotas();
            AcbrEcf.Aliquotas.ForEach(a =>
            {
                aliquotas.Add(new Aliquota
                {
                    Indice = a.Indice,
                    Valor = a.ValorAliquota,
                    Tipo = a.Tipo
                });
            });

            return aliquotas;
        }

        public override bool Arredonda()
        {
            return AcbrEcf.Arredonda;
        }

        public override void ProgramaFormaPagamento(string nome, bool permiteVinculado = false)
        {
            AcbrEcf.ProgramaFormaPagamento(nome, permiteVinculado);
        }

        public override void ProgramaAliquota(decimal aliquota, string tipo)
        {
            AcbrEcf.ProgramaAliquota(aliquota, tipo);
        }

        public override void LeituraX()
        {
            AcbrEcf.LeituraX();
        }

        public override void ReducaoZ()
        {
            var timeOutAntigo = AcbrEcf.Device.TimeOut;

            try
            {
                AcbrEcf.Device.TimeOut = 600;
                AcbrEcf.ReducaoZ();
            }
            finally
            {
                AcbrEcf.Device.TimeOut = timeOutAntigo;
            }
        }

        public override void MudaHorarioVerao()
        {
            AcbrEcf.MudaHorarioVerao();
        }

        public override void LeituraMemoriaFiscal(DateTime dataInicial, DateTime dataFinal, bool simplificada = false)
        {
            AcbrEcf.LeituraMemoriaFiscal(dataInicial, dataFinal, simplificada);
        }

        public override void AbreCupom(string cpf, string nome, string endereco)
        {
            AcbrEcf.AbreCupom(cpf, nome, endereco);
        }

        public override void VendeItem(string codigo, string descricao, string aliquota, decimal quantidade, decimal valorUnitario,
            string unidade, decimal descontoPorcentagem)
        {
            AcbrEcf.VendeItem(codigo, descricao, aliquota, quantidade, valorUnitario, descontoPorcentagem, unidade);
        }

        public override void CancelarCupom()
        {
            AcbrEcf.CancelaCupom();
        }

        public override void CancelarItem(int numero)
        {
            AcbrEcf.CancelaItemVendido(numero);
        }

        public override void SubtotalizaCupom(decimal descontoAcrescimo)
        {
            AcbrEcf.SubtotalizaCupom(descontoAcrescimo);
        }

        public override void EfetuaPagamento(string codigo, decimal valor)
        {
            AcbrEcf.EfetuaPagamento(codigo, valor);
        }

        public override void FechaCupom(string nome, string cpfCnpj, string endereco, string observacao)
        {
            AcbrEcf.FechaCupom(observacao);
        }

        public override void FechaCupom(string observacao)
        {
            AcbrEcf.FechaCupom(observacao);
        }

        public override void Ibtp(string fonte, decimal valorAproximadoFederal, decimal valorAproximadoMunicipal,
            decimal valorAproximadoEstadual)
        {
            AcbrEcf.InfoRodapeCupom.Imposto.Fonte = fonte;
            AcbrEcf.InfoRodapeCupom.Imposto.ValorAproximadoFederal = new TruncaArredonda(valorAproximadoFederal).Executa();
            AcbrEcf.InfoRodapeCupom.Imposto.ValorAproximadoMunicipal = new TruncaArredonda(valorAproximadoMunicipal).Executa();
            AcbrEcf.InfoRodapeCupom.Imposto.ValorAproximadoEstadual = new TruncaArredonda(valorAproximadoEstadual).Executa();
        }

        public override void IdentificaConsumidor(string nome, string cpfCnpj, string endereco)
        {
            NomeCliente = nome;
            DocumentoCliente = cpfCnpj;
            EnderecoCliente = endereco;
            AcbrEcf.IdentificaConsumidor(nome, cpfCnpj, endereco);
        }

        public override void Desativar()
        {
            AcbrEcf.Desativar();
        }

        public override void LinhaRelatorioGerencial(string[] imagemComprovante)
        {
            AcbrEcf.LinhaRelatorioGerencial(imagemComprovante);
        }

        public override void LinhaCupomVinculado(string[] imagemComprovante)
        {
            AcbrEcf.LinhaCupomVinculado(imagemComprovante);
        }

        public override void AbreCupomVinculado(string coo, string indiceEcf, decimal valor)
        {
            AcbrEcf.AbreCupomVinculado(coo, indiceEcf, valor);
        }

        public override void AbreRelatorioGerencial(int indice = 0)
        {
            AcbrEcf.AbreRelatorioGerencial(0);
        }

        public override void PulaLinhas(int linhasEntreCupons)
        {
            AcbrEcf.PulaLinhas(linhasEntreCupons);
        }

        public override void CortaPapel(bool corta)
        {
            AcbrEcf.CortaPapel(corta);
        }

        public override void FechaRelatorio()
        {
            AcbrEcf.FechaRelatorio();
        }

        public override void CorrigeEstadoErro(bool reducaoZ = false)
        {
            AcbrEcf.CorrigeEstadoErro(reducaoZ);
        }

        public override void PafEspelhoMfd(DateTime dataInicial, DateTime dataFinal, string caminho)
        {
            AcbrEcf.PafMF_MFD_Espelho(dataInicial, dataFinal, caminho);
        }

        public override void AbreGaveta()
        {
            AcbrEcf.GavetaSinalInvertido = true;
            AcbrEcf.AbreGaveta();
            AcbrEcf.GavetaSinalInvertido = false;
            AcbrEcf.AbreGaveta();
        }

        public override void Close()
        {
            AcbrEcf.Desativar();
        }
    }
}
