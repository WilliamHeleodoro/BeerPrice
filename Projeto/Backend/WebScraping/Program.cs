using WebScraping.WebScraping;

var superVerde = new Moura();
superVerde.BuscarMoura("https://www.sitemercado.com.br/mourasupermercados/chapeco-loja-sao-cristovao-sao-cristovao-av-sao-pedro/produtos/bebidas-alcoolicas/cervejas", "Moura");

var superRoxo = new Celeiro();
superRoxo.BuscarCeleiro("https://www.celeiro.com.br/produtos/departamento/bebidas/cerveja-tradicional", "Celeiro");
superRoxo.BuscarCeleiro("https://www.celeiro.com.br/produtos/departamento/bebidas/cerveja-tradicional?page=2", "Celeiro");

var superVermelho = new Brasao();
superVermelho.BuscarBrasao("https://www.brasao.com.br/avenida/bebidas-189/?subtermo=Cerveja", "Brasão");
superVermelho.BuscarBrasao("https://www.brasao.com.br/avenida/bebidas-189/?subtermo=Cerveja&page=2", "Brasão");
superVermelho.BuscarBrasao("https://www.brasao.com.br/avenida/bebidas-189/?subtermo=Cerveja&page=3", "Brasão");
superVermelho.BuscarBrasao("https://www.brasao.com.br/avenida/bebidas-189/?subtermo=Cerveja&page=4", "Brasão");
superVermelho.BuscarBrasao("https://www.brasao.com.br/avenida/bebidas-189/?subtermo=Cerveja&page=5", "Brasão");
superVermelho.BuscarBrasao("https://www.brasao.com.br/avenida/bebidas-189/?subtermo=Cerveja&page=6", "Brasão");
superVermelho.BuscarBrasao("https://www.brasao.com.br/avenida/bebidas-189/?subtermo=Cerveja&page=7", "Brasão");
superVermelho.BuscarBrasao("https://www.brasao.com.br/avenida/bebidas-189/?subtermo=Cerveja&page=8", "Brasão");
superVermelho.BuscarBrasao("https://www.brasao.com.br/avenida/bebidas-189/?subtermo=Cerveja&page=9", "Brasão");

