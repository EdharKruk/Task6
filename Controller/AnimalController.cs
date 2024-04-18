using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;


namespace Task6.Controller;

[Route("api/animals")]
[ApiController]
public class AnimalController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAnimalById(int id)
    {
        string connectionString = _configuration.GetConnectionString("SQLconnection");
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        var command = new SqlCommand("SELECT * FROM Animal WHERE IdAnimal = @id", connection);
        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (!reader.Read())
        {
            return Ok(null); 
        }
        else
        {
            var animal = new 
            {
                IdAnimal = reader.GetInt32("IdAnimal"),
                Name = reader.GetString("Name"),
                Description = reader.GetString("Description"),
                Category = reader.GetString("Category"),
                Area = reader.GetString("Area")
            };
            return Ok(animal); 
        }
    }
}