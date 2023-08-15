using com.sun.crypto.provider;
using Dados.DTO;
using Dados.Filtros;
using Dapper;
using org.omg.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Model;

namespace Dados.Repositorios
{
    public class RepositorioBuscarCervejaPeloMaiorPreco
    {
        Conexao conexao = new Conexao();

        public List<CervejaPorCodigoDTO> BuscarCervejaPorCodigo(long codigoCerveja)
        {
            var sql = @"SELECT TITULO, IMAGEM, QUANTIDADE, UNIDADE FROM ITEM
                         WHERE ID = @codigoCerveja";


            var parametros = new Dictionary<string, object> { { "@codigoCerveja", codigoCerveja } };

            var cervejas = conexao.ConexaoPostgres().Query<CervejaPorCodigoDTO>(sql, parametros).ToList();
            conexao.ConexaoPostgres().Close();

            return cervejas;

        }

        public List<CervejaMaiorPrecoDTO> BuscarCervejaPreco(long codigoCerveja)
        {
            var sql = @"WITH ITEM_QUERY AS (
                          SELECT MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE, PRECO, ECOMMERCE
                          FROM ITEM
                          WHERE ITEM.ID = @codigoCerveja
                        )
                        SELECT 
                            MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE, PRECO, ECOMMERCE
                        FROM (
                                SELECT MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE, PRECO, ECOMMERCE
                                FROM ITEM_QUERY
                                UNION
                                SELECT MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE, PRECO, ECOMMERCE
                                FROM ITEM ITENS 
                                WHERE (ITENS.TIPO, ITENS.MARCA, ITENS.CARACTERISTICA, ITENS.QUANTIDADE, ITENS.UNIDADE)
                                    = (SELECT TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE FROM ITEM_QUERY)
                        ) AS COMBINED_RESULTS
                        ORDER BY PRECO";


            var parametros = new Dictionary<string, object> { { "@codigoCerveja", codigoCerveja } };

            var cervejas = conexao.ConexaoPostgres().Query<CervejaMaiorPrecoDTO>(sql, parametros).ToList();

            conexao.ConexaoPostgres().Close();

            return cervejas;
        }
    }
}
