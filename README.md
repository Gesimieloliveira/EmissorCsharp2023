# EmissorCafé #


Executando o projeto pela primeira vez. <br/>
1- Instale o SQLServer 2008, o mesmo se encontra aqui no projeto, dentro da pasta "_setup/files" <br>
2- Instale o .NetFramework462, o mesmo se encontra aqui no projeto, dentro da pasta "_setup/files" <br>
3- Restaure os banco de dados .bak , que estão dentro da pasta "_setup/files" todos com extenção .bak <br>
4- Abra a Solução no Visual Studio Fusion.sln, se encontra na raiz do projeto (VISUAL STUDIO DEVE ESTAR EM ADMINISTRADOR) <br>
5- Crie uma variável de ambiente chamada PROJETO_FUSION coloque nela o caminho do projeto (Anexo 1)
6- Dentro do visual studio, restaure os pacotes nugets da solução<br>
7- Não me pergunte o porque mas procure no visual studio algum outro projeto que precise restaurar pacotes nugets, geralmente esse projeto Fusion.ControladorServicos precisa restaurar os pacotes nugets dele, mesmo depois que restaurou a solução. <br>
8- Restaurado os pacotes nugets de acordo com os passos 6 e 7 ? Criou a variável de ambiente PROJETO_FUSION no seu sistema operacional setando o caminho do projeto ? <br>
9- Execute o projeto Fusion ou FusionNfce , outros projetos também , mas os principais são esses. <br>
10- Erros? Manda issue! <br>
11- Para facilitar a vida se não quiser instalar na mão o banco de dados e o .net framework, basta executar o instalador full na pasta "_setup/Instalador" ele irá instalar tudo para você, mas entre no services.msc, desative os serviços Fusion que estiver lá, porque isso pode lhe atrapalhar no desenvolvimento. <br>


<br>
Observações: muitos relatórios foram deletados pois utilizam um sistema chamado taskreports que exclui ele definitivamente antes de abrir os fontes. fast report são dlls demo, tem que comprar se realmente deseja utilizar os fontes. Até mesmo os instaladores estão com dll demo do fastreport, a ideia é passar tudo para o open mas levará tempo. Conto com o apio da comunidade. 

# Com o passar do tempo vamos mudando tudo para EmissorCafe #
# Dentro da pasta compilada existe uma pasta chamada ResponsavelLegal dentro dela contem o json chamado responsaveltecnico.json , preencha com suas informações. #
# Você é responsável por seu sistema, Não tem suporte, não me responsabilizo por você ao utilizar esses fontes. #
