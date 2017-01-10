using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests.Store
{
    [TestClass]
    public class Expression_Tests : Base_Tests
    {
        [TestMethod]
        public void Field_0()
        {
            var e0 = new AzureDataLakeClient.OData.ExprField("foo");
            var s = e0.ToExpressionString();
            Assert.AreEqual("foo", s);
        }

        [TestMethod]
        public void Field_1()
        {
            var e0 = new AzureDataLakeClient.OData.ExprField("foo");
            var e1 = new AzureDataLakeClient.OData.ExprField("bar");
            var e2 = new AzureDataLakeClient.OData.ExprLogicalAnd(e0,e1);

            var s = e2.ToExpressionString();
            Assert.AreEqual("(foo and bar)",s);
        }

        [TestMethod]
        public void Field_2()
        {
            var e0 = new AzureDataLakeClient.OData.ExprField("foo");
            var e2 = new AzureDataLakeClient.OData.ExprLogicalAnd(e0);

            var s = e2.ToExpressionString();
            Assert.AreEqual("foo", s);
        }

    }
}
