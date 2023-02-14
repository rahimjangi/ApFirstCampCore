using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class GetPersons_Stored_Procedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_getAllPersons = @"
                CREATE PROCEDURE [dbo].[GetAllPersons]
                AS BEGIN
                    SELECT PersonId,PersonName,CountryId,Email,Address,DateOfBirth,Gender,
                    ReceiveNewsLetters
                    FROM [dbo].[Persons]
                END
            ";
            migrationBuilder.Sql(sp_getAllPersons);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_getAllPersons = @"
                DROP PROCEDURE [dbo].[GetAllPersons]
            ";
            migrationBuilder.Sql(sp_getAllPersons);
        }
    }
}
