using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient.Store
{
    [TestClass]
    public class Expression_Tests : Base_Tests
    {
        [TestMethod]
        public void Field_0()
        {
            var e0 = new AdlClient.OData.ExprField("foo");
            var s = e0.ToExpressionString();
            Assert.AreEqual("foo", s);
        }

        [TestMethod]
        public void Field_1()
        {
            var e0 = new AdlClient.OData.ExprField("foo");
            var e1 = new AdlClient.OData.ExprField("bar");
            var e2 = new AdlClient.OData.ExprLogicalAnd(e0,e1);

            var s = e2.ToExpressionString();
            Assert.AreEqual("(foo and bar)",s);
        }

        [TestMethod]
        public void Field_2()
        {
            var e0 = new AdlClient.OData.ExprField("foo");
            var e2 = new AdlClient.OData.ExprLogicalAnd(e0);

            var s = e2.ToExpressionString();
            Assert.AreEqual("foo", s);
        }

    }
}
