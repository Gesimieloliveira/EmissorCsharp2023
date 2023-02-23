using System;
using DFe.Classes.Entidades;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.ChaveEletronica
{
    public static class ChaveSefazHelper
    {
        public static ChaveSefaz GerarChave(IChaveComponentes componentes)
        {
            var mod = componentes.GetModelo().ToZeus();
            var serie = componentes.GetSerie();
            var codigoUf = (Estado) componentes.GetCodigoUf();
            var numeroDocumentoFiscal = componentes.GetNumeroDocumento();
            var codigoNumerico = int.Parse(componentes.GetCodigoNumerico());
            var tipoEmissao = (int) componentes.GetTipoEmissao();
            var cnpj = componentes.GetDocumentoUnico();
            var dhEmissao = componentes.GetDhEmissao();

            var chave = ChaveFiscal.ObterChave(codigoUf,
                dhEmissao,
                cnpj,
                mod,
                serie,
                numeroDocumentoFiscal,
                tipoEmissao,
                codigoNumerico);

            return new ChaveSefaz(chave.Chave);
        }

        public static int GeraDigitoVerificador(string chave)
        {
            if (chave?.Length != 43)
                throw new InvalidOperationException(
                    "Erro ao gerar Dígito Verificador da chave. Tamanho da chave é inválida");

            var soma = 0; // Vai guardar a Soma
            var mod = -1; // Vai guardar o Resto da divisão
            var pesso = 2; // vai guardar o pesso de multiplicacao

            //percorrendo cada caracter da chave da direita para esquerda para fazer os calculos com o pesso
            for (var i = chave.Length - 1; i != -1; i--)
            {
                var ch = Convert.ToInt32(chave[i].ToString());
                soma += ch*pesso;
                //sempre que for 9 voltamos o pesso a 2
                if (pesso < 9)
                    pesso += 1;
                else
                    pesso = 2;
            }

            mod = soma%11;

            if (mod == 0 || mod == 1)
                return 0;

            return 11 - mod;
        }

        public static ModeloDocumento ExtrairModelo(string chave)
        {
            if (chave?.Length != 44)
            {
                throw new InvalidOperationException("Chave informado muito curta. Esperado tamanho 44!");
            }

            var posicao = chave.Substring(20, 2);

            if (Enum.TryParse(posicao, out ModeloDocumento modelo) == false)
            {
                throw new InvalidOperationException("Não consegui identificar o modelo do documento na Chave informada!");
            }

            return modelo;
        }
    }
}