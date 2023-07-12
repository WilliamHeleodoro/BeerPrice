using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dados
{
    public class Repositorio
    {
        Conexao conexao = new Conexao();
        public  void InserirItem(string mercado, string marca, string titulo, decimal preco, string unidade, int quantidade, string imagem)
        {
            var sql = "INSERT INTO ITEM (mercado, marca, titulo, preco, unidade, quantidade, imagem) " +
                "VALUES (@mercado, @marca, @titulo, @preco, @unidade, @quantidade, @imagem)";

            var parametros = new Dictionary<string, object> { { "@mercado", mercado },
                { "@marca", marca },
                { "@titulo", titulo },
                { "@preco", preco },
                { "@unidade", unidade },
                { "@quantidade", quantidade },
                { "@imagem", imagem }};


            conexao.ConexaoPostgres().Execute(sql, parametros);
            conexao.ConexaoPostgres().Close();
        }

        public bool ItemExiste(string mercado, string marca, string unidade, int quantidade, string titulo)
        {
            var sql = "SELECT TITULO FROM ITEM WHERE MERCADO = @mercado AND MARCA = @marca AND UNIDADE = @unidade " +
                "AND QUANTIDADE = @quantidade AND TITULO = @titulo";

            var parametros = new Dictionary<string, object> { { "@mercado", mercado },
                { "@marca", marca },
                { "@unidade", unidade },
                { "@quantidade", quantidade },
                { "@titulo", titulo }};

            var T = conexao.ConexaoPostgres().Query(sql, parametros);
            conexao.ConexaoPostgres().Close();

            if (T.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AtualizarItem(string mercado, string marca, string titulo, decimal preco, string unidade, int quantidade, string imagem)
        {
            var sql = "UPDATE ITEM " +
                "SET mercado = @mercado, " +
                "marca = @marca, " +
                "titulo = @titulo, " +
                "preco = @preco, " +
                "unidade = @unidade, " +
                "quantidade = @quantidade, " +
                "imagem = @imagem " +   
                "WHERE MERCADO = @mercado AND MARCA = @marca AND UNIDADE = @unidade AND QUANTIDADE = @quantidade";

            var parametros = new Dictionary<string, object> { { "@mercado", mercado },
                { "@marca", marca },
                { "@titulo", titulo },
                { "@preco", preco },
                { "@unidade", unidade },
                { "@quantidade", quantidade },
                { "@imagem", imagem }};


            conexao.ConexaoPostgres().Execute(sql, parametros);
            conexao.ConexaoPostgres().Close();
        }
    }   
}
