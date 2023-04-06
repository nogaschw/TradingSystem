﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SadnaExpress.DomainLayer.User;
using System;

namespace SadnaExpressTests.Acceptance_Tests.User
{
    [TestClass]
    public class Register : TradingSystemAT
    {
        [TestInitialize]
        public void SetUp()
        {
            base.SetUp();
        }

        [TestMethod]
        public void RegisterHappyTest()
        {
            Assert.IsTrue(proxyBridge.Login(0, "asd1@gmail.com", "Asdqwe123").Value == -1); // log in must fail no user regestered
            //Assert.IsTrue(userFacade.Register(0, "asd1@gmail.com", "asdqwe", "qwezxc", "Asdqwe123")); // user regestered succesfully
            Assert.IsTrue(proxyBridge.Login(0, "asd1@gmail.com", "Asdqwe123").Value == -1); // log in Successed user is regestered
            proxyBridge.Logout(0);
        }

        [TestMethod]
        public void RegisterSadTest()
        {
            // need to fix userFacade.Regestir function return value to return bool 
            // then uncomment the lines with Assert.IsTrue(Register...)
            //Assert.IsTrue(userFacade.Register(1, "asd2@gmail.com", "qwezxc", "zxczxc", "Qwezxc123")); // user regestered succesfully

            Assert.ThrowsException<Exception>(() => proxyBridge.Register(2, "", "qwezxc", "zxczxc", "Qwezxc123")); // user register in with no email
            Assert.ThrowsException<Exception>(() => proxyBridge.Register(2, "asd3@gmail.com", "", "zxczxc", "Qwezxc123")); // user register in with no name
            Assert.ThrowsException<Exception>(() => proxyBridge.Register(2, "asd3@gmail.com", "qwezxc", "zxczxc", "")); // user register in with no password

            Assert.ThrowsException<Exception>(() => proxyBridge.Register(2, "asd2@gmail.com", "Abcd", "defsw", "Asdzxc123")); // user is already registered with this email

        }

        [TestMethod]
        public void RegisterBadTest()
        {

            Assert.ThrowsException<Exception>(() => proxyBridge.Register(2, "asdwqe213", "qwezxc", "zxczxc", "Abcdef123")); // user register in with not valid email
            Assert.ThrowsException<Exception>(() => proxyBridge.Register(2, "asd3@gmail.com", "$$$$$$$", "zxczxc", "Abcdef123")); // user register in with not valid name
            Assert.ThrowsException<Exception>(() => proxyBridge.Register(2, "asd3@gmail.com", "$$$$$$$", "zxczxc", "123")); // user register in with not valid password

        }

        [TestCleanup]
        public void CleanUp()
        {
            proxyBridge.CleanUp();
        }
    }
}
