using System;
using System.Collections.Generic;
using ACBrFramework.ECF;
using FusionPdv.Acbr;
using Aliquota = FusionCore.FusionPdv.Ecf.Aliquota;
using FormaPagamento = FusionCore.FusionPdv.Ecf.FormaPagamento;

namespace FusionPdv.Ecf
{
    
    public abstract class EcfFiscal
    {
        public static Dispositivo Dispositivo { get; set; }
        public virtual bool PoucoPapel => false;
        public virtual bool Ativo { get; set; }

        public virtual bool HorarioVerao => false;

        public virtual decimal SubTotal { get; set; }
        public virtual decimal TotalPago { get; set; }

        public string NomeCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public string EnderecoCliente { get; set; }
        public virtual int LinhasEntreCupons { get; set; }
        public virtual bool GavetaAberta { get; set; }

        public readonly static ACBrECF AcbrEcf = AcbrFactory.ObterAcbrEcf();

        public abstract void Configurar();
        public abstract void ManipulaArquivo();
        public abstract void Ativar();
        public abstract void IdentificaPaf(string nomeVersao, string md5);

        public abstract EstadoEcfFiscal Estado();
        public abstract DateTime DataEHora();
        public abstract DateTime DataMovimento();
        public abstract decimal GrandeTotal();
        public abstract string Serie();
        public abstract string NumeroEcf();
        public abstract string Ccf();
        public abstract string Coo();
        public abstract IList<FormaPagamento> FormasPagamentos();
        public abstract IList<Aliquota> Aliquotas();
        public abstract bool Arredonda();

        public abstract void ProgramaFormaPagamento(string nome, bool permiteVinculado = false);
        public abstract void ProgramaAliquota(decimal aliquota, string tipo);
        public abstract void LeituraX();
        public abstract void ReducaoZ();
        public abstract void MudaHorarioVerao();
        public abstract void LeituraMemoriaFiscal(DateTime dataInicial, DateTime dataFinal, bool simplificada = false);

        public void AbreCupom()
        {
            AbreCupom(null, null, null);
        }

        public abstract void AbreCupom(string cpf, string nome, string endereco);

        public void VendeItem(string codigo, string descricao, string aliquota, decimal quantidade,
            decimal valorUnitario,
            string unidade)
        {
            VendeItem(codigo, descricao, aliquota, quantidade, valorUnitario, unidade, 0);
        }

        public abstract void VendeItem(string codigo, string descricao, string aliquota, decimal quantidade,
            decimal valorUnitario,
            string unidade, decimal descontoPorcentagem);

        public abstract void CancelarCupom();

        public abstract void CancelarItem(int numero);

        public void SubtotalizaCupom()
        {
            SubtotalizaCupom(0);
        }

        public abstract void SubtotalizaCupom(decimal descontoAcrescimo);

        public abstract void EfetuaPagamento(string codigo, decimal valor);

        public void FechaCupom(string nome, string cpfCnpj, string endereco)
        {
            FechaCupom(nome, cpfCnpj, endereco, null);
        }

        public abstract void FechaCupom(string nome, string cpfCnpj, string endereco, string observacao);

        public void FechaCupom()
        {
            FechaCupom(NomeCliente, DocumentoCliente, EnderecoCliente, null);
        }

        public abstract void FechaCupom(string observacao);

        public abstract void Ibtp(string fonte, decimal valorAproximadoFederal, decimal valorAproximadoMunicipal, decimal valorAproximadoEstadual);

        public abstract void IdentificaConsumidor(string nome, string cpfCnpj, string endereco);

        public abstract void Desativar();

        public abstract void LinhaRelatorioGerencial(string[] imagemComprovante);

        public abstract void LinhaCupomVinculado(string[] imagemComprovante);

        public abstract void AbreCupomVinculado(string coo, string indiceEcf, decimal valor);

        public abstract void AbreRelatorioGerencial(int indice = 0);

        public abstract void PulaLinhas(int linhasEntreCupons);

        public abstract void CortaPapel(bool corta);

        public abstract void FechaRelatorio();

        public abstract void CorrigeEstadoErro(bool reducaoZ = false);

        public abstract void PafEspelhoMfd(DateTime dataInicial, DateTime dataFinal, string caminho);

        public abstract void AbreGaveta();

        public abstract void Close();
    }
}
