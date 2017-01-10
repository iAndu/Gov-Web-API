using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using APIGovApp.Models;
using System.Web;

namespace APIGovApp.Classes
{
    public class Utils
    {
        public static string ReplaceUnicodeWithAscii(string s)
        {
            s = s.Replace("Ã®", "i");
            s = s.Replace("ÅŸ", "s");
            s = s.Replace("Å¢", "T");
            s = s.Replace("Äƒ", "a");
            s = s.Replace("Å£", "t");
            s = s.Replace("Ã¢", "a");
            s = s.Replace("ÃŽ", "I");
            s = s.Replace("Åž", "S");
            return s;
        }

        public static List<string> Xml2Json(List<string> lista_url)
        {
            List<string> json_string = new List<string>();
            string json = null;

            foreach (string s in lista_url)
            {
                var web_path = s;

                string xmlString;
                using (var web = new WebClient())
                {
                    xmlString = web.DownloadString(web_path);
                }

                xmlString = ReplaceUnicodeWithAscii(xmlString);

                XmlDocument xml_doc = new XmlDocument();
                xml_doc.LoadXml(xmlString);

                xml_doc.RemoveChild(xml_doc.FirstChild);

                //xml to string
                StringWriter sw = new StringWriter();
                XmlTextWriter tx = new XmlTextWriter(sw);
                xml_doc.WriteTo(tx);

                string str = sw.ToString();

                XmlDocument xml_final = new XmlDocument();
                xml_final.LoadXml(str);

                json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xml_final);
                json_string.Add(json);
            }

            return json_string;
        }

        public static bool UrlExists(string url)
        {
            try
            {
                new WebClient().DownloadData(url);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error on GetUrl" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
        }

        public static int ValidInteger(string text)
        {
            bool output = true;
            int number = 0;
            output = int.TryParse(text, out number);
            if ((output && number >= 0) || text == null)
            {
                return number;
            }
            else
            {
                return -1;
            }
        }

        public static ErrorMessage SendErrorMessage(int err_code)
        {
            ErrorMessage e = new ErrorMessage();
            e.code = err_code;
            switch(err_code)
            {
                case 401:
                    e.error = "invalid integer input for jud_cod";
                    break;
                case 402:
                    e.error = "invalid integer input for cod";
                    break;
                case 403:
                    e.error = "invalid integer input for cod_politie";
                    break;
                case 404:
                    e.error = "invalid integer input for cod_postal";
                    break;
                case 405:
                    e.error = "invalid integer input for loc_jud_cod";
                    break;
                case 406:
                    e.error = "invalid integer input for cod_loc";
                    break;
                case 407:
                    e.error = "invalid integer input for cod_fiscal_primarie";
                    break;
                case 408:
                    e.error = "invalid integer input for cod_siruta";
                    break;
                case 409:
                    e.error = "invalid integer input for cod_siruta_tata";
                    break;
                case 410:
                    e.error = "invalid string parameter givenor no results";
                    break;
                case 411:
                    e.error = "invalid token given";
                    break;
                case 500:
                    e.error = "server timeout";
                    break;
                default:
                    e.error = "unexpected error";
                    break;
            }
            if (e.code / 100 == 4)
            {
                e.type = "client";
            } else if (e.code / 100 == 5) {
                e.type = "server";
            } else
            {
                e.type = "unkown";
            }

            return e;
        }

        public static String EncodeMessage(String msg)
        {
            msg = HttpContext.Current.Server.HtmlEncode(msg);
            return msg;
        }
    }
}