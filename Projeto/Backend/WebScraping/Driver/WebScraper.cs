using Dados.Repositorios;
using EasyAutomationFramework;
using OpenQA.Selenium;
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
        Regex montarQuantidade = new Regex(@"\d+([,.]\d+)?(L|l|ml|Ml|ML)");
        Regex montarUnidade = new Regex(@"\d+(Un|un|Unidade|Unidades|unidade|unidades)");
        Regex montarUnidades = new Regex(@"\d+");

        string[] marcasCerveja = { @"Skol", "Heineken", "Amstel", "Dalla", "Devassa", "Budweiser", "Brahma", "Original", "Antarctica",
                                    "Patagonia", "Bohemia", "Eisenbahn", "Corona","Stella", "Big John", "Coronita"
                                    , "Sol", "Estrella", "Itaipava", "Spaten", "Becks", "Petra", "Kaiser", 
                                    "LassBerg", "Kilsen", "Cristal", "Baly", "BellaVista", "Cabare" };

        string[] tipoCerveja = { @"Ipa", "Pale Ale", "Beats" };

        string[] caracteristicas = { @"Unfiltered", "Duplo Malte Escura", "Duplo Malte", "Zero", "Sem Alcool", "Malzbier", 
                                    "Sem Glúten", "Extra", "Puro Malte", "Amber", "Weisse" };

        string[] produtos = { @"Cerveja", "Chopp" };
        public void BuscarBrasaoeMoura(string link, string mercado)
        {
            if(driver == null)
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
                
                if (GetValue(TypeElement.Xpath, "/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div")
                        .element.FindElements(By.ClassName("list-product-item")).Count() > 0)
                {
                    try
                    {
                       
                        elements = GetValue(TypeElement.Xpath, "/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div")
                               .element.FindElements(By.ClassName("list-product-item"));

                       
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "Brasão");
                    }
                }
                else
                    Console.WriteLine("Não conseguiu buscar os dados do Brasão contador " + i);
                    


                if(elements.Count == 0)
                    return;
               

                Console.WriteLine("passa aqui");
            }

            foreach(var element in elements)
            {
                var item = new Item();

                //EXCESSOES
                string nome = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;
                
                if (nome.ToLower().Contains("chopp"))
                    continue;

                if (nome.ToLower().Contains("liverpool"))
                    continue;

                //MERCADO
                item.Mercado = mercado;

                //AJUSTAR PREÇO
                Match preco = montarPreco.Match(element.FindElement(By.ClassName("area-preco")).Text);
                item.Preco = Convert.ToDecimal(preco.Value);

                //AJUSTAR QUANTIDADE
                var buscarString = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;
                buscarString = buscarString.Replace(" Ml", "Ml");
                buscarString = buscarString.Replace("Litros", "L");
                buscarString = buscarString.Replace(" L", "L");

                Match quantidade = montarQuantidade.Match(buscarString);
                item.Quantidade = Convert.ToString(quantidade.Value).ToUpper();

                //AJUSTAR UNIDADE
                Match  unidade  = montarUnidade.Match(element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text);
                Match unidades = montarUnidades.Match(unidade.Value); 
 
                if (unidades.Value == "")
                {
                    Match unidade2 = montarUnidade.Match(element.FindElement(By.ClassName("badge-mob")).Text);
                    if(Convert.ToString(unidade2) != "")
                    {
                        Match unidades2 = montarUnidades.Match(unidade2.Value);
                        item.Unidade = Convert.ToInt32(unidades2.Value);
                    }
                    else
                        item.Unidade = 1;
                }
                    
                else
                    item.Unidade = Convert.ToInt32(unidades.Value);

                if (item.Unidade == 1 && item.Preco > 20 && (item.Quantidade == "350ML" || item.Quantidade == "300ML"))
                    continue;

                //AJUSTAR IMAGEM
                var imgElement = element.FindElement(By.ClassName("img-fluid"));
                item.Imagem = imgElement.GetAttribute("src");

                //AJUSTAR TIPO
                string tipo = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;

                foreach (var tipos in tipoCerveja)
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
                string marca = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text; 

                marca = marca.Replace("'", "");

                foreach (var marcas in marcasCerveja)
                {
                    if (marca.Normalize(NormalizationForm.FormD).Contains(marcas.Normalize(NormalizationForm.FormD)))
                    {
                        item.Marca = marcas;
                        break;
                    }
                        
                }

                //AJUSTAR CARACTERISTICA
                string tituloCerveja = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;

                foreach (var caracter in caracteristicas)
                {
                    if (tituloCerveja.ToLower().Normalize(NormalizationForm.FormD).Contains(caracter.ToLower().Normalize(NormalizationForm.FormD)))
                    {
                        if (caracter == "Zero" || caracter == "Sem Alcool")
                            item.Caracteristica = "Zero";
                        else if (caracter == "Puro Malte" && (item.Marca != "Skol" && item.Marca != "Itaipava"))
                            item.Caracteristica = "";
                        else
                            item.Caracteristica = caracter;

                        break;
                    }

                }

                //ECOMMERCE
                var ecommerce = element.FindElement(By.ClassName("list-product-link"));
                item.Ecommerce = ecommerce.GetAttribute("href");
                
                //DATA ATUALIZAÇÃO
                item.DataAtualizacao = DateTime.Now;

                //TITULO
                var titulo = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;

                foreach (var produto in produtos)
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

                    if (item.Marca != "" && item.Quantidade != "")
                          items.Add(item);
            }

            CloseBrowser();
            driver = null;
        }

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
                    Match preco = montarPreco.Match(precoAjustado[2].Text);
                    item.Preco = Convert.ToDecimal(preco.Value);
                }
                else
                {
                    Match preco = montarPreco.Match(precoAjustado[0].Text);
                    item.Preco = Convert.ToDecimal(preco.Value);
                }

                //AJUSTAR QUANTIDADE
                var buscarString = element.FindElement(By.ClassName("description")).Text;

                buscarString = buscarString.Replace(" Ml", "Ml");
                buscarString = buscarString.Replace("Litros", "L");
                buscarString = buscarString.Replace(" L", "L");

                Match quantidade = montarQuantidade.Match(buscarString);
                item.Quantidade = Convert.ToString(quantidade.Value).ToUpper();

                //AJUSTAR Unidade
                Match unidade = montarUnidade.Match(element.FindElement(By.ClassName("description")).Text);
                Match unidades = montarUnidades.Match(unidade.Value);
                if (unidades.Value == "")
                    item.Unidade = 1;
                else
                    item.Unidade = Convert.ToInt32(unidades.Value);

                //AJUSTAR IMAGEM

                var imgElement = element.FindElement(By.ClassName("img-container--product"));
                item.Imagem = imgElement.GetAttribute("src");

                //AJUSTAR TIPO
                string tipo = element.FindElement(By.ClassName("description")).Text;

                foreach (var tipos in tipoCerveja)
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

                foreach (var marcas in marcasCerveja)
                {
                    if (marca.ToLower().Contains(marcas.ToLower()))
                    {
                        item.Marca = marcas;
                        break;
                    }
                }

                //AJUSTAR CARACTERISTICA
                string tituloCerveja = element.FindElement(By.ClassName("description")).Text;

                foreach (var caracter in caracteristicas)
                {
                    if (tituloCerveja.ToLower().Normalize(NormalizationForm.FormD).Contains(caracter.ToLower().Normalize(NormalizationForm.FormD)))
                    {
                        if (caracter == "Zero" || caracter == "Sem Alcool")
                            item.Caracteristica = "Zero";
                        else if (caracter == "Puro Malte" && (item.Marca != "Skol" && item.Marca != "Itaipava"))
                            item.Caracteristica = "";
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

                foreach (var produto in produtos)
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

                if (item.Marca != "" && item.Tipo != "Beats" && item.Quantidade != "")
                        items.Add(item);

            }

            CloseBrowser();
            driver = null;

        }

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
