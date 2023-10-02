using Dados.DTO;
using Dapper;

namespace Dados.Repositorios
{
    public class RepositorioBuscarCervejaPeloMaiorPreco
    {
        Conexao conexao = new Conexao();

        public List<CervejaPorCodigoDTO> BuscarCervejaPorCodigo(long codigoCerveja)
        {
            var sql = @"SELECT 
                            TITULO, 
                            CASE WHEN (SELECT IMAGEM FROM ITEM AS IMAGEM WHERE IMAGEM.ID = ITEM.ID AND IMAGEM.IMAGEM like '%.png%') IS NOT NULL THEN 
                            	(SELECT IMAGEM FROM ITEM AS IMAGEM WHERE IMAGEM.ID = ITEM.ID AND IMAGEM.IMAGEM like '%.png%')
                            ELSE ITEM.IMAGEM
                            END AS IMAGEM,
                            QUANTIDADE, 
                            UNIDADE, 
                            TO_CHAR(DATAATUALIZACAO, 'DD/MM/YYYY') AS DATAATUALIZACAO 
                            FROM ITEM
                            WHERE ID = @codigoCerveja";


            var parametros = new Dictionary<string, object> { { "@codigoCerveja", codigoCerveja } };

            var cervejas = conexao.ConexaoPostgres().Query<CervejaPorCodigoDTO>(sql, parametros).ToList();
            conexao.ConexaoPostgres().Close();

            return cervejas;

        }

        public List<CervejaMaiorPrecoDTO> BuscarCervejaPreco(long codigoCerveja)
        {
            var sql = @"WITH ITEM_QUERY AS (
                            SELECT MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE, PRECO, ECOMMERCE, DATAATUALIZACAO
                            FROM ITEM
                            WHERE ITEM.ID = @codigoCerveja
                        )
                        SELECT 
                            MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE,
                            PRECO, ECOMMERCE, DATAATUALIZACAO
                        FROM(
                                SELECT MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE, PRECO, ECOMMERCE, DATAATUALIZACAO
                                FROM ITEM_QUERY
                                UNION
                                SELECT MERCADO, TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE, PRECO, ECOMMERCE, DATAATUALIZACAO
                                FROM ITEM ITENS 
                                WHERE (ITENS.TIPO, ITENS.MARCA, ITENS.CARACTERISTICA, ITENS.QUANTIDADE, ITENS.UNIDADE)
                                    IN (SELECT TIPO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE FROM ITEM_QUERY)
									AND DATE(ITENS.DATAATUALIZACAO) = 
										(
											SELECT DATE(DATAATUALIZACAO)
                             					FROM ITEM AS ATUALIZACAO
											WHERE ATUALIZACAO.MARCA = ITENS.MARCA 
                             				AND ATUALIZACAO.TIPO = ITENS.TIPO 
                             				AND ATUALIZACAO.CARACTERISTICA = ITENS.CARACTERISTICA 
                             				AND ATUALIZACAO.QUANTIDADE = ITENS.QUANTIDADE 
                             				AND ATUALIZACAO.UNIDADE = ITENS.UNIDADE 
                             				ORDER BY DATAATUALIZACAO DESC
                             				LIMIT 1 
										)
                            ) AS COMBINED_RESULTS
               
                        ORDER BY PRECO";


            var parametros = new Dictionary<string, object> { { "@codigoCerveja", codigoCerveja } };

            var cervejas = conexao.ConexaoPostgres().Query<CervejaMaiorPrecoDTO>(sql, parametros).ToList();

            conexao.ConexaoPostgres().Close();

            return cervejas;
        }
    }
}
