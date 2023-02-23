exec sp_drop_view_if_exists 'dbo', 'view_vendas_com_itens';
go

create view view_vendas_com_itens as
select 
	'faturamento' as modelo,
	v.id as id,
	v.empresa_id as empresa_id,
	v.usuarioCriacao_id as usuarioCriouId,
	dest.cliente_id as cliente_id,
	case v.estadoAtual when 0 then 0 when 1 then 1 when 2 then 2 end as estadoVenda,
	case v.estadoAtual when 0 then 'Aberta' when 1 then 'Finalizada' when 2 then 'Cancelada' end as estadoVendaTexto,
	cast(v.finalizadoEm as date) as dataVenda,
	v.totalDesconto as totalDesconto,
	v.total as totalVenda,
	vi.produto_id as produto_id,
	vi.siglaUnidade as siglaUnidade,
	vi.precoCusto as itemPrecoCusto,
	vi.precoVenda as itemPrecoVenda,
	vi.quantidade as itemQuantidade,
	vi.precoUnitario as itemPrecoUnitario,
	cast((vi.precoUnitario*vi.quantidade) as decimal(15,2)) as itemTotalBruto,
	cast((vi.totalDesconto + vi.totalDescontoFixo) as decimal(15,2)) as itemDesconto,
	cast((vi.precoUnitario*vi.quantidade-vi.totalDesconto-vi.totalDescontoFixo) as decimal(15,2)) as itemTotal
from faturamento_venda v
inner join faturamento_produto vi on v.id = vi.faturamentoVenda_id
left join faturamento_destinatario dest on v.id = dest.faturamentoVenda_id
union
select 
	'nf-e' as modelo,
	v.id as id,
	v.empresa_id as empresa_id,
	v.usuarioCriacao_id as usuarioCriouId,
	dest.pessoa_id as cliente_id,
	case v.statusAtual when 1 then 0 when 2 then 1 when 3 then 2 when 4 then 3 end as estadoVenda,
	case v.statusAtual when 1 then 'Aberta' when 2 then 'Finalizada' when 3 then 'Cancelada' when 4  then 'Invalida' end as estadoVendaTexto,
	cast(vemi.recebidoEm as date) as dataVenda,
	v.totalDescontoFinal as totalDesconto,
	v.totalFinal as totalVenda,
	vi.produto_id as produto_id,
	vi.siglaUnidade as siglaUnidade,
	vi.precoCusto as itemPrecoCusto,
	vi.precoVenda as itemPrecoVenda,
	vi.quantidade as itemQuantidade,
	vi.valorUnitario as itemPrecoUnitario,
	cast((vi.valorUnitario*vi.quantidade) as decimal(15,2)) as itemTotalBruto,
	cast((vi.totalDescontoItem + vi.valorDescontoFixoRateio) as decimal(15,2)) as itemDesconto,
	cast((vi.valorUnitario*vi.quantidade-vi.totalDescontoItem-vi.valorDescontoFixoRateio) as decimal(15,2)) as itemTotal
from nfe v
inner join nfe_item vi on v.id = vi.nfe_id
inner join nfe_emissao vemi on v.id = vemi.nfe_id
inner join nfe_destinatario dest on v.id = dest.nfe_id
where v.finalidadeEmissao = 1 and v.tipoOperacao = 1
union
select 
	'nfc-e' as modelo,
	v.id as id,
	emit.empresa_id as empresa_id,
	v.usuarioCriacao_id as usuarioCriouId,
	dest.pessoa_id as cliente_id,
	case when v.status in(0,3) then 0 when v.status = 2 then 1 when v.status = 1 then 2 end as estadoVenda,
	case when v.status in(0,3) then 'Aberta' when v.status = 2 then 'Finalizada' when v.status = 1 then 'Cancelada' end as estadoVendaTexto,
	cast(vemi.recebidoEm as date) as dataVenda,
	v.totalDesconto as totalDesconto,
	v.totalNfce as totalVenda,
	vi.produto_id as produto_id,
	vi.siglaUnidade as siglaUnidade,
	vi.precoCusto as itemPrecoCusto,
	vi.precoVenda as itemPrecoVenda,
	vi.quantidade as itemQuantidade,
	vi.valorUnitario as itemPrecoUnitario,
	cast((vi.valorUnitario*vi.quantidade) as decimal(15,2)) as itemTotalBruto,
	cast((vi.desconto + vi.descontoAlteraItem) as decimal(15,2)) as itemDesconto,
	cast((vi.valorUnitario*vi.quantidade-vi.desconto-vi.descontoAlteraItem) as decimal(15,2)) as itemTotal
from nfce v
inner join nfce_item as vi on v.id = vi.nfce_id
inner join nfce_emitente emit on v.id = emit.nfce_id
inner join nfce_emissao vemi on v.id = vemi.nfce_id
left join nfce_destinatario dest on v.id = dest.nfce_id;
go