using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetEase.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addDoctorRequestTableee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbDoctorRequest_TbDoctor_DoctorId",
                table: "TbDoctorRequest");

            migrationBuilder.DropIndex(
                name: "IX_TbDoctorRequest_DoctorId",
                table: "TbDoctorRequest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TbDoctorRequest_DoctorId",
                table: "TbDoctorRequest",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbDoctorRequest_TbDoctor_DoctorId",
                table: "TbDoctorRequest",
                column: "DoctorId",
                principalTable: "TbDoctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
