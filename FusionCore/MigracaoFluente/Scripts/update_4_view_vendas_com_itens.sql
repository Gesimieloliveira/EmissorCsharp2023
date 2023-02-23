exec sp_drop_view_if_exists 'dbo', 'view_vendas_com_itens';
go

create view view_vendas_com_itens as
select all
    v.*,
    nfvi.id as item_id,
	nfvi.produto_id as produto_id,
	nfvi.siglaUnidade as siglaUnidade,
	nfvi.precoCusto as itemPrecoCusto,
	nfvi.precoVenda as itemPrecoVenda,
	nfvi.quantidade as itemQuantidade,
	nfvi.valorUnitario as itemPrecoUnitario,
	cast((nfvi.valorUnitario*nfvi.quantidade) as decimal(15,2)) as itemTotalBruto,
	cast((nfvi.totalDescontoItem + nfvi.valorDescontoFixoRateio) as decimal(15,2)) as itemDesconto,
	cast((nfvi.valorUnitario*nfvi.quantidade-nfvi.totalDescontoItem-nfvi.valorDescontoFixoRateio) as decimal(15,2)) as itemTotal,
	pcf.cfop_id as codigoCfop,
	nfprod.tabelaNcm_id as codigoNcm,
	icms.tributacaoCst_id as itemCstIcms,
	icms.aliquotaIcms as itemAliquotaIcms,
	icms.valorIcms as itemValorIcms,
	pis.situacaoTributariaPis_id as itemPis,
	pis.aliquotaPis as itemAliquotaPis,
	pis.valorPis as itemValorPis,
	cfs.situacaoTributariaCofins_id as itemCofins,
	cfs.aliquotaCofins as itemAliquotaCofins,
	cfs.valorCofins as itemValorCofins,
	ipi.situacaoTributariaIpi_id as itemIpi,
	ipi.aliquotaIpi as itemAliquotaIpi,
	ipi.valorIpi as itemValorIpi,
	nfprod.nome as nomeProduto
from view_vendas v
inner join nfe_item nfvi on v.id = nfvi.nfe_id
inner join produto nfprod on nfprod.id = nfvi.produto_id
inner join nfe_emissao nfvemi on v.id = nfvemi.nfe_id
inner join nfe_destinatario nfdest on v.id = nfdest.nfe_id
inner join perfil_cfop pcf on nfvi.perfilCfop_id = pcf.id
left join nfe_item_icms icms on nfvi.id = icms.nfeItem_id
left join nfe_item_pis pis on nfvi.id = pis.nfeItem_id
left join nfe_item_cofins cfs on nfvi.id = cfs.nfeItem_id
left join nfe_item_ipi ipi on nfvi.id = ipi.nfeItem_id
where v.modelo = 'nf-e'
union all
select all
    v.*,
    ncvi.id as item_id,
	ncvi.produto_id as produto_id,
	ncvi.siglaUnidade as siglaUnidade,
	ncvi.precoCusto as itemPrecoCusto,
	ncvi.precoVenda as itemPrecoVenda,
	ncvi.quantidade as itemQuantidade,
	ncvi.valorUnitario as itemPrecoUnitario,
	cast((ncvi.valorUnitario*ncvi.quantidade) as decimal(15,2)) as itemTotalBruto,
	cast((ncvi.desconto + ncvi.descontoAlteraItem) as decimal(15,2)) as itemDesconto,
	cast((ncvi.valorUnitario*ncvi.quantidade-ncvi.desconto-ncvi.descontoAlteraItem) as decimal(15,2)) as itemTotal,
	ncvi.cfop_id as codigoCfop,
	ncvi.codigoNcm as codigoNcm,
	icms.csosn as itemCstIcms,
	icms.aliquotaIcms as itemAliquotaIcms,
	icms.valorIcms as itemValorIcms,
	pis.situacaoTributariaPis_id as itemPis,
	pis.aliquota as itemAliquotaPis,
	pis.valor as itemValorPis,
	cfs.situacaoTributariaCofins_id as itemCofins,
	cfs.aliquota as itemAliquotaCofins,
	cfs.valor as itemValorCofins,
	null as itemIpi,
	null as itemAliquotaIpi,
	null as itemValorIpi,
	p.nome as nomeProduto

from view_vendas as v
inner join nfce_item as ncvi on v.id = ncvi.nfce_id
inner join nfce_emitente ncemit on v.id = ncemit.nfce_id
inner join nfce_emissao ncvemi on v.id = ncvemi.nfce_id
left join nfce_destinatario ncdest on v.id = ncdest.nfce_id
left join nfce_item_icms icms on ncvi.id = icms.nfceItem_id
left join nfce_item_cofins cfs on ncvi.id = cfs.nfceItem_id
left join nfce_item_pis pis on ncvi.id = pis.nfceItem_id
inner join produto p on p.id = ncvi.produto_id
where v.modelo = 'nfc-e'
union all
select all
    v.*,
    vi.id as item_id,
	vi.produto_id as produto_id,
    vi.siglaUnidade as siglaUnidade,
	vi.precoCusto as itemPrecoCusto,
	vi.precoVenda as itemPrecoVenda,
    vi.quantidade as itemQuantidade,
	vi.precoUnitario as itemPrecoUnitario,
    cast((vi.precoUnitario*vi.quantidade) as decimal(15,2)) as itemTotalBruto,
	cast((vi.totalDesconto + vi.totalDescontoFixo) as decimal(15,2)) as itemDesconto,
	cast((vi.precoUnitario*vi.quantidade-vi.totalDesconto-vi.totalDescontoFixo) as decimal(15,2)) as itemTotal,
	null as codigoCfop,
	null as codigoNcm,
	null as itemCstIcms,
	null as itemAliquotaIcms,
	null as itemValorIcms,
	null as itemPis,
	null as itemAliquotaPis,
	null as itemValorPis,
	null as itemCofins,
	null as itemAliquotaCofins,
	null as itemValorCofins,
	null as itemIpi,
	null as itemAliquotaIpi,
	null as itemValorIpi,
	pp.nome as nomeProduto
from view_vendas v
inner join faturamento_produto vi on v.id = vi.faturamentoVenda_id
left join faturamento_destinatario dest on v.id = dest.faturamentoVenda_id
inner join produto pp on pp.id = vi.produto_id
where v.modelo = 'faturamento'
GO
