using APIGovApp.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace APIGovApp.Classes
{
    public class AccesAPI
    {
        private List<string> ModifiedJuds { get; set; }
        private bool ok { get; set; }
        private List<string> ModifiedJudsDates { get; set; }
        private bool[] ModifiedJudsVector { get; set; }

        String facets_url = "&facet.field=[%22results%22]&facet.limit=1&rows=1";
        String base_url = "http://data.gov.ro/api/3/action/package_search?q=";

        public List<string> GetModifiedJuds()
        {
            return this.ModifiedJuds;
        }

        public bool GetOk()
        {
            return ok;
        }

        public List<string> GetModifiedJudDates()
        {
            return ModifiedJudsDates;
        }

        public bool[] GetModifiedJudsBoolVector()
        {
            return ModifiedJudsVector;
        }

        public async Task<List<string>> GetDownloadUrlsAsync(string text)
        {
            List<string> urls_APIGOV = getJudete(text);
            ModifiedJuds = new List<string>();
            HttpClient client = new HttpClient();
            List<string> urls = new List<string>();
            List<GovRequest.Request> reqs = new List<GovRequest.Request>();
            int i = 1;
            if (urls_APIGOV.Count == 0)
            {
                ok = true;
            }
            foreach (var url_GOV in urls_APIGOV)
            {
                Stream stream;
                HttpResponseMessage response = await client.GetAsync(url_GOV);
                if (response.IsSuccessStatusCode)
                {
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(GovRequest.Request));
                    using (stream = await response.Content.ReadAsStreamAsync())
                    {
                        GovRequest.Request req = (GovRequest.Request)json.ReadObject(stream);
                        reqs.Add(req);
                    }
                    ok = true;
                }
                else
                {
                    ok = false;
                    break;
                }
            }
            if (ok == true)
            {
                int j = 0;
                foreach (var req in reqs)
                {
                    string url;
                    while (ModifiedJudsVector[i] == false)
                        ++i;
                    int k = 0;
                    while (k < req.result.results[0].resources.Count
                        && !(req.result.results[0].resources[k].url.Contains("nomlocalitati")))
                        ++k;
                    if (k < req.result.results[0].resources.Count
                        && ModifiedJudsDates[i - 1] != req.result.results[0].resources[k].last_modified)
                    {
                        ModifiedJudsDates[i - 1] = req.result.results[0].resources[k].last_modified;
                        url = req.result.results[0].resources[k].url;
                        urls.Add(url);
                        string judName = urls_APIGOV[j++].Remove(0, base_url.Length);
                        judName = judName.Substring(0, judName.Length - facets_url.Length).Split('-').Last();
                        ModifiedJuds.Add(judName);
                    }
                    else
                    {
                        ModifiedJudsVector[i] = false;
                    }
                    ++i;
                    if (i > 42)
                        break;
                }
            }
            if (ModifiedJuds.Count > 0)
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Resources/Date.txt");
                StreamWriter writer = new StreamWriter(path);
                foreach (var str in ModifiedJudsDates)
                {
                    writer.WriteLine(str);
                }
                writer.Close();
            }
            if (urls.Count == 0)
                return null;
            return urls;
        }

        public List<string> getJudete(string text)
        {
            String current_jud, full_url, date;
            int i = 1;
            ModifiedJudsVector = new bool[43];
            ModifiedJudsDates = new List<string>();
            List<string> jud = new List<string>();
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Resources/judete.txt");
            StreamReader reader = new StreamReader(path);
            path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Resources/Date.txt");
            StreamReader reader1 = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                date = reader1.ReadLine();
                ModifiedJudsDates.Add(date);
                current_jud = reader.ReadLine();
                current_jud = current_jud.Substring(0, current_jud.Length - 2);
                full_url = string.Concat(base_url, current_jud, facets_url);
                if (String.IsNullOrEmpty(text) || full_url.Contains(text))
                {
                    jud.Add(full_url);
                    ModifiedJudsVector[i] = true;
                    ++i;
                }
            }
            reader.Close();
            reader1.Close();
            return jud;
        }
    }
}