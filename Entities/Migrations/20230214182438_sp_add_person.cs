using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class sp_add_person : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string add_person = @"
CREATE PROCEDURE [dbo].SP_ADD_PERSON(
        @PersonName NVARCHAR(50),
        @Email NVARCHAR(50),
        @DateOfBirth DATE,
        @Gender NVARCHAR(6),
        @CountryId UNIQUEIDENTIFIER,
        @Address NVARCHAR(150),
        @ReceiveNewsLetters BIT
)
    AS BEGIN
        INSERT INTO PERSONS(PersonName,Email,DateOfBirth,Gender,CountryId,Address,ReceiveNewsLetters)
        VALUES(@PersonName,@Email,@DateOfBirth,@Gender,@CountryId,@Address,@ReceiveNewsLetters
        )
    END
    ";
            migrationBuilder.Sql(add_person);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string drop_Person = @"DROP PROCEDURE [dbo].SP_ADD_PERSON";
            migrationBuilder.Sql(drop_Person);

        }
    }
}
