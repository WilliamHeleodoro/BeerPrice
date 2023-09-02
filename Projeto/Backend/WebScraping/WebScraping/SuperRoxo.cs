using EasyAutomationFramework;
using OpenQA.Selenium;
using System.Text;
using System.Text.RegularExpressions;
using WebScraping.Driver;
using WebScraping.Model;

namespace WebScraping.WebScraping
{
    public class SuperRoxo : Web
    {
        RepositorioWebScraping webScraper = new RepositorioWebScraping();

        public void BuscarCeleiro(string link, string mercado)
        {
            if (driver == null)
                StartBrowser();

            Navigate(link);
            Thread.Sleep(10000);

            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            long scrollHeight = 25000;
            int scrollIncrement = 1000;
            for (int i = 0; i <= scrollHeight; i += scrollIncrement)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript($"window.scrollTo(0, {i})");
                Thread.Sleep(200); // espera 100 milissegundos
            }

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

                //EXCESSOES
                string nome = element.FindElement(By.ClassName("info-text")).Text;

                if (nome.ToLower().Contains("chopp"))
                    continue;

                //MERCADO
                item.Mercado = mercado;

                //AJUSTAR PREÇO
                var precoAjustado = element.FindElements(By.ClassName("info-price"));

                string apartir = element.FindElement(By.ClassName("info-text")).Text;

                if (precoAjustado.Count() > 2 && !apartir.Contains("partir"))
                {
                    Match preco = webScraper.montarPreco.Match(precoAjustado[2].Text);
                    item.Preco = Convert.ToDecimal(preco.Value);
                }
                else
                {
                    Match preco = webScraper.montarPreco.Match(precoAjustado[0].Text);
                    item.Preco = Convert.ToDecimal(preco.Value);
                }

                //AJUSTAR QUANTIDADE
                var buscarString = element.FindElement(By.ClassName("description")).Text;

                buscarString = buscarString.Replace(" Ml", "Ml");
                buscarString = buscarString.Replace("Litros", "L");
                buscarString = buscarString.Replace(" L", "L");

                Match quantidade = webScraper.montarQuantidade.Match(buscarString);
                item.Quantidade = Convert.ToString(quantidade.Value).ToUpper();

                //AJUSTAR Unidade
                Match unidade = webScraper.montarUnidade.Match(element.FindElement(By.ClassName("description")).Text);
                Match unidades = webScraper.montarUnidades.Match(unidade.Value);
                if (unidades.Value == "")
                    item.Unidade = 1;
                else
                    item.Unidade = Convert.ToInt32(unidades.Value);

                if (item.Unidade == 1 && item.Preco > 30 && item.Quantidade != "5L")
                    continue;

                //AJUSTAR IMAGEM

                var imgElement = element.FindElement(By.ClassName("img-container--product"));
                item.Imagem = imgElement.GetAttribute("src");

                //AJUSTAR TIPO
                string tipo = element.FindElement(By.ClassName("description")).Text;

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
                string marca = element.FindElement(By.ClassName("description")).Text;

                marca = marca.Replace("'", "");

                foreach (var marcas in webScraper.marcasCerveja)
                {
                    if (marca.ToLower().Contains(marcas.ToLower()))
                    {
                        item.Marca = marcas;
                        break;
                    }
                }

                //AJUSTAR CARACTERISTICA
                string tituloCerveja = element.FindElement(By.ClassName("description")).Text;

                tituloCerveja = tituloCerveja.Replace('Á', 'A');
                tituloCerveja = tituloCerveja.Replace("Gluten", "Glúten");

                if (tituloCerveja.ToLower().Contains("corona") || tituloCerveja.ToLower().Contains("coronita"))
                    item.Caracteristica = "Extra";

                foreach (var caracter in webScraper.caracteristicas)
                {
                    if (tituloCerveja.ToLower().Normalize(NormalizationForm.FormD).Contains(caracter.ToLower().Normalize(NormalizationForm.FormD)))
                    {
                        if (caracter == "Zero" || caracter == "Sem Alcool")
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
                var ecommerce = element.FindElement(By.ClassName("ghost-link"));
                item.Ecommerce = ecommerce.GetAttribute("href");

                //DATA ATUALIZAÇÃO
                item.DataAtualizacao = DateTime.Now;

                //TITULO
                var titulo = element.FindElement(By.ClassName("description")).Text;

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
