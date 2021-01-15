using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    state_number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    is_truck = table.Column<bool>(type: "bit", nullable: false),
                    rental_price_per_day = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cars", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    second_name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    passport = table.Column<string>(type: "nchar(10)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discounts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_id = table.Column<long>(type: "bigint", nullable: false),
                    start_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    amount = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_discounts_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_id = table.Column<long>(type: "bigint", nullable: false),
                    car_id = table.Column<long>(type: "bigint", nullable: false),
                    contract_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    full_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    start_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_datetime = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "(dateadd(day,[number_of_days],[start_datetime]))"),
                    number_of_days = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentals", x => x.id);
                    table.ForeignKey(
                        name: "FK_rentals_cars_car_id",
                        column: x => x.car_id,
                        principalTable: "cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rentals_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fines",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rental_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<byte>(type: "tinyint", nullable: false),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fines", x => x.id);
                    table.ForeignKey(
                        name: "FK_fines_rentals_rental_id",
                        column: x => x.rental_id,
                        principalTable: "rentals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "road_accidents",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rental_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    traffic_police_protocol_id = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_road_accidents", x => x.id);
                    table.ForeignKey(
                        name: "FK_road_accidents_rentals_rental_id",
                        column: x => x.rental_id,
                        principalTable: "rentals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_discounts_client_id",
                table: "discounts",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_fines_rental_id",
                table: "fines",
                column: "rental_id");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_car_id",
                table: "rentals",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_client_id",
                table: "rentals",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_road_accidents_rental_id",
                table: "road_accidents",
                column: "rental_id");

            migrationBuilder.Sql(
                @"USE [car_rental]
                GO
                /****** Object:  StoredProcedure [dbo].[car_registration]    Script Date: 01/15/2021 9:54:59 AM ******/
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE PROCEDURE [dbo].[car_registration]
	                @brand nvarchar(50), 
	                @model nvarchar(50),
	                @number nvarchar(10),
	                @is_truck bit = 0,
	                @price_per_day decimal(18, 0)
                AS
                BEGIN
	                INSERT INTO [dbo].[cars] ([brand], [model], [state_number], [is_truck], [rental_price_per_day])
                        VALUES (@brand, @model, @number, @is_truck, @price_per_day)
                END
                GO
                /****** Object:  StoredProcedure [dbo].[client_registration]    Script Date: 01/15/2021 9:54:59 AM ******/
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE PROCEDURE [dbo].[client_registration]
	                @first_name nvarchar(50), 
	                @second_name nvarchar(60),
	                @last_name nvarchar(60),
	                @passport nvarchar(10),
	                @date_of_birth date,
	                @phone nvarchar(15)
                AS
                BEGIN
                INSERT INTO [dbo].[clients] ([first_name], [second_name], [last_name], [passport], [date_of_birth], [phone])
                        VALUES (@first_name, @second_name, @last_name, @passport, @date_of_birth, @phone)
                END
                GO
                /****** Object:  StoredProcedure [dbo].[discount_registration]    Script Date: 01/15/2021 9:54:59 AM ******/
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE PROCEDURE [dbo].[discount_registration]
	                @client_id bigint, 
	                @amount tinyint, 
	                @start_datetime datetime2(7) = GETDATE,
	                @end_datetime datetime2(7) = NULL
                AS
                BEGIN
                INSERT INTO [dbo].[discounts] ([client_id], [start_datetime], [end_datetime], [amount])
                        VALUES (@client_id, @start_datetime, @end_datetime, @amount)
                END
                GO
                /****** Object:  StoredProcedure [dbo].[fines_registration]    Script Date: 01/15/2021 9:54:59 AM ******/
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE PROCEDURE [dbo].[fines_registration]
	                @rental_id bigint, 
	                @amount decimal, 
	                @description nvarchar(200) = NULL
                AS
                BEGIN
                INSERT INTO [dbo].[fines] ([rental_id], [amount], [description])
                        VALUES (@rental_id, @amount, @description)
                END
                GO
                /****** Object:  StoredProcedure [dbo].[rental_registration]    Script Date: 01/15/2021 9:54:59 AM ******/
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE PROCEDURE [dbo].[rental_registration]
	                @client_id bigint, 
	                @car_id bigint, 
	                @contract_number nvarchar(50),
	                @start_datetime datetime2(7) = GETDATE,
	                @number_of_days int
                AS
                BEGIN
                Declare @price decimal(18, 0) = (SELECT [rental_price_per_day] from [dbo].[cars] where [id] = @car_id);
                Declare @discount tinyint = (SELECT LAST_VALUE([amount]) over (order by [id]) from [dbo].[discounts] where [client_id] = @client_id AND [start_datetime] <= GETDATE() AND [end_datetime] > GETDATE()) / 100;
                if (@discount is null)
                set @discount = 1;
                INSERT INTO [dbo].[rentals] ([client_id], [car_id], [contract_number], [full_price], [start_datetime], [number_of_days])
                        VALUES (@client_id, @car_id, @contract_number, @price * @number_of_days * @discount, @start_datetime, @number_of_days);
                END
                GO
                /****** Object:  StoredProcedure [dbo].[road_accidient_registration]    Script Date: 01/15/2021 9:54:59 AM ******/
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE PROCEDURE [dbo].[road_accidient_registration]
	                @rental_id bigint, 
	                @date datetime2(7), 
	                @protocol nvarchar(50) = NULL
                AS
                BEGIN
                INSERT INTO [dbo].[road_accidients] ([rental_id], [date], [traffic_police_protocol_id])
                        VALUES (@rental_id, @date, @protocol)
                END
                GO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "discounts");

            migrationBuilder.DropTable(
                name: "fines");

            migrationBuilder.DropTable(
                name: "road_accidents");

            migrationBuilder.DropTable(
                name: "rentals");

            migrationBuilder.DropTable(
                name: "cars");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.Sql(
                @"USE [car_rental]
                GO
                DROP PROCEDURE [dbo].[car_registration]
                GO
                DROP PROCEDURE [dbo].[client_registration]
                GO
                DROP PROCEDURE [dbo].[discount_registration]
                GO
                DROP PROCEDURE [dbo].[fines_registration]
                GO
                DROP PROCEDURE [dbo].[rental_registration]
                GO
                DROP PROCEDURE [dbo].[road_accidient_registration]
                GO");
        }
    }
}
