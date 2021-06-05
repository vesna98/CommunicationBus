using RESTim10.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTim10.Controllers
{
  public  class TipVezeController:ITipVezeController
    {
        private IRepository<TipVeze> repository;
        public TipVezeController(IRepository<TipVeze> repository)
        
        {
            this.repository = repository;
        }

        public bool Delete(string zahtev)
        {
            bool postoji = false;
            //"DELETE FROM " + tabela + " WHERE id=" + IDENTIFIKATOR      ||  + " AND " + uslovi;
            TipVeze t = new TipVeze();
            string[] uslovi = zahtev.Split(new[] { "WHERE" }, StringSplitOptions.None);
            int id = 0;
            string naziv = "**";

            //int tipid = -1;
            List<TipVeze> trazeni = repository.GetAll().ToList();
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
                }

                foreach (TipVeze tip in trazeni)
                {
                    if (tip.TipVezeId == id)
                    {
                        if (!naziv.Equals("**") && id != -1)
                        {
                            if (tip.NazivVeze.Equals(naziv) && tip.TipVezeId == id)
                            {
                                postoji = true;
                            }
                        }
                        t = tip;
                        break;
                    }
                }
            }
            else
            {
                string idstr = uslovi[1].Split('=')[1];
                id = int.Parse(idstr);

                foreach (TipVeze tip in trazeni)
                {
                    if (tip.TipVezeId == id)
                    {
                        postoji = true;
                        t = tip;
                        break;
                    }
                }
            }

            if (postoji)
            {
                repository.Delete(t);
                SaveChanges();
                return true;
            }
            return false;

        }

        public List<TipVeze> GetAll()
        {
            return repository.GetAll().ToList<TipVeze>();
        }

        public TipVeze GetOne(string zahtev)
        {
            string[] uslovi = zahtev.Split(new[] { "WHERE" }, StringSplitOptions.None);
            if (uslovi[1].Contains("AND"))
            {
                string noviNaziv = "";
                string pomocni = "";
                string[] parts = uslovi[1].Split(new[] { "AND" }, StringSplitOptions.None);
                string[] kolone;


                if (parts.Count() == 2)     //id,naziv,tip
                {
                    //postovati redosled!!!!!!!! naziv, pa tip
                    pomocni = parts[1].Split('=')[1];
                    pomocni = pomocni.Replace("'", "");

                    //noviNaziv = pomocni.Substring(1, pomocni.Length - 2);
                    return repository.GetAll().Where(tipV => tipV.TipVezeId == int.Parse(parts[0].Split('=')[1]) && tipV.NazivVeze.Equals(pomocni)).Single();      //naziv-parts1 tip-parts2

                }
                else
                {
                    if (parts[1].Contains("name"))
                    {
                        pomocni = parts[1].Split('=')[1];
                        pomocni = pomocni.Replace("'", "");
                        // noviNaziv = pomocni.Substring(1, pomocni.Length - 2);
                        return repository.GetAll().Where(tipV => tipV.TipVezeId == int.Parse(parts[0].Split('=')[1]) && tipV.NazivVeze.Equals(pomocni)).Single();
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
                return repository.GetAll().Where(tipV => tipV.TipVezeId == int.Parse(uslovi[1].Split('=')[1])).Single();// 
            }
        }

        public bool Insert(string zahtev)
        {

            TipVeze t = new TipVeze();
            //sql upit INSERT INTO tabela (kolona1;kolona2) VALUES (val1;val2)

            int id = 0; //id
            string[] part = zahtev.Split('(');  //...tabela , kolona1;kol2... ) VALUES , val1;val2....)
            string kolone = part[1].Split(')')[0];
            string vrednosti = part[2].Split(')')[0];

            string[] kol = kolone.Split(';');
            string[] vel = vrednosti.Split(';');



            string naziv = "";


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

            }
            t.TipVezeId = id;
            t.NazivVeze = naziv;


            List<TipVeze> trazeni = repository.GetAll().ToList();
            bool postoji = false;
            foreach (TipVeze tp in trazeni)
            {
                if (tp.TipVezeId == id)
                {
                    postoji = true;
                }
            }
            if (!postoji)
            {
                repository.Add(t);
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
            TipVeze r = null;
            //sql upit UPDATE tabela SET kolona1=value
            string[] uslovi = zahtev.Split(new[] { "WHERE" }, StringSplitOptions.None);
            // string temp = uslovi[1];        //id=68678

            int id = 0;
            string naziv = "**";
            string opisUslov = "**";
            int tipid = -1;
            List<TipVeze> trazeni = repository.GetAll().ToList();
            bool postoji = false;
            if (!uslovi[1].Contains("AND"))     //ako nema dodatnih uslova,sam id
            {
                string[] idstr = uslovi[1].Split('=');
                id = int.Parse(idstr[1]);

                string select = "SELECT * FROM tipveze WHERE " + uslovi[1];
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

                }

                foreach (TipVeze res in trazeni)
                {
                    if (res.TipVezeId == id)
                    {
                        if (!naziv.Equals("**"))
                        {
                            if (res.NazivVeze.Equals(naziv))
                            {
                                postoji = true;
                            }
                        }

                        break;
                    }
                }

            }

            if (!postoji)
            {
                return false;
            }

            //izdvajanje za setovanje
            string[] seteri = uslovi[0].Split(new[] { "SET" }, StringSplitOptions.None);
            string[] svi = seteri[1].Split(';');

            int type = 0;

            string name = "";

            for (int i = 0; i < svi.Length; i++)
            {
                if (svi[i].Contains("name"))
                {
                    name = svi[i].Split('=')[1];
                    r.NazivVeze = name.Replace("'", "");
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
