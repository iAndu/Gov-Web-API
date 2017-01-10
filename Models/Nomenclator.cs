using Newtonsoft.Json;

namespace APIGovApp.Models
{
    public class Nomenclator
    {
        public int jud_cod { get; set; }
        public int cod { get; set; }
        public int cod_politie { get; set; }
        public string denumire { get; set; }
        public string tpl_cod { get; set; }
        public int cod_postal { get; set; }
        public string sar_cod { get; set; }
        public int loc_jud_cod { get; set; }
        public int cod_loc { get; set; }
        public string are_primarie { get; set; }
        public int cod_fiscal_primarie { get; set; }
        public int cod_politie_tata { get; set; }
        public string cod_sar_mf { get; set; }
        public int cod_siruta { get; set; }
        public int cod_siruta_tata { get; set; }
    }
}