using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationBus;
using JSONToXMLAdapter;
using RESTim10.Controllers;
using RESTim10.Interfaces;
using RESTim10.Repository;
using WebClient;
using XMLToDBAdapter;

namespace RESTim10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PRIMERI:\n************************************************************************************************************\n");
            Console.WriteLine("GET /resurs/1\n");
            Console.WriteLine("DELETE /veza/4?id1=10\n");
            Console.WriteLine("PATCH /tip/1?id=10\n");
            Console.WriteLine("POST /tipveze/1?id=101&name='Aleksa'\n");
            Console.WriteLine("[ ? -potrebno je navesti kolonu i prednost polja,vise polja razdvajate sa ' & ']\n");
            Console.WriteLine("[ | - potrebno je navesti nazive kolona, radvajate sa ' ; ']");
            Console.WriteLine("Dodatni filteri kad radite nad tabelom resurs su:\n");
            Console.WriteLine("[ % - navodite unutar zagrada dva id-a npr. %(id=1;id=2)]\n");
            Console.WriteLine("[ # - navodite unutar zagrada id-eve npr. #(id=1;id=2)]\n");
            Console.WriteLine("**********************************************************************************************************************\n");
            while (true)
            {

                Console.WriteLine("\n");
                 
                Console.WriteLine("                   > > > > > > > > > >     Unesite Vas zahtev       < < < < < < < < < <                           \n");

                string zahtev = Console.ReadLine();
                Console.WriteLine("=======================================================================================================================\n");
               
                string[] tokens = zahtev.Split(' ');

                string request = "";
                string response = "";

                Validacija validiranje = new Validacija();
                // if (tokens.Count() < 2)
                if (!validiranje.CheckRequest(zahtev))
                {

                    Console.WriteLine("{\n\t\"status\": \"BAD_REQUEST\",\n\t\"code\"\"5000\",\n\t\"payload\":{\"Error message\"\"Niste dobro uneli zahtev!\"}\n}");
                    
                    Console.WriteLine("=========================================================================================================================\n");
                    

                }
                else
                {
                    WebClientKlasa wc = new WebClientKlasa(zahtev);
                    request = wc.ConvertToJSON();
                    XMLAdapterKlasa jx = new XMLAdapterKlasa(request);
                    request = jx.ConvertToXML();
                    CommunicationBusKlasa cb = new CommunicationBusKlasa(request);
                    request = cb.Forward();
                    DBAdapterKlasa xdb = new DBAdapterKlasa(request);
                    request = xdb.ConvertToDB();

                    RESContext pk = new RESContext();

                    if (tokens[1].Contains("resurs"))
                    {
                        IRepository<Resurs> Repository = new ResursRepository(pk);
                        IResursController ResursModel = new ResursController(Repository);
                        IResursOperacije operacije = new ResursOperacije(ResursModel);


                        if (tokens[0].Equals("GET"))
                        {
                            operacije.GetOne(request);
                        }
                        else if (tokens[0].Equals("PATCH"))
                        {
                            operacije.Update(request);
                        }
                        else if (tokens[0].Equals("POST"))
                        {
                            operacije.Insert(request);
                        }
                        else if (tokens[0].Equals("DELETE"))
                        {


                            operacije.Delete(request);
                        }
                        response = operacije.Odgovor;
                    }
                    else if (tokens[1].Contains("/tip/"))
                    {
                        IRepository<Tip> RepositoryTip = new TipRepository(pk);
                        ITipController TipModel = new TipController(RepositoryTip);
                        ITipOperacije op = new TipOperacije(TipModel);

                        if (tokens[0].Equals("GET"))
                        {
                            op.GetOne(request);
                        }
                        else if (tokens[0].Equals("PATCH"))
                        {
                            op.Update(request);
                        }
                        else if (tokens[0].Equals("POST"))
                        {
                            op.Insert(request);
                        }
                        else if (tokens[0].Equals("DELETE"))
                        {


                            op.Delete(request);
                        }
                        response = op.Odgovor;
                    }
                    else if (tokens[1].Contains("/tipveze/"))
                    {

                        IRepository<TipVeze> RepositoryTip = new TipVezeRepository(pk);
                        ITipVezeController Tip = new TipVezeController(RepositoryTip);
                        ITipVezeOperacije op = new TIipVezeOperacije(Tip);

                        if (tokens[0].Equals("GET"))
                        {
                            op.GetOne(request);
                        }
                        else if (tokens[0].Equals("PATCH"))
                        {
                            op.Update(request);
                        }
                        else if (tokens[0].Equals("POST"))
                        {
                            op.Insert(request);
                        }
                        else if (tokens[0].Equals("DELETE"))
                        {


                            op.Delete(request);
                        }
                        response = op.Odgovor;


                    }
                    else
                    {
                        IRepository<Veza> RepositoryVeza = new VezaRepository(pk);
                        IVezaController Veza = new VezaController(RepositoryVeza);
                        IVezaOperacije opVeza = new VezaOperacije(Veza);

                        if (tokens[0].Equals("GET"))
                        {
                            opVeza.GetOne(request);
                        }
                        else if (tokens[0].Equals("PATCH"))
                        {
                            opVeza.Update(request);
                        }
                        else if (tokens[0].Equals("POST"))
                        {
                            opVeza.Insert(request);
                        }
                        else if (tokens[0].Equals("DELETE"))
                        {

                            opVeza.Delete(request);
                        }
                        response = opVeza.Odgovor;

                       
                    }
                    response = xdb.BackToXML(response);
                    response = cb.Back(response);
                    response = jx.BackToJSON(response);
                    wc.Show(response);

                }

            }


        }
    }
}
