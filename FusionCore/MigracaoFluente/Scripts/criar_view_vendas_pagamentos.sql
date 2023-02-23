exec sp_drop_view_if_exists 'dbo', 'view_vendas_pagamentos';
go

create view view_vendas_pagamentos as
select all
	u.id as idUsuario,
	u.login as loginUsuario,
	fp.faturamentoVenda_id as idVenda,
	'faturamento' as modelo,
	fp.criadoEm as dataPagamento,
	case when fp.especie = 0 then 'dinheiro'
	when fp.especie = 1 then 'a prazo'
	when fp.especie = 2 then 'cartão crédito'
	when fp.especie = 3 then 'cartão débito' else 'pix' end as formaPagamento,
	fv.total as valorVenda,
	fp.valor as valorPagamento


from faturamento_pagamento fp
inner join usuario u on u.id = fp.usuario_id
inner join faturamento_venda fv on fv.id = fp.faturamentoVenda_id

union all

select all
	uu.id as idUsuario,
	uu.login as loginUsuario,
	nfp.nfe_id as idVenda,
	'nf-e' as modelo,
	nfp.criadoEm as dataPagamento,
	case when nfp.tipo = 0 then 'dinheiro'
	when nfp.tipo = 1 then 'a prazo'
	when nfp.tipo = 2 then 'cartão crédito'
	when nfp.tipo = 3 then 'cartão débito' else 'pix' end as formaPagamento,
	n.totalFinal as valorVenda,
	nfp.valor as valorPagamento
	

from nfe_forma_pagamento nfp
inner join usuario uu on uu.id = nfp.usuario_id
inner join nfe n on n.id = nfp.nfe_id

union all

select all
	uuu.id as idUsuario,
	uuu.login as loginUsuario,
	nfcp.nfce_id as idVenda,
	'nfc-e' as modelo,
	nfcp.dataEHoraTransacao as dataPagamento,
	nfcp.nome as formaPagamento,
	n.totalNfce as valorVenda,
	nfcp.valorPagamento as valorPagamento

from nfce_forma_pagamento nfcp
inner join nfce n on n.id = nfcp.nfce_id
inner join usuario uuu on uuu.id = n.usuarioCriacao_id
GO


