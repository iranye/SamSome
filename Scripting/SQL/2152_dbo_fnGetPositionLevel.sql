SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



ALTER   FUNCTION dbo.fnGetPositionLevel
    (
        @String         varchar(100),       -- The input string value that will be parsed.
        @ParseChar  char(1),            -- The type of character that is used to distinguish between levels
        @Level      int             -- The level in which you would like the position to
    )
RETURNS INT AS
BEGIN
    /*
    This function returns the starting of the level specified.
    Written By Casey Tercek
    Date: 10/20/2005
    
    --This is meant for debugging
    declare @String varchar(100),
    @ParseChar  char(1),            
    @Level      int
    set @String = 'Chase-0008-1002-0001-0001'
    set @ParseChar = '-'
    set @Level = 3
    */
    if @string is not Null
        BEGIN
            declare @CurPosition    int
            declare @PartName       varchar(100)
            declare @CurChar        char(1)
            declare @intFolder      int
            declare @LevelPosition  int
            
            set         @PartName   = @String
            set         @CurPosition    = 0
            set         @intFolder  = 1
            set         @LevelPosition = 0
            
            while @CurPosition <> len(@PartName)
                begin
                    set @CurChar = substring(@PartName,@CurPosition,1)
                    if @CurChar = '-'
                        begin
                            set @intFolder = (@intFolder + 1)
                            if @intFolder = @Level
                                begin
                                    
                                    set @LevelPosition = @CurPosition
                                    break
                                end
                        end
                    set @CurPosition = (@CurPosition + 1)
                end
            --print @CurPosition
            set @CurPosition = (@CurPosition + 1)
        END
    ELSE    
        BEGIN
            SET @CurPosition = 0
        END
    RETURN @CurPosition
END







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

