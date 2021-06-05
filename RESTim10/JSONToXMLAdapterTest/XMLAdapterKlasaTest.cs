using JSONToXMLAdapter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONToXMLAdapterTest
{
    [TestFixture]
    public class XMLAdapterKlasaTest
    {
        [Test]
        public void ConvertToXML()
        {
            string good1 = "{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\"\n}";
            string good2 = "{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo'\"\n}";
            string good3 = "{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\",\n\"fields\":\"name\"\n}";
            string good4 = "{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\",\n\"fields\":\"name\",\n\"connectedType\":\"id=3;id=6\"\n}";
            string good5 = "{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\",\n\"fields\":\"name\",\n\"connectedTo\":\"id=3;id=6\"\n}";
            string good6 = "{\n\"verb\":\"GET\",\n\"noun\":\"/resurs/9\",\n\"query\":\"name='Jovo';type=3\",\n\"fields\":\"name\",\n\"connectedType\":\"id=5;id=10\",\n\"connectedTo\":\"id=3;id=6\"\n}";

            XMLAdapterKlasa xak1 = new XMLAdapterKlasa(good1);
            Assert.AreEqual("<request>\n<verb>GET</verb>\n<noun>/resurs/9</noun>\n</request>", xak1.ConvertToXML());

            XMLAdapterKlasa xak2 = new XMLAdapterKlasa(good2);
            Assert.AreEqual("<request>\n<verb>GET</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo'</query>\n</request>", xak2.ConvertToXML());

            XMLAdapterKlasa xak3 = new XMLAdapterKlasa(good3);
            Assert.AreEqual("<request>\n<verb>GET</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo';type=3</query>\n<fields>name</fields>\n</request>", xak3.ConvertToXML());

            XMLAdapterKlasa xak4 = new XMLAdapterKlasa(good4);
            Assert.AreEqual("<request>\n<verb>GET</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo';type=3</query>\n<fields>name</fields>\n<connectedType>id=3;id=6</connectedType>\n</request>", xak4.ConvertToXML());

            XMLAdapterKlasa xak5 = new XMLAdapterKlasa(good5);
            Assert.AreEqual("<request>\n<verb>GET</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo';type=3</query>\n<fields>name</fields>\n<connectedTo>id=3;id=6</connectedTo>\n</request>", xak5.ConvertToXML());

            XMLAdapterKlasa xak6 = new XMLAdapterKlasa(good6);
            Assert.AreEqual("<request>\n<verb>GET</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo';type=3</query>\n<fields>name</fields>\n<connectedTo>id=3;id=6</connectedTo>\n<connectedType>id=5;id=10</connectedType>\n</request>", xak6.ConvertToXML());


        }


        [Test]
        //"{\"status\":\"" + status + "\",\"code\":\"" + kod + "\",\"payload\":{" + payload + "}}"
        public void BackToJSON()
        {
            string good1 = "<response><status>SUCCESS</status><code>2000</code><payload>'prezime'='Matijevic','oci'='plave','godina'='18'</payload></response>";
            string good2 = "<response><status>REJECTED</status><code>3000</code><payload>'Error message'='Nije pronadjeno poklapanje!'</payload></response>";
            string good3 = "<response><status>SUCCESS</status><code>2000</code><payload>'Message'='Uspesno dodato!'</payload></response>";
            string good4 = "<response><status>REJECTED</status><code>3000</code><payload>'Error message'='Vec postoji element sa navedenim id-em!'</payload></response>";
            string good5 = "<response><status>SUCCESS</status><code>2000</code><payload>'Message'='Uspesno azurirano!'</payload></response>";
            string good6 = "<response><status>SUCCESS</status><code>2000</code><payload>'Message'='Uspesno obrisano!'</payload></response>";
            string good7 = "<response><status>REJECTED</status><code>3000</code><payload>'Error message'='Ne postoji element za brisanje!'</payload></response>";
            XMLAdapterKlasa xak1 = new XMLAdapterKlasa(good1);
            Assert.AreEqual("{\"status\":\"SUCCESS\",\"code\":\"2000\",\"payload\":{\"prezime\":\"Matijevic\",\"oci\":\"plave\",\"godina\":\"18\"}}", xak1.BackToJSON(good1));

            XMLAdapterKlasa xak2 = new XMLAdapterKlasa(good2);
            Assert.AreEqual("{\"status\":\"REJECTED\",\"code\":\"3000\",\"payload\":{\"Error message\":\"Nije pronadjeno poklapanje!\"}}", xak2.BackToJSON(good2));

            XMLAdapterKlasa xak3 = new XMLAdapterKlasa(good3);
            Assert.AreEqual("{\"status\":\"SUCCESS\",\"code\":\"2000\",\"payload\":{\"Message\":\"Uspesno dodato!\"}}", xak3.BackToJSON(good3));

            XMLAdapterKlasa xak4 = new XMLAdapterKlasa(good4);
            Assert.AreEqual("{\"status\":\"REJECTED\",\"code\":\"3000\",\"payload\":{\"Error message\":\"Vec postoji element sa navedenim id-em!\"}}", xak4.BackToJSON(good4));

            XMLAdapterKlasa xak5 = new XMLAdapterKlasa(good5);
            Assert.AreEqual("{\"status\":\"SUCCESS\",\"code\":\"2000\",\"payload\":{\"Message\":\"Uspesno azurirano!\"}}", xak5.BackToJSON(good5));

            XMLAdapterKlasa xak6 = new XMLAdapterKlasa(good6);
            Assert.AreEqual("{\"status\":\"SUCCESS\",\"code\":\"2000\",\"payload\":{\"Message\":\"Uspesno obrisano!\"}}", xak6.BackToJSON(good6));

            XMLAdapterKlasa xak7 = new XMLAdapterKlasa(good7);
            Assert.AreEqual("{\"status\":\"REJECTED\",\"code\":\"3000\",\"payload\":{\"Error message\":\"Ne postoji element za brisanje!\"}}", xak7.BackToJSON(good7));
        }
    }
}
