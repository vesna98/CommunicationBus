using System;
using System.Collections.Generic;

namespace RESTim10.Controllers
{
    public partial class Resurs
    {
        public int IdResurs { get; set; }
        public string NazivR { get; set; }
        public string Opis { get; set; }
        public int? TipId { get; set; }

        public override string ToString()
        {
            string xmlopis = "";
            int br = 0;
            string[] tokens = Opis.Split('\"');
            for (int i = 1; i < tokens.Length - 1; i++)
            {
                if (tokens[i].Equals(":"))
                {
                    xmlopis += "=";
                }
                else if (int.TryParse(tokens[i], out br))
                {
                    xmlopis += "'" + tokens[i] + "'";
                }
                else if (tokens[i].Equals(","))
                {
                    xmlopis += ",";
                }
                else
                {
                    xmlopis += "'" + tokens[i] + "'";
                }

            }   //id=1,name='pera',....
            return xmlopis;
        }

    }
}
