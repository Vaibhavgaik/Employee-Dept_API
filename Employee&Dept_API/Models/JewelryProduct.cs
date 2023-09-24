
using Microsoft.AspNetCore.Mvc; // For defining API controllers
using Microsoft.Extensions.Configuration; // For accessing configuration settings
using MySql.Data.MySqlClient; // For MySQL database operations
using System; // For general system-related functionality
using System.Data; // For working with data tables
public class JewelryProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public double Weight { get; set; }
    public decimal Price { get; set; }
}
