using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLToDBAdapter
{
    public class DBAdapterKlasa
    {
        public string Zahtev { get; set; }

        public DBAdapterKlasa(string zahtev)
        {
            Zahtev = zahtev;
        }

        public string ConvertToDB()
        {
            string temp = "";
            string selektuj = "";
            string poruka = Zahtev;
            string connectedTo = "-";
            string connectedType = "-";
            string dodatniUsloviResursTO = "-";   //bice za connected to i connected type
            string dodatniUsloviResursTYPE = "-";   //bice za connected to i connected type

            //string kolone = "";
            string tabela = ""; //tabela iz koje izdvjamo
            string[] tokens = Zahtev.Split('/');    //..GET</,verb><noun>,resurs,1<,noun....
            tabela = tokens[2];
            string IDENTIFIKATOR = tokens[3].Substring(0, tokens[3].Length - 1);//da li treba -"2???

            //===============
            string koloneSavrednostima = "";
            if (poruka.Contains("query"))
            {

                string[] delovi1 = tokens[4].Split('>');
                string[] delovi2 = delovi1[2].Split('<');
                koloneSavrednostima = delovi2[0];
            }

            if (poruka.Contains("fields"))
            {
                selektuj = poruka.Split(new string[] { "<fields>" }, StringSplitOptions.None)[1];
                selektuj = selektuj.Split('<')[0];
            }

            if (poruka.Contains("connectedTo"))
            {
                connectedTo = poruka.Split(new string[] { "<connectedTo>" }, StringSplitOptions.None)[1];
                connectedTo = connectedTo.Split('<')[0];
                string prviBroj = connectedTo.Split(';')[0];
                prviBroj = prviBroj.Split('=')[1];

                string drugiBroj = connectedTo.Split(';')[1];
                drugiBroj = drugiBroj.Split('=')[1];

                string varijanta1 = "(IDPRVOG=" + prviBroj + " AND IDDRUGOG=" + drugiBroj + ")";
                string varijanta2 = "(IDPRVOG=" + drugiBroj + " AND IDDRUGOG=" + prviBroj + ")";
                dodatniUsloviResursTO = "(SELECT IDPRVOG,IDDRUGOG FROM VEZA WHERE " + varijanta1 + " OR " + varijanta2 + ")";
            }

            if (poruka.Contains("connectedType"))

            {
                connectedType = poruka.Split(new string[] { "<connectedType>" }, StringSplitOptions.None)[1];
                connectedType = connectedType.Split('<')[0];

                List<string> idevi = new List<string>();
                string[] delovi = connectedType.Split(';');
                for (int i = 0; i < delovi.Count(); i++)
                {
                    idevi.Add(delovi[i].Split('=')[1]);
                }

                string varijanta1;
                string varijanta2;
                dodatniUsloviResursTYPE = "(SELECT IDPRVOG,IDDRUGOG FROM VEZA WHERE ";
                for (int i = 0; i < idevi.Count(); i++)
                {
                    varijanta1 = "(IDPRVOG=" + idevi[i] + " AND IDDRUGOG=" + IDENTIFIKATOR + ")";
                    varijanta2 = "(IDPRVOG=" + IDENTIFIKATOR + " AND IDDRUGOG=" + idevi[i] + ")";
                    dodatniUsloviResursTYPE += "(" + varijanta1 + " OR " + varijanta2 + ")";
                    if (i < idevi.Count() - 1)
                    {
                        dodatniUsloviResursTYPE += " AND ";
                    }
                }

            }

            if (tokens[0].Contains("GET"))
            {
                // tabela = tokens[2];
                string uslov = "";
                if (!poruka.Contains("fields"))
                {
                    selektuj = "*"; //nema nista konkretno sto izdvjamo iz tabele vec cemo sve kolone 
                    if (!poruka.Contains("query") && dodatniUsloviResursTO.Equals("-") && dodatniUsloviResursTYPE.Equals("-"))
                    {                           //onda trazi po id ako je naveden???Asistenta pitati
                        uslov = "id=" + IDENTIFIKATOR; //DODALA ;
                    }
                    else if (!poruka.Contains("query"))
                    {
                        uslov = "id=" + IDENTIFIKATOR;

                        if (poruka.Contains("connectedTo"))
                        {
                            uslov += " AND " + dodatniUsloviResursTO;
                        }

                        if (poruka.Contains("connectedType"))
                        {
                            uslov += " AND " + dodatniUsloviResursTYPE;
                        }
                    }
                }

                if (poruka.Contains("query"))
                {
                    uslov = "id=" + IDENTIFIKATOR + " AND ";
                    //***********************************************************************************QUERY DEO
                    string[] queryDeo = koloneSavrednostima.Split(';');

                    for (int i = 0; i < queryDeo.Length; i++)
                    {
                        uslov += queryDeo[i];//.Replace("'","");// + " ";// dodala
                        if (i < queryDeo.Length - 1)
                        {
                            uslov += " AND ";
                        }
                    }

                    if (poruka.Contains("connectedTo"))
                    {
                        uslov += " AND " + dodatniUsloviResursTO;
                    }

                    if (poruka.Contains("connectedType"))
                    {
                        uslov += " AND " + dodatniUsloviResursTYPE;
                    }

                }

                temp += "SELECT " + selektuj + " FROM " + tabela + " WHERE " + uslov;

            }
            else if (tokens[0].Contains("PATCH"))
            {
                string uslov = "";
                if (!poruka.Contains("query"))
                {
                    uslov = "id=" + IDENTIFIKATOR; //+ ";";

                    if (poruka.Contains("connectedTo"))
                    {
                        uslov += " AND " + dodatniUsloviResursTO;

                    }

                    if (poruka.Contains("connectedType"))
                    {
                        uslov += " AND " + dodatniUsloviResursTYPE;

                    }

                    temp += "UPDATE " + tabela + " SET " + koloneSavrednostima + " WHERE " + uslov;

                }
                else if (poruka.Contains("query"))
                {
                    string[] queryDeo = koloneSavrednostima.Split(';');
                    uslov = "id=" + IDENTIFIKATOR;// + " AND ";
                    /*for (int i = 0; i < queryDeo.Length; i++)
                    {
                        uslov += queryDeo[i];
                        if (i < queryDeo.Length - 1)
                        {
                            uslov += " AND ";
                        }
                    }*/

                    if (poruka.Contains("connectedTo"))
                    {
                        uslov += " AND " + dodatniUsloviResursTO;
                    }

                    if (poruka.Contains("connectedType"))
                    {
                        uslov += " AND " + dodatniUsloviResursTYPE;
                    }

                    temp += "UPDATE " + tabela + " SET " + koloneSavrednostima + " WHERE " + uslov;
                }

            }
            else if (tokens[0].Contains("POST"))
            {
                string kolone = "id;";
                string vrednosti = IDENTIFIKATOR + ";";

                string[] rasparcani = koloneSavrednostima.Split(';');   //name='fsdgds'   type=35 ...
                for (int i = 0; i < rasparcani.Length; i++)
                {
                    kolone += rasparcani[i].Split('=')[0];
                    vrednosti += rasparcani[i].Split('=')[1];
                    int p = i;
                    if (!(++p == rasparcani.Length))
                    {
                        kolone += ";";
                        vrednosti += ";";
                    }
                }

                temp += "INSERT INTO " + tabela + " (" + kolone + ")" + " VALUES (" + vrednosti + ");";
            }
            else if (tokens[0].Contains("DELETE"))
            {
                if (!poruka.Contains("query"))
                {
                    string uslovi = " id=" + IDENTIFIKATOR;

                    if (poruka.Contains("connectedTo"))
                    {
                        uslovi += " AND " + dodatniUsloviResursTO;

                    }

                    if (poruka.Contains("connectedType"))
                    {
                        uslovi += " AND " + dodatniUsloviResursTYPE;

                    }

                    temp += "DELETE FROM " + tabela + " WHERE" + uslovi;   //bez filtera DODATI SA QUERY
                }
                else if (poruka.Contains("query"))
                {
                    string uslovi = "";
                    string[] queryDeo = koloneSavrednostima.Split(';');

                    for (int i = 0; i < queryDeo.Length; i++)
                    {
                        uslovi += queryDeo[i];
                        if (i < queryDeo.Length - 1)
                        {
                            uslovi += " AND ";
                        }
                    }

                    if (poruka.Contains("connectedTo"))
                    {
                        uslovi += " AND " + dodatniUsloviResursTO;

                    }

                    if (poruka.Contains("connectedType"))
                    {
                        uslovi += " AND " + dodatniUsloviResursTYPE;
                    }

                    temp += "DELETE FROM " + tabela + " WHERE id=" + IDENTIFIKATOR + " AND " + uslovi;
                }
            }

            return temp;
        }

        public string BackToXML(string poruka)
        {
            string[] tokens = poruka.Split(';');

            string temp = "<response><status>" + tokens[0] + "</status><code>" + tokens[1] + "</code><payload>" + tokens[2] + "</payload>";
            temp += "</response>";

            return temp;
        }
    }
}
