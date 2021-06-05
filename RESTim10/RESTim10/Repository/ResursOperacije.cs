using RESTim10.Controllers;
using RESTim10.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTim10.Repository
{
   public class ResursOperacije:IResursOperacije
    {
        IResursController model;
        public string Odgovor { get; set; }

        public ResursOperacije(IResursController model)
        {
            this.model = model;
        }

        public void Delete(string zahtev)
        {
            try
            {
                bool obrisano = model.Delete(zahtev);
                if (obrisano)
                {
                    Odgovor = "SUCCESS;2000;'Message'='Uspesno obrisano!'";
                }
                else
                {
                    Odgovor = "REJECTED;3000;'Error message'='Ne postoji element za brisanje!'";
                }
            }
            catch (Exception ex)
            {
                Odgovor = "REJECTED;3000;'Error message'='Ne postoji element za brisanje!'";
            }
        }

        public void GetOne(string zahtev)
        {
            try
            {
                Resurs rezultat;
                rezultat = model.GetOne(zahtev);

                if (rezultat == null)
                {
                    //nije pronadjen REJECTED
                    // Console.WriteLine("NE POSTOJI VREDNOST");
                    Odgovor = "REJECTED;3000;'Error message'='Nije pronadjeno poklapanje!'";
                }
                else
                {
                    //SUCCESS
                    //Odgovor = "SUCCESS 2000;" + rezultat.ToString();
                    Odgovor = "SUCCESS;2000;" + rezultat.ToString();
                   // Console.WriteLine(Odgovor);

                }
            }
            catch (Exception ex)
            {
                //REJECTED
                 Console.WriteLine(ex.Message);
                Odgovor = "REJECTED;3000;'Error message'='Nije pronadjeno poklapanje!'";
            }
        }

        public void Insert(string zahtev)
        {
            try
            {
                bool postoji = model.Insert(zahtev);
                if (postoji)
                {
                    Odgovor = "SUCCESS;2000;'Message'='Uspesno dodato!'";
                }
                else
                {
                    Odgovor = "REJECTED;3000;'Error message'='Vec postoji element sa navedenim id-em!'";
                }

            }
            catch (Exception ex)
            {
                //REJECTED
                Odgovor = "REJECTED;3000;'Error message'='Vec postoji element sa navedenim id-em!'";    //kad ce se pozvati???
            }
        }

        public void Update(string zahtev)
        {
            try
            {
                // Resurs r = model.GetOne(zahtev);
                bool uspelo = model.Update(zahtev);
                if (uspelo)
                {
                    Odgovor = "SUCCESS;2000;'Message'='Uspesno azurirano!'";
                }
                else
                {
                    Odgovor = "REJECTED;3000;'Error message'='Nije pronadjeno poklapanje!'";
                }


            }
            catch (Exception ex)
            {
                //REJECTED
                // Console.WriteLine(ex.Message);
                Odgovor = "REJECTED;3000;'Error message'='Nije pronadjeno poklapanje!'";
            }

        }



    }
}
