﻿using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTestApp1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(1,1);
        }
    }
}
