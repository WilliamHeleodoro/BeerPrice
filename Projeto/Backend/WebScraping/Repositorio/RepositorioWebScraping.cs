using Dados.Repositorios;
using EasyAutomationFramework;
using System.Text.RegularExpressions;
using WebScraping.Model;

namespace WebScraping.Driver
{
    public class RepositorioWebScraping : Web
    {
        public List<Item> items { get; } = new List<Item>();

        public Regex montarPreco = new Regex(@"\d+(,\d{1,2})?");
        public Regex montarQuantidade = new Regex(@"\d+([,.]\d+)?(L|l|ml|Ml|ML)");
        public Regex montarUnidade = new Regex(@"\d+(Un|un|Unidade|Unidades|unidade|unidades)");
        public Regex montarUnidades = new Regex(@"\d+");

        public string[] marcasCerveja { get; } = { @"Skol", "Heineken", "Amstel", "Dalla", "Devassa", "Budweiser", "Brahma", "Original", "Antarctica",
                                    "Patagonia", "Bohemia", "Eisenbahn", "Corona","Stella", "Coronita", "Sol", "Estrella", 
                                    "Itaipava", "Spaten", "Becks", "Petra", "Kaiser", "Kilsen", "Cristal", "BellaVista", "Cabare" };

        public string[] tipoCerveja { get; }  = { @"Ipa", "Pale Ale", "Beats" };

        public string[] caracteristicas { get; }  = { @"Unfiltered", "Duplo Malte Escura", "Duplo Malte Tostada", "Duplo Malte Trigo", "Duplo Malte", 
                                    "Zero", "Sem Alcool", "Malzbier", "Glúten", "Extra", "Puro Malte", "Amber", "Weisse", 
                                    "Weizenbier", "Session", "Chapecoense", "Sleek", "Gold" };

        public string[] produtos { get; } = { @"Cerveja", "Chopp" };


        
        public void inserirItem()
        {     
            RepositorioInserirWebScraping repositorio = new RepositorioInserirWebScraping();

            //repositorio.DeletarItens();

      

            foreach (var item in items)
            {
                
                repositorio.InserirItem(item.Mercado, item.Tipo, item.Marca, item.Caracteristica,
                    item.Titulo, item.Preco, item.Unidade, item.Quantidade, item.Imagem, item.Ecommerce, item.DataAtualizacao);
            }

        }

    }
}
