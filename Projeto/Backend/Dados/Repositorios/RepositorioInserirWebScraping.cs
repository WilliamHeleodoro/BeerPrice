using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dados.Repositorios
{
    public class RepositorioInserirWebScraping
    {
        Conexao conexao = new Conexao();
        public void InserirItem(string mercado, string tipo, string marca, string caracteristica, string titulo,
            decimal preco, int unidade, string quantidade, string imagem, string ecommerce, DateTime dataAtualizacao)
        {
            var sql = "INSERT INTO ITEM (mercado, tipo, marca, caracteristica, titulo, preco, unidade, quantidade, imagem, ecommerce, dataAtualizacao) " +
                "VALUES (@mercado, @tipo, @marca, @caracteristica, @titulo, @preco, @unidade, @quantidade, @imagem, @ecommerce, @dataAtualizacao)";

            var parametros = new Dictionary<string, object> {
                { "@mercado", mercado },
                { "@tipo", tipo },
                { "@marca", marca },
                { "@caracteristica", caracteristica},
                { "@titulo", titulo },
                { "@preco", preco },
                { "@unidade", unidade },
                { "@quantidade", quantidade },
                { "@imagem", imagem },
                { "@ecommerce", ecommerce },
                { "@dataAtualizacao", dataAtualizacao } };

            conexao.ConexaoPostgres().Execute(sql, parametros);
            conexao.ConexaoPostgres().Close();
        }

        public void DeletarItens()
        {
            var sql = @"DELETE FROM ITEM";

            conexao.ConexaoPostgres().Execute(sql);
            conexao.ConexaoPostgres().Close();
        }
    }
}
