/**********************************************************************
* Description:  Create Schedule Table
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/24/2009
*
* $Workfile: cust_cccev_bsch_schedule.sql $
* $Revision: 1 $
* $Header: /trunk/Database/Tables/cust/Cccev/bsch/cust_cccev_bsch_schedule.sql   1   2009-12-24 13:45:15-07:00   JasonO $
*
* $Log: /trunk/Database/Tables/cust/Cccev/bsch/cust_cccev_bsch_schedule.sql $
*  
*  Revision: 1   Date: 2009-12-24 20:45:15Z   User: JasonO 
**********************************************************************/

USE [ArenaDB]
GO
/****** Object:  Table [dbo].[cust_cccev_bsch_schedule]    Script Date: 12/24/2009 13:40:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cust_cccev_bsch_schedule](
	[schedule_id] [int] IDENTITY(1,1) NOT NULL,
	[campus_luid] [int] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_campus_luid]  DEFAULT ((-1)),
	[name] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_name]  DEFAULT (''),
	[description] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_description]  DEFAULT (''),
	[date_created] [datetime] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_date_created]  DEFAULT ('1/1/1900'),
	[created_by] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_created_by]  DEFAULT (''),
	[date_modified] [datetime] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_date_modified]  DEFAULT ('1/1/1900'),
	[modified_by] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_modified_by]  DEFAULT (''),
 CONSTRAINT [PK_cust_cccev_bsch_schedule] PRIMARY KEY CLUSTERED 
(
	[schedule_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[cust_cccev_bsch_schedule]  WITH CHECK ADD  CONSTRAINT [FK_cust_cccev_bsch_schedule_core_lookup] FOREIGN KEY([campus_luid])
REFERENCES [dbo].[core_lookup] ([lookup_id])
GO
ALTER TABLE [dbo].[cust_cccev_bsch_schedule] CHECK CONSTRAINT [FK_cust_cccev_bsch_schedule_core_lookup]