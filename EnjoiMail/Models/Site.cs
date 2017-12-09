using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EnjoiMail.Models
{
    public class Site
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public List<Site> LoadJson(string file)
        {
            using (var r = new StreamReader("sites.json"))
            {
                var json = r.ReadToEnd();
                var sites = JsonConvert.DeserializeObject<List<Site>>(json);

                return sites;
            }
        }
    }
}
