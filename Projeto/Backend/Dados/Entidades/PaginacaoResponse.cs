namespace Dados.Entidades
{
    public class PaginacaoResponse<T>
    {
        public long Linhas { get; set; } = 10;
        public long TotalLinhas { get; set; } = 0;
        public long Pagina { get; set; } = 1;
        public long TotalPaginas { get; set; } = 0;
        public string Pesquisa { get; set; } = "";
        public List<T> Dados { get; set; } = new();
    }
}
