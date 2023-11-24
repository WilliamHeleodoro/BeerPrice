using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados
{
    
    public class Conexao
    {
        //private string connectionString =  "Server=localhost;Port=5432;Database=mercado;User Id=postgres;Password=postgres";
        private string connectionString = "Server=beerpricepgsql.postgres.database.azure.com;Database=cerveja;Port=5432;User Id=postgres;Password=z@q4125!;";

        public NpgsqlConnection ConexaoPostgres()
        {
            NpgsqlConnection conexaoConnection = new NpgsqlConnection(connectionString);
            return conexaoConnection;
        }
    }
}
