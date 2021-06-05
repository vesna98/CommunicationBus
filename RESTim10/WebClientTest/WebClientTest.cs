using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient;

namespace WebClientTest
{
    [TestFixture]
    public class WebClientTest
    {
        [Test]
        [TestCase("GET /resurs/9")]
        [TestCase("POST /tip/10")]
        [TestCase("PATCH /tipveze/14")]
        [TestCase("DELETE /veza/33")]
        public void WebClientKlasaDobarParametar(string zahtev)
        {
            WebClientKlasa wc = new WebClientKlasa(zahtev);

            Assert.AreEqual(wc.Zahtev, zahtev);
        }

        [Test]
        public void ConvertToJSONTest()
        {
            string good = "GET /resurs/9";
            string good1 = "GET /resurs/9?name='Jovo'";
            string good2 = "GET /resurs/9?name='Jovo'&type=3";

            string good5 = "GET /resurs/9?name='Jovo'&type=3|name";
            string good6 = "GET /resurs/9?name='Jovo'&type=3|name#(id=3;id=6)";
            string good7 = "GET /resurs/9?name='Jovo'&type=3|name%(id=3;id=6)";
            string good8 = "GET /resurs/9?name='Jovo'&type=3|name%(id=3;id=6)#(id=5;id=10)";
            string good9 = "GET /resurs/9?name='Jovo'&type=3#(id=5;id=10)|name%(id=3;id=6)";
            string good10 = "GET /resurs/9?name='Jovo'&type=3%(id=3;id=6)|name#(id=5;id=10)";


            WebClientKlasa wc = new WebClientKlasa(good);

            Assert.AreEqual("{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\"\n}", wc.ConvertToJSON());

            WebClientKlasa wc1 = new WebClientKlasa(good1);
            Assert.AreEqual("{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo'\"\n}", wc1.ConvertToJSON());
            WebClientKlasa wc2 = new WebClientKlasa(good2);
            Assert.AreEqual("{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\"\n}", wc2.ConvertToJSON());


            WebClientKlasa wc5 = new WebClientKlasa(good5);
            Assert.AreEqual("{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\",\n\"fields\":\"name\"\n}", wc5.ConvertToJSON());

            WebClientKlasa wc6 = new WebClientKlasa(good6);
            Assert.AreEqual("{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\",\n\"fields\":\"name\",\n\"connectedType\":\"id=3;id=6\"\n}", wc6.ConvertToJSON());

            WebClientKlasa wc7 = new WebClientKlasa(good7);
            Assert.AreEqual("{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\",\n\"fields\":\"name\",\n\"connectedTo\":\"id=3;id=6\"\n}", wc7.ConvertToJSON());

            WebClientKlasa wc8 = new WebClientKlasa(good8);
            Assert.AreEqual("{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\",\n\"fields\":\"name\",\n\"connectedType\":\"id=5;id=10\",\n\"connectedTo\":\"id=3;id=6\"\n}", wc8.ConvertToJSON());

            WebClientKlasa wc9 = new WebClientKlasa(good9);
            Assert.AreEqual("{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\",\n\"fields\":\"name\",\n\"connectedType\":\"id=5;id=10\",\n\"connectedTo\":\"id=3;id=6\"\n}", wc9.ConvertToJSON());

            WebClientKlasa wc10 = new WebClientKlasa(good10);
            Assert.AreEqual("{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\",\n\"fields\":\"name\",\n\"connectedType\":\"id=5;id=10\",\n\"connectedTo\":\"id=3;id=6\"\n}", wc10.ConvertToJSON());
        }
    }
}
