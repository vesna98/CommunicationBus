using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONToXMLAdapter
{
    public class XMLAdapterKlasa
    {
        public string JsonZahtev { get; set; }


        public XMLAdapterKlasa(string zahtev)
        {
            JsonZahtev = zahtev;
        }


        public string ConvertToXML()
        {
            string temp = "";

            string[] tokeni = JsonZahtev.Split(',');

            temp += "<request>\n<verb>";
            string verb = "-";
            string noun = "-";
            string query = "-";
            string fields = "-";
            string connectedTo = "-";
            string connectedType = "-";
            for (int i = 0; i < tokeni.Length; i++)
            {
                if (tokeni[i].Contains("verb"))
                {
                    verb = tokeni[i].Split(new string[] { "verb\":\"" }, StringSplitOptions.None)[1]; //GET",
                    verb = verb.Split('\"')[0];
                }
                else if (tokeni[i].Contains("noun"))
                {
                    noun = tokeni[i].Split(new string[] { "noun\":\"" }, StringSplitOptions.None)[1];
                    noun = noun.Split('\"')[0];
                }
                else if (tokeni[i].Contains("query"))
                {
                    query = tokeni[i].Split(new string[] { "query\":\"" }, StringSplitOptions.None)[1];
                    query = query.Split('\"')[0];
                }
                else if (tokeni[i].Contains("fields"))
                {
                    fields = tokeni[i].Split(new string[] { "fields\":\"" }, StringSplitOptions.None)[1];
                    fields = fields.Split('\"')[0];
                }
                else if (tokeni[i].Contains("connectedTo"))
                {
                    connectedTo = tokeni[i].Split(new string[] { "connectedTo\":\"" }, StringSplitOptions.None)[1];
                    connectedTo = connectedTo.Split('\"')[0];
                }
                else if (tokeni[i].Contains("connectedType"))
                {
                    connectedType = tokeni[i].Split(new string[] { "connectedType\":\"" }, StringSplitOptions.None)[1];
                    connectedType = connectedType.Split('\"')[0];
                }
            }

            temp += verb + "</verb>\n<noun>" + noun + "</noun>";

            //ciste provere i dodavanje po potrebi
            if (!query.Equals("-"))
            {
                temp += "\n<query>" + query + "</query>";
            }

            if (!fields.Equals("-"))
            {
                temp += "\n<fields>" + fields + "</fields>";
            }

            if (!connectedTo.Equals("-"))
            {
                temp += "\n<connectedTo>" + connectedTo + "</connectedTo>";
            }

            if (!connectedType.Equals("-"))
            {
                temp += "\n<connectedType>" + connectedType + "</connectedType>";
            }

            temp += "\n</request>";

            return temp;


        }

        public string BackToJSON(string poruka)
        {
            string temp = "";
            string[] tokens = poruka.Split('>');    //<response, <status, SUCCESS</staus, <code ,2000</code, <payload, id..name.... </payload,.....
            string status = tokens[2].Replace("</status", "");
            string kod = tokens[4].Replace("</code", "");
            string payload = tokens[6].Replace("</payload", "");
            // payload = payload.Replace(",", ",\n");

            payload = payload.Replace("'", "\"");
            payload = payload.Replace("=", ":");
            if (status.Equals("REJECTED"))
            {
                temp = "{\"status\":\"" + status + "\",\"code\":\"" + kod + "\",\"payload\":{" + payload + "}}";
            }
            else
            {//redove tab uraditi
                temp = "{\"status\":\"" + status + "\",\"code\":\"" + kod + "\",\"payload\":{" + payload + "}}";
            }
            return temp;
        }
    }
}
