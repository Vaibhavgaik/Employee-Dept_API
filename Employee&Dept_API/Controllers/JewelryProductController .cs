using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JewelryShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JewelryProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public JewelryProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                string query = "SELECT * FROM JewelryProducts";

                DataTable table = new DataTable();
                string connectionString = _configuration.GetConnectionString("Jewelappcon");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            command.CommandType = CommandType.Text;
                            adapter.Fill(table);
                        }
                    }
                }

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                };

                options.Converters.Add(new DataTableJsonConverter());

                string json = JsonSerializer.Serialize(table, options);

                return Ok(json);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(JewelryProduct jewelryProduct)
        {
            try
            {
                string query = "INSERT INTO JewelryProducts (Name, ImageUrl, Weight, Price) " +
                               "VALUES (@Name, @ImageUrl, @Weight, @Price)";

                string connectionString = _configuration.GetConnectionString("Jewelappcon");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", jewelryProduct.Name);
                        command.Parameters.AddWithValue("@ImageUrl", jewelryProduct.ImageUrl);
                        command.Parameters.AddWithValue("@Weight", jewelryProduct.Weight);
                        command.Parameters.AddWithValue("@Price", jewelryProduct.Price);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                return Ok("Jewelry product added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, JewelryProduct jewelryProduct)
        {
            try
            {
                string query = "UPDATE JewelryProducts " +
                               "SET Name = @Name, ImageUrl = @ImageUrl, Weight = @Weight, Price = @Price " +
                               "WHERE Id = @Id";

                string connectionString = _configuration.GetConnectionString("Jewelappcon");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Name", jewelryProduct.Name);
                        command.Parameters.AddWithValue("@ImageUrl", jewelryProduct.ImageUrl);
                        command.Parameters.AddWithValue("@Weight", jewelryProduct.Weight);
                        command.Parameters.AddWithValue("@Price", jewelryProduct.Price);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                return Ok("Jewelry product updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                string query = "DELETE FROM JewelryProducts WHERE Id = @Id";

                string connectionString = _configuration.GetConnectionString("Jewelappcon");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                return Ok("Jewelry product deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class DataTableJsonConverter : JsonConverter<DataTable>
        {
            public override DataTable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, DataTable value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();

                foreach (DataRow row in value.Rows)
                {
                    writer.WriteStartObject();

                    foreach (DataColumn col in value.Columns)
                    {
                        if (col.ColumnName != "DataType")
                        {
                            writer.WritePropertyName(col.ColumnName);
                            JsonSerializer.Serialize(writer, row[col], col.DataType, options);
                        }
                    }

                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
            }
        }
    }
}
