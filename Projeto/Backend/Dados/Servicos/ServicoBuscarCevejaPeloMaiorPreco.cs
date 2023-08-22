using Dados.DTO;
using Dados.Repositorios;
using java.lang;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Model;

namespace Dados.Servicos
{
    public class ServicoBuscarCevejaPeloMaiorPreco
    {
        private readonly RepositorioBuscarCervejaPeloMaiorPreco _repositorioBuscarCervejaPeloMaiorPreco;

        public ServicoBuscarCevejaPeloMaiorPreco(
            RepositorioBuscarCervejaPeloMaiorPreco repositorioBuscarCervejaPeloMaiorPreco)
        {
            _repositorioBuscarCervejaPeloMaiorPreco = repositorioBuscarCervejaPeloMaiorPreco;
        }

        public CervejaPorCodigoDTO BucarCervejaMaiorPreco(long codigoCerveja)
        {
            CervejaPorCodigoDTO cervejaPorCodigoDTO = new CervejaPorCodigoDTO();
         

            var resultado = _repositorioBuscarCervejaPeloMaiorPreco.BuscarCervejaPorCodigo(codigoCerveja);
       
            foreach (var cerveja in resultado)
            {
                cervejaPorCodigoDTO.Titulo = cerveja.Titulo;
                cervejaPorCodigoDTO.Imagem = cerveja.Imagem;
                cervejaPorCodigoDTO.Quantidade = cerveja.Quantidade;
                cervejaPorCodigoDTO.Unidade = cerveja.Unidade;
                cervejaPorCodigoDTO.DataAtualizacao = cerveja.DataAtualizacao;
            }

            var MaiorPreco = _repositorioBuscarCervejaPeloMaiorPreco.BuscarCervejaPreco(codigoCerveja);


            foreach(var cerveja in MaiorPreco)
            {
                cervejaPorCodigoDTO.MaiorPreco.Add(cerveja);
            }

            var entidade = cervejaPorCodigoDTO;
            
            return entidade;
        }
    }
}
