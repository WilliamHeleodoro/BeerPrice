using Dados.DTO;
using Dados.Entidades;
using Dados.Filtros;
using Dapper;

namespace Dados.Repositorios
{
    public class RepositorioBuscarCervejas
    {
        Conexao conexao = new Conexao();

        public PaginacaoResponse<CervejaDTO> BuscarCatalogoCervejas(PaginacaoDTO paginacao)
        {
            long registrosPorPagina = paginacao.Linhas;
            long offset = (paginacao.Pagina - 1) * registrosPorPagina;

            var sql = @"SELECT 
	                        DISTINCT 
                            TITULO, MARCA, TIPO, CARACTERISTICA, QUANTIDADE, UNIDADE,
	                        CASE WHEN (
                                    SELECT IMAGEM
                                    FROM ITEM AS IMAGEM
                                    WHERE  IMAGEM.MARCA = ITEM.MARCA 
                                        AND IMAGEM.TIPO = ITEM.TIPO
                                        AND IMAGEM.CARACTERISTICA = ITEM.CARACTERISTICA 
                                        AND IMAGEM.QUANTIDADE = ITEM.QUANTIDADE 
                                        AND IMAGEM.IMAGEM like '%.png%'  
                                    ORDER BY UNIDADE, CARACTERISTICA, TIPO
                                    LIMIT 1) IS NOT NULL THEN 
	                        		(
                                    	SELECT IMAGEM
                                    		FROM ITEM AS IMAGEM
                                    	WHERE  IMAGEM.MARCA = ITEM.MARCA 
                                        AND IMAGEM.TIPO = ITEM.TIPO
                                        AND IMAGEM.CARACTERISTICA = ITEM.CARACTERISTICA 
                                        AND IMAGEM.QUANTIDADE = ITEM.QUANTIDADE 
                                        AND IMAGEM.IMAGEM like '%.png%' 
                                    	ORDER BY UNIDADE, CARACTERISTICA, TIPO
                                    	LIMIT 1
                                	)
                            ELSE 
	                        	   (
                                    	SELECT IMAGEM
                                    		FROM ITEM AS IMAGEM
                                    	WHERE  IMAGEM.MARCA = ITEM.MARCA 
                                        AND IMAGEM.TIPO = ITEM.TIPO
                                        AND IMAGEM.CARACTERISTICA = ITEM.CARACTERISTICA 
                                        AND IMAGEM.QUANTIDADE = ITEM.QUANTIDADE 
                                    	ORDER BY UNIDADE, CARACTERISTICA, TIPO
                                    	LIMIT 1
                                	)
                            END AS imagem,
                            (SELECT ID
                             	FROM ITEM AS CODIGO
                            WHERE CODIGO.MARCA = ITEM.MARCA 
                            AND CODIGO.TIPO = ITEM.TIPO 
                            AND CODIGO.CARACTERISTICA = ITEM.CARACTERISTICA 
                            AND CODIGO.QUANTIDADE = ITEM.QUANTIDADE 
                            AND CODIGO.UNIDADE = ITEM.UNIDADE 
                            ORDER BY UNIDADE, CARACTERISTICA, TIPO, DATAATUALIZACAO DESC
                            LIMIT 1 
                            ),
	                        (
	                        SELECT DATAATUALIZACAO
                             	FROM ITEM AS ATUALIZACAO
                             WHERE ATUALIZACAO.MARCA = ITEM.MARCA 
                             AND ATUALIZACAO.TIPO = ITEM.TIPO 
                             AND ATUALIZACAO.CARACTERISTICA = ITEM.CARACTERISTICA 
                             AND ATUALIZACAO.QUANTIDADE = ITEM.QUANTIDADE 
                             AND ATUALIZACAO.UNIDADE = ITEM.UNIDADE 
                             ORDER BY DATAATUALIZACAO DESC
                             LIMIT 1 
                             ),
							 
							  (
	                        SELECT MIN(PRECO) AS PRECO
                             	FROM ITEM AS PRECO
                             WHERE PRECO.MARCA = ITEM.MARCA 
                             AND PRECO.TIPO = ITEM.TIPO 
                             AND PRECO.CARACTERISTICA = ITEM.CARACTERISTICA 
                             AND PRECO.QUANTIDADE = ITEM.QUANTIDADE 
                             AND PRECO.UNIDADE = ITEM.UNIDADE
							 AND DATE(PRECO.DATAATUALIZACAO) = 
								   (
	                        	SELECT DATE(DATAATUALIZACAO)
                             		FROM ITEM AS ATUALIZACAO
                             	WHERE ATUALIZACAO.MARCA = ITEM.MARCA 
                             	AND ATUALIZACAO.TIPO = ITEM.TIPO 
                             	AND ATUALIZACAO.CARACTERISTICA = ITEM.CARACTERISTICA 
                             	AND ATUALIZACAO.QUANTIDADE = ITEM.QUANTIDADE 
                             	AND ATUALIZACAO.UNIDADE = ITEM.UNIDADE 
                             	ORDER BY DATAATUALIZACAO DESC
                             	LIMIT 1 
                             	)
                             LIMIT 1 
                             ),
							  (
	                        SELECT MERCADO
                             	FROM ITEM AS MERCADO
                             WHERE MERCADO.MARCA = ITEM.MARCA 
                             AND MERCADO.TIPO = ITEM.TIPO 
                             AND MERCADO.CARACTERISTICA = ITEM.CARACTERISTICA 
                             AND MERCADO.QUANTIDADE = ITEM.QUANTIDADE 
                             AND MERCADO.UNIDADE = ITEM.UNIDADE 
							 AND MERCADO.PRECO = (
							 	 SELECT MIN(PRECO)
                             		FROM ITEM AS PRECO
                             	WHERE PRECO.MARCA = ITEM.MARCA 
                             	AND PRECO.TIPO = ITEM.TIPO 
                             	AND PRECO.CARACTERISTICA = ITEM.CARACTERISTICA 
                             	AND PRECO.QUANTIDADE = ITEM.QUANTIDADE 
                             	AND PRECO.UNIDADE = ITEM.UNIDADE
							 	AND DATE(PRECO.DATAATUALIZACAO) = 
								   (
	                        		SELECT DATE(DATAATUALIZACAO)
                             			FROM ITEM AS ATUALIZACAO
                             		WHERE ATUALIZACAO.MARCA = ITEM.MARCA 
                             		AND ATUALIZACAO.TIPO = ITEM.TIPO 
                             		AND ATUALIZACAO.CARACTERISTICA = ITEM.CARACTERISTICA 
                             		AND ATUALIZACAO.QUANTIDADE = ITEM.QUANTIDADE 
                             		AND ATUALIZACAO.UNIDADE = ITEM.UNIDADE 
                             		ORDER BY DATAATUALIZACAO DESC
                             		LIMIT 1 
                             		)
                             LIMIT 1 
							 )
                             ORDER BY DATAATUALIZACAO DESC
                             LIMIT 1 
                             )
	                        						
                             FROM ITEM
                             WHERE 1=1"
            ;

            var pesquisaTratada = "";
            if (!string.IsNullOrEmpty(paginacao.Pesquisa))
            {
                pesquisaTratada = "%" + paginacao.Pesquisa + "%";
                sql += " AND (MARCA ILIKE @PESQUISA OR CARACTERISTICA ILIKE @PESQUISA OR TIPO ILIKE @PESQUISA)";
            }

            var sqlTotal = $"SELECT count(*) FROM({sql}) as tbl";
            var totalLinhas = conexao.ConexaoPostgres().QueryFirst<long>(sqlTotal, new { PESQUISA = pesquisaTratada });


            // sql += $@" ORDER BY UNIDADE, CARACTERISTICA, TIPO, DATAATUALIZACAO DESC";


            var sql2 = @$"SELECT * FROM(
                                        {sql}
                        ) AS TBL 
                LIMIT {registrosPorPagina} OFFSET {offset}";


            var cervejas = conexao.ConexaoPostgres().Query<CervejaDTO>(sql2, new {
                PESQUISA = pesquisaTratada
            }).ToList();

            conexao.ConexaoPostgres().Close();

            var paginacaoResponse = new PaginacaoResponse<CervejaDTO>() {
                Pagina = paginacao.Pagina,
                Linhas = paginacao.Linhas,
                Pesquisa = paginacao.Pesquisa,
                TotalLinhas = totalLinhas,
                TotalPaginas = (totalLinhas / paginacao.Linhas),
                Dados = cervejas
            };

            return paginacaoResponse;
        }
    }
}
