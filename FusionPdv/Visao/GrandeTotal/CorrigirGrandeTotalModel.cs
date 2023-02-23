using System;
using System.Globalization;
using FusionPdv.Ecf;
using FusionPdv.Servicos.ArquivoAuxiliar;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionPdv.Servicos.ValidacaoInicial;

namespace FusionPdv.Visao.GrandeTotal
{
    public class CorrigirGrandeTotalModel : ModelBase
    {
        private decimal _grandeTotalEcf;

        private decimal _grandeTotalArquivoAuxiliar;

        private string _codigoAutorizacao;

        public CorrigirGrandeTotalModel()
        {
            _grandeTotalEcf = SessaoEcf.EcfFiscal.GrandeTotal();
            _grandeTotalArquivoAuxiliar = decimal.Parse(new BuscarGt().Executar());
        }

        public decimal GrandeTotalEcf
        {
            get { return _grandeTotalEcf; }
            set
            {
                _grandeTotalEcf = value;
                PropriedadeAlterada();
            }
        }

        public decimal GrandeTotalArquivoAuxiliar
        {
            get { return _grandeTotalArquivoAuxiliar; }
            set
            {
                _grandeTotalArquivoAuxiliar = value;
                PropriedadeAlterada();
            }
        }

        public string CodigoAutorizacao
        {
            get { return _codigoAutorizacao; }
            set
            {
                _codigoAutorizacao = value; 
                PropriedadeAlterada();
            }
        }

        public void CorrigirGt()
        {
            Validar();
            try
            {
                new AtualizarGt(SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture)).Executar();
            }
            catch (ExceptionGtEcf ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (ArquivoAuxiliarInvalidoException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            
        }

        private void Validar()
        {
            var codigoAutorizacao = CodigoAutorizacao;

            if (string.IsNullOrEmpty(codigoAutorizacao)) throw new InvalidOperationException("Porfavor digitar um código de autorização.");

            var grandeTotalEcfSha1 = Sha1Helper.Computar(GrandeTotalEcf.ToString("N2")).ToUpper();

            codigoAutorizacao = codigoAutorizacao.Trim();

            if (!codigoAutorizacao.Equals(grandeTotalEcfSha1))
            {
                throw new InvalidOperationException("Código de autorização está inválido.");
            }
        }

        public void GerarCodigoAtorizacao()
        {
            CodigoAutorizacao = Sha1Helper.Computar(_grandeTotalEcf.ToString("N2")).ToUpper();
        }
    }
}
