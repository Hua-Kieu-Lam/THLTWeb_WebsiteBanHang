﻿namespace THLTWeb_WebsiteBanHang.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }
}
