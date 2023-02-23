using System;
using System.Linq;
using System.Text.RegularExpressions;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using static System.Text.RegularExpressions.RegexOptions;
using ZeusNfe = NFe.Classes.NFe;

namespace FusionCore.FusionAdm.Fiscal.NF.Integridade
{
    public class DeOlhoNoImposto
    {
        private readonly ISessaoManager _sessaoManager;
        private decimal _totalEstadual;
        private decimal _totalFederal;
        private decimal _total;
        private ZeusNfe _nfe;

        public DeOlhoNoImposto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public void SetarNfe(ZeusNfe zeusNfe)
        {
            _nfe = zeusNfe;
        }

        public ZeusNfe GetNfe()
        {
            return _nfe;
        }

        public void IncluirTributosAproximados()
        {
            if (_nfe == null)
            {
                throw new InvalidOperationException("Nenhuma nf-e informada para calcular os impostos aproximados (IBPT)");
            }

            _totalEstadual = 0.00M;
            _totalFederal = 0.00M;
            _total = 0.00M;

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioIbpt(sessao);

                _nfe.infNFe.det.ForEach(i =>
                {
                    var ibpt = repositorio.GetPeloNcm(i.prod.NCM);

                    if (ibpt == null)
                    {
                        i.imposto.vTotTrib = 0;
                        return;
                    }

                    var bc = new BaseCalculo(i.prod.vProd);

                    var federal = ibpt.ImpostoFederalAproximado(bc);
                    var estadual = ibpt.ImpostoEstadualAproximado(bc);
                    var total = federal + estadual;

                    i.imposto.vTotTrib = total;

                    _totalEstadual += estadual;
                    _totalFederal += federal;
                    _total += total;
                });
            }

            _nfe.infNFe.total.ICMSTot.vTotTrib = _total;

            _nfe.infNFe.infAdic.infCpl += " ;" + ComputaObs(_nfe).ToUpper();
            _nfe.infNFe.infAdic.infCpl = new Regex(@"^( ;)", IgnoreCase).Replace(_nfe.infNFe.infAdic.infCpl, "");
        }

        private string ComputaObs(ZeusNfe nfe)
        {
            var valorRealProdutos = nfe.infNFe.det.Sum(d => d.prod.vProd - (d.imposto.vTotTrib ?? 0.00M));

            return "Você pagou aproximadamente:" +
                   $" {_totalFederal:C} de tributos federais," +
                   $" {_totalEstadual:C} de tributos estaduais e" +
                   $" {valorRealProdutos:C} pelos produtos." +
                   " Fonte: IBPT";
        }
    }
}