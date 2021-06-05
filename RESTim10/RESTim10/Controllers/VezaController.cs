using RESTim10.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTim10.Controllers
{
   public class VezaController:IVezaController
    {
        private IRepository<Veza> repository;

        public VezaController(IRepository<Veza> repository)
        {
            this.repository = repository;

        }

        public bool Delete(string zahtev)
        {

            bool postoji = false;
            Veza t = new Veza();
            string[] uslovi = zahtev.Split(new[] { "WHERE" }, StringSplitOptions.None);
            int id = 0;
            int idv1 = 0;

            //int tipid = -1;
            List<Veza> trazeni = repository.GetAll().ToList();
            if (uslovi[1].Contains("AND"))
            {
                //ima dodatne uslove //zasad QUERY //TREBA ONA 2 DODATI

                string[] parametri = zahtev.Split(new[] { "AND" }, StringSplitOptions.None);
                for (int i = 0; i < parametri.Length; i++)
                {
                    if (parametri[i].Contains("id1"))
                    {
                        id = int.Parse(parametri[i].Split('=')[1]);
                    }
                    else if (parametri[i].Contains("idV"))
                    {
                        idv1 = int.Parse(parametri[i].Split('=')[1]);

                    }
                }

                foreach (Veza veza in trazeni)
                {
                    if (veza.IdVeze == id)
                    {
                        if (idv1 != -1 && id != -1)
                        {
                            if (veza.IdVeze == id && veza.IdPrvog == idv1)
                            {
                                postoji = true;
                            }
                        }
                        t = veza;
                        break;
                    }
                }
            }
            else
            {
                string idstr = uslovi[1].Split('=')[1];
                id = int.Parse(idstr);

                foreach (Veza tip in trazeni)
                {
                    if (tip.IdVeze == id)
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

        public List<Veza> GetAll()
        {
            return repository.GetAll().ToList<Veza>();
        }

        public Veza GetOne(string zahtev)
        {
            string[] uslovi = zahtev.Split(new[] { "WHERE" }, StringSplitOptions.None);
            if (uslovi[1].Contains("AND"))
            {
                int idNovi = 0;
                string pomocni = "";
                string[] parts = uslovi[1].Split(new[] { "AND" }, StringSplitOptions.None);

                int id1 = 0;
                int id2 = 0;
                int idtv = 0;
                for(int i=0;i<parts.Count();i++)
                {
                    if(parts[i].Contains("id1"))
                    {
                        id1 = int.Parse(parts[i].Split('=')[1]);
                    }
                    if (parts[i].Contains("id2"))
                    {
                        id2 = int.Parse(parts[i].Split('=')[1]);
                    }
                    if (parts[i].Contains("idtv"))
                    {
                        idtv = int.Parse(parts[i].Split('=')[1]);
                    }
                }
                if(id1!=0 && id2==0 & idtv==0)
                {
                    return repository.GetAll().Where(tipV => tipV.IdVeze == int.Parse(parts[0].Split('=')[1]) && tipV.IdPrvog == id1).Single();
                }
                if (id1 != 0 && id2 != 0 & idtv == 0)
                {
                    return repository.GetAll().Where(tipV => tipV.IdVeze == int.Parse(parts[0].Split('=')[1]) && tipV.IdPrvog == id1 && tipV.IdDrugog==id2).Single();
                }
                if (id1 != 0 && id2 != 0 & idtv != 0)
                {
                    return repository.GetAll().Where(tipV => tipV.IdVeze == int.Parse(parts[0].Split('=')[1]) && tipV.IdPrvog == id1 && tipV.IdDrugog == id2 && tipV.TipVezeId==idtv).Single();
                }
                if (id1 == 0 && id2 == 0 & idtv != 0)
                {
                    return repository.GetAll().Where(tipV => tipV.IdVeze == int.Parse(parts[0].Split('=')[1]) && tipV.TipVezeId == idtv).Single();
                }

                return null;



                //if (parts.Count() == 2)     //id1,id2
                //{

                //    pomocni = parts[1].Split('=')[1];

                //   // idNovi = int.Parse(pomocni.Substring(1, pomocni.Length - 2));
                //    return repository.GetAll().Where(tipV => tipV.IdPrvog == int.Parse(parts[0].Split('=')[1]) && tipV.IdDrugog == idNovi).Single();      //naziv-parts1 tip-parts2

                //}
                //else
                //{
                //    return null;
                //}

            }
            else
            {
                // samo id
                return repository.GetAll().Where(tipV => tipV.IdVeze == int.Parse(uslovi[1].Split('=')[1])).Single();// 
            }
        }

        public bool Insert(string zahtev)
        {

            Veza t = new Veza();
            //sql upit INSERT INTO tabela (kolona1;kolona2) VALUES (val1;val2)

            int id = 0; //id
            string[] part = zahtev.Split('(');  //...tabela , kolona1;kol2... ) VALUES , val1;val2....)
            string kolone = part[1].Split(')')[0];
            string vrednosti = part[2].Split(')')[0];

            string[] kol = kolone.Split(';');
            string[] vel = vrednosti.Split(';');

            int prvi = 0;
            int drugi = 0;
            int idtip=0;


            for (int i = 0; i < kol.Length; i++)
            {
                if (i == 0)
                {
                    id = int.Parse(vel[i]);
                }
                if (i == 1)
                {
                    prvi = int.Parse(vel[i]);

                }
                if (i == 2)
                {
                    drugi = int.Parse(vel[i]);

                }
                if(i==3)
                {
                    idtip= int.Parse(vel[i]);
                }

            }
            t.IdVeze = id;
            t.IdPrvog = prvi;
            t.IdDrugog = drugi;
            t.TipVezeId = idtip;


            List<Veza> trazeni = repository.GetAll().ToList();
            bool postoji = false;
            foreach (Veza tp in trazeni)
            {
                if (tp.IdVeze == id)
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
            // throw new NotImplementedException();
        }

        public bool Update(string zahtev)
        {
            Veza r = null;
            //sql upit UPDATE tabela SET kolona1=value
            string[] uslovi = zahtev.Split(new[] { "WHERE" }, StringSplitOptions.None);
            // string temp = uslovi[1];        //id=68678

            int id = 0;
            string naziv = "**";
            string opisUslov = "**";
            int tipV = -1;
            
            int id2 = -1;
            int id1 = -1;
            List<Veza> trazeni = repository.GetAll().ToList();
            bool postoji = false;
            if (!uslovi[1].Contains("AND"))     //ako nema dodatnih uslova,sam id
            {
                string[] idstr = uslovi[1].Split('=');
                id = int.Parse(idstr[1]);

                string select = "SELECT * FROM veza WHERE " + uslovi[1];
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
                    else if (parametri[i].Contains("id1"))
                    {
                        id1 = int.Parse(parametri[i].Split('=')[1]);
                    }
                    else if (parametri[i].Contains("id2"))
                    {
                        id2 = int.Parse(parametri[i].Split('=')[1]);
                    }
                    else if (parametri[i].Contains("tipV"))
                    {
                        tipV = int.Parse(parametri[i].Split('=')[1]);
                    }
                }

                foreach (Veza res in trazeni)
                {
                    if (res.TipVezeId == id)
                    {/*
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
                        }/*/

                        postoji = true;
                       
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

          
            int idP = 0;
            int idD = 0;
            int idTV = 0;

          
            for (int i = 0; i < svi.Length; i++)
            {
                if (svi[i].Contains("id1"))
                {
                    idP = int.Parse(svi[i].Split('=')[1]);
                    r.IdPrvog = idP;
                }
                else if (svi[i].Contains("id2"))
                {
                    idD = int.Parse(svi[i].Split('=')[1]);
                    r.IdDrugog= idD;
                }
                else if(svi[i].Contains("idV"))
                {
                    idTV = int.Parse(svi[i].Split('=')[1]);
                    r.TipVezeId = idTV;
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
