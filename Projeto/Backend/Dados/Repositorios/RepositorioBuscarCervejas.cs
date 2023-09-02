using Dados.DTO;
using Dados.Filtros;
using Dapper;

namespace Dados.Repositorios
{
    public class RepositorioBuscarCervejas
    {
        
        Conexao conexao = new Conexao();
        public List<CervejaDTO> BuscarCatalogoCervejas(FiltroObterCerveja filtros)
        {
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
                             )
	                        						
                             FROM ITEM
                             WHERE 1=1";

            if (!string.IsNullOrEmpty(filtros?.filtroGeral))
            {
                filtros.filtroGeral = "%" + filtros.filtroGeral + "%";
                sql += " AND (MARCA ILIKE @filtroGeral OR CARACTERISTICA ILIKE @filtroGeral OR TIPO ILIKE @filtroGeral)";
            }

            var cervejas = conexao.ConexaoPostgres().Query<CervejaDTO>(sql, filtros).ToList();
            conexao.ConexaoPostgres().Close();
            
            return cervejas;
        }
    }
}
