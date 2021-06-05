using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLToDBAdapter;

namespace XmlToDBAdapterTest
{
    [TestFixture]
    public class DBAdapterTest
    {
        [Test]
        public void ConvertToDB()
        {


            string good1 = "<request>\n<verb>GET</verb>\n<noun>/resurs/9</noun>\n</request>";
            string good2 = "<request>\n<verb>GET</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo'</query>\n</request>";
            string good3 = "<request>\n<verb>GET</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo';type=3</query>\n<fields>name</fields>\n</request>";
            // string good4="<request>\n<verb>GET</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo';type=3</query>\n<fields>name</fields>\n<connectedType>id=3;id=6</connectedType>\n</request>";
            string good4 = "<request>\n<verb>DELETE</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo'</query>\n</request>";
            string good5 = "<request>\n<verb>PATCH</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo'</query>\n</request>";
            string good6 = "<request>\n<verb>POST</verb>\n<noun>/resurs/9</noun>\n<query>name='Jovo'</query>\n</request>";

            DBAdapterKlasa db = new DBAdapterKlasa(good1);
            Assert.AreEqual("SELECT * FROM resurs WHERE id=9", db.ConvertToDB());

            DBAdapterKlasa db2 = new DBAdapterKlasa(good2);
            Assert.AreEqual("SELECT * FROM resurs WHERE id=9 AND name='Jovo'", db2.ConvertToDB());

            DBAdapterKlasa db3 = new DBAdapterKlasa(good3);
            Assert.AreEqual("SELECT name FROM resurs WHERE id=9 AND name='Jovo' AND type=3", db3.ConvertToDB());

            DBAdapterKlasa db4 = new DBAdapterKlasa(good4);
            Assert.AreEqual("DELETE FROM resurs WHERE id=9 AND name='Jovo'", db4.ConvertToDB());

            //UPDATE " + tabela + " SET " + koloneSavrednostima + " WHERE 
            DBAdapterKlasa db5 = new DBAdapterKlasa(good5);
            Assert.AreEqual("UPDATE resurs SET name='Jovo' WHERE id=9", db5.ConvertToDB());

            //INSERT INTO " + tabela + " (" + kolone + ")" + " VALUES (" + vrednosti + ");
            DBAdapterKlasa db6 = new DBAdapterKlasa(good6);
            Assert.AreEqual("INSERT INTO resurs (id;name) VALUES (9;'Jovo');", db6.ConvertToDB());

        }

        [Test]

        public void BackToXML()
        {
            string good1 = "SUCCESS;2000;'prezime'='Matijevic','oci'='plave','godina'='18'";
            string good2 = "REJECTED;3000;'Error message'='Nije pronadjeno poklapanje!'";
            string good3 = "SUCCES;2000;'Message'='Uspesno dodato!'";
            string good4 = "REJECTED;3000;'Error message'='Vec postoji element sa navedenim id-em!'";
            string good5 = "SUCCESS;2000;'Message'='Uspesno azurirano!'";
            string good6 = "SUCCESS;2000;'Message'='Uspesno obrisano!'";
            string good7 = "REJECTED;3000;'Error message'='Ne postoji element za brisanje!'";

            DBAdapterKlasa db1 = new DBAdapterKlasa(good1);
            Assert.AreEqual("<response><status>SUCCESS</status><code>2000</code><payload>'prezime'='Matijevic','oci'='plave','godina'='18'</payload></response>", db1.BackToXML(good1));

            DBAdapterKlasa db2 = new DBAdapterKlasa(good2);
            Assert.AreEqual("<response><status>REJECTED</status><code>3000</code><payload>'Error message'='Nije pronadjeno poklapanje!'</payload></response>", db2.BackToXML(good2));

            DBAdapterKlasa db3 = new DBAdapterKlasa(good3);
            Assert.AreEqual("<response><status>SUCCES</status><code>2000</code><payload>'Message'='Uspesno dodato!'</payload></response>", db3.BackToXML(good3));

            DBAdapterKlasa db4 = new DBAdapterKlasa(good4);
            Assert.AreEqual("<response><status>REJECTED</status><code>3000</code><payload>'Error message'='Vec postoji element sa navedenim id-em!'</payload></response>", db4.BackToXML(good4));

            DBAdapterKlasa db5 = new DBAdapterKlasa(good5);
            Assert.AreEqual("<response><status>SUCCESS</status><code>2000</code><payload>'Message'='Uspesno azurirano!'</payload></response>", db5.BackToXML(good5));

            DBAdapterKlasa db6 = new DBAdapterKlasa(good6);
            Assert.AreEqual("<response><status>SUCCESS</status><code>2000</code><payload>'Message'='Uspesno obrisano!'</payload></response>", db6.BackToXML(good6));

            DBAdapterKlasa db7 = new DBAdapterKlasa(good7);
            Assert.AreEqual("<response><status>REJECTED</status><code>3000</code><payload>'Error message'='Ne postoji element za brisanje!'</payload></response>", db7.BackToXML(good7));
        }
    }
}
