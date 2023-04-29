using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SadnaExpress.DomainLayer.Store
{
    public class Item
    {
        private Guid itemID;
        public Guid ItemID {get=>itemID;}

        private string name;
        public string Name {get => name; set => name = value;}
        private string category;
        public string Category {get => category; set => category = value;}
        private double price;
        public double Price {get => price; set => price = value;}
        private int rating;
        public int Rating {get => rating; set => rating = value;}
        
        public Item(string name, string category, double price)
        {
            this.name = name;
            this.category = category;
            this.price = price;
            rating = 0;
            itemID = Guid.NewGuid(); 
        }
    }
}