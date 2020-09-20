﻿// <auto-generated />
using System;
using BrewViewServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BrewViewServer.Migrations
{
    [DbContext(typeof(BrewContext))]
    [Migration("20200920135713_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("BrewViewServer.Models.Brew", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Gtin")
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId");

                    b.ToTable("Brews");
                });

            modelBuilder.Entity("BrewViewServer.Models.FoodAlcoholicEntity", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FoodId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AlcoholicEntityProductId")
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId", "FoodId");

                    b.HasIndex("AlcoholicEntityProductId");

                    b.HasIndex("FoodId");

                    b.ToTable("FoodBrews");
                });

            modelBuilder.Entity("BrewViewServer.Models.GrapeAlcoholicEntity", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<string>("GrapeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AlcoholicEntityProductId")
                        .HasColumnType("TEXT");

                    b.Property<string>("GrapePercent")
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId", "GrapeId");

                    b.HasIndex("AlcoholicEntityProductId");

                    b.HasIndex("GrapeId");

                    b.ToTable("GrapeBrews");
                });

            modelBuilder.Entity("BrewViewServer.Models.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserBrewProductId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserBrewUserId")
                        .HasColumnType("TEXT");

                    b.HasKey("NoteId");

                    b.HasIndex("UserBrewProductId", "UserBrewUserId");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("BrewViewServer.Models.User.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BrewViewServer.Models.UserBrew", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserBrews");
                });

            modelBuilder.Entity("BrewViewServer.Models.VinmonopolModels.AlcoholicEntity", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId");

                    b.ToTable("AlcoholicEntities");
                });

            modelBuilder.Entity("BrewViewServer.Models.VinmonopolModels.Food", b =>
                {
                    b.Property<string>("FoodId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FoodDesc")
                        .HasColumnType("TEXT");

                    b.HasKey("FoodId");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("BrewViewServer.Models.VinmonopolModels.Grape", b =>
                {
                    b.Property<string>("GrapeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("GrapeDesc")
                        .HasColumnType("TEXT");

                    b.HasKey("GrapeId");

                    b.ToTable("Grapes");
                });

            modelBuilder.Entity("BrewViewServer.Models.VinmonopolModels.Price", b =>
                {
                    b.Property<Guid?>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AlcoholicEntityProductId")
                        .HasColumnType("TEXT");

                    b.Property<double?>("BottleReturnValue")
                        .HasColumnType("REAL");

                    b.Property<string>("PriceValidFrom")
                        .HasColumnType("TEXT");

                    b.Property<double?>("SalesPrice")
                        .HasColumnType("REAL");

                    b.Property<double?>("SalesPricePrLiter")
                        .HasColumnType("REAL");

                    b.HasKey("PriceId");

                    b.HasIndex("AlcoholicEntityProductId");

                    b.ToTable("Price");
                });

            modelBuilder.Entity("BrewViewServer.Models.FoodAlcoholicEntity", b =>
                {
                    b.HasOne("BrewViewServer.Models.VinmonopolModels.AlcoholicEntity", "AlcoholicEntity")
                        .WithMany()
                        .HasForeignKey("AlcoholicEntityProductId");

                    b.HasOne("BrewViewServer.Models.VinmonopolModels.Food", "Food")
                        .WithMany()
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BrewViewServer.Models.GrapeAlcoholicEntity", b =>
                {
                    b.HasOne("BrewViewServer.Models.VinmonopolModels.AlcoholicEntity", "AlcoholicEntity")
                        .WithMany()
                        .HasForeignKey("AlcoholicEntityProductId");

                    b.HasOne("BrewViewServer.Models.VinmonopolModels.Grape", "Grape")
                        .WithMany()
                        .HasForeignKey("GrapeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BrewViewServer.Models.Note", b =>
                {
                    b.HasOne("BrewViewServer.Models.UserBrew", null)
                        .WithMany("Notes")
                        .HasForeignKey("UserBrewProductId", "UserBrewUserId");
                });

            modelBuilder.Entity("BrewViewServer.Models.UserBrew", b =>
                {
                    b.HasOne("BrewViewServer.Models.Brew", "Brew")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BrewViewServer.Models.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BrewViewServer.Models.VinmonopolModels.AlcoholicEntity", b =>
                {
                    b.OwnsOne("BrewViewServer.Models.VinmonopolModels.Basic", "Basic", b1 =>
                        {
                            b1.Property<string>("AlcoholicEntityProductId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("AgeLimit")
                                .HasColumnType("TEXT");

                            b1.Property<double?>("AlcoholContent")
                                .HasColumnType("REAL");

                            b1.Property<int?>("BottlePerSalesUnit")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("CorkType")
                                .HasColumnType("TEXT");

                            b1.Property<string>("CorkTypeId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("IntroductionDate")
                                .HasColumnType("TEXT");

                            b1.Property<string>("PackagingMaterial")
                                .HasColumnType("TEXT");

                            b1.Property<string>("PackagingMaterialId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ProductLongName")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ProductShortName")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ProductStatusSaleId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ProductStatusSaleName")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ProductStatusSaleValidFrom")
                                .HasColumnType("TEXT");

                            b1.Property<int?>("Vintage")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("VolumType")
                                .HasColumnType("TEXT");

                            b1.Property<string>("VolumTypeId")
                                .HasColumnType("TEXT");

                            b1.Property<double?>("Volume")
                                .HasColumnType("REAL");

                            b1.HasKey("AlcoholicEntityProductId");

                            b1.ToTable("AlcoholicEntities");

                            b1.WithOwner()
                                .HasForeignKey("AlcoholicEntityProductId");
                        });

                    b.OwnsOne("BrewViewServer.Models.VinmonopolModels.Classification", "Classification", b1 =>
                        {
                            b1.Property<string>("AlcoholicEntityProductId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("MainProductTypeId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("MainProductTypeName")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ProductGroupId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ProductGroupName")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ProductTypeId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ProductTypeName")
                                .HasColumnType("TEXT");

                            b1.Property<string>("SubProductTypeId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("SubProductTypeName")
                                .HasColumnType("TEXT");

                            b1.HasKey("AlcoholicEntityProductId");

                            b1.ToTable("AlcoholicEntities");

                            b1.WithOwner()
                                .HasForeignKey("AlcoholicEntityProductId");
                        });

                    b.OwnsOne("BrewViewServer.Models.VinmonopolModels.Description", "Description", b1 =>
                        {
                            b1.Property<string>("AlcoholicEntityProductId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Bitterness")
                                .HasColumnType("TEXT");

                            b1.Property<Guid?>("DescriptionId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Freshness")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Fullness")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Sweetness")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Tannins")
                                .HasColumnType("TEXT");

                            b1.HasKey("AlcoholicEntityProductId");

                            b1.ToTable("AlcoholicEntities");

                            b1.WithOwner()
                                .HasForeignKey("AlcoholicEntityProductId");

                            b1.OwnsOne("BrewViewServer.Models.VinmonopolModels.Characteristics", "Characteristics", b2 =>
                                {
                                    b2.Property<string>("DescriptionAlcoholicEntityProductId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Colour")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Odour")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Taste")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("DescriptionAlcoholicEntityProductId");

                                    b2.ToTable("AlcoholicEntities");

                                    b2.WithOwner()
                                        .HasForeignKey("DescriptionAlcoholicEntityProductId");
                                });
                        });

                    b.OwnsOne("BrewViewServer.Models.VinmonopolModels.Ingredients", "Ingredients", b1 =>
                        {
                            b1.Property<string>("AlcoholicEntityProductId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Acid")
                                .HasColumnType("TEXT");

                            b1.Property<Guid?>("IngredientsId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Sugar")
                                .HasColumnType("TEXT");

                            b1.HasKey("AlcoholicEntityProductId");

                            b1.ToTable("AlcoholicEntities");

                            b1.WithOwner()
                                .HasForeignKey("AlcoholicEntityProductId");
                        });

                    b.OwnsOne("BrewViewServer.Models.VinmonopolModels.Logistics", "Logistics", b1 =>
                        {
                            b1.Property<string>("AlcoholicEntityProductId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid?>("LogisticsId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ManufacturerId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ManufacturerName")
                                .HasColumnType("TEXT");

                            b1.Property<double?>("MinimumOrderQuantity")
                                .HasColumnType("REAL");

                            b1.Property<string>("OrderPack")
                                .HasColumnType("TEXT");

                            b1.Property<double?>("PackagingWeight")
                                .HasColumnType("REAL");

                            b1.Property<string>("VendorId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("VendorName")
                                .HasColumnType("TEXT");

                            b1.Property<string>("VendorValidFrom")
                                .HasColumnType("TEXT");

                            b1.Property<string>("WholesalerId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("WholesalerName")
                                .HasColumnType("TEXT");

                            b1.HasKey("AlcoholicEntityProductId");

                            b1.ToTable("AlcoholicEntities");

                            b1.WithOwner()
                                .HasForeignKey("AlcoholicEntityProductId");
                        });

                    b.OwnsOne("BrewViewServer.Models.VinmonopolModels.Origins", "Origins", b1 =>
                        {
                            b1.Property<string>("AlcoholicEntityProductId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("LocalQualityClassif")
                                .HasColumnType("TEXT");

                            b1.Property<string>("LocalQualityClassifId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid?>("OriginsId")
                                .HasColumnType("TEXT");

                            b1.HasKey("AlcoholicEntityProductId");

                            b1.ToTable("AlcoholicEntities");

                            b1.WithOwner()
                                .HasForeignKey("AlcoholicEntityProductId");

                            b1.OwnsOne("BrewViewServer.Models.VinmonopolModels.Origin", "Origin", b2 =>
                                {
                                    b2.Property<string>("OriginsAlcoholicEntityProductId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Country")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("CountryId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Region")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("RegionId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("SubRegion")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("SubRegionId")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("OriginsAlcoholicEntityProductId");

                                    b2.ToTable("AlcoholicEntities");

                                    b2.WithOwner()
                                        .HasForeignKey("OriginsAlcoholicEntityProductId");
                                });

                            b1.OwnsOne("BrewViewServer.Models.VinmonopolModels.Production", "Production", b2 =>
                                {
                                    b2.Property<string>("OriginsAlcoholicEntityProductId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Country")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("CountryId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Region")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("RegionId")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("OriginsAlcoholicEntityProductId");

                                    b2.ToTable("AlcoholicEntities");

                                    b2.WithOwner()
                                        .HasForeignKey("OriginsAlcoholicEntityProductId");
                                });
                        });

                    b.OwnsOne("BrewViewServer.Models.VinmonopolModels.Properties", "Properties", b1 =>
                        {
                            b1.Property<string>("AlcoholicEntityProductId")
                                .HasColumnType("TEXT");

                            b1.Property<bool>("Biodynamic")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("EcoLabelling")
                                .HasColumnType("TEXT");

                            b1.Property<string>("EcoLabellingId")
                                .HasColumnType("TEXT");

                            b1.Property<bool>("EnvironmentallySmart")
                                .HasColumnType("INTEGER");

                            b1.Property<bool>("EthicallyCertified")
                                .HasColumnType("INTEGER");

                            b1.Property<bool>("FreeOrLowOnGluten")
                                .HasColumnType("INTEGER");

                            b1.Property<bool>("Kosher")
                                .HasColumnType("INTEGER");

                            b1.Property<bool>("LocallyProduced")
                                .HasColumnType("INTEGER");

                            b1.Property<bool>("NoAddedSulphur")
                                .HasColumnType("INTEGER");

                            b1.Property<bool>("Organic")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("ProductionMethodStorage")
                                .HasColumnType("TEXT");

                            b1.Property<string>("StoragePotential")
                                .HasColumnType("TEXT");

                            b1.Property<string>("StoragePotentialId")
                                .HasColumnType("TEXT");

                            b1.Property<bool>("SweetWine")
                                .HasColumnType("INTEGER");

                            b1.Property<bool>("VintageControlled")
                                .HasColumnType("INTEGER");

                            b1.HasKey("AlcoholicEntityProductId");

                            b1.ToTable("AlcoholicEntities");

                            b1.WithOwner()
                                .HasForeignKey("AlcoholicEntityProductId");
                        });
                });

            modelBuilder.Entity("BrewViewServer.Models.VinmonopolModels.Price", b =>
                {
                    b.HasOne("BrewViewServer.Models.VinmonopolModels.AlcoholicEntity", null)
                        .WithMany("Prices")
                        .HasForeignKey("AlcoholicEntityProductId");
                });
#pragma warning restore 612, 618
        }
    }
}