﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SadnaExpress.DomainLayer.Store;
using SadnaExpressTests.Integration_Tests;

namespace SadnaExpressTests.Unit_Tests
{
    [TestClass()]
    public class StoreFacadeUT
    {
        private IStoreFacade storeFacade;
        private Guid storeID;



        private static Orders _orders;
        private Guid userID1;
        private Guid userID2;
        private Guid storeID1;
        private Guid itemID1;
        private Guid itemID2;
        private Order order;

        #region SetUp
        [TestInitialize]
        public void SetUp()
        {
            storeFacade = new StoreFacade();
            storeFacade.SetIsSystemInitialize(true);
            storeID = Guid.NewGuid();


            _orders = Orders.Instance;
            userID1 = Guid.NewGuid();
            userID2 = Guid.NewGuid();
            storeID1 = storeFacade.OpenNewStore("Bamba store");
            itemID1 = storeFacade.AddItemToStore(storeID1, "Bamba shosh limited edition", "food", 20.0, 1);
            itemID2 = Guid.NewGuid();
            order = new Order(userID1, storeID1, new List<Guid> { itemID1 }, 70);
            _orders.AddOrder(order);

        }
        #endregion

        #region Tests

        [TestMethod]
        public void openStoreWithOutName()
        {
            Assert.ThrowsException<Exception>(() => storeFacade.OpenNewStore(""));
        }
        
        [TestMethod]
        public void GetItemsByNameSuccess()
        {
            Guid store1 = storeFacade.OpenNewStore("hello");
            storeFacade.AddItemToStore(store1, "Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver", "garden", 4000.0, 1);
            Guid store2 = storeFacade.OpenNewStore("hi");
            storeFacade.AddItemToStore(store2, "Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver", "electronics", 5000.0, 2);
            Assert.AreEqual(2, storeFacade.GetItemsByName("Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver").Count);
            Assert.AreEqual(1, storeFacade.GetItemsByName("Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver", maxPrice:4000).Count);
            Assert.AreEqual(1, storeFacade.GetItemsByName("Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver", category:"garden").Count);
        }
        
        [TestMethod]
        public void GetItemsByNameOneExist()
        {
            Guid store1 = storeFacade.OpenNewStore("hello");
            storeFacade.AddItemToStore(store1, "Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver", "electronics", 4000.0, 1);
            Guid store2 = storeFacade.OpenNewStore("hi");
            storeFacade.AddItemToStore(store2, "Apple iPhone 11 Unlocked, 64GB/128GB/256GB, All Colours", "electronics", 5000.0, 2);
            Assert.AreEqual(1, storeFacade.GetItemsByName("Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver").Count);
        }
        
        [TestMethod]
        public void GetItemsByCategorySuccess()
        {
            Guid store1 = storeFacade.OpenNewStore("hello");
            storeFacade.AddItemToStore(store1, "Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver", "electronics", 4000.0, 1);
            Guid store2 = storeFacade.OpenNewStore("hi");
            storeFacade.AddItemToStore(store2, "Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver", "electronics", 5000.0, 2);
            storeFacade.AddItemToStore(store2, "Apple iPhone 11 Unlocked, 64GB/128GB/256GB, All Colours", "electronics", 5000.0, 2);
            Assert.AreEqual(3, storeFacade.GetItemsByCategory("electronics").Count);
            Assert.AreEqual(2, storeFacade.GetItemsByCategory("electronics", minPrice:4500, maxPrice:5000).Count);
        }
        
        [TestMethod]
        public void GetItemsByCategoryOneExist()
        {
            Guid store1 = storeFacade.OpenNewStore("hello");
            storeFacade.AddItemToStore(store1, "Royal Copenhagen Blue Fluted Full Lace BUTTER PAT 1004 Denmark 3", "dishes", 4000.0, 1);
            Guid store2 = storeFacade.OpenNewStore("hi");
            storeFacade.AddItemToStore(store2, "Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver", "electronics", 5000.0, 2);
            storeFacade.AddItemToStore(store2, "Apple iPhone 11 Unlocked, 64GB/128GB/256GB, All Colours", "electronics", 5000.0, 2);
            Assert.AreEqual(2, storeFacade.GetItemsByCategory("electronics").Count);
        }
        
        [TestMethod]
        public void GetItemsByKeyWordsSuccess()
        {
            Guid store1 = storeFacade.OpenNewStore("hello");
            storeFacade.AddItemToStore(store1, "Apple iPad Air 2 32 GB Space Gray Excellent Condition", "garden", 4000.0, 1);
            Guid store2 = storeFacade.OpenNewStore("hi");
            storeFacade.AddItemToStore(store2, "Apple iPad Air A1474 32GB Wi-Fi 9.7 inch Silver", "electronics", 5000.0, 2);
            storeFacade.AddItemToStore(store2, "Apple iPhone 11 Unlocked, 64GB/128GB/256GB, All Colours", "electronics", 5000.0, 2);
            Assert.AreEqual(3, storeFacade.GetItemsByKeysWord("Apple").Count);
            Assert.AreEqual(2, storeFacade.GetItemsByKeysWord("iPad").Count);
            Assert.AreEqual(1, storeFacade.GetItemsByKeysWord("Gray").Count);
            Assert.AreEqual(2, storeFacade.GetItemsByKeysWord("Apple", category:"electronics").Count);
        }


        [TestMethod]
        public void WriteItemReviewGood()
        {
            storeFacade.WriteItemReview(userID1, storeID1, itemID1, "very bad bamba");
            Assert.AreEqual(1, storeFacade.GetItemReviews(storeID1, itemID1).Count);
        }

        [TestMethod]
        public void WriteItemReviewUserDidNotBuyItem()
        {
            Assert.ThrowsException<NullReferenceException>(() => storeFacade.WriteItemReview(userID2, storeID1, itemID1, "very bad bamba"));
        }

        [TestMethod]
        public void WriteItemReviewUserDidNotBuyItem2()
        {
            Assert.ThrowsException<Exception>(() => storeFacade.WriteItemReview(userID1, storeID1, itemID2, "very bad bamba"));
        }

        #endregion 

        #region CleanUp
        [TestCleanup]
        public void CleanUp()
        {
            storeFacade.CleanUp();
        }
        #endregion

    }
}
