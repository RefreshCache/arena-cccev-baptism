/**********************************************************************
* Description:  Create Blackout Date Table
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/24/2009
*
* $Workfile: cust_cccev_bsch_blackout_date.sql $
* $Revision: 1 $
* $Header: /trunk/Database/Tables/cust/Cccev/bsch/cust_cccev_bsch_blackout_date.sql   1   2009-12-24 13:45:15-07:00   JasonO $
*
* $Log: /trunk/Database/Tables/cust/Cccev/bsch/cust_cccev_bsch_blackout_date.sql $
*  
*  Revision: 1   Date: 2009-12-24 20:45:15Z   User: JasonO 
**********************************************************************/

USE [ArenaDB]
GO
/****** Object:  Table [dbo].[cust_cccev_bsch_blackout_date]    Script Date: 12/24/2009 13:43:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cust_cccev_bsch_blackout_date](
	[blackout_date_id] [int] IDENTITY(1,1) NOT NULL,
	[schedule_id] [int] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_blackout_date_schedule_id]  DEFAULT ((-1)),
	[description] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_cust_cccev_bsch_blackout_date_description]  DEFAULT (''),
	[date] [datetime] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_blackout_date_date]  DEFAULT ('1/1/1900'),
	[created_by] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_cust_cccev_bsch_blackout_date_created_by]  DEFAULT (''),
	[date_created] [datetime] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_blackout_date_date_created]  DEFAULT ('1/1/1900'),
	[modified_by] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_cust_cccev_bsch_blackout_date_modified_by]  DEFAULT (''),
	[date_modified] [datetime] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_blackout_date_date_modified]  DEFAULT ('1/1/1900'),
 CONSTRAINT [PK_cust_cccev_bsch_blackout_date] PRIMARY KEY CLUSTERED 
(
	[blackout_date_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[cust_cccev_bsch_blackout_date]  WITH CHECK ADD  CONSTRAINT [FK_cust_cccev_bsch_blackout_date_cust_cccev_bsch_schedule] FOREIGN KEY([schedule_id])
REFERENCES [dbo].[cust_cccev_bsch_schedule] ([schedule_id])
GO
ALTER TABLE [dbo].[cust_cccev_bsch_blackout_date] CHECK CONSTRAINT [FK_cust_cccev_bsch_blackout_date_cust_cccev_bsch_schedule]