using RESTim10.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTim10.Controllers
{
   public class ResursController:IResursController
    {
        private IRepository<Resurs> repository;

        public ResursController(IRepository<Resurs> repository)
        {
            this.repository = repository;
        }


        public bool Delete(string zahtev)
        {
            bool postoji = false;
            //"DELETE FROM " + tabela + " WHERE id=" + IDENTIFIKATOR      ||  + " AND " + uslovi;
            Resurs r = new Resurs();
            string[] uslovi = zahtev.Split(new[] { "WHERE" }, StringSplitOptions.None);
            int id = 0;
            string naziv = "**";
            string opis = "**";
            int tipid = -1;
            List<Resurs> trazeni = repository.GetAll().ToList();
            if (uslovi[1].Contains("AND"))
            {
                //ima dodatne uslove //zasad QUERY //TREBA ONA 2 DODATI

                string[] parametri = zahtev.Split(new[] { "AND" }, StringSplitOptions.None);
                for (int i = 0; i < parametri.Length; i++)
                {
                    if (parametri[i].Contains("id"))
                    {
                        id = int.Parse(parametri[i].Split('=')[1]);
                    }
                    else if (parametri[i].Contains("name"))
                    {
                        naziv = parametri[i].Split('=')[1];
                        naziv = naziv.Replace("'", "");
                    }
                    else if (parametri[i].Contains("type"))
                    {
                        tipid = int.Parse(parametri[i].Split('=')[1]);
                    }
                    else if (parametri[i].Contains("opis"))
                    {
                        opis = parametri[i].Split('=')[1];
                        opis = opis.Replace("~", ":");
                        opis = opis.Replace("[", "{");
                        opis = opis.Replace("]", "}");
                        opis = opis.Replace("'", "\"");
                    }
                }

                foreach (Resurs res in trazeni)
                {
                    if (res.IdResurs == id)
                    {
                        if (!naziv.Equals("**") && !opis.Equals("**") && tipid != -1)
                        {
                            if (res.NazivR.Equals(naziv) && res.Opis.Equals(opis) && res.TipId == tipid)
                            {
                                postoji = true;
                            }
                        }
                        else if (!naziv.Equals("**") && !opis.Equals("**") && tipid == -1)
                        {
                            //bez tip id
                            if (res.NazivR.Equals(naziv) && res.Opis.Equals(opis))
                            {
                                postoji = true;
                            }
                        }
                        else if (!naziv.Equals("**") && opis.Equals("**") && tipid == -1)
                        {
                            //bez opis tip
                            if (res.NazivR.Equals(naziv))
                            {
                                postoji = true;
                            }
                        }
                        else if (naziv.Equals("**") && !opis.Equals("**") && tipid != -1)
                        {
                            //bez naz
                            if (res.Opis.Equals(opis) && res.TipId == tipid)
                            {
                                postoji = true;
                            }
                        }
                        else if (naziv.Equals("**") && opis.Equals("**") && tipid != -1)
                        {
                            //tip
                            if (res.TipId == tipid)
                            {
                                postoji = true;
                            }
                        }
                        else if (naziv.Equals("**") && !opis.Equals("**") && tipid == -1)
                        {
                            if (res.Opis.Equals(opis))
                            {
                                postoji = true;
                            }
                        }
                        else if (!naziv.Equals("**") && opis.Equals("**") && tipid != -1)
                        {
                            if (res.NazivR.Equals(naziv) && res.TipId == tipid)
                            {
                                postoji = true;
                            }
                        }
                        r = res;
                        break;
                    }
                }
            }
            else
            {
                //samo id
                string idstr = uslovi[1].Split('=')[1];
                id = int.Parse(idstr);

                foreach (Resurs res in trazeni)
                {
                    if (res.IdResurs == id)
                    {
                        postoji = true;
                        r = res;
                        break;
                    }
                }

            }

            if (postoji)
            {
                repository.Delete(r);
                SaveChanges();
                return true;
            }
            return false;
        }

        public List<Resurs> GetAll()
        {
            return repository.GetAll().ToList<Resurs>();
        }

        public Resurs GetOne(string zahtev)
        {
            string[] uslovi = zahtev.Split(new[] { "WHERE" }, StringSplitOptions.None);
            if (uslovi[1].Contains("AND"))
            {
                string noviNaziv = "";
                string pomocni = "";
                string[] parts = uslovi[1].Split(new[] { "AND" }, StringSplitOptions.None);
                // string[] kolone;
                if (parts.Count() == 3)     //id,naziv,tip
                {
                    //postovati redosled!!!!!!!! naziv, pa tip
                    pomocni = parts[1].Split('=')[1];
                    pomocni = pomocni.Replace("'", "");
                    pomocni = pomocni.Substring(0, pomocni.Length - 1);
                    
                    string brid = parts[2].Split('=')[1];
                    int id = int.Parse(brid);
                    int idresurs = int.Parse(parts[0].Split('=')[1]);

                    //noviNaziv = pomocni.Substring(1, pomocni.Length - 3);
                    return repository.GetAll().Where(resurs => resurs.IdResurs ==idresurs && resurs.NazivR.Equals(pomocni) && resurs.TipId == id).Single();      //naziv-parts1 tip-parts2

                }
                else
                {
                    //jedna od kolona
                    if (parts[1].Contains("name"))
                    {
                        pomocni = parts[1].Split('=')[1];
                        pomocni = pomocni.Replace("'", "");
                        // noviNaziv = pomocni.Substring(1, pomocni.Length - 3); IZMJENILA ZA NAZIV SAMO AKO SADRZI
                        return repository.GetAll().Where(resurs => resurs.IdResurs == int.Parse(parts[0].Split('=')[1]) && resurs.NazivR.Equals(pomocni)).Single();
                    }
                    else if (parts[1].Contains("type"))
                    {
                        string brid = parts[1].Split('=')[1];
                        int id = int.Parse(brid);
                        //id je
                        return repository.GetAll().Where(resurs => resurs.IdResurs == int.Parse(parts[0].Split('=')[1]) && resurs.TipId == id).Single();
                    }
                    else
                    {
                        return null;
                        //ovako da proradi DOPISATI GET
                    }

                }

            }
            else
            {
                // samo id
                return repository.GetAll().Where(resurs => resurs.IdResurs == int.Parse(uslovi[1].Split('=')[1])).Single();
            }
        }

        public bool Insert(string zahtev)
        {
            Resurs r = new Resurs();
            //sql upit INSERT INTO tabela (kolona1;kolona2) VALUES (val1;val2)

            int id = 0; //id
            string[] part = zahtev.Split('(');  //...tabela , kolona1;kol2... ) VALUES , val1;val2....)
            string kolone = part[1].Split(')')[0];
            string vrednosti = part[2].Split(')')[0];

            string[] kol = kolone.Split(';');
            string[] vel = vrednosti.Split(';');


            int idtip = 0;
            string naziv = "";
            string opis = "";

            for (int i = 0; i < kol.Length; i++)
            {
                if (i == 0)
                {
                    id = int.Parse(vel[i]);
                }
                if (kol[i].Equals("name"))
                {
                    naziv = vel[i];
                    naziv = naziv.Replace("'", "");
                }
                else if (kol[i].Equals("type"))
                {
                    idtip = int.Parse(vel[i]);
                }
                else if (kol[i].Equals("opis"))
                {
                    opis = vel[i];
                    opis = opis.Replace("~", ":");
                    opis = opis.Replace("[", "{");//dodala
                    opis = opis.Replace("]", "}");// dodala
                    opis = opis.Replace("'", "\"");
                    //opis = opis.Replace(",","\",\"");//boye pomozi
                    //opis
                }
            }
            r.IdResurs = id;
            r.NazivR = naziv;
            r.Opis = opis;
            r.TipId = idtip;

            List<Resurs> trazeni = repository.GetAll().ToList();
            bool postoji = false;
            foreach (Resurs res in trazeni)
            {
                if (res.IdResurs == id)
                {
                    postoji = true;
                }
            }
            if (!postoji)
            {
                repository.Add(r);
                SaveChanges();
                return true;
            }
            return false;
        }

        public void SaveChanges()
        {
            repository.Save();
        }

        public bool Update(string zahtev)
        {
            // bool uspesno = false;
            Resurs r = null;
            //sql upit UPDATE tabela SET kolona1=value
            string[] uslovi = zahtev.Split(new[] { "WHERE" }, StringSplitOptions.None);
            // string temp = uslovi[1];        //id=68678

            int id = 0;
            string naziv = "**";
            string opisUslov = "**";
            int tipid = -1;
            List<Resurs> trazeni = repository.GetAll().ToList();
            bool postoji = false;
            if (!uslovi[1].Contains("AND"))     //ako nema dodatnih uslova,sam id
            {
                string[] idstr = uslovi[1].Split('=');
                id = int.Parse(idstr[1]);

                string select = "SELECT * FROM resurs WHERE " + uslovi[1];
                r = GetOne(select);
                if (r != null)
                {
                    postoji = true;
                }
            }
            else
            {

                string[] parametri = zahtev.Split(new[] { "AND" }, StringSplitOptions.None);
                for (int i = 0; i < parametri.Length; i++)
                {
                    if (parametri[0].Contains("id"))
                    {
                        id = int.Parse(parametri[i].Split('=')[1]);
                    }
                    else if (parametri[i].Contains("name"))
                    {
                        naziv = parametri[i].Split('=')[1];
                        naziv = naziv.Replace("'", "");
                    }
                    else if (parametri[i].Contains("type"))
                    {
                        tipid = int.Parse(parametri[i].Split('=')[1]);
                    }
                    else if (parametri[i].Contains("opis"))
                    {
                        opisUslov = parametri[i].Split('=')[1];
                        opisUslov = opisUslov.Replace("~", ":");
                        opisUslov = opisUslov.Replace("[", "{");
                        opisUslov = opisUslov.Replace("]", "}");
                        opisUslov = opisUslov.Replace("'", "\"");
                    }
                }

                foreach (Resurs res in trazeni)
                {
                    if (res.IdResurs == id)
                    {
                        if (!naziv.Equals("**") && !opisUslov.Equals("**") && tipid != -1)
                        {
                            if (res.NazivR.Equals(naziv) && res.Opis.Equals(opisUslov) && res.TipId == tipid)
                            {
                                postoji = true;
                            }
                        }
                        else if (!naziv.Equals("**") && !opisUslov.Equals("**") && tipid == -1)
                        {
                            //bez tip id
                            if (res.NazivR.Equals(naziv) && res.Opis.Equals(opisUslov))
                            {
                                postoji = true;
                            }
                        }
                        else if (!naziv.Equals("**") && opisUslov.Equals("**") && tipid == -1)
                        {
                            //bez opis tip
                            if (res.NazivR.Equals(naziv))
                            {
                                postoji = true;
                            }
                        }
                        else if (naziv.Equals("**") && !opisUslov.Equals("**") && tipid != -1)
                        {
                            //bez naz
                            if (res.Opis.Equals(opisUslov) && res.TipId == tipid)
                            {
                                postoji = true;
                            }
                        }
                        else if (naziv.Equals("**") && opisUslov.Equals("**") && tipid != -1)
                        {
                            //tip
                            if (res.TipId == tipid)
                            {
                                postoji = true;
                            }
                        }
                        else if (naziv.Equals("**") && !opisUslov.Equals("**") && tipid == -1)
                        {
                            if (res.Opis.Equals(opisUslov))
                            {
                                postoji = true;
                            }
                        }
                        else if (!naziv.Equals("**") && opisUslov.Equals("**") && tipid != -1)
                        {
                            if (res.NazivR.Equals(naziv) && res.TipId == tipid)
                            {
                                postoji = true;
                            }
                        }
                        //r = res;
                        break;
                    }
                }

            }



            //PRONACI SA ODGOVARAJUCIM USLOVIMA
            //AKO NEMA return false odmah

            if (!postoji)
            {
                return false;
            }

            /*
            //napisai select za dobavljanje resursa
            string select = "SELECT * FROM resurs WHERE " + uslovi[1];
            r = GetOne(select);
            */



            //izdvajanje za setovanje
            string[] seteri = uslovi[0].Split(new[] { "SET" }, StringSplitOptions.None);
            string[] svi = seteri[1].Split(';');

            int type = 0;

            string name = "";
            string opis = "";
            for (int i = 0; i < svi.Length; i++)
            {
                if (svi[i].Contains("name"))
                {
                    name = svi[i].Split('=')[1];
                    r.NazivR = name.Replace("'", "");
                }
                else if (svi[i].Contains("type"))
                {
                    type = int.Parse(svi[i].Split('=')[1]);
                    r.TipId = type;
                }
                else
                {
                    opis = svi[i].Split('=')[1];
                    opis = opis.Replace("~", ":");
                    opis = opis.Replace("[", "{");
                    opis = opis.Replace("]", "}");
                    opis = opis.Replace("'", "\"");
                    r.Opis = opis;
                }
            }

            if (postoji)
            {
                repository.Update(r);
                SaveChanges();
                return true;
            }
            return false;
        }
    }
}
