using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class insertPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string insertPerson = @"
CREATE PROCEDURE [dbo].[SP_INSERTPERSON](
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
        VALUES(@PersonName,@Email,@DateOfBirth,@Gender,@CountryId,@Address,@ReceiveNewsLetters)
    END
";
            migrationBuilder.Sql(insertPerson);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string deletePerson = @"DROP PROCEDURE [dbo].[SP_INSERTPERSON]";
            migrationBuilder.Sql(deletePerson);
        }
    }
}
