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
   public class ValidacijaTest
    {

        [Test]
        public void VezaCheckTest()
        {

            string goodInpu1 = "idV=12&id1=1&id2=3&type=1";
            string goodInput2 = "idV=12&id1=1&id2=3";
            string goodInput3 = "id1=1&id2=3&type=1";
            string badInput = "aaaaaaaaaaaaaa";
            string badInput2 = "aaaaaaaaaaaaaaid";
            string badInput3 = "id87968";

            Validacija v = new Validacija();
            Assert.AreEqual(true, v.VezaCheck(goodInpu1));
            Assert.AreEqual(true, v.VezaCheck(goodInput2));
            Assert.AreEqual(true, v.VezaCheck(goodInput3));
            Assert.AreEqual(false, v.VezaCheck(badInput));
            Assert.AreEqual(false, v.VezaCheck(badInput2));
            Assert.AreEqual(false, v.VezaCheck(badInput3));
        }

        [Test]
        public void TipCheckTest()
        {
            string goodInpu1 = "id=1";
            string goodInput2 = "name='Laza'";
            string badInput4 = "name='687687'";
            string goodInput3 = "name='Mirko'&id=1";
            string badInput = "aaaaaaaaaaaaaa";
            string badInput2 = "aaaaaaaaaaaaaaid";
            string badInput3 = "id=hgfgefh";

            Validacija v = new Validacija();
            Assert.AreEqual(true, v.TipCheck(goodInpu1));
            Assert.AreEqual(true, v.TipCheck(goodInput2));
            Assert.AreEqual(false, v.TipCheck(badInput4));
            Assert.AreEqual(true, v.TipCheck(goodInput3));
            Assert.AreEqual(false, v.TipCheck(badInput));
            Assert.AreEqual(false, v.TipCheck(badInput2));
            Assert.AreEqual(false, v.TipCheck(badInput3));
        }

        [Test]
        public void ResursCheckTest()
        {
            string goodInpu1 = "type=1";
            string goodInput2 = "name='Laza'";
            string badInput4 = "name='Laza'&type=2&opis=[oci~smedje~ggjg]";
            string badInput5 = "name='Laza'&type=2&opis=[oci~smedje~ggjg";
            string goodInput3 = "name='Laza'&type=2&opis=[oci~smedje]";
            string goodInput4 = "name='Laza77'";
            string badInput = "aaaaaaaaaaaaaa";
            string badInput3 = "type=hgfgefh";
            Validacija v = new Validacija();
            Assert.AreEqual(true, v.ResursCheck(goodInpu1));
            Assert.AreEqual(true, v.ResursCheck(goodInput2));
            Assert.AreEqual(true, v.ResursCheck(goodInput4));
            Assert.AreEqual(true, v.ResursCheck(goodInput3));
            Assert.AreEqual(false, v.ResursCheck(badInput4));
            Assert.AreEqual(false, v.ResursCheck(badInput));
            Assert.AreEqual(true, v.ResursCheck(goodInput4));
            Assert.AreEqual(false, v.ResursCheck(badInput3));
            Assert.AreEqual(false, v.ResursCheck(badInput5));
        }

        [Test]
        public void VezaPoljaTest()
        {
            string goodInpu1 = "idV;id1;id2;type";
            string goodInput2 = "idV;id1;id2";
            string goodInput3 = "id1;id2;type";
            string badInput = "aaaaaaaidVaaaaaa";
            string badInput2 = "aaaaaaaaaaaaaaid";
            string badInput3 = "87968";
            string badInput4 = "id1;id2=87;type";
            string badInput5 = "id1;id2=87;type=kjhkjh";

            Validacija v = new Validacija();
            Assert.AreEqual(true, v.VezaPolja(goodInpu1));
            Assert.AreEqual(true, v.VezaPolja(goodInput2));
            Assert.AreEqual(true, v.VezaPolja(goodInput3));
            Assert.AreEqual(false, v.VezaPolja(badInput));
            Assert.AreEqual(false, v.VezaPolja(badInput2));
            Assert.AreEqual(false, v.VezaPolja(badInput3));
            Assert.AreEqual(false, v.VezaPolja(badInput4));
            Assert.AreEqual(false, v.VezaPolja(badInput5));
        }

        [Test]
        public void TipPoljaTest()
        {
            string goodInpu1 = "id";
            string goodInput2 = "name";
            string goodInput4 = "name;id";
            string goodInput3 = "id;name";
            string badInput = "aaaaaaaaaaaaaa";
            string badInput2 = "aaaaaaaaaaaaaaid";
            string badInput3 = "id=767";

            Validacija v = new Validacija();
            Assert.AreEqual(true, v.TipPolja(goodInpu1));
            Assert.AreEqual(true, v.TipPolja(goodInput2));
            Assert.AreEqual(true, v.TipPolja(goodInput4));
            Assert.AreEqual(true, v.TipPolja(goodInput3));
            Assert.AreEqual(false, v.TipPolja(badInput));
            Assert.AreEqual(false, v.TipPolja(badInput2));
            Assert.AreEqual(false, v.TipPolja(badInput3));
        }

        [Test]
        public void ResursPoljaTest()
        {
            string goodInpu1 = "type";
            string goodInput2 = "name";
            string badInput4 = "name;type=2;opis=[oci~smedje~ggjg]";
            string badInput5 = "name='Laza';type=2;opis=[oci~smedje~ggjg";
            string goodInput3 = "name;type;opis";
            string goodInput4 = "opis";
            string badInput = "aaaaaaaaopisaaaaaa";
            string badInput3 = "type=hgfgefh";
            Validacija v = new Validacija();
            Assert.AreEqual(true, v.ResursPolja(goodInpu1));
            Assert.AreEqual(true, v.ResursPolja(goodInput2));
            Assert.AreEqual(true, v.ResursPolja(goodInput4));
            Assert.AreEqual(true, v.ResursPolja(goodInput3));
            Assert.AreEqual(false, v.ResursPolja(badInput3));
            Assert.AreEqual(false, v.ResursPolja(badInput4));
            Assert.AreEqual(false, v.ResursPolja(badInput5));
            Assert.AreEqual(false, v.ResursPolja(badInput));
        }

        [Test]
        public void ConnectedTypeCHECKTest()
        {
            string good = "(id=5;id=3;id=42;id=2322)";      //popraviti sa ( ) izmedju
            string good2 = "(id=5;id=3;id=4)";
            string good3 = "(id=5)";
            string bad = "id=;id=3;id=42;id=23";
            string bad2 = "(id=;id=3;id=42;id=23)";
            string bad3 = "(id=dfdsg;id=3;id=42;id=23)";

            Validacija v = new Validacija();

            Assert.AreEqual(true, v.ConnectedTypeCHECK(good));
            Assert.AreEqual(true, v.ConnectedTypeCHECK(good2));
            Assert.AreEqual(true, v.ConnectedTypeCHECK(good3));
            Assert.AreEqual(false, v.ConnectedTypeCHECK(bad));
            Assert.AreEqual(false, v.ConnectedTypeCHECK(bad2));
            Assert.AreEqual(false, v.ConnectedTypeCHECK(bad3));
        }

        [Test]
        public void ConnectedToCHECKTest()
        {
            string good = "(id=5;id=3)";
            string good2 = "(id=5;id=3876)";

            string bad = "id=;id=3;id=42;id=23";
            string bad2 = "(id=;id=3;id=42;id=23)";
            string bad3 = "(id=dfdsg;id=3;id=42;id=23)";

            Validacija v = new Validacija();

            Assert.AreEqual(true, v.ConnectedToCHECK(good));
            Assert.AreEqual(true, v.ConnectedToCHECK(good2));
            Assert.AreEqual(false, v.ConnectedToCHECK(bad));
            Assert.AreEqual(false, v.ConnectedToCHECK(bad2));
            Assert.AreEqual(false, v.ConnectedToCHECK(bad3));
        }

        [Test]
        public void CheckRequestTest()
        {
            string good = "GET /resurs/1";
            string good1 = "GET /resurs/1?name='Lazo'";
            string good2 = "GET /resurs/1|name";
            string good3 = "GET /resurs/1?type=10";
            string good4 = "GET /resurs/1%(id=4;id=8)";
            string good5 = "GET /resurs/1#(id=4;id=8)";
            string good6 = "GET /resurs/1#(id=4;id=8;id=45;id=12)";
            string good7 = "DELETE /veza/1?id1=10";
            string good8 = "DELETE /veza/1?id1=10&idV=5";
            string good9 = "PATCH /tip/1?id=10";
            string good10 = "POST /tipveze/1?id=101";
            string good11 = "POST /tipveze/1?id=101|name";
            string good12 = "POST /tipveze/1?id=101&name='Aleksa'";
            string good14 = "POST /tipveze/1|name?id=1";
            string good15 = "POST /resurs/1|name#(id=12;id=43)";
            string good16 = "POST /resurs/1|name#(id=12;id=43)%(id=3;id=7)";
            string good17 = "POST /resurs/1|name#(id=12;id=43)?name='Jovo'%(id=3;id=7)";
            string good18 = "GET /resurs/1#(id=4;id=8;id=45;id=12)?name='Jovo'";
            string bad = "GET /resurs/1gxdg";
            string bad1 = "GET /tip/1%(id=4;id=8)";
            string bad2 = "DELETE /tipveze/1#(id=4;id=8)";
            string bad3 = "DELETE /tipveze/1?id=&";
            string bad4 = "DELETE /tipveze/1?id=hhjgh";
            string bad5 = "PATCH /tipveze/1???id=hhjgh";
            string bad6 = "PATCH /tip/1&id=hhjgh";
            string bad7 = "PATCH /tip/1&id=&&&&&";
            string bad8 = "PATCH /tip/1&id=;";
            string bad9 = "PATCH /veza/1|id=;#";
            string bad10 = "POST /tipveze/1%(id=4;)";

            Validacija v = new Validacija();

            Assert.AreEqual(true, v.CheckRequest(good));
            Assert.AreEqual(true, v.CheckRequest(good1));
            Assert.AreEqual(true, v.CheckRequest(good2));
            Assert.AreEqual(true, v.CheckRequest(good3));
            Assert.AreEqual(true, v.CheckRequest(good4));
            Assert.AreEqual(true, v.CheckRequest(good5));
            Assert.AreEqual(true, v.CheckRequest(good6));
            Assert.AreEqual(true, v.CheckRequest(good7));
            Assert.AreEqual(true, v.CheckRequest(good8));
            Assert.AreEqual(true, v.CheckRequest(good9));
            Assert.AreEqual(true, v.CheckRequest(good10));
            Assert.AreEqual(true, v.CheckRequest(good11));
            Assert.AreEqual(true, v.CheckRequest(good14));
            Assert.AreEqual(true, v.CheckRequest(good12));
            Assert.AreEqual(true, v.CheckRequest(good15));
            Assert.AreEqual(true, v.CheckRequest(good16));
            Assert.AreEqual(true, v.CheckRequest(good17));
            Assert.AreEqual(true, v.CheckRequest(good18));
            Assert.AreEqual(false, v.CheckRequest(bad));
            Assert.AreEqual(false, v.CheckRequest(bad1));
            Assert.AreEqual(false, v.CheckRequest(bad2));
            Assert.AreEqual(false, v.CheckRequest(bad3));
            Assert.AreEqual(false, v.CheckRequest(bad4));
            Assert.AreEqual(false, v.CheckRequest(bad5));
            Assert.AreEqual(false, v.CheckRequest(bad6));
            Assert.AreEqual(false, v.CheckRequest(bad7));
            Assert.AreEqual(false, v.CheckRequest(bad8));
            Assert.AreEqual(false, v.CheckRequest(bad9));
            Assert.AreEqual(false, v.CheckRequest(bad10));
        }

        [Test]
        public void CheckQueryFilterTest()
        {
            string tabela1 = "resurs";
            string tabela2 = "tipveze";
            string tabela3 = "tip";
            string tabela4 = "veza";

            string good = "1?name='Lazo'";
            string good1 = "1?type=10";
            string good2 = "1?id1=10";
            string good3 = "1?id1=10&idV=5";
            string good4 = "1?id=101|name";
            string bad = "1?gxdg";
            string bad1 = "1???id=hhjgh";
            string bad2 = "10?name='Lazo";
            string bad3 = "10?name='Lazo'#";
            string bad4 = "10?='Lazo'";
            string bad5 = "10|name?";

            Validacija v = new Validacija();

            Assert.AreEqual(true, v.CheckQueryFilter(tabela1, good));
            Assert.AreEqual(true, v.CheckQueryFilter(tabela1, good1));
            Assert.AreEqual(true, v.CheckQueryFilter(tabela4, good2));
            Assert.AreEqual(true, v.CheckQueryFilter(tabela4, good3));
            Assert.AreEqual(true, v.CheckQueryFilter(tabela2, good4));
            Assert.AreEqual(false, v.CheckQueryFilter(tabela3, bad));
            Assert.AreEqual(false, v.CheckQueryFilter(tabela3, bad1));
            Assert.AreEqual(false, v.CheckQueryFilter(tabela1, bad2));
            Assert.AreEqual(false, v.CheckQueryFilter(tabela1, bad3));
            Assert.AreEqual(false, v.CheckQueryFilter(tabela1, bad4));
            Assert.AreEqual(false, v.CheckQueryFilter(tabela1, bad5));
        }

        [Test]
        public void CheckFieldsTest()
        {
            string tabela1 = "resurs";
            string tabela2 = "tipveze";
            string tabela3 = "tip";
            string tabela4 = "veza";

            string good = "1|name";
            string good1 = "1|name#(id=12;id=43)";
            string good2 = "1|name#(id=12;id=43)?name='Jovo'%(id=3;id=7)";
            string good3 = "1|idV;id1";
            string good4 = "1|idV;id1?id2=54";
            string good5 = "1|idV;id1?id2=54;type=33";
            string good6 = "1|name";
            string good7 = "1|name;id";
            string bad = "1|id=;#";
            string bad1 = "1|id=76????";
            string bad2 = "1|id=76";
            string bad3 = "1|id#(id=fsdfga)";
            string bad4 = "1|id#(name=5677)";
            string bad5 = "1|id#(id)";
            string bad6 = "1|id#(id;fds;fsdf;;;)";
            string bad7 = "1|id=343";
            string bad8 = "1|name=5435;3";
            string bad9 = "1|name=5435;3&&&";



            Validacija v = new Validacija();

            Assert.AreEqual(true, v.CheckFields(tabela1, good));
            Assert.AreEqual(true, v.CheckFields(tabela1, good1));
            Assert.AreEqual(true, v.CheckFields(tabela1, good2));
            Assert.AreEqual(true, v.CheckFields(tabela4, good3));
            Assert.AreEqual(true, v.CheckFields(tabela4, good4));
            Assert.AreEqual(true, v.CheckFields(tabela4, good5));
            Assert.AreEqual(true, v.CheckFields(tabela3, good6));
            Assert.AreEqual(true, v.CheckFields(tabela2, good7));
            Assert.AreEqual(false, v.CheckFields(tabela4, bad));
            Assert.AreEqual(false, v.CheckFields(tabela4, bad1));
            Assert.AreEqual(false, v.CheckFields(tabela4, bad2));
            Assert.AreEqual(false, v.CheckFields(tabela1, bad3));
            Assert.AreEqual(false, v.CheckFields(tabela4, bad4));
            Assert.AreEqual(false, v.CheckFields(tabela4, bad5));
            Assert.AreEqual(false, v.CheckFields(tabela3, bad6));
            Assert.AreEqual(false, v.CheckFields(tabela2, bad7));
            Assert.AreEqual(false, v.CheckFields(tabela3, bad8));
            Assert.AreEqual(false, v.CheckFields(tabela3, bad9));
        }
    }
}
