/**********************************************************************
* Description:  Returns a comma-delimited list of baptizer names given a Schedule Item ID
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/24/2009
*
* $Workfile: cust_cccev_funct_bsch_get_baptizers.sql $
* $Revision: 1 $
* $Header: /trunk/Database/Functions/cust/Cccev/bsch/cust_cccev_funct_bsch_get_baptizers.sql   1   2009-12-24 08:34:56-07:00   JasonO $
*
* $Log: /trunk/Database/Functions/cust/Cccev/bsch/cust_cccev_funct_bsch_get_baptizers.sql $
*  
*  Revision: 1   Date: 2009-12-24 15:34:56Z   User: JasonO 
**********************************************************************/

USE [ArenaDB]
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[cust_cccev_funct_bsch_get_baptizers]'))
	DROP FUNCTION [dbo].[cust_cccev_funct_bsch_get_baptizers]
GO

CREATE FUNCTION [dbo].[cust_cccev_funct_bsch_get_baptizers](@ScheduleItemID INT)
RETURNS VARCHAR(2000)

BEGIN
	DECLARE @Baptizers VARCHAR(2000)

	SELECT @Baptizers = COALESCE(@Baptizers + ', ', '') + (p.first_name + ' ' + p.last_name)
	FROM cust_cccev_bsch_baptizer b
	INNER JOIN core_person p
		ON b.person_id = p.person_id
	WHERE b.schedule_item_id = @ScheduleItemID

	RETURN @Baptizers
END