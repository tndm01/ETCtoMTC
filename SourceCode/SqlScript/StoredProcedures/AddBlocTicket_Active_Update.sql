USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[AddBlocTicket_Active]    Script Date: 23/03/2017 1:23:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery2.sql|7|0|C:\Users\WINUIT\AppData\Local\Temp\~vsD9DC.sql
-- Batch submitted through debugger: SQLQuery5.sql|7|0|C:\Users\WINUIT\AppData\Local\Temp\~vsEF84.sql
CREATE PROCEDURE [dbo].[AddBlocTicket_Active_Update]
@SerialRoot as varchar(20),
@SoLuong as int,
@Login as varchar(20),
@Type as varchar(1)=1,
@Result smallint=1 Output
 AS
DECLARE @sTID varchar(20)
DECLARE @Serial as varchar(20)
DECLARE @SoBatDau as int
declare @iCount as int
declare @sTemp as varchar(20)
select @Serial=left(@SerialRoot,LEN(@SerialRoot) - 7)
select @SoBatDau=convert(int, right(@SerialRoot,7))
select @iCount=@SoBatDau
set @Type = SUBSTRING(@SerialRoot,len(@SerialRoot)-8,1)
while @iCount < @SoBatDau + @SoLuong
    BEGIN
    select @sTemp=right(('0000000'+ convert(varchar(20),@iCount)),7)
    select @sTID= @Serial + @sTemp        
 if(@Type = '1')    
  begin
   DELETE from Active where tid = @sTID
  
   UPDATE dbo.Active
   SET LOGIN = @Login
   WHERE TID =@sTID
  end     
 else     
     INSERT INTO dbo.Active_VeThang_Qui(TID,Login) VALUES(@sTID,@Login)
  select @iCount=@iCount+1
    End    
Return