using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Localidade;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionLibrary.VisaoModel;
using System;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models
{
    public class ExportacaoModel : ModelBase
    {
        private string _ufSaidaPais;
        private string _localEmbarque;
        private string _localDespacho;
        private short? _codigoBacen;

        public string UfSaidaPais
        {
            get { return _ufSaidaPais; }
            set
            {
                if (value == _ufSaidaPais) return;
                _ufSaidaPais = value;
                PropriedadeAlterada();
            }
        }

        public string LocalEmbarque
        {
            get { return _localEmbarque; }
            set
            {
                if (value == _localEmbarque) return;
                _localEmbarque = value;
                PropriedadeAlterada();
            }
        }

        public string LocalDespacho
        {
            get { return _localDespacho; }
            set
            {
                if (value == _localDespacho) return;
                _localDespacho = value;
                PropriedadeAlterada();
            }
        }

        public short? CodigoBacen
        {
            get { return _codigoBacen; }
            set
            {
                if (value == _codigoBacen) return;
                _codigoBacen = value;
                PropriedadeAlterada();
            }
        }

        public void PreecherCom(Nfeletronica nfe)
        {
            UfSaidaPais = nfe.Exportacao?.UfSaidaPais;
            LocalDespacho = nfe.Exportacao?.LocalDespacho;
            LocalEmbarque = nfe.Exportacao?.LocalEmbarque;

            if (nfe.Destinatario.ResideExterior())
                CodigoBacen = nfe.Destinatario.Endereco.Localizacao.CodigoPais;
        }

        public bool NenhumCampoExportacaoInformado() => 
            string.IsNullOrWhiteSpace(UfSaidaPais) && 
            string.IsNullOrWhiteSpace(LocalEmbarque) && 
            string.IsNullOrWhiteSpace(LocalDespacho);

        public Pais ObterPaisDestino()
        {
            var pais = LocalidadesServico.GetInstancia().GetPais(p => p.Bacen == CodigoBacen);

            if (pais == null)
            {
                throw new ArgumentException("País do destinatário não informado!");
            }

            return pais;
        }
    }
}