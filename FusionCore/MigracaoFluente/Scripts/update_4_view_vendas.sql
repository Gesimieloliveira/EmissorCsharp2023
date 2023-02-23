exec sp_drop_view_if_exists 'dbo', 'view_vendas';
go

create view view_vendas as
select all
	'faturamento' as modelo,
    0 as ehFiscal,
	v.id as id,
	v.empresa_id as empresa_id,
	v.usuarioCriacao_id as usuarioCriouId,
	dest.cliente_id as cliente_id,
	case v.estadoAtual when 0 then 0 when 1 then 1 when 2 then 2 end as estadoVenda,
	case v.estadoAtual when 0 then 'Aberta' when 1 then 'Finalizada' when 2 then 'Cancelada' end as estadoVendaTexto,
    cast(v.finalizadoEm as date) as dataVenda,
    v.id as numeroDocumento,
    null as serieDocumento,
    null as chaveDocumento,
	v.totalDesconto as totalDesconto,
	v.total as totalVenda,
	u.login as usuarioLogin
from faturamento_venda v
left join faturamento_destinatario dest on v.id = dest.faturamentoVenda_id
inner join usuario u on u.id = v.usuarioCriacao_id
union all
select all
	'nf-e' as modelo,
    1 as ehFiscal,
	nfv.id as id,
	nfv.empresa_id as empresa_id,
	nfv.usuarioCriacao_id as usuarioCriouId,
	nfdest.pessoa_id as cliente_id,
	case nfv.statusAtual when 1 then 0 when 2 then 1 when 3 then 2 when 4 then 3 end as estadoVenda,
	case nfv.statusAtual when 1 then 'Aberta' when 2 then 'Finalizada' when 3 then 'Cancelada' when 4 then 'Denegada' end as estadoVendaTexto,
	cast(nfvemi.recebidoEm as date) as dataVenda,
    nfv.numeroDocumento as numeroDocumento,
    nfv.serieDocumento as serieDocumento,
    nfvemi.chave as chaveDocumento,
	nfv.totalDescontoFinal as totalDesconto,
	nfv.totalFinal as totalVenda,
	uu.login as usuarioLogin
from nfe nfv
inner join nfe_emissao nfvemi on nfv.id = nfvemi.nfe_id
inner join nfe_destinatario nfdest on nfv.id = nfdest.nfe_id
inner join usuario uu on uu.id = nfv.usuarioCriacao_id
where nfv.finalidadeEmissao = 1 and nfv.tipoOperacao = 1
union all
select all
	'nfc-e' as modelo,
    1 as ehFiscal,
	ncv.id as id,
	ncemit.empresa_id as empresa_id,
	ncv.usuarioCriacao_id as usuarioCriouId,
	ncdest.pessoa_id as cliente_id,
	case when ncv.denegada = 1 then 3 when ncv.status in(0,3) then 0 when ncv.status = 2 then 1 when ncv.status = 1 then 2 end as estadoVenda,
	case when ncv.denegada = 1 then 'Denegada' when ncv.status in(0,3) then 'Aberta' when ncv.status = 2 then 'Finalizada' when ncv.status = 1 then 'Cancelada' end as estadoVendaTexto,
	cast(ncvemi.recebidoEm as date) as dataVenda,
    ncv.numeroFiscal as numeroDocumento,
    ncv.serie as serieDocumento,
    ncvemi.chave as chaveDocumento,
	ncv.totalDesconto as totalDesconto,
	ncv.totalNfce as totalVenda,
	uuu.login as usuarioLogin
from nfce ncv
inner join nfce_emitente ncemit on ncv.id = ncemit.nfce_id
inner join nfce_emissao ncvemi on ncv.id = ncvemi.nfce_id
left join nfce_destinatario ncdest on ncv.id = ncdest.nfce_id
inner join usuario uuu on uuu.id = ncv.usuarioCriacao_id
GO


