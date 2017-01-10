using System.Collections.Generic;
using APIGovApp.Models;
using Newtonsoft.Json;

namespace APIGovApp.Classes
{
    public class XmlModel
    {
        public string Staff { get; set; }
        [JsonProperty(PropertyName = "rand")]
        public List<Nomenclator> noms { get; set; }
    }

    public class Root
    {
        public XmlModel nom_localitati { get; set; }
    }
}