using EasyAutomationFramework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebScraping.Model;

namespace WebScraping.Driver
{
    public class WebScraper : Web
    {
        public string GetDate(string link)
        {
            if(driver == null)
                StartBrowser();

            var items = new List<Item>();

            Navigate(link);

            Regex montarPreco = new Regex(@"\d+(,\d{1,2})?");
            Regex montarUnidade = new Regex(@"\d+([,.]\d+)?(L|l|ml|Ml|ML)");
            Regex montarQuantidade = new Regex(@"\d+(Un|Unidade|Unidades|unidade|unidades)");
            Regex montarQuantidades = new Regex(@"\d+");

            string[] marcasCerveja = { "skol", "heineken", "amstel", "dalla","big john", "brahma" };

            var texto = "";


            var elements = GetValue(TypeElement.Xpath, "/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div")
                .element.FindElements(By.ClassName("list-product-item"));
        
            foreach(var element in elements)
            {
                var item = new Item();
                item.Mercado = "Moura";
                //AJUSTAR PREÇO
                Match preco = montarPreco.Match(element.FindElement(By.ClassName("area-bloco-preco")).Text);
                item.Preco = Convert.ToString(preco.Value);

                //AJUSTAR TITULO
                item.Titulo = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;

                //AJUSTAR UNIDADE
                Match unidade = montarUnidade.Match(element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text);
                item.Unidade = Convert.ToString(unidade.Value);

                //AJUSTAR QUANTIDADE
                Match  quantidade  = montarQuantidade.Match(element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text);
                Match quantidades = montarQuantidades.Match(quantidade.Value); 
                if (quantidades.Value == "")
                    item.Quantidade = "1";
                else
                    item.Quantidade = Convert.ToString(quantidades.Value);

                //AJUSTAR MARCA
                string marca = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;
                foreach(var marcas in marcasCerveja)
                {   
                    if (marca.ToLower().Contains(marcas))
                        item.Marca = marcas;
                }

                items.Add(item);

              
            }
            
                return texto;

        }

        public void gerarString(string texto)
        {
            string[] itens = texto.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in itens)
            {
                // Extrair o preço
                string padraoPreco = @"R\$ ([\d,]+) ";
                Match matchPreco = Regex.Match(item, padraoPreco);
                string preco = matchPreco.Groups[1].Value;

                // Extrair o título e a marca
                string tituloMarca = item.Replace(matchPreco.Value, "").Trim();
                string[] partesTituloMarca = tituloMarca.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                string marca = partesTituloMarca[0];
                string titulo = string.Join(" ", partesTituloMarca, 1, partesTituloMarca.Length - 1);

                Console.WriteLine("Título: " + titulo);
                Console.WriteLine("Marca: " + marca);
                Console.WriteLine("Preço: " + preco);
            }
         }
    }
}
