using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using THLTWeb_WebsiteBanHang.Models;

namespace THLTWeb_WebsiteBanHang.Data
{
    public class THLTWeb_WebsiteBanHangContext : IdentityDbContext<ApplicationUser>
    {
        public THLTWeb_WebsiteBanHangContext (DbContextOptions<THLTWeb_WebsiteBanHangContext> options)
            : base(options)
        {
        }

        public DbSet<THLTWeb_WebsiteBanHang.Models.Category> Category { get; set; } = default!;

        public DbSet<THLTWeb_WebsiteBanHang.Models.Product>? Product { get; set; }

        public DbSet<THLTWeb_WebsiteBanHang.Models.ProductImage>? ProductImage { get; set; }

        public DbSet<THLTWeb_WebsiteBanHang.Models.CartItem>? CartItem { get; set; }

        public DbSet<THLTWeb_WebsiteBanHang.Models.Order>? Order { get; set; }

        public DbSet<THLTWeb_WebsiteBanHang.Models.OrderDetail>? OrderDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
