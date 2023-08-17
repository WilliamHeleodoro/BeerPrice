using Dados.DTO;
using Dados.Filtros;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.Repositorios
{
    public class RepositorioBuscarHistoricoPrecos
    {
        Conexao conexao = new Conexao();

        public List<CervejaHistoricoPrecoDTO> BuscarHistoricoPreco(long codigoCerveja, FiltroObterHistorico filtros)
        {
            var sql = @"WITH ITEM_QUERY AS (
                          SELECT MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE, PRECO, ECOMMERCE, DATAATUALIZACAO
                          FROM ITEM
                          WHERE ITEM.ID = @codigoCerveja
                        )
                        SELECT 
                            MERCADO, PRECO, DATAATUALIZACAO
                        FROM (
                                SELECT MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE, PRECO, ECOMMERCE, DATAATUALIZACAO
                                FROM ITEM_QUERY
                                UNION
                                SELECT MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE, PRECO, ECOMMERCE, DATAATUALIZACAO
                                FROM ITEM ITENS 
                                WHERE (  ITENS.TIPO, ITENS.MARCA, ITENS.CARACTERISTICA, ITENS.QUANTIDADE, ITENS.UNIDADE)
                                    = (SELECT TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE FROM ITEM_QUERY)
							
                        ) AS COMBINED_RESULTS
						WHERE 1=1";


            var parametros = new Dictionary<string, object> {
                { "@codigoCerveja", codigoCerveja} };

            if (!string.IsNullOrEmpty(filtros.filtroMercado))
           {
                sql += " AND MERCADO = @filtroMercado ";
                parametros.Add("filtroMercado", filtros.filtroMercado);
           }

            sql += " ORDER BY DATAATUALIZACAO";


            var cervejas = conexao.ConexaoPostgres().Query<CervejaHistoricoPrecoDTO>(sql, parametros).ToList();

            conexao.ConexaoPostgres().Close();

            return cervejas;
        }
    }
}
