using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicServer.Migrations
{
    public partial class InitMusic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MusicAssets",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicAssets", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "MusicInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Album = table.Column<string>(nullable: true),
                    PublishingYear = table.Column<string>(nullable: true),
                    OwnerId = table.Column<long>(nullable: false),
                    LicenceLink = table.Column<string>(nullable: true),
                    MusicLink = table.Column<string>(nullable: true),
                    DemoLink = table.Column<string>(nullable: true),
                    Key1 = table.Column<string>(nullable: true),
                    Key2 = table.Column<string>(nullable: true),
                    FullKey = table.Column<string>(nullable: true),
                    LicencePrice = table.Column<long>(nullable: false),
                    CreatureType = table.Column<int>(nullable: false),
                    OwnerType = table.Column<int>(nullable: false),
                    TransactionHash = table.Column<string>(nullable: true),
                    ContractAddress = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    TransactionStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareOwnerShips",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MusicId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareOwnerShips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailID = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    userType = table.Column<int>(nullable: false),
                    IsEmailVerified = table.Column<bool>(nullable: false),
                    ActivationCode = table.Column<Guid>(nullable: false),
                    ConfirmPassword = table.Column<string>(nullable: true),
                    ResetPasswordCode = table.Column<string>(nullable: true),
                    OwnerAddress = table.Column<string>(nullable: true),
                    OwnerPrivateKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "MusicAssetTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionHash = table.Column<string>(nullable: true),
                    ContractAddress = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    TransactionStatus = table.Column<int>(nullable: false),
                    MusicId = table.Column<Guid>(nullable: false),
                    MusicInfoId = table.Column<Guid>(nullable: true),
                    FromId = table.Column<string>(nullable: false),
                    FromUserID = table.Column<int>(nullable: true),
                    ToId = table.Column<string>(nullable: false),
                    ToUserID = table.Column<int>(nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    Key2 = table.Column<string>(nullable: true),
                    MediaLink = table.Column<string>(nullable: true),
                    DateTransferred = table.Column<long>(nullable: false),
                    TranType = table.Column<int>(nullable: false),
                    FanType = table.Column<int>(nullable: false),
                    DateStart = table.Column<long>(nullable: false),
                    DateEnd = table.Column<long>(nullable: false),
                    IsPermanent = table.Column<bool>(nullable: false),
                    IsConfirmed = table.Column<bool>(nullable: false),
                    AmountValue = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicAssetTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusicAssetTransfers_Users_FromUserID",
                        column: x => x.FromUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MusicAssetTransfers_MusicInfos_MusicInfoId",
                        column: x => x.MusicInfoId,
                        principalTable: "MusicInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MusicAssetTransfers_Users_ToUserID",
                        column: x => x.ToUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MusicAssetTransfers_FromUserID",
                table: "MusicAssetTransfers",
                column: "FromUserID");

            migrationBuilder.CreateIndex(
                name: "IX_MusicAssetTransfers_MusicInfoId",
                table: "MusicAssetTransfers",
                column: "MusicInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicAssetTransfers_ToUserID",
                table: "MusicAssetTransfers",
                column: "ToUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusicAssets");

            migrationBuilder.DropTable(
                name: "MusicAssetTransfers");

            migrationBuilder.DropTable(
                name: "ShareOwnerShips");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MusicInfos");
        }
    }
}
