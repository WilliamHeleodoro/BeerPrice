using Dados;
using EasyAutomationFramework;
using javax.swing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebScraping.Model;
using static sun.net.dns.ResolverConfiguration;

namespace WebScraping.Driver
{
    public class WebScraper : Web
    {
        List<Item> items = new List<Item>();

        Regex montarPreco = new Regex(@"\d+(,\d{1,2})?");
        Regex montarUnidade = new Regex(@"\d+([,.]\d+)?(L|l|ml|Ml|ML)");
        Regex montarQuantidade = new Regex(@"\d+(Un|Unidade|Unidades|unidade|unidades)");
        Regex montarQuantidades = new Regex(@"\d+");

        string[] marcasCerveja = { "skol", "heineken", "amstel", "dalla", "big john", "brahma", "antarctica" };
        
        public void BuscarMoura(string link)
        {
            if(driver == null)
                StartBrowser();

            Navigate(link);
            Thread.Sleep(20000);

            ICollection<IWebElement> elements = new List<IWebElement>();

            for (int i = 0; i < 3; i++)
            {
                
                if (GetValue(TypeElement.Xpath, "/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div")
                        .element.FindElements(By.ClassName("list-product-item")).Count() > 0)
                {
                    try
                    {
                       
                        elements = GetValue(TypeElement.Xpath, "/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div")
                               .element.FindElements(By.ClassName("list-product-link"));

                       
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "Moura");
                    }
                }
                else
                    Console.WriteLine("Não conseguiu buscar os dados do moura contador " + i);
                    


                if(elements.Count == 0)
                    return;
               

                Console.WriteLine("passa aqui");
            }

            foreach(var element in elements)
            {
                var item = new Item();
                item.Mercado = "Moura";
                //AJUSTAR PREÇO
                Match preco = montarPreco.Match(element.FindElement(By.XPath("/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div/div/div/div[1]/div[1]/app-list-product-item[1]/div/div/a/div[1]/div[1]")).Text);
                item.Preco = Convert.ToDecimal(preco.Value);

                //AJUSTAR TITULO
                item.Titulo = element.FindElement(By.XPath("/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div/div/div/div[1]/div[1]/app-list-product-item[1]/div/div/a/div[2]/h3/a")).Text;

                //AJUSTAR UNIDADE
                Match unidade = montarUnidade.Match(element.FindElement(By.XPath("/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div/div/div/div[1]/div[1]/app-list-product-item[1]/div/div/a/div[2]/h3/a")).Text);
                item.Unidade = Convert.ToString(unidade.Value);

                //AJUSTAR QUANTIDADE
                Match  quantidade  = montarQuantidade.Match(element.FindElement(By.XPath("/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div/div/div/div[1]/div[1]/app-list-product-item[1]/div/div/a/div[2]/h3/a")).Text);
                Match quantidades = montarQuantidades.Match(quantidade.Value); 
                if (quantidades.Value == "")
                    item.Quantidade = 1;
                else
                    item.Quantidade = Convert.ToInt32(quantidades.Value);

                //AJUSTAR MARCA
                string marca = element.FindElement(By.XPath("/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div/div/div/div[1]/div[1]/app-list-product-item[1]/div/div/a/div[2]/h3/a")).Text;
                foreach(var marcas in marcasCerveja)
                {   
                    if (marca.ToLower().Contains(marcas))
                        item.Marca = marcas;
                }

                items.Add(item);

            }

            inserirItem();
            CloseBrowser();
            driver = null;

        }

        public void BuscarCeleiro(string link)
        {
            if (driver == null)
                StartBrowser();

            Navigate(link);
            Thread.Sleep(10000);

            ICollection<IWebElement> elements = new List<IWebElement>();

            for (int i = 0; i < 10; i++)
            {
                if (GetValue(TypeElement.Xpath, "/html/body/app-root/app-produtos").element.FindElements(By.ClassName("produto-disponivel")).Count() > 0)
                {
                    try
                    {
                        elements = GetValue(TypeElement.Xpath, "/html/body/app-root/app-produtos")
                            .element.FindElements(By.ClassName("produto-disponivel"));

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "Celeiro");
                    }

                }
                else
                {
                    Console.WriteLine("Não conseguiu buscar os dados do celeiro contador " + i);
                   
                }
            }
                
            foreach (var element in elements)
            {
                var item = new Item();
                item.Mercado = "Celeiro";
                //AJUSTAR PREÇO
                Match preco = montarPreco.Match(element.FindElement(By.ClassName("info-price")).Text);
                item.Preco = Convert.ToDecimal(preco.Value);

                //AJUSTAR TITULO
                item.Titulo = element.FindElement(By.ClassName("description ")).Text;

                //AJUSTAR UNIDADE
                Match unidade = montarUnidade.Match(element.FindElement(By.ClassName("description")).Text);
                item.Unidade = Convert.ToString(unidade.Value);

                //AJUSTAR QUANTIDADE
                Match quantidade = montarQuantidade.Match(element.FindElement(By.ClassName("description")).Text);
                Match quantidades = montarQuantidades.Match(quantidade.Value);
                if (quantidades.Value == "")
                    item.Quantidade = 1;
                else
                    item.Quantidade = Convert.ToInt32(quantidades.Value);

                //AJUSTAR MARCA
                string marca = element.FindElement(By.ClassName("description")).Text;
                foreach (var marcas in marcasCerveja)
                {
                    if (marca.ToLower().Contains(marcas))
                        item.Marca = marcas;
                }

                items.Add(item);

            }

            inserirItem();
            CloseBrowser();

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
