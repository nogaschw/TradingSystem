﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadnaExpress.DomainLayer.Store.DiscountPolicy;

namespace SadnaExpress.API.ClientRequests
{
    public class ClientRequest
    {
        public Guid UserID { get; set; }
    }
    public class LoginRequest : ClientRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterRequest : ClientRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
    public class StoreRequest : ClientRequest
    {
        public Guid StoreId { get; set; }
        public string storeName{ get; set; }
    }
    public class ItemCartRequest : ClientRequest
    {
        public Guid StoreId { get; set; }
        public Guid ItemId { get; set; }
        public int ItemAmount { get; set; }
    }
    public class SearchItemRequest : ClientRequest
    {
        public string ItemName { get; set; }
        public string Category { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int RatingItem { get; set; }
        public int RatingStore { get; set; }
        public string KeyWord { get; set; }
    }
    public class PurchaseRequest : ClientRequest
    {
        public string PaymentDetails { get; set; }
        public string UsersDetail { get; set; }
    }
    public class WriteItemReviewRequest : ClientRequest
    {
        public Guid StoreId { get; set; }
        public Guid ItemId { get; set; }
        public string Review { get; set; }
    }

    public class ItemReview : ClientRequest
    {
        public Guid StoreId { get; set; }
        public Guid ItemId { get; set; }
    }
    public class ItemRequest : ClientRequest
    {
        public Guid storeID { get; set; }
        public string itemName { get; set; }
        public string itemCategory { get; set; }
        public double itemPrice { get; set; }
        public int quantity { get; set; }

    }
    
    public class ItemEditRequest : ItemRequest
    {
        public Guid itemId { get; set; }

    }
    
    public class StoreIDRequest : ClientRequest
    {
        public Guid storeId { get; set; }

    }

    public class StoreManagerRequest : ClientRequest
    {
        public Guid storeId { get; set; }
        public string email { get; set; }

    }
    public class StoreManagerPerRequest : StoreManagerRequest
    {
        public string permission { get; set; }

    }
    
    public class SQARequest : ClientRequest
    {
        public string field { get; set; }
        public string field2 { get; set; }

    }
    public class ListGuidRequest : ClientRequest
    {
        public List<Guid> stores { get; set; }
    }
}
