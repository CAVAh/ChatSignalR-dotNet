/**
 * Classe transiente para controlar o filtro que vem do cliente.
 * tomar como base o seguinte esquema para criar as condições de filtro (filter conditions)
 * https://github.com/nestjsx/crud/wiki/Requests
 */
namespace ChatSignalR.Models
{
    public class Filtro
    {

        public Filtro() { }

        public Filtro(string filter)
        {
            string[] partesDoFiltro = filter.Split("?");

            for (int i = 0; i < partesDoFiltro.Length; i++)
            {
                string abreParenteses = "";
                string fechaParenteses = "";
                string parteFiltro = partesDoFiltro[i];
                if (parteFiltro.Contains("("))
                {
                    abreParenteses = "(";
                    parteFiltro = parteFiltro.Replace("(", "");
                }

                if (parteFiltro.Contains(")"))
                {
                    fechaParenteses = ")";
                    parteFiltro = parteFiltro.Replace(")", "");
                }

                string[] condicoes = parteFiltro.Split("||");                

                if (i > 0)
                {
                    string operador = "AND";

                    string[] operadores = condicoes[0].Split("=");

                    if (operadores.Length > 1 && operadores[0].Contains("OR"))
                    {
                        operador = "OR";
                        condicoes[0] = operadores[1];
                    }

                   
                    Where = Where + " " + operador + " ";
                }

                // $cont (LIKE %val%, contains)
                if (condicoes[1] == "$cont")
                {
                    Campo = condicoes[0];
                    Valor = condicoes[2];
                    Where = Where + abreParenteses + Campo + " like '%" + Valor + "%'" + fechaParenteses;
                }

                // $eq (=, equal)
                if (condicoes[1] == "$eq")
                {
                    Campo = condicoes[0];
                    Valor = condicoes[2];
                    Where = Where + abreParenteses + Campo + " = '" + Valor + "'" + fechaParenteses;
                }

                // $neq (<>, not equal)
                if (condicoes[1] == "$neq")
                {
                    Campo = condicoes[0];
                    Valor = condicoes[2];
                    Where = Where + abreParenteses + Campo + " <> '" + Valor + "'" + fechaParenteses;
                }

                // $between (BETWEEN 'DATA1' and 'DATA2')
                if (condicoes[1] == "$between")
                {
                    string[] datas = condicoes[2].Split(",");
                    Campo = condicoes[0];
                    DataInicial = datas[0];
                    DataFinal = datas[1];
                    Where = Where + abreParenteses + Campo + " between '" + DataInicial + "' and '" + DataFinal + "'" + fechaParenteses;
                }

                if(condicoes[1] == "$isnull")
                {
                    Campo = condicoes[0];
                    Where = Where + abreParenteses + Campo + " IS NULL " + fechaParenteses;  
                }
            }
        }

        public string Campo { get; set; }
        public string Valor { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }

        public string Where { get; set; }
    }
}
