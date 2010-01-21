/**********************************************************************
* Description:  Gets a list of baptisms given a Schedule ID and Date Range
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/24/2009
*
* $Workfile: cust_cccev_bsch_get_baptismsByScheduleAndDateRange.sql $
* $Revision: 5 $
* $Header: /trunk/Database/Stored Procedures/cust/Cccev/bsch/cust_cccev_bsch_get_baptismsByScheduleAndDateRange.sql   5   2010-01-06 12:36:26-07:00   JasonO $
*
* $Log: /trunk/Database/Stored Procedures/cust/Cccev/bsch/cust_cccev_bsch_get_baptismsByScheduleAndDateRange.sql $
*  
*  Revision: 5   Date: 2010-01-06 19:36:26Z   User: JasonO 
*  
*  Revision: 4   Date: 2010-01-06 18:28:53Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-12-24 15:37:31Z   User: JasonO 
*  
**********************************************************************/

USE [ArenaDB]
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[cust_cccev_bsch_get_baptismsByScheduleAndDateRange]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[cust_cccev_bsch_get_baptismsByScheduleAndDateRange]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[cust_cccev_bsch_get_baptismsByScheduleAndDateRange]
@ScheduleID INT,
@StartDate DATETIME,
@EndDate DATETIME
AS

DECLARE @OrganizationID INT
SET @OrganizationID = 1;

SELECT 
	si.schedule_item_id,
	si.schedule_item_date,
	p.first_name + ' ' + p.last_name AS full_name,
	si.person_id,
	dbo.cust_cccev_funct_bsch_get_baptizers(si.schedule_item_id) AS baptizers,
	(
		SELECT TOP 1
			dbo.fn_FormatPhone(@OrganizationID, ph.phone_number)
		FROM core_person_phone ph
		WHERE ph.person_id = si.person_id
		AND phone_luid = 276
	) AS phone_number,
	a.first_name + ' ' + a.last_name AS approver,
	si.is_confirmed
FROM cust_cccev_bsch_schedule_item si WITH(NOLOCK)
INNER JOIN core_person p
	ON si.person_id = p.person_id
LEFT OUTER JOIN core_person a
	ON si.approved_by = a.person_id
WHERE si.schedule_id = @ScheduleID
AND schedule_item_date BETWEEN @StartDate AND @EndDate
ORDER BY si.schedule_item_date ASC
