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
                        	(SELECT IMAGEM
                        		FROM ITEM AS IMAGEM
                        	    WHERE IMAGEM.MARCA = ITEM.MARCA 
                        			AND IMAGEM.TIPO = ITEM.TIPO
                        			AND IMAGEM.CARACTERISTICA = ITEM.CARACTERISTICA 
                        			AND IMAGEM.QUANTIDADE = ITEM.QUANTIDADE 
                        			AND IMAGEM.UNIDADE = ITEM.UNIDADE 
                        		ORDER BY UNIDADE, CARACTERISTICA, TIPO
                        		LIMIT 1 
                        	),
                        	(SELECT ID
                        		FROM ITEM AS CODIGO
                        		WHERE CODIGO.MARCA = ITEM.MARCA 
                        			AND CODIGO.TIPO = ITEM.TIPO 
                        			AND CODIGO.CARACTERISTICA = ITEM.CARACTERISTICA 
                        			AND CODIGO.QUANTIDADE = ITEM.QUANTIDADE 
                        			AND CODIGO.UNIDADE = ITEM.UNIDADE 
                        		ORDER BY UNIDADE, CARACTERISTICA, TIPO
                        		LIMIT 1 
                        	)
                        FROM ITEM
                        WHERE 1=1 ";

            if (!string.IsNullOrEmpty(filtros?.filtroCaracteristica))
            {
                filtros.filtroCaracteristica = "%" + filtros.filtroCaracteristica + "%";
                sql += "AND ITEM.CARACTERISTICA like @filtroCaracteristica ";
            }

            if (!string.IsNullOrEmpty(filtros?.filtroTipo))
            {
                filtros.filtroTipo = "%" + filtros.filtroTipo + "%";
                sql += "AND ITEM.TIPO like @filtroTipo ";
            }

            if (filtros?.filtroUnidade != 0)
                sql += "AND ITEM.UNIDADE = @filtroUnidade ";

            if (!string.IsNullOrEmpty(filtros?.filtroMarca))
            {
                filtros.filtroMarca = "%" + filtros.filtroMarca + "%";
                sql += "AND ITEM.MARCA like @filtroMarca ";
            }

            if (!string.IsNullOrEmpty(filtros?.filtroQuantidade))
            {
                filtros.filtroQuantidade = "%" + filtros.filtroQuantidade + "%";
                sql += "AND ITEM.QUANTIDADE like @filtroQuantidade ";
            }

            var cervejas = conexao.ConexaoPostgres().Query<CervejaDTO>(sql, filtros).ToList();
            conexao.ConexaoPostgres().Close();
            
            return cervejas;
        }
    }
}
