using EasyAutomationFramework;
using EasyAutomationFramework.Model;
using WebScraping.Driver;

var web = new WebScraper();
web.BuscarBrasaoeMoura("https://www.sitemercado.com.br/brasaochapeco/chapeco-loja-avenida-digital-centro-rua-rio-de-janeiro/produtos/bebidas-alcoolicas/cervejas/cerveja-nacional", "Brasão");
web.BuscarBrasaoeMoura("https://www.sitemercado.com.br/mourasupermercados/chapeco-loja-sao-cristovao-sao-cristovao-av-sao-pedro/produtos/bebidas-alcoolicas/cervejas", "Moura");
web.BuscarCeleiro("https://www.celeiro.com.br/produtos/departamento/bebidas/cerveja-tradicional");
web.BuscarCeleiro("https://www.celeiro.com.br/produtos/departamento/bebidas/cerveja-tradicional?page=2");




