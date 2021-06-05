using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
   public class Validacija
    {

        public Validacija()
        {

        }

        public bool CheckRequest(string zahtev)
        {
            //treba inicijalizovati zahtev ako je dobar
            string tabela = "";
            int id = -1;
            if (zahtev.StartsWith("GET ") || zahtev.StartsWith("POST ") || zahtev.StartsWith("PATCH ") || zahtev.StartsWith("DELETE "))
            {
                string[] tokens = zahtev.Split(' ');
                if (tokens[1].StartsWith("/resurs/") || tokens[1].StartsWith("/tip/") || tokens[1].StartsWith("/veza/") || tokens[1].StartsWith("/tipveze/"))
                {
                    string pomocni = tokens[1].Substring(1, tokens[1].Length - 1);
                    string[] parts = pomocni.Split('/');
                    tabela = parts[0];
                    if (!parts[1].Contains("?"))
                    {
                        if (!parts[1].Contains("|"))
                        {
                            if (!parts[1].Contains("%"))
                            {
                                if (!parts[1].Contains("#"))
                                {
                                    //jeste samo id valjda
                                    if (int.TryParse(parts[1], out id))
                                    {

                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    //-**************************************************************
                    if (tabela.Equals("resurs") && parts[1].Contains("#") && !parts[1].Contains("%") && !parts[1].Contains("?") && !parts[1].Contains("|"))
                    {
                        string[] delovi = parts[1].Split('#');
                        if (!ConnectedTypeCHECK(delovi[1])) { return false; }
                    }
                    else if (tabela.Equals("resurs") && parts[1].Contains("%") && !parts[1].Contains("#") && !parts[1].Contains("?") && !parts[1].Contains("|"))
                    {
                        string[] delovi = parts[1].Split('%');
                        if (!ConnectedToCHECK(delovi[1])) { return false; }
                    }
                    else if (tabela.Equals("resurs") && parts[1].Contains("%") && parts[1].Contains("#") && !parts[1].Contains("?") && !parts[1].Contains("|"))
                    {
                        // DOPUNITI
                        char prvi = 'x';
                        for (int i = 0; i < parts[1].Length; i++)
                        {
                            prvi = parts[1][i];

                            if (prvi.Equals('%'))
                            {
                                string[] delovi = parts[1].Split('%');
                                if (!ConnectedToCHECK(delovi[1])) { return false; }
                                string[] drugifilter = delovi[1].Split('#');
                                if (!ConnectedTypeCHECK(drugifilter[1])) { return false; }

                            }
                            else if (prvi.Equals('#'))
                            {
                                string[] delovi = parts[1].Split('#');
                                if (!ConnectedTypeCHECK(delovi[1])) { return false; }
                                string[] drugifilter = delovi[1].Split('%');
                                if (!ConnectedToCHECK(drugifilter[1])) { return false; };
                            }
                        }


                    }
                    else if ((tabela.Equals("tip") || tabela.Equals("tipveze") || tabela.Equals("veza")) && (parts[1].Contains('#') || parts[1].Contains('%')))
                    {
                        return false;
                    }

                    char prviFilter = 'x';
                    for (int i = 0; i < parts[1].Length; i++)
                    {
                        prviFilter = parts[1][i];
                        if (prviFilter.Equals('?'))
                        {
                            string[] delovi = parts[1].Split('?');
                            if (int.TryParse(delovi[0], out id))
                            {
                                prviFilter = '?';

                                if (CheckQueryFilter(tabela, parts[1]))
                                {     //TU UBACILI PROVERE ZA FILTERE
                                    break;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            { return false; }
                        }
                        else if (prviFilter.Equals('|'))
                        {
                            string[] delovi = parts[1].Split('|');
                            if (int.TryParse(delovi[0], out id))
                            {
                                prviFilter = '|';
                                if (CheckFields(tabela, parts[1]))
                                {
                                    break;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            { return false; }
                        }
                        else if (tabela.Equals("resurs") && prviFilter.Equals('#'))
                        {
                            prviFilter = '#';
                            string[] delovi = parts[1].Split('#');
                            if (int.TryParse(delovi[0], out id))
                            { break; }
                            else
                            { return false; }

                        }
                        else if (tabela.Equals("resurs") && prviFilter.Equals('%'))
                        {
                            prviFilter = '%';
                            string[] delovi = parts[1].Split('%');
                            if (int.TryParse(delovi[0], out id))
                            { break; }
                            else
                            { return false; }
                        }

                    }

                    return true;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public bool CheckQueryFilter(string tabela, string query)
        {
            string[] delovi = query.Split('?');
            string param = delovi[1];
            if (tabela.Equals("resurs"))
            {
                if (delovi[1].Contains("|"))
                {
                    string[] posalji = delovi[1].Split('|');
                    if (posalji[0].Contains('#') && !posalji[0].Contains('%'))
                    {
                        string[] izdvoj = posalji[0].Split('#');
                        param = izdvoj[0];
                    }
                    else if (!posalji[0].Contains('#') && posalji[0].Contains('%'))
                    {
                        string[] izdvoj = posalji[0].Split('%');
                        param = izdvoj[0];
                    }
                    else if (posalji[0].Contains('#') && posalji[0].Contains('%'))
                    {
                        //char prvi;
                        for (int i = 0; i < posalji[0].Length; i++)
                        {
                            if ((posalji[0][i]).Equals('#'))
                            {
                                // prvi = '#';
                                string[] rez = posalji[0].Split('#');
                                param = rez[0];
                                break;
                            }
                            else //if ((posalji[0][i]).Equals('%'))
                            {
                                string[] rez = posalji[0].Split('%');
                                param = rez[0];
                                break;
                            }
                        }
                    }
                    else
                    {
                        param = posalji[0];
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////

                //if (!ResursCheck(delovi[1]))
                if (!ResursCheck(param))
                {
                    return false;
                }
            }
            else if (tabela.Equals("tip") || tabela.Equals("tipveze"))
            {
                //DA LI IMA NEKI FILTER POSLE?? PROVERA SVUDA
                //----------------

                if (delovi[1].Contains('|'))
                {
                    string[] iseci = delovi[1].Split('|');
                    param = iseci[0];
                }
                else
                {
                    param = delovi[1];
                }
                //-------------
                if (!TipCheck(param))
                {
                    return false;
                }
            }
            else if (tabela.Equals("veza"))
            {
                //--------------

                if (delovi[1].Contains('|'))
                {
                    string[] iseci = delovi[1].Split('|');
                    param = iseci[0];
                }
                else
                {
                    param = delovi[1];
                }
                //---------------------
                if (!VezaCheck(param))
                {
                    return false;
                }
            }
            return true;

        }

        public bool CheckFields(string tabela, string fields)
        {
            string[] delovi = fields.Split('|');
            string param = "";
            if (tabela.Equals("resurs"))
            {
                if (delovi[1].Contains("?"))
                {
                    string[] posalji = delovi[1].Split('?');
                    if (posalji[0].Contains('#') && !posalji[0].Contains('%'))
                    {
                        string[] izdvoj = posalji[0].Split('#');
                        param = izdvoj[0];
                    }
                    else if (!posalji[0].Contains('#') && posalji[0].Contains('%'))
                    {
                        string[] izdvoj = posalji[0].Split('%');
                        param = izdvoj[0];
                    }
                    else if (posalji[0].Contains('#') && posalji[0].Contains('%'))
                    {
                        //char prvi;
                        for (int i = 0; i < posalji[0].Length; i++)
                        {
                            if ((posalji[0][i]).Equals('#'))
                            {
                                // prvi = '#';
                                string[] rez = posalji[0].Split('#');
                                param = rez[0];
                                break;
                            }
                            else //if ((posalji[0][i]).Equals('%'))
                            {
                                string[] rez = posalji[0].Split('%');
                                param = rez[0];
                                break;
                            }
                        }
                    }
                    else
                    {
                        param = posalji[0];
                    }
                }
                else if (delovi[1].Contains("#") || delovi[1].Contains("%"))// # %
                {
                    for (int i = 0; i < delovi[1].Length; i++)
                    {
                        if ((delovi[1][i]).Equals('#'))
                        {
                            // prvi = '#';
                            string[] rez = delovi[1].Split('#');
                            //da li je tacan #
                            if (!ConnectedTypeCHECK(rez[1]))           ///****************
                            {
                                return false;
                            }
                            else
                            {
                                param = rez[0];
                                break;
                            }                   //*****************************
                        }
                        else if ((delovi[1][i]).Equals('%'))
                        {
                            string[] rez = delovi[1].Split('%');
                            //da li je tacan %
                            if (!ConnectedToCHECK(rez[1]))
                            {
                                return false;
                            }
                            else
                            {
                                param = rez[0];
                                break;
                            }
                        }
                    }
                }
                else
                {
                    param = delovi[1];
                }

                if (!ResursPolja(param))
                {
                    return false;
                }
            }
            else if (tabela.Equals("tip") || tabela.Equals("tipveze"))
            {
                if (delovi[1].Contains("?"))
                {
                    string[] posalji = delovi[1].Split('?');
                    param = posalji[0];
                }
                else
                {
                    param = delovi[1];
                }
                if (!TipPolja(param))
                {
                    return false;
                }
            }
            else if (tabela.Equals("veza"))
            {
                if (delovi[1].Contains("?"))
                {
                    string[] posalji = delovi[1].Split('?');
                    param = posalji[0];
                }
                else
                {
                    param = delovi[1];
                }
                if (!VezaPolja(param))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ConnectedToCHECK(string poruka)
        {
            if (poruka.StartsWith("(") && poruka.EndsWith(")") && poruka.Contains(";") && poruka.Contains("id="))
            {
                //string[] parts = poruka.Split('(');
                //string[] obrada = parts[1].Split(')');
                poruka = poruka.Substring(1, poruka.Length - 2);

                if (poruka.Contains("#"))
                {
                    string[] prvifilter = poruka.Split('#');
                    poruka = prvifilter[0];
                }
                string[] delici = poruka.Split(';');
                int id1;
                if (delici.Length == 2)
                {
                    foreach (string sss in delici)
                    {
                        string[] polja = sss.Split(new[] { "id=" }, StringSplitOptions.None);
                        if (int.TryParse(polja[1], out id1))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool ConnectedTypeCHECK(string poruka)
        {
            if (poruka.StartsWith("(") && poruka.EndsWith(")") && poruka.Contains(";") && poruka.Contains("id="))
            {
                //string[] parts = poruka.Split('(');
                //string[] obrada = parts[1].Split(')');
                //string[] delici = obrada[0].Split(';');


                if (poruka.Contains("%"))
                {
                    string[] prvifilter = poruka.Split('%');
                    poruka = prvifilter[0];
                }
                //poruka = poruka.Replace("(", "");
                //poruka = poruka.Replace(")","");
                poruka = poruka.Substring(1, poruka.Length - 2);
                string[] delici = poruka.Split(';');
                int id;

                foreach (string sss in delici)
                {
                    string[] polja = sss.Split(new[] { "id=" }, StringSplitOptions.None);

                    if (int.TryParse(polja[1], out id))
                    { }
                    else
                    {
                        return false;
                    }

                }
                return true;

            }
            else if (poruka.StartsWith("(") && poruka.EndsWith(")") && !poruka.Contains(";") && poruka.Contains("id="))
            {
                poruka = poruka.Replace("(", "");
                poruka = poruka.Replace(")", "");
                int id;

                string[] polja = poruka.Split(new[] { "id=" }, StringSplitOptions.None);

                if (int.TryParse(polja[1], out id))
                { }
                else
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ResursPolja(string poruka)
        {
            if (!poruka.Contains(";"))
            {
                if (!poruka.Equals("id") && !poruka.Equals("name") && !poruka.Equals("opis") && !poruka.Equals("type"))        //nece moci
                {
                    return false;
                }
                return true;
            }
            else
            {
                string[] parts = poruka.Split(';');
                foreach (string deo in parts)
                {
                    if (deo.Equals("name") || deo.Equals("id") || deo.Equals("type") || deo.Equals("opis"))
                    {

                    }
                    else
                    {
                        //return true;
                        return false;
                    }

                }
                return true;
            }
        }

        public bool TipPolja(string poruka)
        {
            if (!poruka.Contains(';'))
            {
                if (poruka.Equals("id"))
                {
                    return true;
                }
                else
                {
                    if (poruka.Equals("name"))
                    {
                        return true; ;
                    }
                    return false;
                }
            }
            else
            {
                string[] parts = poruka.Split(';');
                foreach (string deo in parts)
                {
                    if (!deo.Equals("id"))
                    {
                        if (!deo.Equals("name"))
                        {
                            return false;
                        }
                    }
                }
                return true;

            }
        }

        public bool VezaPolja(string poruka)
        {
            if (!poruka.Contains(';'))
            {
                if (poruka.Equals("idV"))
                {
                    return true;
                }
                else
                {
                    if (poruka.Equals("type"))
                    {
                        return true;
                    }
                    else
                    {
                        if (poruka.Equals("id1"))
                        {
                            return true;
                        }
                        else
                        {
                            if (poruka.Equals("id2"))
                            {
                                return true;
                            }
                            return false;
                        }
                    }

                }
            }
            else
            {
                string[] parts = poruka.Split(';');
                foreach (string deo in parts)
                {
                    if (!deo.Equals("idV"))
                    {
                        if (!deo.Equals("id1"))
                        {
                            if (!deo.Equals("id2"))
                            {
                                if (!deo.Equals("type"))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                return true;
            }
        }

        public bool ResursCheck(string porukaU)
        {
            if (porukaU.Contains("name=") || porukaU.Contains("type=") || porukaU.Contains("opis="))
            {
                string[] por = porukaU.Split('&');
                foreach (string poruka in por)
                {
                    if (poruka.Contains("name="))
                    {
                        string[] polja = poruka.Split(new[] { "name=" }, StringSplitOptions.None);
                        string naziv = polja[1];
                        if (naziv.StartsWith("'") && naziv.EndsWith("'"))
                        {
                            naziv = naziv.Substring(1, naziv.Length - 2);
                            int n;
                            if (int.TryParse(naziv, out n))
                            {
                                return false;
                            }

                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (poruka.Contains("type="))
                    {
                        string[] polja = poruka.Split(new[] { "type=" }, StringSplitOptions.None);
                        string tip = polja[1];
                        int brTIP;
                        if (int.TryParse(tip, out brTIP))
                        {
                            //typeOK = true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (poruka.Contains("opis="))
                    {
                        string[] polja = poruka.Split(new[] { "opis=" }, StringSplitOptions.None);
                        string opis = polja[1];
                        if (opis.StartsWith("[") && opis.EndsWith("]"))
                        {
                            //if (Regex.IsMatch(naziv, @"^[a-zA-Z]+$"))
                            opis = opis.Substring(1, opis.Length - 1);     // izbacujemo []
                            if (opis.Contains('~'))
                            {
                                string[] parovi = opis.Split('~');
                                if (parovi.Length % 2 == 0) //paran broj
                                {
                                    //opisOK = true;
                                }
                                else
                                {
                                    return false;
                                }

                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                return true;    //prosao je kroz sve prvere
            }
            else
            {
                return false;
            }
        }

        public bool TipCheck(string porukaU)
        {

            if (porukaU.Contains("name=") && porukaU.Contains("id=") && porukaU.Contains("&"))
            {

                string[] por = porukaU.Split('&');
                foreach (string poruka in por)
                {
                    if (poruka.Contains("name="))
                    {
                        string[] polja = poruka.Split(new[] { "name=" }, StringSplitOptions.None);
                        string naziv = polja[1];
                        if (naziv.StartsWith("'") && naziv.EndsWith("'"))
                        {
                            // naziv = naziv.Replace("'", "");
                            naziv = naziv.Substring(1, naziv.Length - 2);
                            int n;
                            if (int.TryParse(naziv, out n))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (poruka.Contains("id="))
                    {
                        string[] polja = poruka.Split(new[] { "id=" }, StringSplitOptions.None);
                        string tip = polja[1];
                        int brTIP;
                        if (int.TryParse(tip, out brTIP))
                        {
                            // typeOK = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else if (porukaU.Contains("name=") && !porukaU.Contains("id=") && !porukaU.Contains("&"))
            {
                string[] polja = porukaU.Split(new[] { "name=" }, StringSplitOptions.None);
                string naziv = polja[1];
                if (naziv.StartsWith("'") && naziv.EndsWith("'"))
                {
                    // naziv = naziv.Replace("'", "");
                    naziv = naziv.Substring(1, naziv.Length - 2);
                    int n;
                    if (int.TryParse(naziv, out n))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (!porukaU.Contains("name=") && porukaU.Contains("id=") && !porukaU.Contains("&"))
            {
                string[] polja = porukaU.Split(new[] { "id=" }, StringSplitOptions.None);
                string tip = polja[1];
                int brTIP;
                if (int.TryParse(tip, out brTIP))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool VezaCheck(string porukaU)
        {
            if (porukaU.Contains("idV=") || porukaU.Contains("id1=") || porukaU.Contains("id2=") || porukaU.Contains("type="))
            {
                string[] por = porukaU.Split('&');
                foreach (string poruka in por)
                {
                    if (poruka.Contains("type="))
                    {
                        // postojiType = true;
                        string[] polja = poruka.Split(new[] { "type=" }, StringSplitOptions.None);
                        string tip = polja[1];
                        int brTIP;
                        if (int.TryParse(tip, out brTIP))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (poruka.Contains("id1="))
                    {
                        // postojiType = true;
                        string[] polja = poruka.Split(new[] { "id1=" }, StringSplitOptions.None);
                        string tip = polja[1];
                        int brTIP;
                        if (int.TryParse(tip, out brTIP))
                        {
                            //typeOK = true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (poruka.Contains("id2="))
                    {
                        // postojiType = true;
                        string[] polja = poruka.Split(new[] { "id2=" }, StringSplitOptions.None);
                        string tip = polja[1];
                        int brTIP;
                        if (int.TryParse(tip, out brTIP))
                        {
                            // typeOK = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (poruka.Contains("idV="))
                    {
                        // postojiType = true;
                        string[] polja = poruka.Split(new[] { "idV=" }, StringSplitOptions.None);
                        string tip = polja[1];
                        int brTIP;
                        if (int.TryParse(tip, out brTIP))
                        {
                            //  typeOK = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
