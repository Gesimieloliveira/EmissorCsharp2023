exec sp_drop_view_if_exists 'dbo', 'view_vendas_pagamentos';
go

create view [dbo].[view_vendas_pagamentos] as
select all
	case fv.estadoAtual when 0 then 0 when 1 then 1 when 2 then 2 end as estadoVenda,
	case fv.estadoAtual when 0 then 'Aberta' when 1 then 'Finalizada' when 2 then 'Cancelada' end as estadoVendaTexto,
	u.id as idUsuario,
	u.login as loginUsuario,
	fv.empresa_id as idEmpresa,
	fp.faturamentoVenda_id as idVenda,
	'faturamento' as modelo,
	cast(fv.finalizadoEm as date) as dataVenda,
	case when fp.especie = 0 then 'DINHEIRO'
	when fp.especie = 1 then 'A PRAZO'
	when fp.especie = 2 then 'CARTÃO CRÉDITO'
	when fp.especie = 3 then 'CARTÃO DÉBITO' else 'pix' end as formaPagamento,
	fv.total as valorVenda,
	fp.valor as valorPagamento


from faturamento_pagamento fp
inner join usuario u on u.id = fp.usuario_id
inner join faturamento_venda fv on fv.id = fp.faturamentoVenda_id

union all

select all
	
	case n.statusAtual when 0 then 0 when 1 then 1 when 2 then 2 end as estadoVenda,
	case n.statusAtual when 0 then 'Aberta' when 1 then 'Finalizada' when 2 then 'Cancelada' end as estadoVendaTexto,
	uu.id as idUsuario,
	uu.login as loginUsuario,
	n.empresa_id as idEmpresa,
	nfp.nfe_id as idVenda,
	'nf-e' as modelo,
	cast(n.emitidaEm as date) dataVenda,
	case when nfp.tipo = 0 then 'DINHEIRO'
	when nfp.tipo = 1 then 'A PRAZO'
	when nfp.tipo = 2 then 'CARTÃO CRÉDITO'
	when nfp.tipo = 3 then 'CARTÃO DÉBITO' else 'PIX' end as formaPagamento,
	n.totalFinal as valorVenda,
	nfp.valor as valorPagamento
	

from nfe_forma_pagamento nfp
inner join usuario uu on uu.id = nfp.usuario_id
inner join nfe n on n.id = nfp.nfe_id

union all

select all
	case n.status when 0 then 0 when 1 then 1 when 2 then 2 end as estadoVenda,
	case n.status when 0 then 'Aberta' when 1 then 'Finalizada' when 2 then 'Cancelada' end as estadoVendaTexto,
	uuu.id as idUsuario,
	uuu.login as loginUsuario,
	ef.empresa_id as idEmpresa,
	nfcp.nfce_id as idVenda,
	'nfc-e' as modelo,
	cast(n.emitidaEm as date) as dataVenda,
	Upper(nfcp.nome) as formaPagamento,
	n.totalNfce as valorVenda,
	nfcp.valorPagamento as valorPagamento

from nfce_forma_pagamento nfcp
inner join nfce n on n.id = nfcp.nfce_id
inner join usuario uuu on uuu.id = n.usuarioCriacao_id
inner join nfce_emissao ne on ne.nfce_id = n.id
inner join emissor_fiscal ef on ef.id = ne.emissorFiscal_id
where nfcp.idFormaPagamento not in ('10')

GO
