using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    public class WebClientKlasa:IWebClient
    {
        public string Zahtev { get; set; }

        public WebClientKlasa(string zahtev)
        {
            Zahtev = zahtev;

        }

        public string ConvertToJSON()
        {
            string temp = "";

            string[] tokens = Zahtev.Split(' ');
            temp += "{\n" + "\"verb\":\"" + tokens[0] + "\",\n\"noun\":\"";// + tokens[1] + "\"\n";

            string filter = ToDoFilter(tokens[1]);

            if (filter.Equals("nema"))
            {
                temp += tokens[1] + "\"\n}";
                return temp;
            }

            if (tokens[1].Contains("resurs"))
            {
                //sva 4
                if (filter.Equals("?"))
                {
                    string[] sledeci = tokens[1].Split('?');
                    if (sledeci[1].Contains('|') && !sledeci[1].Contains('#') && !sledeci[1].Contains('%'))
                    {
                        string query = sledeci[1].Split('|')[0];

                        //--------
                        query = query.Replace("&", ";");
                        //---------
                        string fields = sledeci[1].Split('|')[1];
                        temp += sledeci[0] + "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\"\n}";
                        return temp;
                    }
                    else if (sledeci[1].Contains('|') && sledeci[1].Contains('#') && !sledeci[1].Contains('%'))
                    {
                        string prviObrada = ToDoFilter(sledeci[1]);
                        if (prviObrada.Equals("|"))
                        {
                            string query = sledeci[1].Split('|')[0];
                            query = query.Replace("&", ";");
                            string fieldsSaType = sledeci[1].Split('|')[1];
                            string fields = fieldsSaType.Split('#')[0];
                            string connType = fieldsSaType.Split('#')[1];
                            //ide connwcted type

                            //BEZ ZAGRADA
                            connType = connType.Replace("(", "");
                            connType = connType.Replace(")", "");
                            temp += sledeci[0] + "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\",\n\"connectedType\":\"" + connType + "\"\n}";
                            return temp;
                        }
                        else
                        {
                            string query = sledeci[1].Split('#')[0];
                            query = query.Replace("&", ";");
                            string fieldsSaType = sledeci[1].Split('#')[1];
                            string connType = fieldsSaType.Split('|')[0];
                            string fields = fieldsSaType.Split('|')[1];
                            connType = connType.Replace("(", "");
                            connType = connType.Replace(")", "");
                            //ide connwcted type
                            temp += sledeci[0] + "\",\n\"query\":\"" + query + "\",\n\"fields\":" + fields + "\",\n\"connectedType\":" + connType + "\"\n}";
                        }
                    }
                    else if (sledeci[1].Contains('|') && !sledeci[1].Contains('#') && sledeci[1].Contains('%'))
                    {
                        string prviObrada = ToDoFilter(sledeci[1]);
                        if (prviObrada.Equals("|"))
                        {
                            string query = sledeci[1].Split('|')[0];
                            query = query.Replace("&", ";");
                            string fieldsSaType = sledeci[1].Split('|')[1];
                            string fields = fieldsSaType.Split('%')[0];
                            string connTo = fieldsSaType.Split('%')[1];
                            connTo = connTo.Replace("(", "");
                            connTo = connTo.Replace(")", "");
                            //ide connwcted type
                            temp += sledeci[0] + "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\",\n\"connectedTo\":\"" + connTo + "\"\n}";
                        }
                        else
                        {
                            string query = sledeci[1].Split('%')[0];
                            query = query.Replace("&", ";");
                            string fieldsSaType = sledeci[1].Split('%')[1];
                            string connTo = fieldsSaType.Split('|')[0];
                            string fields = fieldsSaType.Split('|')[1];
                            //ide connwcted type
                            connTo = connTo.Replace("(", "");
                            connTo = connTo.Replace(")", "");
                            temp += sledeci[0] + "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\",\n\"connectedTo\":\"" + connTo + "\"\n}";
                        }
                    }
                    else if (sledeci[1].Contains('|') && sledeci[1].Contains('#') && sledeci[1].Contains('%'))
                    {
                        string prviObrada = ToDoFilter(sledeci[1]);
                        if (prviObrada.Equals("|"))
                        {
                            return temp += sledeci[0] + FieldsFirst(sledeci[1]);
                        }
                        else if (prviObrada.Equals("#"))
                        {
                            return temp += sledeci[0] + TypeFirst(sledeci[1]);
                        }
                        else
                        {
                            //%
                            return temp += sledeci[0] + ToFirst(sledeci[1]);
                        }
                    }
                    else
                    {
                        //sam query
                        string query = sledeci[1];
                        query = query.Replace("&", ";");
                        temp += sledeci[0] + "\",\n\"query\":\"" + query + "\"\n}";
                        return temp;
                    }
                }


            }
            else //if (tokens[0].Contains("tip") || tokens[0].Contains("tipveze")  || veza)
            {
                if (filter.Equals("?"))
                {
                    string[] sledeci = tokens[1].Split('?');
                    if (sledeci[1].Contains('|'))
                    {
                        string query = sledeci[1].Split('|')[0];
                        query = query.Replace("&", ";");
                        string fields = sledeci[1].Split('|')[1];
                        temp += sledeci[0] + "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\"\n}";
                        return temp;
                    }
                    else
                    {
                        //sam query
                        string query = sledeci[1];
                        query = query.Replace("&", ";");
                        temp += sledeci[0] + "\",\n\"query\":\"" + query + "\"\n}";
                        return temp;
                    }
                }

                //DODATI ZA FIELDS PRVI
                if (filter.Equals("|"))
                {
                    string[] sledeci = tokens[1].Split('|');
                    if (sledeci[1].Contains('?'))
                    {
                        string fields = sledeci[1].Split('?')[0];
                        string query = sledeci[1].Split('?')[1];
                        query = query.Replace("&", ";");
                        temp += sledeci[0] + "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\"\n}";
                        return temp;
                    }
                    else
                    {
                        //sam query
                        string fields = sledeci[1];

                        temp += sledeci[0] + "\",\n\"fields\":\"" + fields + "\"\n}";
                        return temp;
                    }
                }

            }

            return temp;
        }

        public void Show(string poruka)
        {
            
            Console.WriteLine(poruka);
            
            Console.WriteLine("=========================================================================================================================\n");
        }

        public string ToDoFilter(string poruka)
        {
            char prviFilter;
            for (int i = 0; i < poruka.Length; i++)
            {
                prviFilter = poruka[i];
                if (prviFilter.Equals('?')) { return "?"; }
                else if (prviFilter.Equals('|')) { return "|"; }
                else if (prviFilter.Equals('#')) { return "#"; }
                else if (prviFilter.Equals('%')) { return "%"; }
            }
            return "nema";
        }

        public string FieldsFirst(string poruka)
        {
            string query = poruka.Split('|')[0];
            query = query.Replace("&", ";");
            string drugiFilter = ToDoFilter(poruka.Split('|')[1]);
            if (drugiFilter.Equals("#"))
            {
                string fieldsSaType = poruka.Split('|')[1];
                string fields = fieldsSaType.Split('#')[0];
                string connTypeSaconnTo = fieldsSaType.Split('#')[1];
                string connType = connTypeSaconnTo.Split('%')[0];
                string connTo = connTypeSaconnTo.Split('%')[1];

                connTo = connTo.Replace("(", "");
                connTo = connTo.Replace(")", "");

                connType = connType.Replace("(", "");
                connType = connType.Replace(")", "");
                return "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\",\n\"connectedType\":\"" + connType + "\",\n\"connectedTo\":\"" + connTo + "\"\n}";
            }
            else
            {
                string fieldsSaType = poruka.Split('|')[1];
                string fields = fieldsSaType.Split('%')[0];
                string connTypeSaconnTo = fieldsSaType.Split('%')[1];
                string connTo = connTypeSaconnTo.Split('#')[0];
                string connType = connTypeSaconnTo.Split('#')[1];


                connTo = connTo.Replace("(", "");
                connTo = connTo.Replace(")", "");

                connType = connType.Replace("(", "");
                connType = connType.Replace(")", "");
                return "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\",\n\"connectedType\":\"" + connType + "\",\n\"connectedTo\":\"" + connTo + "\"\n}";
            }
        }

        public string ToFirst(string poruka)
        {
            string query = poruka.Split('%')[0];
            query = query.Replace("&", ";");
            string drugiFilter = ToDoFilter(poruka.Split('%')[1]);
            if (drugiFilter.Equals("|"))
            {
                string toFields = poruka.Split('%')[1];
                string connTo = toFields.Split('|')[0];
                string fields = toFields.Split('#')[0];
                fields = fields.Split('|')[1];
                string connType = toFields.Split('#')[1];

                connTo = connTo.Replace("(", "");
                connTo = connTo.Replace(")", "");

                connType = connType.Replace("(", "");
                connType = connType.Replace(")", "");
                return "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\",\n\"connectedType\":\"" + connType + "\",\n\"connectedTo\":\"" + connTo + "\"\n}";
            }
            else
            {
                string toType = poruka.Split('%')[1];
                string connTo = toType.Split('#')[0];
                string fields = toType.Split('|')[1];
                string connType = toType.Split('|')[0];
                connType = connType.Split('#')[1];
                connTo = connTo.Replace("(", "");
                connTo = connTo.Replace(")", "");

                connType = connType.Replace("(", "");
                connType = connType.Replace(")", "");
                return "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\",\n\"connectedType\":\"" + connType + "\",\n\"connectedTo\":\"" + connTo + "\"\n}";
            }
        }

        public string TypeFirst(string poruka)
        {
            string query = poruka.Split('#')[0];
            query = query.Replace("&", ";");
            string drugiFilter = ToDoFilter(poruka.Split('#')[1]);
            if (drugiFilter.Equals("|"))
            {
                string typeFields = poruka.Split('#')[1];
                string connType = typeFields.Split('|')[0];
                string fields = typeFields.Split('%')[0];
                fields = fields.Split('|')[1];
                string connTo = typeFields.Split('%')[1];

                connTo = connTo.Replace("(", "");
                connTo = connTo.Replace(")", "");

                connType = connType.Replace("(", "");
                connType = connType.Replace(")", "");
                return "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\",\n\"connectedType\":\"" + connType + "\",\n\"connectedTo\":\"" + connTo + "\"\n}";
            }
            else
            {
                string typeto = poruka.Split('#')[1];
                string connType = typeto.Split('%')[0];
                string connTo = typeto.Split('|')[0];
                string fields = typeto.Split('|')[1];

                connTo = connTo.Replace("(", "");
                connTo = connTo.Replace(")", "");

                connType = connType.Replace("(", "");
                connType = connType.Replace(")", "");
                return "\",\n\"query\":\"" + query + "\",\n\"fields\":\"" + fields + "\",\n\"connectedType\":\"" + connType + "\",\n\"connectedTo\":\"" + connTo + "\"\n}";
            }
        }
    }
}
