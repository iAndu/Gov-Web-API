using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIGovApp.Classes
{
    public class SaveFile
    {

        // Metoda care rescrie continutul unui fisier
        // Parametrii: nume fisier, continutul ce trebuie scris
        public static void WriteContent(List<String> filename, List<String> content)
        {
            int c = 0;
            foreach (string value in filename)
            {
                String path = System.Web.Hosting.HostingEnvironment.MapPath("/Content/Files/" + value + ".json");
                File.WriteAllText(path, content[c]);    // rescrie in fisier continutul dat

                c++;
            }
        }
    }
}