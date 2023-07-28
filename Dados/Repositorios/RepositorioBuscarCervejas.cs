using Dapper;
using WebScraping.Model;

namespace Dados.Repositorios
{
    public class RepositorioBuscarCervejas
    {
        
        Conexao conexao = new Conexao();
        public List<Item> BuscarCatalogoCervejas()
        {
            var sql = @"SELECT 
                            DISTINCT TITULO, MARCA, CARACTERISTICA, QUANTIDADE, UNIDADE,
                            (SELECT IMAGEM
                            	FROM ITEM AS IMAGEM
                             	WHERE IMAGEM.MARCA = ITEM.MARCA 
                             			AND IMAGEM.CARACTERISTICA = ITEM.CARACTERISTICA 
                             			AND IMAGEM.QUANTIDADE = ITEM.QUANTIDADE 
                             			AND IMAGEM.UNIDADE = ITEM.UNIDADE 
                             	ORDER BY UNIDADE, CARACTERISTICA
                             	LIMIT 1 
                            )
                            FROM ITEM";

           var cervejas = conexao.ConexaoPostgres().Query<Item>(sql).ToList();
            conexao.ConexaoPostgres().Close();
            
            return cervejas;
        }
    }
}
