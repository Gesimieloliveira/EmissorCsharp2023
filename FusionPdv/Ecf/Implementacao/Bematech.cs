using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FiscalPrinterBematech;
using FusionCore.FusionPdv.Ecf;

namespace FusionPdv.Ecf.Implementacao
{
    public class Bematech : EcfFiscal
    {
        public override void Configurar()
        {
        }

        public override void ManipulaArquivo()
        {
        }

        public override void Ativar()
        {
            var retorno = BemaFI32.Bematech_FI_HabilitaDesabilitaRetornoEstendidoMFD("1");
            BemaFI32.Analisa_iRetorno(retorno);
            Ativo = true;
        }

        public override void IdentificaPaf(string nomeVersao, string md5)
        {
            
        }

        public override EstadoEcfFiscal Estado()
        {
            var flagFiscal = 0;

            BemaFI32.Bematech_FI_FlagsFiscais(ref flagFiscal);


            int ack = 0, s = 0, s2 = 0, s3 = 0;
            BemaFI32.Bematech_FI_VerificaEstadoImpressoraMFD(ref ack, ref s, ref s2, ref s3);

            if(s == 2)
                return EstadoEcfFiscal.Venda;
            

            switch (flagFiscal)
            {
                case 1:
                    return EstadoEcfFiscal.Venda;
                case 2:
                    return EstadoEcfFiscal.Pagamento;
                case 8:
                    return EstadoEcfFiscal.Bloqueada;
            }

            return VerificaSeTemReducaoZPendente() ? EstadoEcfFiscal.RequerZ : EstadoEcfFiscal.Livre;
        }

        private bool VerificaSeTemReducaoZPendente()
        {
            var dataMovimento = DataMovimentoEmString();
            var dataUltimaReducaoZ = DataHoraUltimaReducaoZEmString();

            return !dataMovimento.Equals("000000") && !dataMovimento.Equals(dataUltimaReducaoZ);
        }

        public override DateTime DataEHora()
        {
            var data = new string('\x20', 6);
            var horaCompleta = new string('\x20', 6);

            BemaFI32.Bematech_FI_DataHoraImpressora(ref data, ref horaCompleta);

            var dia = data.Substring(0, 2);
            var mes = data.Substring(2, 2);
            var ano = data.Substring(4, 2);

            var hora = horaCompleta.Substring(0, 2);
            var minutos = horaCompleta.Substring(2, 2);
            var segundos = horaCompleta.Substring(4, 2);

            var dataEcf = Convert.ToDateTime(dia + "/" + mes + "/" + ano + " " + hora + ":" + minutos + ":" + segundos);

            return dataEcf;
        }

        public override DateTime DataMovimento()
        {
            return DateTime.ParseExact(DataMovimentoEmString(), "ddMMyy", CultureInfo.InvariantCulture); 
        }

        private static string DataMovimentoEmString()
        {
            var dataMovimento = string.Empty.PadLeft(7);

            var retorno = BemaFI32.Bematech_FI_DataMovimento(ref dataMovimento);

            dataMovimento = dataMovimento.TrimEnd('\0');

            return dataMovimento;
        }

        public override decimal GrandeTotal()
        {
            var gt = new string('\x20', 18);
            var retorno = BemaFI32.Bematech_FI_GrandeTotal(ref gt);
            BemaFI32.Analisa_iRetorno(retorno);

            return decimal.Parse(gt);
        }

        public override string Serie()
        {
            var serie = string.Empty.PadLeft(20);

            var retorno1 = BemaFI32.Bematech_FI_NumeroSerieMFD(ref serie);
            BemaFI32.Analisa_iRetorno(retorno1);

            return serie.Trim();
        }

        public string DataHoraUltimaReducaoZEmString()
        {
            var data = new string('\x20', 6);
            var horaCompleta = new string('\x20', 6);

            BemaFI32.Bematech_FI_DataHoraReducao(ref data, ref horaCompleta);

            if (data.Equals("000000") && horaCompleta.Equals("000000")) return "000000";

            var dia = data.Substring(0, 2);
            var mes = data.Substring(2, 2);
            var ano = data.Substring(4, 2);

            var hora = horaCompleta.Substring(0, 2);
            var minutos = horaCompleta.Substring(2, 2);
            var segundos = horaCompleta.Substring(4, 2);

            var dataEcf = Convert.ToDateTime(dia + "/" + mes + "/" + ano + " " + hora + ":" + minutos + ":" + segundos);

            return dataEcf.ToString("G");
        }

        public override string NumeroEcf()
        {
            var caixa = string.Empty.PadLeft(4);
            BemaFI32.Bematech_FI_NumeroCaixa(ref caixa);

            return caixa.Trim();
        }

        public override string Ccf()
        {
            var ccf = " ".PadLeft(6);

            BemaFI32.Bematech_FI_ContadorCupomFiscalMFD(ref ccf);

            return ccf.Trim();
        }

        public override string Coo()
        {
            var cupom = " ".PadLeft(7);

            BemaFI32.Bematech_FI_NumeroCupom(ref cupom);

            return cupom.TrimEnd('\0');
        }

        public override IList<FormaPagamento> FormasPagamentos()
        {
            var formasPagamento = " ".PadRight(3016);

            BemaFI32.Bematech_FI_VerificaFormasPagamentoMFD(ref formasPagamento);


            var regex = new char[1];

            const int virgola = 44;

            regex[0] = (char)virgola;

            var formapgto = new List<FormaPagamento>();

            var codigo = 1;

            formasPagamento.Split(regex[0]).Where(f => f.Length >= 16).ToList().ForEach(fp =>
            {
                var formapagamento = fp.Substring(0, 16).Trim();
                var permiteVinculado = fp.Substring(fp.Length - 1, 1).Trim();

                if (formapagamento == "") return;

                formapgto.Add(new FormaPagamento
                {
                    Descricao = formapagamento,
                    Indice = codigo.ToString(),
                    PermiteVinculado = permiteVinculado == "1"
                });

                codigo++;
            });

            return formapgto;
        }

        public override IList<Aliquota> Aliquotas()
        {
            var aliquotas = " ".PadLeft(79, ' ');
            var indices = " ".PadLeft(48, ' ');

            BemaFI32.Bematech_FI_RetornoAliquotas(ref aliquotas);
            BemaFI32.Bematech_FI_VerificaIndiceAliquotasIss(ref indices);


            indices = indices.Replace("\0", "");

            var aliquotass = new List<Aliquota>();
            var codigo = 1;

            char[] regex = {','};
            var listaliquota =
                aliquotas.Split(regex).Where(p => p.Trim() != "" && decimal.Parse(p.Trim()) != 0).ToList();
            var listaIndices = indices.Split(regex).Where(i => i.Trim() != "\0").ToList();

            listaliquota.ForEach(a =>
            {
                var aliquota = new Aliquota
                {
                    Indice = "0" + codigo,
                    Tipo = "T",
                    Valor = decimal.Parse(a.ToString())/100
                };

                listaIndices.ForEach(li =>
                {
                    if (int.Parse(li).Equals(codigo))
                    {
                        aliquota.Tipo = "I";
                    }
                });

                aliquotass.Add(aliquota);

                codigo++;
            });

            return aliquotass;
        }

        public override bool Arredonda()
        {
            var flag = new string('\x20', 2);
            BemaFI32.Bematech_FI_VerificaTruncamento(ref flag);
            return "0".Equals(flag);
        }

        public override void ProgramaFormaPagamento(string nome, bool permiteVinculado = false)
        {
            var temVinculado = permiteVinculado ? 1.ToString() : 0.ToString();
            BemaFI32.Bematech_FI_ProgramaFormaPagamentoMFD(nome, temVinculado);
        }

        public override void ProgramaAliquota(decimal aliquota, string tipo)
        {
            var aliquotaParametro = (aliquota*100.0M).ToString("0").PadLeft(4, '0').Substring(0, 4);

            BemaFI32.Bematech_FI_ProgramaAliquota(aliquotaParametro, tipo == "T" ? 0 : 1);
        }

        public override void LeituraX()
        {
            BemaFI32.Bematech_FI_LeituraX();
        }

        public override void ReducaoZ()
        {
            BemaFI32.Bematech_FI_ReducaoZ("", "");
        }

        public override void MudaHorarioVerao()
        {
            BemaFI32.Bematech_FI_ProgramaHorarioVerao();
        }

        public override void LeituraMemoriaFiscal(DateTime dataInicial, DateTime dataFinal, bool simplificada = false)
        {
            BemaFI32.Bematech_FI_LeituraMemoriaFiscalDataMFD(dataInicial.ToString("d"), dataFinal.ToString("d"),
                simplificada ? "c" : "s");
        }

        public override void AbreCupom(string cpf, string nome, string endereco)
        {
            var oi = BemaFI32.Bematech_FI_AbreCupomMFD(cpf, nome, endereco);
            BemaFI32.Analisa_RetornoImpressora();
        }

        public override void VendeItem(string codigo, string descricao, string aliquota, decimal quantidade,
            decimal valorUnitario,
            string unidade, decimal descontoPorcentagem)
        {
            BemaFI32.Bematech_FI_VendeItemDepartamento(codigo, descricao, aliquota,
                valorUnitario.ToString("N"), quantidade.ToString("N"), "0,00", "0,00", "01", unidade);
        }

        public override void CancelarCupom()
        {
            BemaFI32.Bematech_FI_CancelaCupom();
        }

        public override void CancelarItem(int numero)
        {
            BemaFI32.Bematech_FI_CancelaItemGenerico(numero.ToString());
        }

        public override void SubtotalizaCupom(decimal descontoAcrescimo)
        {
            BemaFI32.Bematech_FI_IniciaFechamentoCupomMFD("X", "%", "0", "0");
        }

        public override void EfetuaPagamento(string codigo, decimal valor)
        {
            BemaFI32.Bematech_FI_EfetuaFormaPagamento(FormasPagamentos().First(f => f.Indice.Equals(codigo)).Descricao,
                valor.ToString("N"));
        }

        public override void FechaCupom(string nome, string cpfCnpj, string endereco, string observacao)
        {
            BemaFI32.Bematech_FI_TerminaFechamentoCupom("");
        }

        public override void FechaCupom(string observacao)
        {
            BemaFI32.Bematech_FI_TerminaFechamentoCupom(observacao);
        }

        public override void Ibtp(string fonte, decimal valorAproximadoFederal, decimal valorAproximadoMunicipal,
            decimal valorAproximadoEstadual)
        {
        }

        public override void IdentificaConsumidor(string nome, string cpfCnpj, string endereco)
        {
        }

        public override void Desativar()
        {
            Ativo = false;
        }

        public override void LinhaRelatorioGerencial(string[] imagemComprovante)
        {
            
        }

        public override void LinhaCupomVinculado(string[] imagemComprovante)
        {
            
        }

        public override void AbreCupomVinculado(string coo, string indiceEcf, decimal valor)
        {
            
        }

        public override void AbreRelatorioGerencial(int indice = 0)
        {
            

        }

        public override void PulaLinhas(int linhasEntreCupons)
        {
            
        }

        public override void CortaPapel(bool corta)
        {
            
        }

        public override void FechaRelatorio()
        {
            
        }

        public override void CorrigeEstadoErro(bool reducaoZ = false)
        {
            
        }

        public override void PafEspelhoMfd(DateTime dataInicial, DateTime dataFinal, string caminho)
        {
            
        }

        public override void AbreGaveta()
        {
            
        }

        public override void Close()
        {
            

        }
    }
}