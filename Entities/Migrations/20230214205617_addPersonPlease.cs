using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class addPersonPlease : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string pp = @"
CREATE PROCEDURE [dbo].[SP_INSERTPERSON](
        @PersonId UNIQUEIDENTIFIER,
        @PersonName NVARCHAR(50),
        @Email NVARCHAR(50),
        @DateOfBirth DATE,
        @Gender NVARCHAR(6),
        @CountryId UNIQUEIDENTIFIER,
        @Address NVARCHAR(150),
        @ReceiveNewsLetters BIT

)
    AS BEGIN
        INSERT INTO [dbo].[Persons](PersonId,PersonName,Email,DateOfBirth,Gender,CountryId,Address,ReceiveNewsLetters)
        VALUES(@PersonId,@PersonName,@Email,@DateOfBirth,@Gender,@CountryId,@Address,@ReceiveNewsLetters)
    END";
            migrationBuilder.Sql(pp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string pp = @"DROP PROCEDURE [dbo].[SP_INSERTPERSON]";
            migrationBuilder.Sql(pp);
        }
    }
}
