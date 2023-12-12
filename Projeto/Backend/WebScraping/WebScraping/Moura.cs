using EasyAutomationFramework;
using java.sql;
using OpenQA.Selenium;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Text;
using WebScraping.Model;
using WebScraping.Driver;
using sun.rmi.runtime;
using WebScraping.Repositorio;
using static sun.awt.geom.AreaOp;

namespace WebScraping.WebScraping
{
    public class Moura : Web
    {
        RepositorioWebScraping webScraper = new RepositorioWebScraping();

        RepositorioLogAuditoria auditoria = new RepositorioLogAuditoria();
        public void BuscarMoura(string link, string mercado)
        {
            webScraper.items.Clear();

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

                if (GetValue(TypeElement.Xpath, "/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div")
                        .element.FindElements(By.ClassName("list-product-item")).Count() > 0)
                {
                    try
                    {

                        elements = GetValue(TypeElement.Xpath, "/html/body/app-root/app-sm-master-page/main/section/app-departments/app-list-departments-products-category/app-grid-product-body/div/div")
                               .element.FindElements(By.ClassName("list-product-item"));

                        auditoria.Log("Elementos encontrados com sucesso, Moura");
                        break;
                    }
                    catch (Exception ex)
                    {
                        auditoria.Log("Erro: " + ex.Message + "Moura: " + link);
                    }
                }
                else
                    auditoria.Log("Erro: Não conseguiu buscar os dados do Moura contador " + i);



                if (elements.Count == 0)
                    return;


                Console.WriteLine("passa aqui");
            }

            foreach (var element in elements)
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
                Match preco = webScraper.montarPreco.Match(element.FindElement(By.ClassName("area-preco")).Text);
                item.Preco = Convert.ToDecimal(preco.Value);

                //AJUSTAR QUANTIDADE
                var buscarString = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;
                buscarString = buscarString.Replace(" Ml", "Ml");
                buscarString = buscarString.Replace("Litros", "L");
                buscarString = buscarString.Replace(" L", "L");

                Match quantidade = webScraper.montarQuantidade.Match(buscarString);
                item.Quantidade = Convert.ToString(quantidade.Value).ToUpper();

                //AJUSTAR UNIDADE
                Match unidade = webScraper.montarUnidade.Match(element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text);
                Match unidades = webScraper.montarUnidades.Match(unidade.Value);

                if (unidades.Value == "")
                {
                    Match unidade2 = webScraper.montarUnidade.Match(element.FindElement(By.ClassName("badge-mob")).Text);
                    if (Convert.ToString(unidade2) != "")
                    {
                        Match unidades2 = webScraper.montarUnidades.Match(unidade2.Value);
                        item.Unidade = Convert.ToInt32(unidades2.Value);
                    }
                    else
                        item.Unidade = 1;
                }

                else
                    item.Unidade = Convert.ToInt32(unidades.Value);

                if (item.Unidade == 1 && item.Preco > 30 && item.Quantidade != "5L")
                    continue;

                //AJUSTAR IMAGEM
                var imgElement = element.FindElement(By.ClassName("img-fluid"));
                item.Imagem = imgElement.GetAttribute("src");

                //AJUSTAR TIPO
                string tipo = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;

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
                string marca = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;

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
                string tituloCerveja = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;

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
                var ecommerce = element.FindElement(By.ClassName("list-product-link"));
                item.Ecommerce = ecommerce.GetAttribute("href");

                //DATA ATUALIZAÇÃO
                item.DataAtualizacao = DateTime.Now;

                //TITULO
                var titulo = element.FindElement(By.ClassName("txt-desc-product-itemtext-muted")).Text;

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
