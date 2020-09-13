using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrewViewServer.Migrations
{
    public partial class New : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlcoholicEntities",
                columns: table => new
                {
                    ProductId = table.Column<string>(nullable: false),
                    Basic_ProductShortName = table.Column<string>(nullable: true),
                    Basic_ProductLongName = table.Column<string>(nullable: true),
                    Basic_Volume = table.Column<double>(nullable: true),
                    Basic_AlcoholContent = table.Column<double>(nullable: true),
                    Basic_Vintage = table.Column<int>(nullable: true),
                    Basic_AgeLimit = table.Column<string>(nullable: true),
                    Basic_PackagingMaterialId = table.Column<string>(nullable: true),
                    Basic_PackagingMaterial = table.Column<string>(nullable: true),
                    Basic_VolumTypeId = table.Column<string>(nullable: true),
                    Basic_VolumType = table.Column<string>(nullable: true),
                    Basic_CorkTypeId = table.Column<string>(nullable: true),
                    Basic_CorkType = table.Column<string>(nullable: true),
                    Basic_BottlePerSalesUnit = table.Column<int>(nullable: true),
                    Basic_IntroductionDate = table.Column<string>(nullable: true),
                    Basic_ProductStatusSaleId = table.Column<string>(nullable: true),
                    Basic_ProductStatusSaleName = table.Column<string>(nullable: true),
                    Basic_ProductStatusSaleValidFrom = table.Column<string>(nullable: true),
                    Logistics_LogisticsId = table.Column<Guid>(nullable: true),
                    Logistics_WholesalerId = table.Column<string>(nullable: true),
                    Logistics_WholesalerName = table.Column<string>(nullable: true),
                    Logistics_VendorId = table.Column<string>(nullable: true),
                    Logistics_VendorName = table.Column<string>(nullable: true),
                    Logistics_VendorValidFrom = table.Column<string>(nullable: true),
                    Logistics_ManufacturerId = table.Column<string>(nullable: true),
                    Logistics_ManufacturerName = table.Column<string>(nullable: true),
                    Logistics_OrderPack = table.Column<string>(nullable: true),
                    Logistics_MinimumOrderQuantity = table.Column<double>(nullable: true),
                    Logistics_PackagingWeight = table.Column<double>(nullable: true),
                    Origins_OriginsId = table.Column<Guid>(nullable: true),
                    Origins_Origin_CountryId = table.Column<string>(nullable: true),
                    Origins_Origin_Country = table.Column<string>(nullable: true),
                    Origins_Origin_RegionId = table.Column<string>(nullable: true),
                    Origins_Origin_Region = table.Column<string>(nullable: true),
                    Origins_Origin_SubRegionId = table.Column<string>(nullable: true),
                    Origins_Origin_SubRegion = table.Column<string>(nullable: true),
                    Origins_Production_CountryId = table.Column<string>(nullable: true),
                    Origins_Production_Country = table.Column<string>(nullable: true),
                    Origins_Production_RegionId = table.Column<string>(nullable: true),
                    Origins_Production_Region = table.Column<string>(nullable: true),
                    Origins_LocalQualityClassifId = table.Column<string>(nullable: true),
                    Origins_LocalQualityClassif = table.Column<string>(nullable: true),
                    Properties_EcoLabellingId = table.Column<string>(nullable: true),
                    Properties_EcoLabelling = table.Column<string>(nullable: true),
                    Properties_StoragePotentialId = table.Column<string>(nullable: true),
                    Properties_StoragePotential = table.Column<string>(nullable: true),
                    Properties_Organic = table.Column<bool>(nullable: true),
                    Properties_Biodynamic = table.Column<bool>(nullable: true),
                    Properties_EthicallyCertified = table.Column<bool>(nullable: true),
                    Properties_VintageControlled = table.Column<bool>(nullable: true),
                    Properties_SweetWine = table.Column<bool>(nullable: true),
                    Properties_FreeOrLowOnGluten = table.Column<bool>(nullable: true),
                    Properties_Kosher = table.Column<bool>(nullable: true),
                    Properties_LocallyProduced = table.Column<bool>(nullable: true),
                    Properties_NoAddedSulphur = table.Column<bool>(nullable: true),
                    Properties_EnvironmentallySmart = table.Column<bool>(nullable: true),
                    Properties_ProductionMethodStorage = table.Column<string>(nullable: true),
                    Classification_MainProductTypeId = table.Column<string>(nullable: true),
                    Classification_MainProductTypeName = table.Column<string>(nullable: true),
                    Classification_SubProductTypeId = table.Column<string>(nullable: true),
                    Classification_SubProductTypeName = table.Column<string>(nullable: true),
                    Classification_ProductTypeId = table.Column<string>(nullable: true),
                    Classification_ProductTypeName = table.Column<string>(nullable: true),
                    Classification_ProductGroupId = table.Column<string>(nullable: true),
                    Classification_ProductGroupName = table.Column<string>(nullable: true),
                    Ingredients_IngredientsId = table.Column<Guid>(nullable: true),
                    Ingredients_Sugar = table.Column<string>(nullable: true),
                    Ingredients_Acid = table.Column<string>(nullable: true),
                    Description_DescriptionId = table.Column<Guid>(nullable: true),
                    Description_Characteristics_Colour = table.Column<string>(nullable: true),
                    Description_Characteristics_Odour = table.Column<string>(nullable: true),
                    Description_Characteristics_Taste = table.Column<string>(nullable: true),
                    Description_Freshness = table.Column<string>(nullable: true),
                    Description_Fullness = table.Column<string>(nullable: true),
                    Description_Bitterness = table.Column<string>(nullable: true),
                    Description_Sweetness = table.Column<string>(nullable: true),
                    Description_Tannins = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlcoholicEntities", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Brews",
                columns: table => new
                {
                    ProductId = table.Column<string>(nullable: false),
                    Gtin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brews", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    PriceId = table.Column<Guid>(nullable: false),
                    PriceValidFrom = table.Column<string>(nullable: true),
                    SalesPrice = table.Column<double>(nullable: true),
                    SalesPricePrLiter = table.Column<double>(nullable: true),
                    BottleReturnValue = table.Column<double>(nullable: true),
                    AlcoholicEntityProductId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_Price_AlcoholicEntities_AlcoholicEntityProductId",
                        column: x => x.AlcoholicEntityProductId,
                        principalTable: "AlcoholicEntities",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBrews",
                columns: table => new
                {
                    ProductId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBrews", x => new { x.ProductId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserBrews_Brews_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Brews",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBrews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    NoteId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    UserBrewProductId = table.Column<string>(nullable: true),
                    UserBrewUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Note_UserBrews_UserBrewProductId_UserBrewUserId",
                        columns: x => new { x.UserBrewProductId, x.UserBrewUserId },
                        principalTable: "UserBrews",
                        principalColumns: new[] { "ProductId", "UserId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Note_UserBrewProductId_UserBrewUserId",
                table: "Note",
                columns: new[] { "UserBrewProductId", "UserBrewUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Price_AlcoholicEntityProductId",
                table: "Price",
                column: "AlcoholicEntityProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBrews_UserId",
                table: "UserBrews",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "UserBrews");

            migrationBuilder.DropTable(
                name: "AlcoholicEntities");

            migrationBuilder.DropTable(
                name: "Brews");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
