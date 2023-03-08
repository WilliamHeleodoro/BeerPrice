using Dados;
using EasyAutomationFramework;
using javax.swing;
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
        List<Item> items = new List<Item>();
        public string GetDate(string link)
        {
            if(driver == null)
                StartBrowser();

            Navigate(link);

            Regex montarPreco = new Regex(@"\d+(,\d{1,2})?");
            Regex montarUnidade = new Regex(@"\d+([,.]\d+)?(L|l|ml|Ml|ML)");
            Regex montarQuantidade = new Regex(@"\d+(Un|Unidade|Unidades|unidade|unidades)");
            Regex montarQuantidades = new Regex(@"\d+");

            string[] marcasCerveja = { "skol", "heineken", "amstel", "dalla","big john", "brahma", "antarctica" };

            var texto = "";


            var elements = GetValue(TypeElement.Xpath, "/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div")
                .element.FindElements(By.ClassName("list-product-item"));
        
            foreach(var element in elements)
            {
                var item = new Item();
                item.Mercado = "Moura";
                //AJUSTAR PREÇO
                Match preco = montarPreco.Match(element.FindElement(By.ClassName("area-bloco-preco")).Text);
                item.Preco = Convert.ToDecimal(preco.Value);

                //AJUSTAR TITULO
                item.Titulo = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;

                //AJUSTAR UNIDADE
                Match unidade = montarUnidade.Match(element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text);
                item.Unidade = Convert.ToString(unidade.Value);

                //AJUSTAR QUANTIDADE
                Match  quantidade  = montarQuantidade.Match(element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text);
                Match quantidades = montarQuantidades.Match(quantidade.Value); 
                if (quantidades.Value == "")
                    item.Quantidade = 1;
                else
                    item.Quantidade = Convert.ToInt32(quantidades.Value);

                //AJUSTAR MARCA
                string marca = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;
                foreach(var marcas in marcasCerveja)
                {   
                    if (marca.ToLower().Contains(marcas))
                        item.Marca = marcas;
                }

                items.Add(item);

            }

            inserirItem();
            return texto;

        }

        public void inserirItem()
        {
            Repositorio repositorio = new Repositorio();
            foreach (var item in items)
            {
                if(repositorio.ItemExiste(item.Mercado, item.Marca, item.Unidade, item.Quantidade))
                {
                    repositorio.AtualizarItem(item.Mercado, item.Marca, item.Titulo, item.Preco, item.Unidade, item.Quantidade);
                    continue;
                }

                repositorio.InserirItem(item.Mercado, item.Marca, item.Titulo, item.Preco, item.Unidade, item.Quantidade);
            }

        }

    }
}
