USE [car_rental]
GO
/****** Object:  StoredProcedure [dbo].[car_registration]    Script Date: 01/15/2021 12:32:27 AM ******/
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
/****** Object:  StoredProcedure [dbo].[client_registration]    Script Date: 01/15/2021 12:32:27 AM ******/
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
	@phone nchar(15)
AS
BEGIN
INSERT INTO [dbo].[clients] ([first_name], [second_name], [last_name], [passport], [date_of_birth], [phone])
     VALUES (@first_name, @second_name, @last_name, @passport, @date_of_birth, @phone)
END
GO
/****** Object:  StoredProcedure [dbo].[discount_registration]    Script Date: 01/15/2021 12:32:27 AM ******/
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
/****** Object:  StoredProcedure [dbo].[fines_registration]    Script Date: 01/15/2021 12:32:27 AM ******/
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
/****** Object:  StoredProcedure [dbo].[rental_registration]    Script Date: 01/15/2021 12:32:27 AM ******/
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
/****** Object:  StoredProcedure [dbo].[road_accidient_registration]    Script Date: 01/15/2021 12:32:27 AM ******/
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
GO
