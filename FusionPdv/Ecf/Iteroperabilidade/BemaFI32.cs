using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FiscalPrinterBematech
{

	public class BemaFI32
	{	


		#region Fun��es de tratamento de erro
		
			/// <summary>
		/// Fun��o para analizar os retorno da impressora (ST1 e ST2).
		/// </summary>
			public static void Analisa_RetornoImpressora()
			{
				int ACK,ST1,ST2, ST3;
				string Erros = "";
				ACK = ST1 = ST2 = ST3 =0;

				Bematech_FI_RetornoImpressoraMFD(ref ACK,ref ST1,ref ST2, ref ST3);

				#region Tratando o ST1
				if ( ST1 >= 128)
				{
					ST1 = ST1 - 128;
					Erros += "BIT 7 - Fim de Papel" + '\x0D';				
				}
				if ( ST1 >= 64)
				{
					ST1 = ST1 - 64;
					Erros += "BIT 6 - Pouco Papel" + '\x0D';
				}
				if ( ST1 >= 32)
				{
					ST1 = ST1 - 32;
					Erros += "BIT 5 - Erro no Rel�gio" + '\x0D';
				}
				if ( ST1 >= 16)
				{
					ST1 = ST1 - 16;
					Erros += "BIT 4 - Impressora em ERRO" + '\x0D';
				}
				if ( ST1 >= 8)
				{
					ST1 = ST1 - 8;
					Erros += "BIT 3 - CMD n�o iniciado com ESC" + '\x0D';
				}
				if ( ST1 >= 4)
				{
					ST1 = ST1 - 4;
					Erros += "BIT 2 - Comando Inexistente" + '\x0D';
				}
				if ( ST1 >= 2)
				{
					ST1 = ST1 - 2;
					Erros += "BIT 1 - Cupom Aberto" + '\x0D';
				}
				if ( ST1 >= 1)
				{
					ST1 = ST1 - 1;
					Erros += "BIT 0 - N� de Par�metros Inv�lidos" + '\x0D';
				}
				#endregion

				#region Tratando o ST2
				if ( ST2 >= 128)
				{
					ST2 = ST2 - 128;
					Erros += "BIT 7 - Tipo de Par�metro Inv�lido" + '\x0D';
				}
				if ( ST2 >= 64)
				{
					ST2 = ST2 - 64;
					Erros += "BIT 6 - Mem�ria Fiscal Lotada" + '\x0D';
				}
				if ( ST2 >= 32)
				{
					ST2 = ST2 - 32;
					Erros += "BIT 5 - CMOS n�o Vol�til" + '\x0D';
				}
				if ( ST2 >= 16)
				{
					ST2 = ST2 - 16;
					Erros += "BIT 4 - Al�quota N�o Programada" + '\x0D';
				}
				if ( ST2 >= 8)
				{
					ST2 = ST2 - 8;
					Erros += "BIT 3 - Al�quotas lotadas" + '\x0D';
				}
				if ( ST2 >= 4)
				{
					ST2 = ST2 - 4;
					Erros += "BIT 2 - Cancelamento � Permitido" + '\x0D';
				}
				if ( ST2 >= 2)
				{
					ST2 = ST2 - 2;
					Erros += "BIT 1 - CGC/IE n�o Programados" + '\x0D';
				}
				if ( ST2 >= 1)
				{
					ST2 = ST2 - 1;
					Erros += "BIT 0 - Comando n�o Executado" + '\x0D';
				}

            #endregion

                #region Tratando o ST3

                #endregion

            if (Erros.Length != 0)
					System.Windows.Forms.MessageBox.Show(Erros,"Erro na Execu��o do Comando",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}

			/// <summary>
		/// Fun��o que analiza o retorno da fun��o.
		/// </summary>
		/// <param name="IRetorno">Inteiro com o valor a ser analizado.</param>
			public static void Analisa_iRetorno(int IRetorno)
			{
				string MSG = "";
				string MSGCaption = "Aten��o";
				MessageBoxIcon MSGIco = MessageBoxIcon.Information;
                
				switch(IRetorno)
				{
					case  0: 
						MSG = "Erro de Comunica��o !";
						MSGCaption = "Erro";
						MSGIco = MessageBoxIcon.Error;
						break;
					case -1: 
						MSG = "Erro de Execu��o na Fun��o. Verifique!";
						MSGCaption = "Erro";
						MSGIco = MessageBoxIcon.Error;
						break;
					case -2: 
						MSG = "Par�metro Inv�lido !";
						MSGCaption = "Erro";
						MSGIco = MessageBoxIcon.Error;
						break;
					case -3: 
						MSG = "Al�quota n�o programada !";
						break;
					case -4: 
						MSG = "Arquivo BemaFI32.INI n�o encontrado. Verifique!";
						break;
					case -5: 
						MSG = "Erro ao Abrir a Porta de Comunica��o";
						MSGCaption = "Erro";
						MSGIco = MessageBoxIcon.Error;
						break;
					case -6: 
						MSG = "Impressora Desligada ou Desconectada.";
						break;
					case -7: 
						MSG = "Banco N�o Cadastrado no Arquivo BemaFI32.ini";
						break;
					case -8: 
						MSG = "Erro ao Criar ou Gravar no Arquivo Retorno.txt ou Status.txt.";
						MSGCaption = "Erro";
						MSGIco = MessageBoxIcon.Error;
						break;
					case -18: 
						MSG = "N�o foi poss�vel abrir arquivo INTPOS.001!";
						break;
					case -19: 
						MSG = "Par�metros diferentes!";
						break;
					case -20: 
						MSG = "Transa��o cancelada pelo Operador!";
						break;
					case -21: 
						MSG = "A Transa��o n�o foi aprovada!";
						break;
					case -22: 
						MSG = "N�o foi poss�vel terminar a Impress�o!";
						break;
					case -23: MSG = "N�o foi poss�vel terminar a Opera��o!";
						break;
					case -24: MSG = "N�o foi poss�vel terminal a Opera��o!";
						break;
					case -25: MSG = "Totalizador n�o fiscal n�o programado.";
						break;
					case -26: MSG = "Transa��o j� Efetuada!";
						break;
					case -27: Analisa_RetornoImpressora();
						break;
					case -28: MSG = "N�o h� Informa��es para serem Impressas!";
						break;
				}
				if (MSG.Length != 0)
					System.Windows.Forms.MessageBox.Show(MSG,MSGCaption,MessageBoxButtons.OK,MSGIco);

			}

		
		#endregion

		#region IMPORT DAS FUN��ES DA BemaFI32.dll
		/*
		 ===============================================================================
			********************************************************************************

								DECLARA��O DAS FUN��ES DA BemaFI32.dll
  
			********************************************************************************
		 ===============================================================================
		*/

	    [DllImport("BemaFI32.dll")]
	    public static extern int Bematech_FI_ModeloImpressora(string modelo);
        #region Fun��es de Inicializa��o
        /// <summary>
        /// Altera o s�mbolo da moeda programada na Impressora Fiscal. 
        /// </summary>
        /// <param name="SimboloMoeda">STRING contendo o s�mbolo da moeda. O $ (cifr�o) � inserido automaticamente.</param>
        /// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
        [DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AlteraSimboloMoeda(string SimboloMoeda );
		/// <summary>
		/// Programa al�quota tribut�ria na Impressora Fiscal. 
		/// </summary>
		/// <param name="Aliquota">STRING com o valor da al�quota a ser programada</param>
		/// <param name="ICMS_ISS">INTEIRO com o valor 0 (zero) para vincular a al�quota ao ICMS e 1 (um) para vincular ao ISS</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ProgramaAliquota(string Aliquota, int ICMS_ISS);
		/// <summary>
		/// Programa departamento na impressora.
		/// </summary>
		/// <param name="Indice">INTEIRO com a posi��o em que o Departamento ser� cadastrado. </param>
		/// <param name="Departamento">STRING com at� 10 caracteres com o nome do departamento. </param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NomeiaDepartamento(int Indice, string Departamento);
		/// <summary>
		/// Programa Totalizador N�o Sujeito ao ICMS. 
		/// </summary>
		/// <param name="Indice">INTEIRO com a posi��o em que o totalizador ser� programado. </param>
		/// <param name="Totalizador">STRING at� 19 caracteres com o nome do totalizador.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NomeiaTotalizadorNaoSujeitoIcms(int Indice, string Totalizador);
		/// <summary>
		/// Programa o espa�amento de linhas entre os cupons.
		/// </summary>
		/// <param name="Linhas">INTEIRO entre 0 e 255 indicando o n�mero de linhas. O valor default da impressora � 8 linhas.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LinhasEntreCupons(int Linhas);
		/// <summary>
		/// Programa o espa�amento entre as linhas impressas no cupom
		/// </summary>
		/// <param name="Dots">INTEIRO entre 0 e 255 indicando o espa�o (dots) entre as linhas. O valor default da impressora � 0.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_EspacoEntreLinhas(int Dots);
		/// <summary>
		/// Permite tornar a impress�o mais forte nos equipamentos baseados na MP-20 FI II.
		/// </summary>
		/// <param name="ForcaImpacto">INTEIRO com o valor da for�a de impacto das agulhas que pode ser: 
		///			<br>1 � Impacto fraco (default) </br>
		///			<br>2 � Impacto m�dio </br> 
		///			<br>3 � Impacto forte </br></param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ForcaImpactoAgulhas(int ForcaImpacto);
		/// <summary>
		/// Programa e desprograma o hor�rio de ver�o. Se a impressora j� estiver no hor�rio de ver�o o mesmo ser� desprogramado atrasando o rel�gio em 1 (uma) hora, caso contr�rio ser� adiantado 1 (uma) hora.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ProgramaHorarioVerao();
		/// <summary>
		/// Programa o modo arrendondamento na impressora. Este arredondamento se refere � venda de item com quantidade fracion�ria.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ProgramaArredondamento();
		/// <summary>
		/// Programa o modo truncamento na impressora.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ProgramaTruncamento();
		#endregion
		
		#region Fun��es do Cupom Fiscal
		/// <summary>
		/// Abre o cupom fiscal na impressora.
		/// </summary>
		/// <param name="CGC_CPF">STRING at� 29 caracteres com o CNPJ ou CPF do cliente.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AbreCupom(string CGC_CPF);
		/// <summary>
		/// Vende item ap�s a abertura do cupom fiscal. Essa fun��o permite tamb�m a venda de itens com 3 casas decimais no valor unit�rio.
		/// </summary>
		/// <param name="Codigo">STRING at� 13 caracteres com o c�digo do produto.</param>
		/// <param name="Descricao">STRING at� 29 caracteres com a descri��o do produto.</param>
		/// <param name="Aliquota">STRING com o valor ou o �ndice da al�quota tribut�ria. Se for o valor deve ser informado com o tamanho de 4 caracteres ou 5 com a v�rgula. Se for o �ndice da al�quota deve ser 2 caracteres. Ex. (18,00 para o valor ou 05 para o �ndice).</param>
		/// <param name="TipoQuantidade">1 (um) caracter indicando o tipo de quantidade. I - Inteira e F - Fracion�ria.</param>
		/// <param name="Quantidade">STRING com at� 4 d�gitos para quantidade inteira e 7 d�gitos para quantidade fracion�ria. Na quantidade fracion�ria s�o 3 casas decimais.</param>
		/// <param name="CasasDecimais">INTEIRO indicando o n�mero de casas decimais para o valor unit�rio (2 ou 3).</param>
		/// <param name="ValorUnitario">STRING at� 8 d�gitos para valor unit�rio.</param>
		/// <param name="TipoDesconto">1 (um) caracter indicando a forma do desconto. '$' desconto por valor e '%' desconto percentual</param>
		/// <param name="Desconto">String com at� 8 d�gitos para desconto por valor (2 casas decimais) e 4 d�gitos para desconto percentual.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VendeItem(string Codigo, string Descricao, string Aliquota, string TipoQuantidade, string Quantidade, int CasasDecimais, string ValorUnitario, string TipoDesconto, string Desconto);
		/// <summary>
		/// Essa fun��o permite a venda de itens com entrada de departamento, desconto e unidade de medida.
		/// </summary>
		/// <param name="Codigo">STRING at� 49 caracteres com o c�digo do produto.</param>
		/// <param name="Descricao">STRING at� 201 caracteres com a descri��o do produto.</param>
		/// <param name="Aliquota">STRING com o valor ou o �ndice da al�quota tribut�ria. Se for o valor deve ser informado com o tamanho de 4 caracteres ou 5 com a v�rgula. Se for o �ndice da al�quota deve ser 2 caracteres. Ex. (18,00 para o valor ou 05 para o �ndice)</param>
		/// <param name="ValorUnitario">STRING com at� 9 d�gitos para o valor (tres casas decimais).</param>
		/// <param name="Quantidade"> STRING com at� 7 d�gitos para a quantidade. Na venda com departamento a quantidade � fracion�ria e s�o 3 casas decimais.</param>
		/// <param name="Acrescimo">STRING com o acr�scimo por valor com at� 10 d�gitos (2 casas decimais).</param>
		/// <param name="Desconto">STRING com o desconto por valor com at� 10 d�gitos (2 casas decimais).</param>
		/// <param name="IndiceDepartamento">STRING com o �ndice do departamento com 2 d�gitos.</param>
		/// <param name="UnidadeMedida">STRING com no m�ximo 2 caracteres para a unidade de medida. Caso n�o seja passado nenhum caracter a unidade n�o � impressa.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VendeItemDepartamento(string Codigo, string Descricao, string Aliquota, string ValorUnitario, string Quantidade, string Acrescimo, string Desconto, string IndiceDepartamento, string UnidadeMedida);
		/// <summary>
		/// Cancela o �ltimo item vendido.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CancelaItemAnterior();
		/// <summary>
		/// Cancela qualquer item dentre os cem (100) �ltimos itens vendidos.
		/// </summary>
		/// <param name="NumeroItem">STRING com o n�mero do item a ser cancelado com no m�ximo 3 d�gitos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CancelaItemGenerico(string NumeroItem);
		/// <summary>
		/// Cancela o �ltimo cupom emitido.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CancelaCupom();
		/// <summary>
		/// Permite fechar o cupom de forma resumida, ou seja, sem acr�scimo ou desconto no cupom e com apenas uma forma de pagamento. Essa fun��o l� o subtotal do cupom para fecha-lo.
		/// </summary>
		/// <param name="FormaPagamento">STRING com a forma de pagamento com no m�ximo 16 caracteres.</param>
		/// <param name="Mensagem">STRING com a mensagem promocional com at� 384 caracteres (8 linhas X 48 colunas), para a impressora fiscal MP-20 FI II, e 320 caracteres (8 linhas X 40 colunas), para a impressora fiscal MP-40 FI II.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_FechaCupomResumido(string FormaPagamento, string Mensagem);
		/// <summary>
		/// Fecha o cupom fiscal com a impress�o da mensagem promocional.
		/// </summary>
		/// <param name="FormaPagamento">STRING com o nome da forma de pagamento com no m�ximo 16 caracteres.</param>
		/// <param name="AcrescimoDesconto">Indica se haver� acr�scimo ou desconto no cupom. 'A' para acr�scimo e 'D' para desconto.</param>
		/// <param name="TipoAcrescimoDesconto">Indica se o acr�scimo ou desconto � por valor ou por percentual. '$' para desconto por valor e '%' para percentual.</param>
		/// <param name="ValorAcrescimoDesconto">STRING com no m�ximo 14 d�gitos para acr�scimo ou desconto por valor e 4 d�gitos para acr�scimo ou desconto por percentual.</param>
		/// <param name="ValorPago">STRING com o valor pago com no m�ximo 14 d�gitos.</param>
		/// <param name="Mensagem">STRING com a mensagem promocional com at� 384 caracteres (8 linhas X 48 colunas), para a impressora fiscal MP-20 FI II, e 320 caracteres (8 linhas X 40 colunas), para a impressora fiscal MP-40 FI II.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_FechaCupom(string FormaPagamento, string AcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimoDesconto, string ValorPago, string Mensagem);
		/// <summary>
		/// Inicia o fechamento do cupom com o uso das formas de pagamento.
		/// </summary>
		/// <param name="AcrescimoDesconto">Indica se haver� acr�scimo ou desconto no cupom. 'A' para acr�scimo e 'D' para desconto.</param>
		/// <param name="TipoAcrescimoDesconto">Indica se o acr�scimo ou desconto � por valor ou por percentual. '$' para desconto por valor e '%' para percentual.</param>
		/// <param name="ValorAcrescimoDesconto">STRING com no m�ximo 14 d�gitos para acr�scimo ou desconto por valor e 4 d�gitos para acr�scimo ou desconto por percentual.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_IniciaFechamentoCupom(string AcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimoDesconto);
		/// <summary>
		/// Imprime a(s) forma(s) de pagamento e o(s) valor(es) pago(s) nessa forma.
		/// </summary>
		/// <param name="FormaPagamento">STRING com a forma de pagamento com no m�ximo 16 caracteres.</param>
		/// <param name="ValorFormaPagamento">STRING com o valor da forma de pagamento com at� 14 d�gitos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_EfetuaFormaPagamento(string FormaPagamento, string ValorFormaPagamento);
		/// <summary>
		/// Imprime a(s) forma(s) de pagamento e o(s) valor(es) pago(s). Permite a impress�o de coment�rios na(s) forma(s) de pagamento.
		/// </summary>
		/// <param name="FormaPagamento">STRING com a forma de pagamento com no m�ximo 16 caracteres.</param>
		/// <param name="ValorFormaPagamento">STRING com o valor da forma de pagamento com at� 14 d�gitos.</param>
		/// <param name="Descricao">STRING com a descri��o da forma de pagamento com no m�ximo 80 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_EfetuaFormaPagamentoDescricaoForma(string FormaPagamento, string ValorFormaPagamento, string Descricao);
		/// <summary>
		/// Termina o fechamento do cupom com mensagem promocional.
		/// </summary>
		/// <param name="Mensagem">STRING com a mensagem promocional com at� 384 caracteres (8 linhas X 48 colunas), para a impressora fiscal MP-20 FI II, e 320 caracteres (8 linhas X 40 colunas), para a impressora fiscal MP-40 FI II.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_TerminaFechamentoCupom(string Mensagem);
		/// <summary>
		/// Permite estornar valores de uma forma de pagamento e inserir em outra.
		/// </summary>
		/// <param name="FormaOrigem">STRING com a forma de pagamento de onde o valor ser� estornado, com at� 16 caracteres.</param>
		/// <param name="FormaDestino">STRING com a forma de pagamento onde o valor ser� inserido, com at� 16 caracteres.</param>
		/// <param name="Valor"> STRING com o valor a ser estornado com at� 14 d�gitos. N�o pode ser maior que o total da forma de pagamento de origem.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_EstornoFormasPagamento(string FormaOrigem, string FormaDestino, string Valor);
		/// <summary>
		/// Esta fun��o permite aumentar a descri��o do item at� 200 caracteres.
		/// </summary>
		/// <param name="Descricao">STRING com a descri��o do item com at� 200 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AumentaDescricaoItem(string Descricao);
		/// <summary>
		/// Imprime a unidade de medida ap�s a quantidade do produto na venda de item.
		/// </summary>
		/// <param name="UnidadeMedida">STRING com a unidade de medida at� 2 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_UsaUnidadeMedida(string UnidadeMedida);
		#endregion
				
		#region Fun��es dos Relat�rios Fiscais
		/// <summary>
		/// Emite a Leitura X na impressora.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraX();
		/// <summary>
		/// Recebe os dados da Leitura X pela serial e grava em arquivo texto.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraXSerial();
		/// <summary>
		/// Emite a Redu��o Z na impressora. Permite ajustar o rel�gio interno da impressora em at� 5 minutos.
		/// </summary>
		/// <param name="Data">STRING com a Data atual da impressora no formato ddmmaa ou dd/mm/aa, dd/mm/aaaa ou dd/mm/aa.</param>
		/// <param name="Hora">STRING com a Hora a ser alterada no formato hhmmss ou hh:mm:ss.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ReducaoZ(string Data, string Hora);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Texto"></param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_RelatorioGerencial(string Texto);
		/// <summary>
		/// 
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_FechaRelatorioGerencial();
		/// <summary>
		/// Emite a leitura da mem�ria fiscal da impressora por intervalo de datas.
		/// </summary>
		/// <param name="DataInicial">STRING com a Data inicial no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <param name="DataFinal">STRING com a Data final no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraMemoriaFiscalData(string DataInicial,string DataFinal);
		/// <summary>
		/// Emite a leitura da mem�ria fiscal da impressora por intervalo de redu��es.
		/// </summary>
		/// <param name="ReducaoInicial">STRING com o N�mero da redu��o inicial com at� 4 d�gitos.</param>
		/// <param name="ReducaoFinal">STRING com o N�mero da redu��o final com at� 4 d�gitos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraMemoriaFiscalReducao(string ReducaoInicial, string ReducaoFinal);
		/// <summary>
		/// Recebe os dados da mem�ria fiscal por intervalo de datas pela serial e grava em arquivo texto.
		/// </summary>
		/// <param name="DataInicial">STRING com a Data inicial no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <param name="DataFinal">STRING com a Data final no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraMemoriaFiscalSerialData(string DataInicial, string DataFinal);
		/// <summary>
		/// Recebe os dados da leitura da mem�ria fiscal por intervalo de redu��es pela serial e grava em arquivo texto.
		/// </summary>
		/// <param name="ReducaoInicial">STRING com o N�mero da reducao inicial com at� 4 d�gitos.</param>
		/// <param name="ReducaoFinal">STRING com o N�mero da reducao final com at� 4 d�gitos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraMemoriaFiscalSerialReducao(string ReducaoInicial, string ReducaoFinal);

		#endregion
			
		#region Fun��es das Opera��es N�o Fiscais
		/// <summary>
		/// Imprime o comprovante n�o fiscal n�o vinculado.
		/// </summary>
		/// <param name="IndiceTotalizador">STRING com o Indice do totalizador para recebimento parcial com at� 2 d�gitos.</param>
		/// <param name="Valor">STRING com o Valor do recebimento com at� 14 d�gitos (duas casas decimais).</param>
		/// <param name="FormaPagamento">STRING com a Forma de pagamento com at� 16 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_RecebimentoNaoFiscal(string IndiceTotalizador, string Valor, string FormaPagamento);
		/// <summary>
		/// Abre o comprovante n�o fiscal vinculado.
		/// </summary>
		/// <param name="FormaPagamento">Forma de pagamento com at� 16 caracteres.</param>
		/// <param name="Valor">Valor pago na forma de pagamento com at� 14 d�gitos (2 casas decimais).</param>
		/// <param name="NumeroCupom">N�mero do cupom a que se refere o comprovante com at� 6 d�gitos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AbreComprovanteNaoFiscalVinculado(string FormaPagamento, string Valor, string NumeroCupom);
		/// <summary>
		/// Imprime o comprovante n�o fiscal vinculado.
		/// </summary>
		/// <param name="Texto">STRING com o Texto a ser impresso no comprovante n�o fiscal vinculado com at� 618 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_UsaComprovanteNaoFiscalVinculado(string Texto);
		/// <summary>
		/// Encerrar o comprovante n�o fiscal vinculado.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_FechaComprovanteNaoFiscalVinculado();
		/// <summary>
		/// Faz uma sangria na impressora (retirada de dinheiro).
		/// </summary>
		/// <param name="Valor">STRING com o Valor da sangria com at� 14 d�gitos (2 casas decimais).</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_Sangria(string Valor);
		/// <summary>
		/// Faz um suprimento na impressora (entrada de dinheiro).
		/// </summary>
		/// <param name="Valor">STRING com o Valor do suprimento com at� 14 d�gitos (2 casas decimais).</param>
		/// <param name="FormaPagamento">STRING com a Forma de pagamento com at� 16 caracteres. Se n�o for informada, o suprimento ser� feito em Dinheiro.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_Suprimento(string Valor, string FormaPagamento);
		#endregion
				
		#region Fun��es de Informa��es da Impressora
		/// <summary>
		/// Retorna a valor acumulado dos acr�scimos efetuados nos cupons. 
		/// </summary>
		/// <param name="ValorAcrescimos">Vari�vel string com 14 posi��es para receber o valor dos acr�scimos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_Acrescimos([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorAcrescimos);
		/// <summary>
		/// Retorna o valor acumulado dos itens e dos cupons cancelados.
		/// </summary>
		/// <param name="ValorCancelamentos">Vari�vel string com 14 posi��es para receber o valor dos cancelamentos com 2 casas decimais.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_Cancelamentos([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorCancelamentos);
		/// <summary>
		/// Retorna o CGC e a Inscri��o Estadual do cliente/propriet�rio cadastrado na impressora.
		/// </summary>
		/// <param name="CGC">Vari�vel string com 18 posi��es para receber o CGC.</param>
		/// <param name="IE">Vari�vel string com 15 posi��es para receber a Inscri��o Estadual.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CGC_IE([MarshalAs(UnmanagedType.VBByRefStr)] ref string CGC,[MarshalAs(UnmanagedType.VBByRefStr)] ref string IE);
		/// <summary>
		/// Retorna o clich� do propriet�rio cadastrado na impressora.
		/// </summary>
		/// <param name="Cliche">Vari�vel string com 186 posi��es para receber clich� cadastrado.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ClicheProprietario([MarshalAs(UnmanagedType.VBByRefStr)] ref string Cliche);
		/// <summary>
		/// Retorna o n�mero de bilhetes de passagem emitidos.
		/// </summary>
		/// <param name="ContadorPassagem">Vari�vel string com 6 posi��es para receber o n�mero de passagens emitidas.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ContadorBilhetePassagem(string ContadorPassagem);
		/// <summary>
		/// Retorna o n�mero de vezes em que os totalizadores n�o sujeitos ao ICMS foram usados.
		/// </summary>
		/// <param name="Contadores">Vari�vel string com 44 posi��es para receber os contadores dos totalizadores.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ContadoresTotalizadoresNaoFiscais([MarshalAs(UnmanagedType.VBByRefStr)] ref string Contadores);
		/// <summary>
		/// Retorna os dados da impressora no momento da �ltima Redu��o Z.
		/// </summary>
		/// <param name="DadosReducao">Retorna os dados da impressora no momento da �ltima Redu��o Z.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_DadosUltimaReducao([MarshalAs(UnmanagedType.VBByRefStr)] ref string DadosReducao);
		/// <summary>
		/// Retorna a data e a hora atual da impressora.
		/// </summary>
		/// <param name="Data">Vari�vel string com 6 posi��es para receber a data atual da impressora no formato ddmmaa.</param>
		/// <param name="Hora">Vari�vel string com 6 posi��es para receber a hora atual da impressora no formato hhmmss.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_DataHoraImpressora([MarshalAs(UnmanagedType.VBByRefStr)] ref string Data, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Hora);
		/// <summary>
		/// Retorna a data da �ltima Redu��o Z.
		/// </summary>
		/// <param name="Data">Vari�vel string com 6 posi��es para receber a data da �ltima redu��o no formato ddmmaa.</param>
		/// <param name="Hora">Vari�vel string com 6 posi��es parar eceber a hora da �ltima redu��o no formato hhmmss.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_DataHoraReducao([MarshalAs(UnmanagedType.VBByRefStr)] ref string Data, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Hora);
		/// <summary>
		/// Retorna a data do �ltimo movimento.
		/// </summary>
		/// <param name="Data">Vari�vel string com 6 posi��es para receber a data do movimento no formato ddmmaa.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_DataMovimento([MarshalAs(UnmanagedType.VBByRefStr)] ref string Data);
		/// <summary>
		/// Retorna a valor acumulado dos descontos.
		/// </summary>
		/// <param name="ValorDescontos">Vari�vel string com 14 posi��es para receber o valor dos descontos com 2 casas decimais.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_Descontos([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorDescontos);
		/// <summary>
		/// Retorna um n�mero referente ao flag fiscal da impressora. Veja discrimina��o abaixo.
		/// </summary>
		/// <param name="Flag">Vari�vel inteira para receber um n�mero representando o flag fiscal da impressora. Veja discrimina��o abaixo.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_FlagsFiscais(ref int Flag);
		/// <summary>
		/// Retorna o valor do Grande Total da impressora.
		/// </summary>
		/// <param name="GrandeTotal">Vari�vel string com 18 posi��es para receber o valor do grande total com 2 casas decimais.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_GrandeTotal([MarshalAs(UnmanagedType.VBByRefStr)] ref string GrandeTotal);
		/// <summary>
		/// Retorna o tempo em minutos que a impressora est� ligada.
		/// </summary>
		/// <param name="Minutos">Vari�vel string com 4 posi��es para receber os minutos em que a impressora est� ligada.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_MinutosLigada([MarshalAs(UnmanagedType.VBByRefStr)] ref string Minutos);
		/// <summary>
		/// Retorna o tempo em minutos que a impressora est� ou esteve imprimindo.
		/// </summary>
		/// <param name="Minutos">Vari�vel string com 4 posi��es para receber os minutos em impress�o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_MinutosImprimindo([MarshalAs(UnmanagedType.VBByRefStr)] ref string Minutos);
		/// <summary>
		/// Retorna o n�mero de linhas impressas ap�s o status de Pouco Papel.
		/// </summary>
		/// <param name="Linhas">Vari�vel inteira para receber a quantidade de linhas impressas.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_MonitoramentoPapel(ref int Linhas);
		/// <summary>
		/// Retorna o n�mero do caixa cadastrado na impressora.
		/// </summary>
		/// <param name="NumeroCaixa">Vari�vel string com 4 posi��es para receber o n�mero do caixa cadastrado na impressora.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroCaixa([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroCaixa);
		/// <summary>
		/// Retorna o n�mero do cupom.
		/// </summary>
		/// <param name="NumeroCupom">Vari�vel string com 6 posi��es para receber o n�mero do �ltimo cupom.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroCupom([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroCupom);
		/// <summary>
		/// Retorna o n�mero de cupons cancelados.
		/// </summary>
		/// <param name="NumeroCancelamentos">Vari�vel STRING com o tamanho de 4 bytes para receber o n�mero de cupons cancelados.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroCuponsCancelados([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroCancelamentos);
		/// <summary>
		/// Retorna o n�mero de interven��es t�cnicas realizadas na impressora.
		/// </summary>
		/// <param name="NumeroIntervencoes">Vari�vel string com 4 posi��es para receber o n�mero de interven��es.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroIntervencoes([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroIntervencoes);
		/// <summary>
		/// Retorna o n�mero da loja cadastrado na impressora.
		/// </summary>
		/// <param name="NumeroLoja">Vari�vel string com 4 posi��es para receber o n�mero da loja cadastrado na impressora.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroLoja([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroLoja);
		/// <summary>
		/// Retorna o n�mero de opera��es n�o fiscais executadas na impressora.
		/// </summary>
		/// <param name="NumeroOperacoes">Vari�vel string com 6 posi��es para receber o n�mero de opera��es.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroOperacoesNaoFiscais([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroOperacoes);
		/// <summary>
		/// Retorna o n�mero de redu��es Z realizadas na impressora.
		/// </summary>
		/// <param name="NumeroReducoes">Vari�vel string com 4 posi��es para receber o n�mero de Redu��es Z.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroReducoes([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroReducoes);
		/// <summary>
		/// Retorna o n�mero de s�rie da impressora.
		/// </summary>
		/// <param name="NumeroSerie">Vari�vel string com o tamanho de 15 posi��es para receber o n�mero de s�rie.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroSerie([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroSerie);
		/// <summary>
		/// Retorna o n�mero de substitui��es de propriet�rio.
		/// </summary>
		/// <param name="NumeroSubstituicoes">Vari�vel string com 4 posi��es para receber o n�mero de substitui��es.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroSubstituicoesProprietario([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroSubstituicoes);
		/// <summary>
		/// Retorna as al�quotas cadastradas na impressora.
		/// </summary>
		/// <param name="Aliquotas">Vari�vel string com o tamanho de 79 posi��es para receber as al�quotas.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_RetornoAliquotas([MarshalAs(UnmanagedType.VBByRefStr)] ref string Aliquotas);
		/// <summary>
		/// Retorna o s�mbolo da moeda cadastrado na impressora.
		/// </summary>
		/// <param name="SimboloMoeda">Vari�vel string com 2 posi��es para receber o s�mbolo da moeda.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_SimboloMoeda([MarshalAs(UnmanagedType.VBByRefStr)] ref string SimboloMoeda);
		/// <summary>
		/// Retorna o valor do subtotal do cupom.
		/// </summary>
		/// <param name="SubTotal">Vari�vel string com o tamanho de 14 posi��es para receber o subtotal do cupom.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_SubTotal([MarshalAs(UnmanagedType.VBByRefStr)] ref string SubTotal);
		/// <summary>
		/// Retorna o n�mero do �ltimo item vendido.
		/// </summary>
		/// <param name="NumeroItem">Vari�vel string com 4 posi��es para receber o n�mero do �ltimo item vendido.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_UltimoItemVendido([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroItem);
		/// <summary>
		/// Retorna o valor acumulado em uma determinada forma de pagamento.
		/// </summary>
		/// <param name="Forma">Vari�vel STRING com at� 16 posi��es com a descri��o da Forma de Pagamento que deseja retornar o seu valor.</param>
		/// <param name="ValorForma">Vari�vel STRING com 14 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ValorFormaPagamento(string Forma, [MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorForma);
		/// <summary>
		/// Retorna o valor pago no �ltimo cupom.
		/// </summary>
		/// <param name="ValorCupom">Vari�vel string com 14 posi��es para receber o valor pago no �ltimo cupom.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ValorPagoUltimoCupom([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorCupom);
		/// <summary>
		/// Retorna o valor acumulado em um determinado totalizador n�o fiscal.
		/// </summary>
		/// <param name="Totalizador">Vari�vel STRING com at� 19 posi��es com a descri��o do Totalizador.</param>
		/// <param name="ValorTotalizador">Vari�vel STRING com 14 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ValorTotalizadorNaoFiscal(string Totalizador, [MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorTotalizador);
		/// <summary>
		/// Retorna as al�quotas de vincula��o ao ISS.
		/// </summary>
		/// <param name="Flag">Vari�vel string com 79 posi��es para receber as al�quotas vinculadas ao Iss.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaAliquotasIss([MarshalAs(UnmanagedType.VBByRefStr)] ref string Flag);
		/// <summary>
		/// Verifica se a Eprom est� conectada.
		/// </summary>
		/// <param name="Flag">Vari�vel string com 2 posi��o para receber o flag de Eprom conectada. Onde: 
		/// <br></br>1 - Eprom conectada 
		///	<br></br>0 - Eprom desconectada. 
		///	</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaEpromConectada([MarshalAs(UnmanagedType.VBByRefStr)] ref string Flag);
		/// <summary>
		/// Retorna os departamentos e seus valores acumulados.
		/// </summary>
		/// <param name="Departamentos">Vari�vel string com 1019 posi��es para receber as informa��es dos departamentos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaDepartamentos([MarshalAs(UnmanagedType.VBByRefStr)] ref string Departamentos);
		/// <summary>
		/// Retorna o estado da impressora.
		/// </summary>
		/// <param name="ACK">Vari�vel inteira para receber o primeiro byte.</param>
		/// <param name="ST1">Vari�vel inteira para receber o segundo byte</param>
		/// <param name="ST2">Vari�vel inteira para receber o terceiro byte</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaEstadoImpressora(ref int ACK, ref int ST1, ref int ST2);
		/// <summary>
		/// Retorna as formas de pagamento e seus valores acumulados.
		/// </summary>
		/// <param name="Formas">Vari�vel string com 3016 posi��es para receber as formas programadas.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaFormasPagamento([MarshalAs(UnmanagedType.VBByRefStr)] ref string Formas);
		/// <summary>
		/// Retorna os �ndices das al�quotas de ISS. 
		/// </summary>
		/// <param name="Modo">Vari�vel string com o tamanho de 48 posi��es para receber os �ndices das al�quotas de ISS.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaIndiceAliquotasIss([MarshalAs(UnmanagedType.VBByRefStr)] ref string Flag);
		/// <summary>
		/// Verifica se a impressora est� em modo normal ou em interven��o t�cnica
		/// </summary>
		/// <param name="Modo">Vari�vel string com 1 posi��o para receber o modo de opera��o da impressora. Onde: 
		///		<br></br>1 - Modo normal 
		///		<br></br>0 - Interven��o t�cnica.
		///	</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaModoOperacao([MarshalAs(UnmanagedType.VBByRefStr)] ref string Modo);
		/// <summary>
		/// Retorna os recebimentos n�o fiscais n�o vinculados programados na impressora.
		/// </summary>
		/// <param name="Recebimentos">Vari�vel string com 2200 posi��es para receber as informa��es.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaRecebimentoNaoFiscal([MarshalAs(UnmanagedType.VBByRefStr)] ref string Recebimentos);
		/// <summary>
		/// Retorna o tipo de impressora.
		/// </summary>
		/// <param name="TipoImpressora">Vari�vel inteira para receber o tipo da impressora (veja abaixo no help os valores retornados).</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaTipoImpressora(ref int TipoImpressora);
		/// <summary>
		/// Retorna a descri��o dos totalizadores n�o fiscais programados na impressora.
		/// </summary>
		/// <param name="Totalizadores">Vari�vel string com 179 posi��es para receber a descri��o dos totalizadores n�o fiscais programados</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaTotalizadoresNaoFiscais([MarshalAs(UnmanagedType.VBByRefStr)] ref string Totalizadores);
		/// <summary>
		/// Retorna os totalizadores parciais cadastrados na impressora.
		/// </summary>
		/// <param name="Totalizadores">Vari�vel string com o tamanho de 445 posi��es para receber os totalizadores parciais cadastrados.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaTotalizadoresParciais([MarshalAs(UnmanagedType.VBByRefStr)] ref string Totalizadores);
		/// <summary>
		/// Retorna 1 se a impressora estiver no modo truncamento e 0 se estiver no modo arredondamento.
		/// </summary>
		/// <param name="Flag">Vari�vel string com 1 posi��o para receber o flag de truncamento</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaTruncamento([MarshalAs(UnmanagedType.VBByRefStr)] ref string Flag);
		/// <summary>
		/// Retorna a vers�o do firmware da impressora.
		/// </summary>
		/// <param name="VersaoFirmware">Vari�vel string com o tamanho de 4 posi��es para receber a vers�o do firmware.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VersaoFirmware([MarshalAs(UnmanagedType.VBByRefStr)] ref string VersaoFirmware);
		/// <summary>
		/// Imprime configura��es da impressora fiscal em um relat�rio gerencial. Ser� emitida uma leitura X antes. Veja abaixo em "Observa��es" as informa��es que ser�o impressas. 
		/// </summary>
		/// <returns></returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ImprimeConfiguracoes();

		#endregion
		
		#region Fun��es de Autentica��o e Gaveta de Dinheiro
		/// <summary>
		/// Permite a autentica��o de documentos.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_Autenticacao();
		/// <summary>
		/// Programa um caracter gr�fico para autentica��o.
		/// </summary>
		/// <param name="Parametros">STRING com os 18 valores para programa��o do caracter gr�fico, separados por v�rgula. </param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ProgramaCaracterAutenticacao(string Parametros);
		/// <summary>
		/// Abre a gaveta de dinheiro.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AcionaGaveta();
		/// <summary>
		/// Retorna se a gaveta est� fechada ou aberta.
		/// </summary>
		/// <param name="EstadoGaveta">INTEIRO com a Vari�vel para receber o estado da gaveta, onde: 
		///		<br></br>Estado = 1 sensor em n�vel 1 (fechada) 
		///		<br></br>Estado = 0 sensor em n�vel 0 (aberta) 
		///	</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaEstadoGaveta(out int EstadoGaveta);
		#endregion
		
		#region Fun��es de Impress�o de Cheques
		/// <summary>
		/// Cancela a impress�o do cheque que est� sendo aguardado pela impressora. O cheque que est� em impress�o n�o pode ser cancelado.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CancelaImpressaoCheque();
		/// <summary>
		/// Imprime cheque na impressora MP-40 FI II Bematech e na impressora YANCO 8500.
		/// </summary>
		/// <param name="Banco">STRING com o N�mero do banco com 3 d�gitos.</param>
		/// <param name="Valor">STRING com o Valor do cheque com at� 14 d�gitos.</param>
		/// <param name="Favorecido">STRING com o Favorecido com at� 45 caracteres.</param>
		/// <param name="Cidade">STRING com a Cidade com at� 27 caracteres.</param>
		/// <param name="Data">STRING com a Data no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <param name="Mensagem">STRING com o Coment�rios at� 120 caracteres. A mensagem ser� impressa 1 (uma) linha ap�s a cidade.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ImprimeCheque(string Banco, string Valor, string Favorecido, string Cidade, string Data, string Mensagem);
		/// <summary>
		/// Imprime c�pia do �ltimo cheque impresso.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ImprimeCopiaCheque();
		/// <summary>
		/// Inclui o nome da cidade e do favorecido no arquivo de configura��o BEMAFI32.INI.
		/// </summary>
		/// <param name="Cidade">STRING com o Nome da cidade com at� 27 caracteres.</param>
		/// <param name="Favorecido">STRING com o Nome do favorecido com at� 45 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_IncluiCidadeFavorecido(string Cidade, string Favorecido);
		/// <summary>
		/// Programa o nome da moeda no plural para a impress�o de cheques. Ex. (Reais)
		/// </summary>
		/// <param name="MoedaPlural">STRING com o Nome da moeda no plural com at� 22 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ProgramaMoedaPlural(string MoedaPlural);
		/// <summary>
		/// Programa o nome da moeda no singular para a impress�o de cheques. Ex. (Real)
		/// </summary>
		/// <param name="MoedaSingular">STRING com o Nome da Moeda no singular com at� 19 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ProgramaMoedaSingular(string MoedaSingular);
		/// <summary>
		/// Verifica o status do cheque.
		/// </summary>
		/// <param name="StatusCheque">Vari�vel inteira para receber o status do cheque.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaStatusCheque(ref int StatusCheque);
		#endregion
		
		#region Outras Fun��es
		/// <summary>
		/// Faz a abertura do caixa emitindo um suprimento e uma leitura X. Essa fun��o grava o COO inicial e o Grande Total inicial que ser�o usados na fun��o Bematech_FI_RelatorioTipo60Mestre. Portanto, se voc� for emitir o relat�rio "tipo 60 mestre" � obrigat�rio o uso dessa fun��o.
		/// </summary>
		/// <param name="Valor">STRING com o Valor do suprimento com at� 14 d�gitos (2 casas decimais). Informe o valor "0" para n�o fazer suprimento.</param>
		/// <param name="FormaPagto">STRING com a Forma de pagamento com at� 16 caracteres. Se n�o for informado, o suprimento ser� feito em Dinheiro.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AberturaDoDia(string Valor, string FormaPagto);
		/// <summary>
		/// Abre a porta serial para comunica��o entre a impressora e o micro. 
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AbrePortaSerial();
		/// <summary>
		/// Faz o fechamento do dia emitindo uma Redu��o Z. Essa fun��o grava o COO final e o Grande Total final que ser�o usados na fun��o Bematech_FI_RelatorioTipo60Mestre.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_FechamentoDoDia();
		/// <summary>
		/// Fecha a porta serial.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_FechaPortaSerial();
		/// <summary>
		/// Imprime os departamentos e seus valores acumulados em um relat�rio gerencial. Ser� emitida uma leitura X antes. Essas informa��es eram impressas na leitura X at� a vers�o 3.0 e foram retiradas por solicita��o do fisco. 
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ImprimeDepartamentos();
		/// <summary>
		/// Gera o relat�rio "Mapa Resumo" referente ao movimento do dia. As informa��es ser�o geradas no arquivo RETORNO.TXT no diret�rio configurado no par�metro "path" do arquivo ini. O diret�rio default configurado � o diret�rio raiz (C:\).
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_MapaResumo();
		/// <summary>
		/// Gera o relat�rio "Tipo 60 anal�tico" exigido pelo conv�nio de ICMS 85/2001. As informa��es ser�o geradas no arquivo RETORNO.TXT no diret�rio configurado no par�metro "path" do arquivo ini. O diret�rio default � o diret�rio raiz (C:\).
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_RelatorioTipo60Analitico();
		/// <summary>
		/// Gera o relat�rio "Tipo 60 Mestre" exigido pelo conv�nio de ICMS 85/2001. As informa��es ser�o geradas no arquivo RETORNO.TXT no diret�rio configurado no par�metro "path" do arquivo ini. O diret�rio default � o diret�rio raiz (C:\).
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_RelatorioTipo60Mestre();
		/// <summary>
		/// L� o retorno da impressora referente ao �ltimo comando enviado. 
		/// </summary>
		/// <param name="ACK">Vari�vel INTEIRA para receber o primeiro byte.</param>
		/// <param name="ST1">Vari�vel INTEIRA para receber o segundo byte.</param>
		/// <param name="ST2">Vari�vel INTEIRA para receber o terceiro byte.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_RetornoImpressora(ref int ACK, ref int ST1, ref int ST2);
		/// <summary>
		/// Verifica se a impressora est� ligada ou conectada no computador.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaImpressoraLigada();
		/// <summary>
		/// Reseta a impressora em caso de erro.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ResetaImpressora();
		/// <summary>
		/// Abre o cupom na impressora bilhete de passagem.
		/// </summary>
		/// <param name="ImprimeValorFinal">"1" - Imprime o valor pago no final do cupom. "0" - N�o Imprime o valor pago no final do cupom.</param>
		/// <param name="ImprimeEnfatizado">"1" - Imprime as informa��es "EMBARQUE, POLTRONA e PLATAFORMA" enfatizadas. "0" - N�o Imprime as informa��es enfatizadas (negrito).</param>
		/// <param name="Embarque">STRING com at� 40 caracteres com o local de embarque.</param>
		/// <param name="Destino">STRING com at� 40 caracteres com o local de destino.</param>
		/// <param name="Linha">STRING com at� 40 caracteres com o nome da linha (Ex. Curitiba x S�o Paulo - Executivo).</param>
		/// <param name="Prefixo">STRING com at� 40 caracteres.</param>
		/// <param name="Agente">STRING com at� 40 caracteres com o nome do agente.</param>
		/// <param name="Agencia">STRING com at� 40 caracteres com o nome da ag�ncia.</param>
		/// <param name="Data">STRING com a data de embarque no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <param name="Hora">STIRNG com a hora do embarque no formato hhmmss ou hh:mm:ss.</param>
		/// <param name="Poltrona">STRING com at� 2 caracteres com o n�mero da poltrona.</param>
		/// <param name="Plataforma">STRING com at� 3 caracteres com o n�mero da poltrona.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AbreBilhetePassagem(string ImprimeValorFinal, string ImprimeEnfatizado, string	Embarque, string Destino, string Linha, string Prefixo, string Agente, string	Agencia, string	Data, string Hora, string Poltrona, string Plataforma);
		/// <summary>
		/// Imprime um carn� de pagamento.
		/// </summary>
		/// <param name="Titulo">STRING com o titulo para o carn�, impresso centralizado e expandido em cada parcela. Limitado em 20 caracteres.</param>
		/// <param name="Parcelas">STRING com o(s) valor(es) de cada parcela, separadas por ';' (ponto virgula), com duas casas decimais obrigat�riamente. Formatos v�lidos: "23,23;1.200,00" ou "2323;120000". Ver observa��es abaixo</param>
		/// <param name="Datas">STRING com a(s) data(s) de vencimento das parcelas separadas por ';'. Formato v�lidos: "10/10/2003;10112003; ".</param>
		/// <param name="Quantidade">INTEGER com a quantidade de Parcelas. Deve ser diferente de zero.</param>
		/// <param name="Texto">STRING com o texto livre com at� 200 caracteres.</param>
		/// <param name="Cliente">STRING com o nome do cliente com at� 30 caracteres</param>
		/// <param name="RG_CPF">STRING com o n�mero do RG/CPF do cliente. Pode ser nulo ou vazio.</param>
		/// <param name="Cupom">STRING com o COO do Cupom Fiscal com 6 caracteres.</param>
		/// <param name="Vias">INTEGER com a quantidade de Vias. (1 ou 2 apenas).</param>
		/// <param name="Assina">INTEGER para habilitar ou n�o a assinatura do cliente, onde: 
		///		<br></br>1: Habilita a impress�o de uma linha tracejada para a assinatura do cliente. 
		///		<br></br>0: N�o habilita a impress�o da linha tracejada para a assinatura do cliente. 
		///	</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ImpressaoCarne(string Titulo, string Parcelas, string Datas,	int	Quantidade, string Texto, string Cliente, string RG_CPF, string Cupom, int Vias, int Assina);
		#endregion	
		
		#region Fun��es para a Impressora Restaurante
		/// <summary>
		/// Abre o cupom de confer�ncia de mesa e imprime os itens registrados nessa mesa. Essa fun��o mant�m o cupom de confer�ncia aberto permitindo registrar outros itens na mesa. S� s�o permitidos registros com o mesmo n�mero da mesa a qual foi aberta o cupom de confer�ncia.
		/// </summary>
		/// <param name="Mesa">STRING com o n�mero da Mesa com at� 4 d�gitos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_AbreConferenciaMesa(string Mesa);
		/// <summary>
		/// Abre o cupom fiscal na impressora restaurante e imprime os itens registrados na mesa. Se a mesa for "0000", abre o cupom e aguarda a venda dos itens.
		/// </summary>
		/// <param name="Mesa">STRING com o n�mero da Mesa com at� 4 d�gitos.</param>
		/// <param name="CGC_CPF">STRING com at� 29 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_AbreCupomRestaurante(string Mesa, string CGC_CPF);
		/// <summary>
		/// Essa fun��o cancela um registro de venda da mesa informada.
		/// </summary>
		/// <param name="Mesa">STRING com o n�mero da Mesa at� 4 d�gitos.</param>
		/// <param name="Codigo">STRING com o c�digo do item at� 14 d�gitos.</param>
		/// <param name="Descricao">STRING com a descri��o do item at� 17 caracteres.</param>
		/// <param name="Aliquota">STRING com o valor ou o �ndice da al�quota tribut�ria. Se for o valor deve ser informado com o tamanho de 4 caracteres ou 5 com a v�rgula. Se for o �ndice da al�quota deve ser 2 caracteres. Ex. (18,00 para o valor ou 05 para o �ndice).</param>
		/// <param name="Quantidade">STRING com at� 6 d�gitos (s�o tr�s casas decimais).</param>
		/// <param name="ValorUnitario">STRING com at� 8 d�gitos (s�o duas casas decimais).</param>
		/// <param name="FlagAcrescimoDesconto"> "A" para acr�scimo ou "D" para desconto.</param>
		/// <param name="ValorAcrescimoDesconto"> STRING com at� 8 d�gitos (s�o duas casas decimais). Se n�o tiver acr�scimo nem desconto use "0" no valor.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_CancelaVenda(string Mesa, string Codigo, string Descricao, string Aliquota, string Quantidade, string ValorUnitario, string FlagAcrescimoDesconto, string ValorAcrescimoDesconto);
		/// <summary>
		/// Emite um cupom de confer�ncia de mesa. Essa fun��o re�ne as fun��es Bematech_FIR_AbreConferenciaMesa e Bematech_FIR_FechaConferenciaMesa. Ela abre e fecha o cupom de confer�ncia n�o permitindo registrar produtos nesse cupom de confer�ncia.
		/// </summary>
		/// <param name="Mesa">STRING com o n�mero da Mesa com at� 4 d�gitos.</param>
		/// <param name="FlagAcrescimoDesconto">"A" para acr�scimo e "D" para desconto.</param>
		/// <param name="TipoAcrescimoDesconto">"$" para acr�scimo ou desconto por valor e "%" para percentual.</param>
		/// <param name="ValorAcrescimoDesconto">STRING com no m�ximo 14 d�gitos para acr�scimo ou desconto por valor e 4 d�gitos para acr�scimo ou desconto por percentual (s�o duas casas decimais).</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_ConferenciaMesa(string Mesa, string FlagAcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimoDesconto);
		/// <summary>
		/// Permite que a conta seja dividida por todos os clientes. Essa fun��o termina o fechamento do cupom fiscal e imprime um cupom para cada cliente.
		/// </summary>
		/// <param name="NumeroCupons">STRING com at� 2 d�gitos com o n�mero de cupons em que a conta ser� divida. O n�mero m�nimo de cupons � 2 e o m�ximo � 20.</param>
		/// <param name="ValorPago">STRING com os valores pagos por cada cliente. Os valores devem ter no m�ximo 14 d�gitos e serem separados por ponto e v�rgula ";". Ex.: 10,00; 5,00</param>
		/// <param name="CGC_CPF">STRING com o CPF dos clientes. Os CPF's devem ter no m�ximo 29 caracteres e serem separados por ponto e v�rgula ";"</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_ContaDividida(string NumeroCupons, string ValorPago, string CGC_CPF);
		/// <summary>
		/// Retorna os itens do card�pio pela serial com as seguintes informa��es: C�digo, Descri��o, Al�quota e Quantidade.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_CardapioPelaSerial();
		/// <summary>
		/// Fecha o cupom de confer�ncia de mesa. Essa fun��o permite incluir um acr�scimo ou desconto sobre o valor total vendido na mesa. 
		/// </summary>
		/// <param name="FlagAcrescimoDesconto">"A" para acr�scimo ou "D" para desconto.</param>
		/// <param name="TipoAcrescimoDesconto">"$" para acr�scimo ou desconto por valor e "%" para percentual.</param>
		/// <param name="ValorAcrescimoDesconto">STRING com no m�ximo 14 d�gitos para acr�scimo ou desconto por valor e 4 d�gitos para acr�scimo ou desconto por percentual (s�o duas casas decimais).</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_FechaConferenciaMesa(string FlagAcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimoDesconto);
		/// <summary>
		/// Essa fun��o permite fechar o cupom fiscal com formas de pagamento e permite dividir a conta por todos os clientes.
		/// </summary>
		/// <param name="NumeroCupons">STRING com at� 2 d�gitos com o n�mero de cupons em que a conta ser� divida. O n�mero m�nimo de cupons � 2 e o m�ximo � 20.</param>
		/// <param name="FlagAcrescimoDesconto">Indica se haver� acr�scimo ou desconto no cupom. "A" para acr�scimo ou "D" para desconto.</param>
		/// <param name="TipoAcrescimoDesconto">Indica se o acr�scimo ou desconto � por valor ou por percentual. "$" para desconto por valor ou "%" para percentual.</param>
		/// <param name="ValorAcrescimoDesconto">STRING com no m�ximo 14 d�gitos para acr�scimo ou desconto por valor e 4 d�gitos para acr�scimo ou desconto percentual.</param>
		/// <param name="FormasPagamento">STRING com as formas de pagamento. As formas devem ser separadas por ponto e v�rgula (";") se for utilizada mais de uma, e deve ter no m�ximo 16 caracteres cada. Ex: Dinheiro;Cart�o. � permitido a utiliza��o de at� 20 formas.</param>
		/// <param name="ValorFormasPagamento">STRING com os valores das formas de pagamento. Os valores devem ter no m�ximo 14 d�gitos e serem separados por ponto e v�rgula ";".</param>
		/// <param name="ValorPagoCliente">STRING com os valores pagos por cada cliente. Obedecem � mesma situa��o acima.</param>
		/// <param name="CGC_CPF">STRING com o CPF dos clientes. Os CPF's devem ter no m�ximo 29 caracteres e serem separados por ponto e v�rgula ";".</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_FechaCupomContaDividida(string NumeroCupons, string FlagAcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimoDesconto, string FormasPagamento, string ValorFormasPagamento, string ValorPagoCliente, string CGC_CPF);
		/// <summary>
		/// Fecha o cupom fiscal na impressora restaurante com acr�scimo ou desconto, usando apenas uma forma de pagamento.
		/// </summary>
		/// <param name="FormaPagamento">STRING com o nome da forma de pagamento com no m�ximo 16 caracteres.</param>
		/// <param name="FlagAcrescimoDesconto">Indica se haver� acr�scimo ou desconto no cupom. "A" para acr�scimo ou "D" para desconto.</param>
		/// <param name="TipoAcrescimoDesconto">Indica se o acr�scimo ou desconto � por valor ou por percentual. "$" para desconto por valor ou "%" para percentual.</param>
		/// <param name="ValorAcrescimoDesconto">STRING com no m�ximo 14 d�gitos para acr�scimo ou desconto por valor e 4 d�gitos para acr�scimo ou desconto por percentual.</param>
		/// <param name="ValorFormaPagto">STRING com o Valor pago com no m�ximo 14 d�gitos.</param>
		/// <param name="Mensagem">STRING com a Mensagem promocional com at� 490 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_FechaCupomRestaurante(string FormaPagamento, string FlagAcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimoDesconto, string ValorFormaPagto, string Mensagem);
		/// <summary>
		///Imprime os itens do card�pio com as seguintes informa��es: C�digo, Descri��o, Al�quota e Quantidade. 
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_ImprimeCardapio();
		/// <summary>
		/// Faz um registro de venda na mesa informada e cadastra o item no card�pio com o c�digo informado se ele ainda n�o existir.
		/// </summary>
		/// <param name="Mesa">STRING com o n�mero da Mesa at� 4 d�gitos.</param>
		/// <param name="Codigo">STRING com o c�digo do item at� 14 d�gitos.</param>
		/// <param name="Descricao">STRING com a descri��o do item at� 17 caracteres.</param>
		/// <param name="Aliquota">STRING com o valor ou o �ndice da al�quota tribut�ria. Se for o valor deve ser informado com o tamanho de 4 caracteres ou 5 com a v�rgula. Se for o �ndice da al�quota deve ser 2 caracteres. Ex. (18,00 para o valor ou 05 para o �ndice).</param>
		/// <param name="Quantidade">STRING com at� 6 d�gitos (s�o tr�s casas decimais).</param>
		/// <param name="ValorUnitario">STRING com at� 8 d�gitos (s�o duas casas decimais).</param>
		/// <param name="FlagAcrescimoDesconto">"A" para acr�scimo ou "D" para desconto.</param>
		/// <param name="ValorAcrescimoDesconto">STRING com at� 8 d�gitos (s�o duas casas decimais). Se n�o tiver acr�scimo nem desconto use "0" no valor.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_RegistraVenda(string Mesa, string Codigo, string Descricao, string Aliquota, string Quantidade, string ValorUnitario, string FlagAcrescimoDesconto, string ValorAcrescimoDesconto);
		/// <summary>
		/// Retorna os registros de venda da mesa pela porta serial.
		/// </summary>
		/// <param name="Mesa">STRING com o n�mero da Mesa com at� 4 d�gitos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_RegistroVendaSerial(string Mesa);
		/// <summary>
		/// Imprime um relat�rio das mesas que est�o abertas.
		/// </summary>
		/// <param name="TipoRelatorio">INTEIRO, onde: 0 para relatorio parcial ou 1 para relat�rio completo.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_RelatorioMesasAbertas(int TipoRelatorio);
		/// <summary>
		/// Retorna, pela porta serial da impressora, o relat�rio das mesas que est�o abertas.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_RelatorioMesasAbertasSerial();
		/// <summary>
		/// Permite a transfer�ncia parcial ou total dos itens registrados em uma mesa para outra.
		/// </summary>
		/// <param name="MesaOrigem">STRING com o n�mero da Mesa de origem com at� 4 d�gitos.</param>
		/// <param name="Codigo">STRING com o c�digo do item com at� 14 d�gitos.</param>
		/// <param name="Descricao">STRING com a Descri��o do item com at� 17 caracteres.</param>
		/// <param name="Aliquota">STRING com o valor ou o �ndice da al�quota tribut�ria. Se for o valor deve ser informado com o tamanho de 4 caracteres ou 5 com a v�rgula. Se for o �ndice da al�quota deve ser 2 caracteres. Ex. (18,00 para o valor ou 05 para o �ndice ).</param>
		/// <param name="Quantidade">STRING com at� 6 d�gitos (s�o tr�s casas decimais).</param>
		/// <param name="ValorUnitario">STRING com at� 8 d�gitos (s�o duas casas decimais).</param>
		/// <param name="FlagAcrescimoDesconto">"A" para acr�scimo e "D" para desconto.</param>
		/// <param name="ValorAcrescimoDesconto"> STRING com at� 8 d�gitos (s�o duas casas decimais). Se n�o tiver acr�scimo nem desconto use "0" no valor.</param>
		/// <param name="MesaDestino">STRING com o n�mero da Mesa de destino com at� 4 d�gitos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_TransferenciaItem(string MesaOrigem, string Codigo, string Descricao, string Aliquota, string Quantidade, string ValorUnitario, string FlagAcrescimoDesconto, string ValorAcrescimoDesconto, string MesaDestino);
		/// <summary>
		/// Faz a transfer�ncia dos registros de venda da mesa de origem para a mesa de destino, se a mesa de destino j� tiver itens registrados os registros ser�o acrescentados. 
		/// </summary>
		/// <param name="MesaOrigem">STRING com o c�digo da Mesa de origem com at� 4 d�gitos.</param>
		/// <param name="MesaDestino">STRING com o c�digo da Mesa de destino com at� 4 d�gitos.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_TransferenciaMesa(string MesaOrigem, string MesaDestino);
		/// <summary>
		/// Retorna a quantidade de bytes livres na mem�ria da impressora para registros de venda ou itens de card�pio.
		/// </summary>
		/// <param name="Bytes">Vari�vel string com o tamanho de 6 posi��es para o valor correspondente ao bytes de mem�ria livres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_VerificaMemoriaLivre(string Bytes);
		/// <summary>
		/// Permite fechar o cupom de forma resumida, ou seja, sem acr�scimo ou desconto no cupom e com apenas uma forma de pagamento. Essa fun��o l� o subtotal do cupom para fech�-lo.
		/// </summary>
		/// <param name="FormaPagamento">STRING com a Forma de pagamento com no m�ximo 16 caracteres.</param>
		/// <param name="Mensagem">STRING com a Mensagem promocional com at� 490 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FIR_FechaCupomResumidoRestaurante(string FormaPagamento, string Mensagem);
		#endregion

		#region Fun��es da Impressora Fiscal MFD
		/// <summary>
		/// Abre o cupom na impressora bilhete de passagem MFD.
		/// </summary>
		/// <param name="Embarque">STRING com at� 40 caracteres com o local de embarque.</param>
		/// <param name="Destino">STRING com at� 40 caracteres com o local de destino.</param>
		/// <param name="Linha">STRING com at� 40 caracteres com o nome da linha (Ex. Curitiba x S�o Paulo � Executivo</param>
		/// <param name="Agencia">STRING com at� 40 caracteres com o nome da ag�ncia.</param>
		/// <param name="Data">STRING com a data de embarque no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <param name="Hora">STRING com a hora do embarque no formato hhmmss ou hh:mm:ss.</param>
		/// <param name="Poltrona">STRING com at� 2 caracteres com o n�mero da poltrona.</param>
		/// <param name="Plataforma">STRING com at� 3 caracteres com o n�mero da plataforma.</param>
		/// <param name="TipoPassagem"> STRING com: 
		///		<br></br>0 (zero) - passagem Rodovi�rio Intermunicipal; 
		///		<br></br>1 (um) - passagem Ferrovi�rio Intermunicipal; 
		///		<br></br>2 (dois) - passagem Aquavi�rio Intermunicipal; 
		///		<br></br>3 (tr�s) - passagem Rodovi�rio Interestadual; 
		///		<br></br>4 (quatro) - passagem Ferrovi�rio Interestadual; 
		///		<br></br>5 (cinco) - passagem Aquavi�rio Interestadual; 
		///		<br></br>6 (seis) - passagem Rodovi�rio Internacional; 
		///		<br></br>7 (sete) - passagem Ferrovi�rio Internacional ou; 
		///		<br></br>8 (oito) - passagem Aquavi�rio Internacional. 
		/// </param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AbreBilhetePassagemMFD(string Embarque, string Destino, string Linha, string Agencia, string Data, string Hora, string Poltrona, string Plataforma, string TipoPassagem);
		/// <summary>
		/// Abre o Comprovante N�o Fiscal Vinculado
		/// </summary>
		/// <param name="FormaPagamento">STRING com a Forma de Pagamento com at� 16 caracteres.</param>
		/// <param name="Valor"> STRING com o Valor Pago na forma de pagamento do cupom a que se refere o comprovante, com at� 14 d�gitos (2 casas decimais).</param>
		/// <param name="NumeroCupom">STRING com o N�mero do cupom a que se refere o comprovante com at� 6 d�gitos</param>
		/// <param name="CGC">STRING com at� 29 caracteres com o CGC ou CPF do cliente.</param>
		/// <param name="nome">STRING com at� 30 caracteres com o nome do cliente.</param>
		/// <param name="Endereco">STRING com at� 80 caracteres com o endere�o do cliente. </param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AbreComprovanteNaoFiscalVinculadoMFD(string FormaPagamento, string Valor, string NumeroCupom, string CGC, string nome, string Endereco);
		/// <summary>
		/// Abre o cupom fiscal na impressora MFD. 
		/// </summary>
		/// <param name="CGC">STRING at� 29 caracteres com o CGC ou CPF do cliente.</param>
		/// <param name="Nome">STRING at� 30 caracteres com o nome do cliente.</param>
		/// <param name="Endereco">STRING at� 80 caracteres com o endere�o do cliente.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AbreCupomMFD(string CGC, string Nome, string Endereco);
		/// <summary>
		/// Abre o comprovante n�o fiscal n�o vinculado para que sejam lan�ados os recebimentos n�o fiscais.
		/// </summary>
		/// <param name="CGC">STRING at� 29 caracteres com o CGC ou CPF do cliente.</param>
		/// <param name="Nome">STRING at� 30 caracteres com o nome do cliente.</param>
		/// <param name="Endereco">STRING at� 80 caracteres com o endere�o do cliente.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AbreRecebimentoNaoFiscalMFD(string CGC, string Nome, string Endereco);
		/// <summary>
		/// Abre Relat�rio Gerencial, na impressora fiscal MFD.
		/// </summary>
		/// <param name="Indice">STRING num�rica com o valor entre 1 e 30, com o �ndice do relat�rio.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AbreRelatorioGerencialMFD(string Indice);
		/// <summary>
		/// Efetua acr�scimo ou desconto em qualquer item enquanto o cupom fiscal n�o estiver totalizado.
		/// </summary>
		/// <param name="Item">STRING num�rica at� 3 d�gitos com o n�mero do item.</param>
		/// <param name="AcrescimoDesconto">Indica se � acr�scimo ou desconto. 'A' para acr�scimo ou 'D' para desconto.</param>
		/// <param name="TipoAcrescimoDesconto">Indica se o acr�scimo ou desconto � por valor ou por percentual. '$' para desconto por valor e '%' para percentual.</param>
		/// <param name="ValorAcrescimoDesconto">STRING com no m�ximo 14 d�gitos para acr�scimo ou desconto por valor e 4 d�gitos para acr�scimo ou desconto percentual.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AcrescimoDescontoItemMFD(string Item, string AcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimoDesconto);
		/// <summary>
		/// Efetua acr�scimo ou desconto em subtotal do recebimento n�o fiscal.
		/// </summary>
		/// <param name="cFlag">STRING com "A" para Acr�scimo ou "'D" para Desconto.</param>
		/// <param name="cTipo">STRING com "$" para acr�scimo ou desconto por valor, ou "%" para acr�scimo ou desconto por percentual.</param>
		/// <param name="cValor">STRING com no m�ximo 14 d�gitos para o valor ou 4 d�gitos para o percentual.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AcrescimoDescontoSubtotalRecebimentoMFD(string cFlag, string cTipo, string cValor);
		/// <summary>
		/// Efetua acr�scimo ou desconto em subtotal do cupom. 
		/// </summary>
		/// <param name="cFlag">STRING com "A" para Acr�scimo ou "D" para Desconto.</param>
		/// <param name="cTipo">STRING com "$" para Acr�scimo ou Desconto por valor, ou "%" para Acr�scimo ou Desconto percentual.</param>
		/// <param name="cValor">STRING com o valor no m�ximo de 14 d�gitos para Acr�scimo ou Desconto, ou valor com 4 d�gitos para Acr�scimo ou Desconto por percentual.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AcrescimoDescontoSubtotalMFD(string cFlag, string cTipo, string cValor);
		/// <summary>
		/// Permite a autentica��o de documentos.
		/// </summary>
		/// <param name="Linhas">STRING num�rica com valor entre 1 e 99 com o n�mero de linhas que ser�o saltadas para imprimir o texto.</param>
		/// <param name="Texto">STRING com at� 48 caracteres com o texto a ser impresso.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_AutenticacaoMFD(string Linhas, string Texto);
		/// <summary>
		/// Cancela a acr�scimo ou a desconto dado no item.
		/// </summary>
		/// <param name="cFlag">STRING com "A" para cancelar o Acr�scimo ou "D" para cancelar o Desconto.</param>
		/// <param name="cItem">STRING de at� 3 d�gitos com o n�mero do item a ser cancelado restrito aos 300 �ltimos registros efetuados.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CancelaAcrescimoDescontoItemMFD(string cFlag, string cItem);
		/// <summary>
		/// Cancela acr�scimo e desconto efetuados em subtotal do cupom. 
		/// </summary>
		/// <param name="cFlag">STRING com "A" para cancelar o Acr�scimo ou "D" para cancelar o Desconto, dado no subtotal.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CancelaAcrescimoDescontoSubtotalMFD(string cFlag);
		/// <summary>
		/// Cancela acr�scimo e desconto efetuados em subtotal do recebimento n�o fiscal.
		/// </summary>
		/// <param name="cFlag">STRING com "A" para cancelar o Acr�scimo ou "D" para cancelar o Desconto, dado no subtotal do recebimento.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CancelaAcrescimoDescontoSubtotalRecebimentoMFD(string cFlag);
		/// <summary>
		/// Cancela o �ltimo cupom emitido.
		/// </summary>
		/// <param name="CGC">STRING at� 29 caracteres com o CGC ou CPF do cliente. </param>
		/// <param name="Nome">STRING at� 30 caracteres com o nome do cliente.</param>
		/// <param name="Endereco">STRING at� 80 caracteres com o endere�o do cliente.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CancelaCupomMFD(string CGC, string Nome, string Endereco);
		/// <summary>
		/// Cancela o recebimento n�o fiscal.
		/// </summary>
		/// <param name="CGC">STRING at� 29 caracteres com o CGC ou CPF do cliente</param>
		/// <param name="Nome">STRING at� 30 caracteres com o nome do cliente.</param>
		/// <param name="Endereco">STRING at� 80 caracteres com o endere�o do cliente.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CancelaRecebimentoNaoFiscalMFD(string CGC, string Nome, string Endereco);
		/// <summary>
		/// Retorna o n�mero de comprovantes n�o fiscais n�o emitidos.
		/// </summary>
		/// <param name="Comprovantes">Vari�vel STRING com o tamanho de 4 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ComprovantesNaoFiscaisNaoEmitidosMFD(string Comprovantes);
		/// <summary>
		///  Retorna o CNPJ do cliente cadastrado na impressora. 
		/// </summary>
		/// <param name="CNPJ">Vari�vel STRING com o tamanho de 20 posi��es para receber o CNPJ.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CNPJMFD(string CNPJ);
		/// <summary>
		/// Retorna o n�mero de comprovantes de cr�dito emitidos. 
		/// </summary>
		/// <param name="Comprovantes">Vari�vel STRING com o tamanho de 4 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ContadorComprovantesCreditoMFD(string Comprovantes);
		/// <summary>
		/// Retorna o n�mero de cupons fiscais emitidos.
		/// </summary>
		/// <param name="CuponsEmitidos">Vari�vel STRING com o tamanho de 6 posi��es para receber a informa��o</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ContadorCupomFiscalMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string CuponsEmitidos);
		/// <summary>
		/// Retorna o n�mero de vezes em que foi impressa a fita detalhe. 
		/// </summary>
		/// <param name="ContadorFita">Vari�vel STRING com o tamanho de 6 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ContadorFitaDetalheMFD(string ContadorFita);
		/// <summary>
		/// Retorna o n�mero de opera��es n�o fiscais canceladas. 
		/// </summary>
		/// <param name="OperacoesCanceladas">Vari�vel STRING com o tamanho de 4 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ContadorOperacoesNaoFiscaisCanceladasMFD(string OperacoesCanceladas);
		/// <summary>
		/// Retorna o n�mero de relat�rios gerenciais emitidos.
		/// </summary>
		/// <param name="Relatorios">Vari�vel STRING com o tamanho de 6 posi��es para receber a informa��o</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ContadorRelatoriosGerenciaisMFD (string Relatorios);
		/// <summary>
		/// Retorna o n�mero de vezes em que os totalizadores n�o sujeitos ao ICMS foram usados.
		/// </summary>
		/// <param name="Contadores">Vari�vel STRING com 149 posi��es para receber as informa��es. </param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ContadoresTotalizadoresNaoFiscaisMFD(string Contadores);
		/// <summary>
		/// Emite um cupom adicional com as informa��es do COO e valor do cupom fiscal.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_CupomAdicionalMFD();
		/// <summary>
		/// Retorna os dados da impressora no momento da �ltima redu��o Z.
		/// </summary>
		/// <param name="DadosReducao">Vari�vel STRING com o tamanho de 1278 posi��es para receber os dados da �ltima redu��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_DadosUltimaReducaoMFD(string DadosReducao);
		/// <summary>
		/// Retorna a data e hora do �ltimo documento armazenado na MFD no formato dd/mm/aa hh/mm/ss (sem barras e espa�o). 
		/// </summary>
		/// <param name="cDataHora">Vari�vel STRING com o tamanho de 12 posi��es para receber os dados.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_DataHoraUltimoDocumentoMFD(string cDataHora);
		/// <summary>
		/// Imprime a(s) forma(s) de pagamento e o(s) valor(es) pago(s) nessa forma.
		/// </summary>
		/// <param name="FormaPagamento">STRING com a forma de pagamento com no m�ximo 16 caracteres.</param>
		/// <param name="ValorFormaPagamento">STRING com o valor da forma de pagamento com at� 14 d�gitos.</param>
		/// <param name="Parcelas">STRING num�rica entre 1 e 24 com o n�mero de parcelas em que o pagamento ser� realizado.</param>
		/// <param name="DescricaoFormaPagto">STRING com a descri��o da forma de pagamento com no m�ximo 80 caracteres</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_EfetuaFormaPagamentoMFD(string FormaPagamento, string ValorFormaPagamento, string Parcelas, string DescricaoFormaPagto);
		/// <summary>
		/// Efetua o recebimento n�o fiscal.
		/// </summary>
		/// <param name="IndiceTotalizador">STRING com o �ndice do Totalizador com at� 2 d�gitos para o recebimento.</param>
		/// <param name="ValorRecebimento">STRING com o Valor do recebimento com at� 14 d�gitos (duas casas decimais).</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_EfetuaRecebimentoNaoFiscalMFD(string IndiceTotalizador, string ValorRecebimento);
		/// <summary>
		/// Estorna os lan�amentos de um comprovante de cr�dito ou d�bito vinculado. Deve ser executado imediatamente ap�s a impress�o do comprovante vinculado.
		/// </summary>
		/// <param name="CGC">STRING at� 29 caracteres com o CGC ou CPF do cliente.</param>
		/// <param name="Nome">STRING at� 30 caracteres com o nome do cliente.</param>
		/// <param name="Endereco">STRING at� 80 caracteres com o endere�o do cliente.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_EstornoNaoFiscalVinculadoMFD(string CGC, string Nome, string Endereco);
		/// <summary>
		/// Termina o fechamento do recebimento n�o fiscal. 
		/// </summary>
		/// <param name="Mensagem">STRING com a Mensagem promocional com at� 490 caracteres.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_FechaRecebimentoNaoFiscalMFD(string Mensagem);
		/// <summary>
		/// Habilita e desabilita o retorno estendido na MFD. O retorno estendido � ACK, ST1, ST2 e ST3. Caso n�o seja habilitado, ser� retornado apenas ACK, ST1 e ST2 como na impressora fiscal matricial MP-20 FI II ou MP-40 FI II.
		/// </summary>
		/// <param name="FlagRetorno">STRING com o valor um (1) para habilitar ou zero (0) para desabilitar o retorno estendido.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_HabilitaDesabilitaRetornoEstendidoMFD(string FlagRetorno);
		/// <summary>
		/// Imprime cheque na impressora MFD. Somente na impressora MP 6000.
		/// </summary>
		/// <param name="NumeroBanco">STRING com o N�mero do banco com 3 d�gitos.</param>
		/// <param name="Valor">STRING com o Valor do cheque com at� 14 d�gitos.</param>
		/// <param name="Favorecido">STRING com o Favorecido com at� 45 caracteres.</param>
		/// <param name="Cidade">STRING com a Cidade com at� 27 caracteres.</param>
		/// <param name="Data">STRING com a Data no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <param name="Mensagem">STRING com Coment�rios at� 120 caracteres. A mensagem ser� impressa uma (1) linha ap�s a cidade caso n�o tenha sido indicada para impress�o no verso.</param>
		/// <param name="ImpressaoVerso">STRING com o valor zero (0) para impress�o da mensagem na frente do cheque e o valor um (1) para impress�o no verso.</param>
		/// <param name="Linhas">STRING com um valor entre 0 e 35 com o n�mero de linhas a serem saltadas antes da impress�o da mensagem (s� � utilizada na impress�o da mensagem no verso).</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ImprimeChequeMFD(string NumeroBanco, string Valor, string Favorecido, string Cidade, string Data, string Mensagem, string ImpressaoVerso, string Linhas);
		/// <summary>
		/// Inicia o fechamento do cupom fiscal. Permite acr�scimo e desconto no fechamento do cupom.
		/// </summary>
		/// <param name="AcrescimoDesconto">STRING que indica se haver� acr�scimo no cupom, desconto ou ambos. "A" para acr�scimo, "D" para desconto e "X" para acr�scimo e desconto.</param>
		/// <param name="TipoAcrescimoDesconto">STRING que indica se o acr�scimo ou desconto � por valor ou por percentual. "$" para desconto por valor e "%" para percentual.</param>
		/// <param name="ValorAcrescimo">STRING com no m�ximo 14 d�gitos para acr�scimo por valor e 4 d�gitos para acr�scimo percentual.</param>
		/// <param name="ValorDesconto">STRING com no m�ximo 14 d�gitos para desconto por valor e 4 d�gitos para desconto percentual.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_IniciaFechamentoCupomMFD(string AcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimo, string ValorDesconto);
		/// <summary>
		/// Inicia o fechamento do recebimento n�o fiscal. Permite acr�scimo e desconto no fechamento do recebimento.
		/// </summary>
		/// <param name="AcrescimoDesconto">STRING que indica se haver� acr�scimo no cupom, desconto ou ambos. "A" para acr�scimo, "D" para desconto e "X" para acr�scimo e desconto.</param>
		/// <param name="TipoAcrescimoDesconto">STRING que indica se o acr�scimo ou desconto � por valor ou por percentual. "$" para desconto por valor e "%" para percentual.</param>
		/// <param name="ValorAcrescimo">STRING com no m�ximo 14 d�gitos para acr�scimo por valor e 4 d�gitos para acr�scimo percentual.</param>
		/// <param name="ValorDesconto">STRING com no m�ximo 14 d�gitos para desconto por valor e 4 d�gitos para desconto percentual.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_IniciaFechamentoRecebimentoNaoFiscalMFD(string AcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimo, string ValorDesconto);
		/// <summary>
		/// Retorna a incri��o estadual do cliente cadatrada na impressora.
		/// </summary>
		/// <param name="InscricaoEstadual">Vari�vel STRING com o tamanho de 20 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_InscricaoEstadualMFD(string InscricaoEstadual);
		/// <summary>
		/// Retorna a incri��o municipal do cliente cadatrada na impressora.
		/// </summary>
		/// <param name="InscricaoMunicipal">Vari�vel STRING com o tamanho de 20 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_InscricaoMunicipalMFD(string InscricaoMunicipal);
		/// <summary>
		/// Realiza a leitura do c�digo CMC7 do cheque. 
		/// </summary>
		/// <param name="CodigoCMC7">Vari�vel STRING com 36 posi��es para receber o c�digo CMC7.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraChequeMFD(string CodigoCMC7);
		/// <summary>
		/// Emite a leitura da mem�ria fiscal da impressora por intervalo de datas.
		/// </summary>
		/// <param name="DataInicial">STRING para receber a Data inicial no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <param name="DataFinal">STRING para receber a Data final no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <param name="FlagLeitura">STRING com o valor "s" para leitura simplificada e "c" para leitura completa.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraMemoriaFiscalDataMFD(string DataInicial, string DataFinal, string FlagLeitura);
		/// <summary>
		/// Emite a leitura da mem�ria fiscal da impressora por intervalo de redu��es.
		/// </summary>
		/// <param name="ReducaoInicial">Emite a leitura da mem�ria fiscal da impressora por intervalo de redu��es</param>
		/// <param name="ReducaoFinal">STRING com o N�mero da reducao final com at� 4 d�gitos. </param>
		/// <param name="FlagLeitura">STRING com o valor "s" para leitura simplificada e "c" para leitura completa.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraMemoriaFiscalReducaoMFD(string ReducaoInicial, string ReducaoFinal, string FlagLeitura);
		/// <summary>
		/// Recebe os dados da mem�ria fiscal por intervalo de datas pela serial e grava em arquivo texto.
		/// </summary>
		/// <param name="DataInicial">STRING para receber a Data inicial no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <param name="DataFinal">STRING para receber a Data final no formato ddmmaa, dd/mm/aa, ddmmaaaa ou dd/mm/aaaa.</param>
		/// <param name="FlagLeitura">STRING com o valor "s" para leitura simplificada e "c" para leitura completa.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraMemoriaFiscalSerialDataMFD(string DataInicial, string DataFinal, string FlagLeitura);
		/// <summary>
		/// Recebe os dados da leitura da mem�ria fiscal por intervalo de redu��es pela serial e grava em arquivo texto.
		/// </summary>
		/// <param name="ReducaoInicial">STRING com o N�mero da reducao inicial com at� 4 d�gitos.</param>
		/// <param name="ReducaoFinal">STRING com o N�mero da reducao final com at� 4 d�gitos.</param>
		/// <param name="FlagLeitura">STRING com o valor "s" para leitura simplificada e "c" para leitura completa.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_LeituraMemoriaFiscalSerialReducaoMFD(string ReducaoInicial, string ReducaoFinal, string FlagLeitura);
		/// <summary>
		/// Gera o relat�rio "Mapa Resumo" referente ao movimento do dia. As informa��es ser�o geradas no arquivo RETORNO.TXT no diret�rio configurado no par�metro "path" do arquivo ini. O diret�rio default configurado � o diret�rio raiz (C:\).
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_MapaResumoMFD();
		/// <summary>
		/// Retorna a marca, o modelo e o tipo da impressora.
		/// </summary>
		/// <param name="Marca">Vari�vel STRING com 15 posi��es para receber a marca da impressora.</param>
		/// <param name="Modelo">Vari�vel STRING com 20 posi��es para receber o modelo.</param>
		/// <param name="Tipo">Vari�vel STRING com 7 posi��es para receber o tipo da impressora.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_MarcaModeloTipoImpressoraMFD(string Marca, string Modelo, string Tipo);
        /// <summary>
        /// Retorna o tempo em que a impressora emitiu documentos fiscais.
        /// </summary>
        /// <param name="Minutos">Vari�vel STRING com o tamanho de 4 posi��es para receber a informa��o.</param>
        /// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
        [DllImport("BemaFI32.dll")]public static extern int Bematech_FI_MinutosEmitindoDocumentosFiscaisMFD(string Minutos);
		/// <summary>
		/// Programa Relat�rio Gerencial. A impressora possui um relat�rio default pr�-programado: "Abertura de Caixa", no �ndice "01".
		/// </summary>
		/// <param name="Indice">STRING num�rica com valor entre 2 e 30 para o �ndice do relat�rio.</param>
		/// <param name="Descricao">STRING com at� 17 caracteres com o nome do relat�rio.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NomeiaRelatorioGerencialMFD(string Indice, string Descricao);
		/// <summary>
		/// Retorna o n�mero de s�rie da impressora MFD. 
		/// </summary>
		/// <param name="NumeroSerie">Vari�vel STRING com o tamanho de 20 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroSerieMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroSerie);
		/// <summary>
		/// Retorna o n�mero de s�rie da mem�ria de fita detalhe (MFD). 
		/// </summary>
		/// <param name="NumeroSerieMFD">Vari�vel STRING com o tamanho de 20 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_NumeroSerieMemoriaMFD(string NumeroSerieMFD);
		/// <summary>
		/// Retorna o percentual livre da Mem�ria Fita Detalhe (MFD) no formato XX,XX% (com a virgula e o %). 
		/// </summary>
		/// <param name="cMemoriaLivre">Vari�vel STRING com o tamanho de 6 posi��es para receber os dados.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_PercentualLivreMFD(string cMemoriaLivre);
		/// <summary>
		/// Programa as formas de pagamento. 
		/// </summary>
		/// <param name="FormaPagto">STRING at� 16 caracteres com a forma de pagamento.</param>
		/// <param name="OperacaoTef">STRING com 0 (zero) ou 1 (um) indicando se a forma de pagamento permite opera��o TEF ou n�o, onde: 
		///		<br></br>1 - permite opera��o TEF 
		///		<br></br>0 - n�o permite opera��o TEF. 
		///	</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ProgramaFormaPagamentoMFD(string FormaPagto, string OperacaoTef);
		/// <summary>
		/// Retorna o n�mero de redu��es restantes na impressora.
		/// </summary>
		/// <param name="Reducoes">Vari�vel STRING com o tamanho de 4 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ReducoesRestantesMFD(string Reducoes);
		/// <summary>
		/// Reimprime o comprovante n�o fiscal vinculado. Ser� executado, somente, se o comando for enviado imediatamente ap�s a impress�o do comprovante.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ReimpressaoNaoFiscalVinculadoMFD();
		/// <summary>
		/// Gera o relat�rio "Tipo 60 anal�tico" exigido pelo conv�nio de ICMS 85/2001. As informa��es ser�o geradas no arquivo RETORNO.TXT no diret�rio configurado no par�metro "path" do arquivo ini. O diret�rio default � o diret�rio raiz (C:\).
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_RelatorioTipo60AnaliticoMFD();
		/// <summary>
		/// L� o retorno estendido da impressora (ACK, ST1, ST2 e ST3) referente ao �ltimo comando enviado.
		/// </summary>
		/// <param name="ACK">Vari�vel inteira para receber o primeiro bytes de status da impressora.</param>
		/// <param name="ST1">Vari�vel inteira para receber o segundo bytes de status da impressora.</param>
		/// <param name="ST2">Vari�vel inteira para receber o terceiro bytes de status da impressora.</param>
		/// <param name="ST3">Vari�vel inteira para receber o quarto bytes de status da impressora.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_RetornoImpressoraMFD(ref int ACK, ref int ST1, ref int ST2, ref int ST3);
		/// <summary>
		/// Imprime a segunda via do comprovante n�o fiscal vinculado. Deve ser executada imediatamente ap�s a emiss�o da primeira via.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_SegundaViaNaoFiscalVinculadoMFD();
		/// <summary>
		/// Subtotaliza o cupom fiscal, ou seja, inicia o fechamento imprimindo o valor total do cupom.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_SubTotalizaCupomMFD();
		/// <summary>
		/// Subtotaliza o recebimento n�o fiscal (comprovante n�o fiscal n�o vinculado), ou seja, inicia o fechamento imprimindo o valor total do recebimento.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_SubTotalizaRecebimentoMFD();
		/// <summary>
		/// Retorna o quantidade de bytes livres na MFD.
		/// </summary>
		/// <param name="cMemoriaLivre">Vari�vel STRING com o tamanho de 10 posi��es para receber os dados.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_TotalLivreMFD(string cMemoriaLivre);
		/// <summary>
		/// Retorna o tamanho total da MFD em bytes.
		/// </summary>
		/// <param name="cTamanhoMFD">Vari�vel STRING com o tamanho de 10 posi��es para receber os dados.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_TamanhoTotalMFD(string cTamanhoMFD);
		/// <summary>
		/// Retorna o tempo em que a impressora est� operacional.
		/// </summary>
		/// <param name="TempoOperacional">Vari�vel STRING com o tamanho de 4 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_TempoOperacionalMFD(string TempoOperacional);
		/// <summary>
		/// Totaliza o cupom fiscal habilitando o uso das formas de pagamento.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_TotalizaCupomMFD();
		/// <summary>
		/// Totaliza o recebimento n�o fiscal habilitando o uso das formas de pagamento.
		/// </summary>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_TotalizaRecebimentoMFD();
		/// <summary>
		/// Imprime as informa��es do Relat�rio Gerencial.
		/// </summary>
		/// <param name="Texto">STRING Texto a ser impresso no relat�rio com at� 618 caracteres. </param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_UsaRelatorioGerencialMFD(string Texto);
		/// <summary>
		/// Retorna o valor acumulado em uma determinada forma de pagamento
		/// </summary>
		/// <param name="Forma">Vari�vel STRING com at� 16 posi��es com a descri��o da Forma de Pagamento que deseja retornar o seu valor.</param>
		/// <param name="ValorForma">Vari�vel STRING com 14 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ValorFormaPagamentoMFD(string Forma, string ValorForma);
		/// <summary>
		/// Retorna o valor acumulado em um determinado totalizador n�o fiscal.
		/// </summary>
		/// <param name="Totalizador">Vari�vel STRING com at� 19 posi��es com a descri��o do Totalizador.</param>
		/// <param name="ValorTotalizador">Vari�vel STRING com 14 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_ValorTotalizadorNaoFiscalMFD(string Totalizador, string ValorTotalizador);
		/// <summary>
		/// Retorna o estado da impressora.
		/// </summary>
		/// <param name="ACK">Vari�vel inteira para receber o primeiro byte.</param>
		/// <param name="ST1">Vari�vel inteira para receber o segundo byte.</param>
		/// <param name="ST2">Vari�vel inteira para receber o terceiro byte.</param>
		/// <param name="ST3">Vari�vel inteira para receber o quarto byte.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaEstadoImpressoraMFD(ref int ACK, ref int ST1, ref int ST2, ref int ST3);
		/// <summary>
		/// Retorna as formas de pagamento e seus valores acumulados.
		/// </summary>
		/// <param name="FormasPagamento">Vari�vel string com 3016 posi��es para receber as formas programadas.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaFormasPagamentoMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string FormasPagamento);
		/// <summary>
		/// Retorna os recebimentos n�o fiscais n�o vinculados programados na impressora.
		/// </summary>
		/// <param name="Recebimentos">Vari�vel STRING com 1077 posi��es para receber as informa��es.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaRecebimentoNaoFiscalMFD(string Recebimentos);
		/// <summary>
		/// Retorna os relat�rios gerenciais programados e seus valores acumulados.
		/// </summary>
		/// <param name="Relatorios">Vari�vel STRING com 659 posi��es para receber as informa��es.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaRelatorioGerencialMFD(string Relatorios);
		/// <summary>
		/// Retorna a descri��o dos totalizadores n�o fiscais programados na impressora.
		/// </summary>
		/// <param name="Totalizadores">Vari�vel STRING com 599 posi��es para receber a descri��o dos totalizadores n�o fiscais programados.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaTotalizadoresNaoFiscaisMFD(string Totalizadores);
		/// <summary>
		/// Retorna os totalizadores parciais da impressora.
		/// </summary>
		/// <param name="Totalizadores">Vari�vel STRING com o tamanho de 889 posi��es para receber os totalizadores parciais.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VerificaTotalizadoresParciaisMFD(string Totalizadores);
		/// <summary>
		/// Retorna a vers�o do firmware da impressora MFD.
		/// </summary>
		/// <param name="VersaoFirmware">Vari�vel STRING com o tamanho de 6 posi��es para receber a informa��o.</param>
		/// <returns>INTEIRO - Indica se a fun��o conseguiu enviar o comando para impressora.</returns>
		[DllImport("BemaFI32.dll")]public static extern int Bematech_FI_VersaoFirmwareMFD(string VersaoFirmware);
		#endregion
		
		
		// Fim da Declara��o ///////////////////////////////////////////////////////////
		#endregion	

	}
}
