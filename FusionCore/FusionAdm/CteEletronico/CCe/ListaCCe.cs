using System.Collections.Generic;

namespace FusionCore.FusionAdm.CteEletronico.CCe
{
    public static class ListaCCe
    {
        public static List<ElementoCCe> ElementosCCes { get; set; }

        static ListaCCe()
        {
            var ide = ElementoCCe.Cria("ide", "Identificação do CT-e");
            var toma4 = ElementoCCe.Cria("toma4", "Indicador do \"papel\" do tomador do serviço no CT-e");
            var enderToma = ElementoCCe.Cria("enderToma", "Tomador Dados do Endereço");
            var compl = ElementoCCe.Cria("compl", "Dados complementares do CT-e para fins operacionais ou comerciais");
            var fluxo = ElementoCCe.Cria("fluxo", "Previsão do fluxo da carga");
            var pass = ElementoCCe.Cria("pass", "");
            var semData = ElementoCCe.Cria("semData", "Entrega sem data definida");
            var comData = ElementoCCe.Cria("comData", "Entrega com data definida");
            var noPeriodo = ElementoCCe.Cria("noPeriodo", "Entrega no período definido");
            var semHora = ElementoCCe.Cria("semHora", "Entrega sem hora definida");
            var comHora = ElementoCCe.Cria("comHora", "Entrega com hora definida");
            var noInter = ElementoCCe.Cria("noInter", "Entrega no intervalo de horário definido");
            var obsCont = ElementoCCe.Cria("ObsCont", "Campo de uso livre do contribuinte");
            var obsFisco = ElementoCCe.Cria("ObsFisco", "Campo de uso livre do contribuinte");
            var emit = ElementoCCe.Cria("emit", "Identificação do Emitente do CT-e");
            var enderEmit = ElementoCCe.Cria("enderEmit", "Endereço do emitente");
            var rem = ElementoCCe.Cria("rem", "Informações do Remetente das mercadorias transportadas pelo CT-e");
            var enderReme = ElementoCCe.Cria("enderReme", "Dados do endereço do Remetente");
            var locColeta = ElementoCCe.Cria("locColeta", "Local da Coleta Remetente");
            var exped = ElementoCCe.Cria("exped", "Informações do Expedidor da Carga");
            var enderExped = ElementoCCe.Cria("enderExped", "Dados do endereço do Expedidor");
            var receb = ElementoCCe.Cria("receb", "Informações do Recebedor da Carga");
            var enderReceb = ElementoCCe.Cria("enderReceb", "Dados do Endereço do Recebedor");
            var dest = ElementoCCe.Cria("dest", "Informações do Destinatário do CT-e");
            var enderDest = ElementoCCe.Cria("enderDest", "Dados do endereço do Destinatário");
            var locEnt = ElementoCCe.Cria("locEnt", "Local de Entrega constante na Nota Fiscal Destinatário");
            var vPrest = ElementoCCe.Cria("vPrest", "Valores da prestação de Serviço");
            var comp = ElementoCCe.Cria("comp", "Componentes do Valor da Prestação");
            var imp = ElementoCCe.Cria("ICMS", "Informações relativas aos Impostos");
            var infCarga = ElementoCCe.Cria("infCarga", "Informações da Carga do CT-e");
            var infQ = ElementoCCe.Cria("infQ", "Informações de quantidades da Carga do CT-e");
            var infNf = ElementoCCe.Cria("infNf", "Informações das NF");
            var infNFe = ElementoCCe.Cria("infNFe", "informações das NF-e");
            var infOutros = ElementoCCe.Cria("infOutros", "Informações dos demais documentos");
            var seg = ElementoCCe.Cria("seg", "Informações de Seguro da Carga");
            var infModal = ElementoCCe.Cria("infModal", "Informações do modal");
            var peri = ElementoCCe.Cria("peri", "Preenchido quando for transporte de produtos classificados pela ONU como perigosos");
            var veicNovos = ElementoCCe.Cria("veicNovos", "Informações dos veículos transportados");

            ElementosCCes = new List<ElementoCCe>
            {
                ElementoCCe.Cria("CFOP", "Código Fiscal de Operações e Prestações", ide),
                ElementoCCe.Cria("natOp", "Natureza da Operação", ide),
                ElementoCCe.Cria("forPag", "Forma de Pagamento do serviço", ide),
                ElementoCCe.Cria("tpImp", "Formato de impressão do DACTE", ide),
                ElementoCCe.Cria("tpCTe", "Tipo do CT-e", ide),
                ElementoCCe.Cria("procEmi", "Identificador do processo de emissão do CT-e", ide),
                ElementoCCe.Cria("verProc", "Versão do processo de emissão", ide),
                ElementoCCe.Cria("refCTE", "Chave de acesso do CT-e referenciado", ide),
                ElementoCCe.Cria("cMunEnv", "Código do Município de envio do CT-e (de onde o documento foi transmitido)", ide),
                ElementoCCe.Cria("xMunEnv", "Nome do Município de envio do CT-e (de onde o documento foi transmitido)", ide),
                ElementoCCe.Cria("UFEnv", "Sigla da UF de envio do CT-e (de onde o documento foi transmitido)", ide),
                ElementoCCe.Cria("tpServ", "Tipo do Serviço", ide),
                ElementoCCe.Cria("cMunIni", "Código do Município de início da prestação", ide),
                ElementoCCe.Cria("xMunIni", "Nome do Município do início da prestação", ide),
                ElementoCCe.Cria("UFIni", "UF do início da prestação", ide),
                ElementoCCe.Cria("cMunFim", "Código do Município de término da prestação", ide),
                ElementoCCe.Cria("xMunFim", "Nome do Município do término da prestação", ide),
                ElementoCCe.Cria("UFFim", "UF do término da prestação", ide),
                ElementoCCe.Cria("retira", "Indicador se o Recebedor retira no Aeroporto, Filial, Porto ou Estação de Destino?", ide),
                ElementoCCe.Cria("xDetRetira", "Detalhes do retira", ide),

                ElementoCCe.Cria("xNome", "Razão Social ou Nome", toma4),
                ElementoCCe.Cria("xFant", "Nome Fantasia", toma4),
                ElementoCCe.Cria("fone", "Telefone", toma4),
                ElementoCCe.Cria("email", "Endereço de email", toma4),

                ElementoCCe.Cria("xLgr", "Logradouro", enderToma),
                ElementoCCe.Cria("nro", "Número", enderToma),
                ElementoCCe.Cria("xCpl", "Complemento", enderToma),
                ElementoCCe.Cria("xBairro", "Bairro", enderToma),
                ElementoCCe.Cria("cMun", "Código do município (utilizar a tabela do IBGE)", enderToma),
                ElementoCCe.Cria("xMun", "Nome do município", enderToma),
                ElementoCCe.Cria("CEP", "CEP", enderToma),
                ElementoCCe.Cria("UF", "Sigla da UF", enderToma),
                ElementoCCe.Cria("cPais", "Código do país", enderToma),
                ElementoCCe.Cria("xPais", "Nome do país", enderToma),

                ElementoCCe.Cria("dhCont", "Data e Hora da entrada em contingência", ide),
                ElementoCCe.Cria("xJust", "Justificativa da entrada em contingência", ide),

                ElementoCCe.Cria("xCaracAd", "Característica adicional do transporte", compl),
                ElementoCCe.Cria("xCaracSer", "Caraterística adicional do serviço", compl),
                ElementoCCe.Cria("xEmi", "Funcionário emissor do CTe", compl),

                ElementoCCe.Cria("xOrig", "Sigla ou código interno da Filial/Porto/Estação/Aeroporto de Origem", fluxo),

                ElementoCCe.Cria("xPass", "Sigla ou código interno da Filial/Porto/Estação/Aeroporto de Passagem", pass),

                ElementoCCe.Cria("xDest", "Sigla ou código interno da Filial/Porto/Estação/Aeroporto de Destino", fluxo),
                ElementoCCe.Cria("xRota", "Código da Rota de Entrega", fluxo),

                ElementoCCe.Cria("tpPer", "Tipo de data/período programado para entrega", semData),

                ElementoCCe.Cria("tpPer", "Tipo de data/período programado para entrega", comData),
                ElementoCCe.Cria("dProg", "Data programada", comData),

                ElementoCCe.Cria("tpPer", "Tipo período", noPeriodo),
                ElementoCCe.Cria("dIni", "Data inicial", noPeriodo),
                ElementoCCe.Cria("dFim", "Data final", noPeriodo),

                ElementoCCe.Cria("tpHor", "Tipo de hora", semHora),

                ElementoCCe.Cria("tpHor", "Tipo de hora", comHora),
                ElementoCCe.Cria("hProg", "Hora programada", comHora),

                ElementoCCe.Cria("tpHour", "Tipo de hora", noInter),
                ElementoCCe.Cria("hIni", "Hora Inicial", noInter),
                ElementoCCe.Cria("hFim", "Hora final", noInter),

                ElementoCCe.Cria("origCalc", "Município de origem para efeito de cálculo do frete", compl),
                ElementoCCe.Cria("destCalc", "Município de destino para efeito de cálculo do frete", compl),
                ElementoCCe.Cria("xObs", "Observações Gerais", compl),

                ElementoCCe.Cria("xCampo", "Identificação do campo", obsCont),
                ElementoCCe.Cria("xTexto", "Conteúdo do campo", obsCont),

                ElementoCCe.Cria("xCampo", "Identificação do campo", obsFisco),
                ElementoCCe.Cria("xTexto", "Conteúdo do campo", obsFisco),

                ElementoCCe.Cria("xNome", "Razão social ou Nome do emitente", emit),
                ElementoCCe.Cria("xFant", "Nome fantasia", emit),

                ElementoCCe.Cria("xLgr", "Logradouro", enderEmit),
                ElementoCCe.Cria("nro", "Número", enderEmit),
                ElementoCCe.Cria("xCpl", "Complemento", enderEmit),
                ElementoCCe.Cria("xBairro", "Bairro", enderEmit),
                ElementoCCe.Cria("cMun", "Código do município (utilizar a tabela do IBGE)", enderEmit),
                ElementoCCe.Cria("xMun", "Nome do município", enderEmit),
                ElementoCCe.Cria("CEP", "CEP", enderEmit),
                ElementoCCe.Cria("UF", "Sigla da UF", enderEmit),
                ElementoCCe.Cria("fone", "Telefone", enderEmit),

                ElementoCCe.Cria("xNome", "Razão social ou nome do remetente", rem),
                ElementoCCe.Cria("xFant", "Nome fantasia", rem),
                ElementoCCe.Cria("fone", "Telefone", rem),
                ElementoCCe.Cria("email", "Endereço de email", rem),

                ElementoCCe.Cria("xLgr", "Logradouro", enderReme),
                ElementoCCe.Cria("nro", "Número", enderReme),
                ElementoCCe.Cria("xCpl", "Complemento", enderReme),
                ElementoCCe.Cria("xBairro", "Bairro", enderReme),
                ElementoCCe.Cria("cMun", "Código do município (utilizar a tabela do IBGE)", enderReme),
                ElementoCCe.Cria("xMun", "Nome do município", enderReme),
                ElementoCCe.Cria("CEP", "CEP", enderReme),
                ElementoCCe.Cria("UF", "Sigla da UF", enderReme),
                ElementoCCe.Cria("cPais", "Código do país", enderReme),
                ElementoCCe.Cria("xPais", "Nome do país", enderReme),

                ElementoCCe.Cria("CNPJ", "Número do CNPJ", locColeta),
                ElementoCCe.Cria("CPF", "Número do CPF", locColeta),
                ElementoCCe.Cria("xNome", "Razão Social ou Nome", locColeta),
                ElementoCCe.Cria("xLgr", "Logradouro", locColeta),
                ElementoCCe.Cria("nro", "Número", locColeta),
                ElementoCCe.Cria("xCpl", "Complemento", locColeta),
                ElementoCCe.Cria("xBairro", "Bairro", locColeta),
                ElementoCCe.Cria("cMun", "Código do município (utilizar a tabela do IBGE)", locColeta),
                ElementoCCe.Cria("xMun", "Nome do município", locColeta),
                ElementoCCe.Cria("UF", "Sigla da UF", locColeta),

                ElementoCCe.Cria("CNPJ", "Número do CNPJ", exped),
                ElementoCCe.Cria("CPF", "Número do CPF", exped),
                ElementoCCe.Cria("IE", "Inscrição Estadual", exped),
                ElementoCCe.Cria("xNome", "Razão Social ou Nome", exped),
                ElementoCCe.Cria("fone", "Telefone", exped),
                ElementoCCe.Cria("email", "Endereço de email", exped),

                ElementoCCe.Cria("xLgr", "Logradouro", enderExped),
                ElementoCCe.Cria("nro", "Número", enderExped),
                ElementoCCe.Cria("xCpl", "Complemento", enderExped),
                ElementoCCe.Cria("xBairro", "Bairro", enderExped),
                ElementoCCe.Cria("cMun", "Código do município (utilizar a tabela do IBGE)", enderExped),
                ElementoCCe.Cria("xMun", "Nome do município", enderExped),
                ElementoCCe.Cria("CEP", "CEP", enderExped),
                ElementoCCe.Cria("UF", "Sigla da UF", enderExped),
                ElementoCCe.Cria("cPais", "Código do país", enderExped),
                ElementoCCe.Cria("xPais", "Nome do país", enderExped),

                ElementoCCe.Cria("CNPJ", "Número do CNPJ", receb),
                ElementoCCe.Cria("CPF", "Número do CPF", receb),
                ElementoCCe.Cria("IE", "Iscrição Estadual", receb),
                ElementoCCe.Cria("xNome", "Razão Social ou Nome", receb),
                ElementoCCe.Cria("fone", "Telefone", receb),
                ElementoCCe.Cria("email", "Endereço de email", enderReceb),

                ElementoCCe.Cria("xLgr", "Logradouro", enderReceb),
                ElementoCCe.Cria("nro", "Número", enderReceb),
                ElementoCCe.Cria("xCpl", "Complemento", enderReceb),
                ElementoCCe.Cria("xBairro", "Bairro", enderReceb),
                ElementoCCe.Cria("cMun", "Código do município (utilizar a tabela do IBGE)", enderReceb),
                ElementoCCe.Cria("xMun", "Nome do município", enderReceb),
                ElementoCCe.Cria("CEP", "CEP", enderReceb),
                ElementoCCe.Cria("UF", "Sigla UF", enderReceb),
                ElementoCCe.Cria("cPais", "Código do país", enderReceb),
                ElementoCCe.Cria("xPais", "Nome do país", enderReceb),
                
                ElementoCCe.Cria("xNome", "Razão Social ou Nome do destinatário", dest),
                ElementoCCe.Cria("fone", "Telefone", dest),
                ElementoCCe.Cria("ISUF", "Inscrição na SUFRAMA", dest),
                ElementoCCe.Cria("email", "Endereço de email", dest),

                ElementoCCe.Cria("xLgr", "Logradouro", enderDest),
                ElementoCCe.Cria("nro", "Número", enderDest),
                ElementoCCe.Cria("xCpl", "Complemento", enderDest),
                ElementoCCe.Cria("xBairro", "Bairro", enderDest),
                ElementoCCe.Cria("cMun", "Código do município (utilizar a tabela do IBGE)", enderDest),
                ElementoCCe.Cria("xMun", "Nome do município", enderDest),
                ElementoCCe.Cria("CEP", "CEP", enderDest),
                ElementoCCe.Cria("UF", "Sigla da UF", enderDest),
                ElementoCCe.Cria("cPais", "Código do país", enderDest),
                ElementoCCe.Cria("xPais", "Nome do país", enderDest),

                ElementoCCe.Cria("CNPJ", "Número do CNPJ", locEnt),
                ElementoCCe.Cria("CPF", "Número do CPF", locEnt),
                ElementoCCe.Cria("xNome", "Razão Social ou Nome", locEnt),
                ElementoCCe.Cria("xLgr", "Logradouro", locEnt),
                ElementoCCe.Cria("nro", "Número", locEnt),
                ElementoCCe.Cria("xCpl", "Complemento", locEnt),
                ElementoCCe.Cria("xBairro", "Bairro", locEnt),
                ElementoCCe.Cria("cMun", "Código do município (utilizar tabela do IBGE)", locEnt),
                ElementoCCe.Cria("xMun", "Nome do município", locEnt),
                ElementoCCe.Cria("UF", "Sigla da UF", locEnt),

                ElementoCCe.Cria("vRec", "Valor a Receber", vPrest),

                ElementoCCe.Cria("xNome", "Nome do componente", comp),

                ElementoCCe.Cria("vTotTrib", "Valor de tributos federais, estaduais e municipais", imp),
                ElementoCCe.Cria("infAdFisco", "Informações adicionais de interesse do Fisco", imp),

                ElementoCCe.Cria("vCagar", "Valor total da carga", infCarga),
                ElementoCCe.Cria("prodPred", "Produto predominante", infCarga),
                ElementoCCe.Cria("xOutCat", "Outras características da carga", infCarga),

                ElementoCCe.Cria("cUnid", "Código da Unidade de Medida", infQ),
                ElementoCCe.Cria("tpMed", "Tipo da Medida", infQ),
                ElementoCCe.Cria("qCarga", "Quantidade", infQ),

                ElementoCCe.Cria("nRoma", "Número do Romaneio da NF", infNf),
                ElementoCCe.Cria("nPed", "Número do Pedido da NF", infNf),
                ElementoCCe.Cria("mod", "Modelo da Nota Fiscal", infNf),
                ElementoCCe.Cria("serie", "Série", infNf),
                ElementoCCe.Cria("nDoc", "Número", infNf),
                ElementoCCe.Cria("dEmi", "Data de Emissão", infNf),
                ElementoCCe.Cria("vBC", "Data de Emissão", infNf),
                ElementoCCe.Cria("vICMS", "Valor Total do ICMS", infNf),
                ElementoCCe.Cria("vBCST", "Valor da Base de Cálculo do ICMS ST", infNf),
                ElementoCCe.Cria("vST", "Valor Total do ICMS ST", infNf),
                ElementoCCe.Cria("vProd", "Valor Total dos Produtos", infNf),
                ElementoCCe.Cria("vNF", "Valor Total da NF", infNf),
                ElementoCCe.Cria("nCFOP", "CFOP Predominante", infNf),
                ElementoCCe.Cria("nPeso", "Peso total em Kg", infNf),
                ElementoCCe.Cria("PIN", "PIN SUFRAMA", infNf),
                ElementoCCe.Cria("dPrev", "Data prevista de entrega", infNf),

                ElementoCCe.Cria("PIN", "PIN SUFRAMA", infNFe),
                ElementoCCe.Cria("dPrev", "Data prevista de entrega", infNFe),

                ElementoCCe.Cria("tpDoc", "Tipo de documento originário", infOutros),
                ElementoCCe.Cria("descOutros", "Descrição quando se tratar de 99-Outros", infOutros),
                ElementoCCe.Cria("nDoc", "Número", infOutros),
                ElementoCCe.Cria("dEmi", "Data de Emissão", infOutros),
                ElementoCCe.Cria("vDocFisc", "Valor do documento", infOutros),
                ElementoCCe.Cria("dPrev", "Data prevista de entrega", infOutros),

                ElementoCCe.Cria("respSeg", "Responsável pelo seguro", seg),
                ElementoCCe.Cria("xSeg", "Nome da Seguradora", seg),
                ElementoCCe.Cria("nApol", "Número da Apólice", seg),
                ElementoCCe.Cria("nAver", "Número da Averbação", seg),
                ElementoCCe.Cria("vCarga", "Valor da Carga para efeito de averbação", seg),

                ElementoCCe.Cria("versaoModal", "Versão do leiaute específico para o Modal", infModal),

                ElementoCCe.Cria("nONU", "Número ONU/UN", peri),
                ElementoCCe.Cria("xNomeAE", "Nome apropriado para embarque do produto", peri),
                ElementoCCe.Cria("xClaRisco", "Classe ou subclasse/divisão, e risco subsidiário/risco secundário", peri),
                ElementoCCe.Cria("grEmb", "Grupo de Embalagem", peri),
                ElementoCCe.Cria("qTotProd", "Quantidade total por produto", peri),
                ElementoCCe.Cria("qVolTipo", "Quantidade e Tipo de volumes", peri),
                ElementoCCe.Cria("pontoFulgor", "Ponto de Fulgor", peri),

                ElementoCCe.Cria("chassi", "Chassi do veículo", veicNovos),
                ElementoCCe.Cria("cCor", "Cor do veículo", veicNovos),
                ElementoCCe.Cria("xCor", "Descrição da cor", veicNovos),
                ElementoCCe.Cria("cMod", "Código Marca Modelo", veicNovos),
                ElementoCCe.Cria("vUnit", "Valor Unitário do Veículo", veicNovos),
                ElementoCCe.Cria("vFrete", "Frete Unitário", veicNovos),
            };
        }
    }
}