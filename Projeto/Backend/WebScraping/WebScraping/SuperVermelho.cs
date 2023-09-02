using EasyAutomationFramework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebScraping.Driver;
using WebScraping.Model;

namespace WebScraping.WebScraping
{
    public class SuperVermelho : Web
    {
        RepositorioWebScraping webScraper = new RepositorioWebScraping();
        public void BuscarBrasao(string link, string mercado)
        {

            if (driver == null)
                StartBrowser(TypeDriver.GoogleChorme, null);

            Navigate(link);
            Thread.Sleep(5000);

            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            long scrollHeight = 25000;
            int scrollIncrement = 1000;
            for (int i = 0; i <= scrollHeight; i += scrollIncrement)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript($"window.scrollTo(0, {i})");
                Thread.Sleep(200); // espera 100 milissegundos
            }


            ICollection<IWebElement> elements = new List<IWebElement>();

            for (int i = 0; i < 3; i++)
            {

                if (GetValue(TypeElement.Xpath, "/html/body/section[3]/div")
                        .element.FindElements(By.ClassName("item")).Count() > 0)
                {
                    try
                    {

                        elements = GetValue(TypeElement.Xpath, "/html/body/section[3]/div")
                               .element.FindElements(By.ClassName("item"));


                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "Brasão");
                    }
                }
                else
                    Console.WriteLine("Não conseguiu buscar os dados do Brasão contador " + i);



                if (elements.Count == 0)
                    return;


                Console.WriteLine("passa aqui");
            }

            foreach (var element in elements)
            {
                var item = new Item();

                //EXCESSOES
                string nome = element.FindElement(By.ClassName("nome")).Text;

                if (nome.ToLower().Contains("liverpool"))
                    continue;

                //MERCADO
                item.Mercado = mercado;

                //AJUSTAR PREÇO
                var buscarPreco = element.FindElement(By.ClassName("valor-produto-peso"));
                Match preco = webScraper.montarPreco.Match(buscarPreco.GetAttribute("value"));
                //Match preco = webScraper.montarPreco.Match(element.FindElement(By.ClassName("area-preco")).Text);
                item.Preco = Convert.ToDecimal(preco.Value);

                //AJUSTAR QUANTIDADE
                var buscarString = element.FindElement(By.ClassName("nome")).Text;
                buscarString = buscarString.Replace(" Ml", "Ml");
                buscarString = buscarString.Replace("Litros", "L");
                buscarString = buscarString.Replace(" L", "L");

                Match quantidade = webScraper.montarQuantidade.Match(buscarString);
                item.Quantidade = Convert.ToString(quantidade.Value).ToUpper();

                //AJUSTAR UNIDADE
                Match unidade = webScraper.montarUnidade.Match(element.FindElement(By.ClassName("nome")).Text);
                Match unidades = webScraper.montarUnidades.Match(unidade.Value);

                if (unidades.Value == "")
                    item.Unidade = 1;

                else
                    item.Unidade = Convert.ToInt32(unidades.Value);

                if (item.Unidade == 1 && item.Preco > 30 && item.Quantidade != "5L")
                    continue;

                //AJUSTAR IMAGEM
                var imgElement = element.FindElement(By.ClassName("owl-lazy"));
                item.Imagem = imgElement.GetAttribute("src");

                //AJUSTAR TIPO
                string tipo = element.FindElement(By.ClassName("nome")).Text;

                foreach (var tipos in webScraper.tipoCerveja)
                {
                    if (tipo.Normalize(NormalizationForm.FormD).Contains(tipos.Normalize(NormalizationForm.FormD)))
                    {
                        item.Tipo = tipos;
                        break;
                    }
                    else
                        item.Tipo = "Lager";
                }

                //AJUSTAR MARCA
                string marca = element.FindElement(By.ClassName("nome")).Text;

                marca = marca.Replace("'", "");

                foreach (var marcas in webScraper.marcasCerveja)
                {
                    if (marca.Normalize(NormalizationForm.FormD).Contains(marcas.Normalize(NormalizationForm.FormD)))
                    {
                        item.Marca = marcas;
                        break;
                    }

                }

                //AJUSTAR CARACTERISTICA
                string tituloCerveja = element.FindElement(By.ClassName("nome")).Text;

                tituloCerveja = tituloCerveja.Replace('Á', 'A');
                tituloCerveja = tituloCerveja.Replace("Gluten", "Glúten");

                foreach (var caracter in webScraper.caracteristicas)
                {
                    if (tituloCerveja.ToLower().Normalize(NormalizationForm.FormD).Contains(caracter.ToLower().Normalize(NormalizationForm.FormD)))
                    {
                        if (caracter == "Zero" || caracter == "Sem Alcool" || caracter == "Sem Gluten")
                            item.Caracteristica = "Zero";
                        else if (caracter == "Puro Malte" && (item.Marca != "Skol" && item.Marca != "Itaipava"))
                            item.Caracteristica = "";
                        else if (caracter == "Glúten")
                            item.Caracteristica = "Sem " + caracter;
                        else
                            item.Caracteristica = caracter;

                        break;
                    }

                }

                //ECOMMERCE
                var ecommerce = element.FindElement(By.ClassName("link-nome-produto"));
                item.Ecommerce = ecommerce.GetAttribute("href");

                //DATA ATUALIZAÇÃO
                item.DataAtualizacao = DateTime.Now;

                //TITULO
                var titulo = element.FindElement(By.ClassName("nome")).Text;

                foreach (var produto in webScraper.produtos)
                {
                    if (titulo.Normalize(NormalizationForm.FormD).Contains(produto.Normalize(NormalizationForm.FormD)))
                    {
                        item.Titulo = produto + " " + item.Tipo + " " + item.Marca + " " + item.Caracteristica;
                        break;
                    }
                    else
                    {
                        item.Titulo = "Cerveja " + item.Tipo + " " + item.Marca + " " + item.Caracteristica;
                    }
                }

                if (item.Marca != "" && item.Tipo != "Beats" && item.Quantidade != "" && item.Unidade == 1 && (!item.Titulo.Contains("Chopp")))
                    webScraper.items.Add(item);
            }

            CloseBrowser();
            driver = null;

            webScraper.inserirItem();
        }
    }
}
