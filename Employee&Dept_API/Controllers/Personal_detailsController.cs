using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using JewelAPI.Models;
using BCrypt.Net;
using Swashbuckle.AspNetCore.Filters; // Added namespace for Swagger example

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Personal_detailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Personal_detailsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Helper method to map string to Gender enum
        private Gender MapToGenderEnum(string genderString)
        {
            switch (genderString.ToLower())
            {
                case "male":
                    return Gender.Male;
                case "female":
                    return Gender.Female;
                case "other":
                    return Gender.Other;
                default:
                    return Gender.Other; // You can choose a default value or handle this case differently
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            string query = @"
                SELECT id, first_name, last_name, email, phone_number, gender
                FROM personal_details
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Jewelappcon");
            MySqlDataReader myReader;

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            List<User> users = new List<User>();
            foreach (DataRow row in table.Rows)
            {
                users.Add(new User
                {
                    Id = Convert.ToInt32(row["id"]),
                    FirstName = row["first_name"].ToString(),
                    LastName = row["last_name"].ToString(),
                    Email = row["email"].ToString(),
                    PhoneNumber = row["phone_number"].ToString(),
                    Gender = MapToGenderEnum(row["gender"].ToString())
                });
            }

            return Ok(users);
        }

        [HttpPost]
        [SwaggerRequestExample(typeof(User), typeof(UserExample))] // Swagger example attribute
        public IActionResult Post(User user)
        {
            string query = @"
                INSERT INTO personal_details 
                (first_name, last_name, email, phone_number, gender, password)
                VALUES
                (@FirstName, @LastName, @Email, @PhoneNumber, @Gender, @Password);
            ";

            string sqlDataSource = _configuration.GetConnectionString("Jewelappcon");

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", user.LastName);
                    myCommand.Parameters.AddWithValue("@Email", user.Email);
                    myCommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    myCommand.Parameters.AddWithValue("@Gender", user.Gender.ToString());

                    // Hash the password
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    // Set the hashed password in the database
                    myCommand.Parameters.AddWithValue("@Password", hashedPassword);

                    myCommand.ExecuteNonQuery();

                    mycon.Close();
                }
            }

            return Ok("Added Successfully");
        }

        [HttpPut]
        [SwaggerRequestExample(typeof(User), typeof(UserExample))] // Swagger example attribute
        public IActionResult Put(User user)
        {
            string query = @"
                UPDATE personal_details SET 
                first_name = @FirstName,
                last_name = @LastName,
                email = @Email,
                phone_number = @PhoneNumber,
                gender = @Gender,
                password = @Password
                WHERE id = @Id;
            ";

            string sqlDataSource = _configuration.GetConnectionString("Jewelappcon");

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Id", user.Id);
                    myCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", user.LastName);
                    myCommand.Parameters.AddWithValue("@Email", user.Email);
                    myCommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    myCommand.Parameters.AddWithValue("@Gender", user.Gender.ToString());

                    // Hash the password
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    // Set the hashed password in the database
                    myCommand.Parameters.AddWithValue("@Password", hashedPassword);

                    myCommand.ExecuteNonQuery();

                    mycon.Close();
                }
            }

            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string query = @"
                DELETE FROM personal_details 
                WHERE id = @Id;
            ";

            string sqlDataSource = _configuration.GetConnectionString("Jewelappcon");

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myCommand.ExecuteNonQuery();

                    mycon.Close();
                }
            }

            return Ok("Deleted Successfully");
        }
    }
}
