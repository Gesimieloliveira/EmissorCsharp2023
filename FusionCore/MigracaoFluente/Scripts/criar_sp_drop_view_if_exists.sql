CREATE PROCEDURE [dbo].[sp_drop_view_if_exists] 
@schemaId nvarchar(255),
@viewName nvarchar(255)
	AS
	BEGIN
		SET NOCOUNT ON;
		IF EXISTS(
			SELECT * FROM sys.views WHERE name = @viewName AND schema_id = SCHEMA_ID(@schemaId)
		) EXEC('DROP VIEW ' + @viewName)
    END
GO