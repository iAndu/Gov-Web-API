using APIGovApp.Classes;

using APIGovApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System;

namespace APIGovApp.Controllers
{
    public class APIController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        public async System.Threading.Tasks.Task<ActionResult> GetResults()
        {
            string token = Utils.EncodeMessage(Request.Params["token"]);

            string connectionString = "Server=tcp:apigov.database.windows.net,1433;Initial Catalog=authdb;Persist Security Info=False;User ID=adminapi;Password=Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
            string queryString = "SELECT AccessType FROM dbo.AuthTable WHERE AccessToken = @token;";
            int accessType = 0;

            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@token", token);

                // Open the connection in a try/catch block. 
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        accessType = Convert.ToInt32(reader[0]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            ErrorMessage err = new ErrorMessage();

            #region Setare filter

            Nomenclator req = new Nomenclator();
            int check;
            check = Utils.ValidInteger(Utils.EncodeMessage(Request.Params["jud_cod"]));
            if (check >= 0)
            {
                req.jud_cod = check;
            }
            else
            {
                err = Utils.SendErrorMessage(401);
                return View("Error", err);
            }

            check = Utils.ValidInteger(Utils.EncodeMessage(Request.Params["cod"]));
            if (check >= 0)
            {
                req.cod = check;
            }
            else
            {
                err = Utils.SendErrorMessage(402);
                return View("Error", err);
            }

            check = Utils.ValidInteger(Utils.EncodeMessage(Request.Params["cod_politie"]));
            if (check >= 0)
            {
                req.cod_politie = check;
            }
            else
            {
                err = Utils.SendErrorMessage(403);
                return View("Error", err);
            }

            req.denumire = Utils.EncodeMessage(Request.Params["denumire"]);
            req.tpl_cod = Utils.EncodeMessage(Request.Params["cod_tpl"]);
            check = Utils.ValidInteger(Utils.EncodeMessage(Request.Params["cod_postal"]));
            if (check >= 0)
            {
                req.cod_postal = check;
            }
            else
            {
                err = Utils.SendErrorMessage(404);
                return View("Error", err);
            }
            req.sar_cod = Utils.EncodeMessage(Request.Params["cod_sar"]);
            check = Utils.ValidInteger(Utils.EncodeMessage(Request.Params["loc_jud_cod"]));
            if (check >= 0)
            {
                req.loc_jud_cod = check;
            }
            else
            {
                err = Utils.SendErrorMessage(405);
                return View("Error", err);
            }
            check = Utils.ValidInteger(Utils.EncodeMessage(Request.Params["cod_loc"]));
            if (check >= 0)
            {
                req.cod_loc = check;
            }
            else
            {
                err = Utils.SendErrorMessage(406);
                return View("Error", err);
            }
            req.are_primarie = Utils.EncodeMessage(Request.Params["are_primarie"]);
            check = Utils.ValidInteger(Utils.EncodeMessage(Request.Params["cod_fiscal_primarie"]));
            if (check >= 0)
            {
                req.cod_fiscal_primarie = check;
            }
            else
            {
                err = Utils.SendErrorMessage(407);
                return View("Error", err);
            }
            req.cod_sar_mf = Utils.EncodeMessage(Request.Params["sar_cod_mf"]);
            check = Utils.ValidInteger(Utils.EncodeMessage(Request.Params["cod_siruta"]));
            if (check >= 0)
            {
                req.cod_siruta = check;
            }
            else
            {
                err = Utils.SendErrorMessage(408);
                return View("Error", err);
            }
            check = Utils.ValidInteger(Utils.EncodeMessage(Request.Params["cod_siruta_tata"]));
            if (check >= 0)
            {
                req.cod_siruta_tata = check;
            }
            else
            {
                err = Utils.SendErrorMessage(409);
                return View("Error", err);
            }

            #endregion Setare filter

            #region Actualizare Date

            // Link-uri catre XML-urile de actualizat si numele judetelor actualizate
            AccesAPI apiReq = new AccesAPI();
            List<string> downloadUrls = new List<string>();
            if (!string.IsNullOrEmpty(req.denumire))
            {
                req.denumire = req.denumire.ToLower();
            }
            downloadUrls = await apiReq.GetDownloadUrlsAsync(req.denumire);
            List<string> modifiedJuds = new List<string>();
            modifiedJuds = apiReq.GetModifiedJuds();

            bool ok = apiReq.GetOk();
            if (ok == false)
            {
                err = Utils.SendErrorMessage(500);
                return View("Error", err);
            }

            // Daca exista ceva de actualizat
            if (downloadUrls != null)
            {
                List<string> jsons = new List<string>();
                jsons = Utils.Xml2Json(downloadUrls);
                SaveFile.WriteContent(modifiedJuds, jsons);
            }

            #endregion Actualizare Date

            #region Load list

            List<Root> raw = new List<Root>();
            string[] files = Directory.GetFiles(HostingEnvironment.MapPath("~/Content/Files"));
            foreach (string path in files)
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string json = sr.ReadToEnd();
                    Root root = JsonConvert.DeserializeObject<Root>(json);
                    raw.Add(root);
                }
            }

            List<Nomenclator> list = new List<Nomenclator>();
            foreach (Root x in raw)
            {
                list.AddRange(x.nom_localitati.noms);
            }

            #endregion Load list

            #region Filtrare rezutlate

            IEnumerable<Nomenclator> array = list;
            if (req.jud_cod > 0)
            {
                array = array.Where(x => x.jud_cod == req.jud_cod);
            }
            if (req.cod > 0)
            {
                array = array.Where(x => x.cod == req.cod);
            }
            if (req.cod_politie > 0)
            {
                array = array.Where(x => x.cod_politie == req.cod_politie);
            }
            if (req.denumire != null)
            {
                array = array.Where(x => x.denumire.ToLower() == req.denumire.ToLower());
            }
            if (req.tpl_cod != null)
            {
                array = array.Where(x => x.tpl_cod.ToLower() == req.tpl_cod.ToLower());
            }
            if (req.cod_postal > 0)
            {
                array = array.Where(x => x.cod_postal == req.cod_postal);
            }
            if (req.sar_cod != null)
            {
                array = array.Where(x => x.sar_cod.ToLower() == req.sar_cod.ToLower());
            }
            if (req.loc_jud_cod > 0)
            {
                array = array.Where(x => x.loc_jud_cod == req.loc_jud_cod);
            }
            if (req.cod_loc > 0)
            {
                array = array.Where(x => x.cod_loc == req.cod_loc);
            }
            if (req.are_primarie != null)
            {
                array = array.Where(x => x.are_primarie.ToLower() == req.are_primarie.ToLower());
            }
            if (req.cod_fiscal_primarie > 0)
            {
                array = array.Where(x => x.cod_fiscal_primarie == req.cod_fiscal_primarie);
            }
            if (req.cod_politie_tata > 0)
            {
                array = array.Where(x => x.cod_politie_tata == req.cod_politie_tata);
            }
            if (req.cod_sar_mf != null)
            {
                array = array.Where(x => x.cod_sar_mf.ToLower() == req.cod_sar_mf.ToLower());
            }
            if (req.cod_siruta > 0)
            {
                array = array.Where(x => x.cod_siruta == req.cod_siruta);
            }
            if (req.cod_siruta_tata > 0)
            {
                array = array.Where(x => x.cod_siruta_tata == req.cod_siruta_tata);
            }
            if (array.ToList().Count == 0)
            {
                err = Utils.SendErrorMessage(410);
                return View("Error", err);
            }

            if (accessType == 0)
            {
                foreach (Nomenclator x in array)
                {
                    x.cod = 0;
                    x.cod_politie = 0;
                    x.tpl_cod = null;
                    x.sar_cod = null;
                    x.loc_jud_cod = 0;
                    x.cod_loc = 0;
                    x.cod_fiscal_primarie = 0;
                    x.cod_politie_tata = 0;
                    x.cod_sar_mf = null;
                    x.cod_siruta = 0;
                    x.cod_siruta_tata = 0;
                }
            }

            if (accessType == 1)
            {
                foreach (Nomenclator x in array)
                {
                    x.cod = 0;
                    x.cod_politie = 0;
                    x.tpl_cod = null;
                    x.sar_cod = null;
                    x.cod_fiscal_primarie = 0;
                    x.cod_politie_tata = 0;
                    x.cod_sar_mf = null;
                    x.cod_siruta = 0;
                    x.cod_siruta_tata = 0;
                }
            }
            #endregion Filtrare rezutlate

            ViewModel model = new ViewModel();
            model.nomenclatoare = array;
            return View("GetResults", model);
        }
    }
}