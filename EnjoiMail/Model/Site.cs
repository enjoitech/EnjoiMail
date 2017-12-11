using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EnjoiMail.Model
{
    public class Site
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public static List<Site> LoadFromJson(string file)
        {
            try
            {
                using (var r = new StreamReader(file))
                {
                    var json = r.ReadToEnd();
                    var sites = JsonConvert.DeserializeObject<List<Site>>(json);

                    return sites;
                }
            }
            catch (Exception e)
            {
                return new List<Site>();
            }
        }

        public static void SaveToJson(List<Site> sites, string file)
        {
            try
            {
                using (var r = new StreamWriter(file))
                {
                    var json = JsonConvert.SerializeObject(sites);
                    r.Write(json);
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
