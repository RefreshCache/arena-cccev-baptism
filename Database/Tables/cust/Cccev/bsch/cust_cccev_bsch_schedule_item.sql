/**********************************************************************
* Description:  Create Schedule Item Table
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/24/2009
*
* $Workfile: cust_cccev_bsch_schedule_item.sql $
* $Revision: 1 $
* $Header: /trunk/Database/Tables/cust/Cccev/bsch/cust_cccev_bsch_schedule_item.sql   1   2009-12-24 13:45:15-07:00   JasonO $
*
* $Log: /trunk/Database/Tables/cust/Cccev/bsch/cust_cccev_bsch_schedule_item.sql $
*  
*  Revision: 1   Date: 2009-12-24 20:45:15Z   User: JasonO 
**********************************************************************/

USE [ArenaDB]
GO
/****** Object:  Table [dbo].[cust_cccev_bsch_schedule_item]    Script Date: 12/24/2009 13:42:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cust_cccev_bsch_schedule_item](
	[schedule_item_id] [int] IDENTITY(1,1) NOT NULL,
	[schedule_item_date] [datetime] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_item_schedule_item_date]  DEFAULT ('1/1/1900'),
	[schedule_id] [int] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_item_schedule_id]  DEFAULT ((-1)),
	[person_id] [int] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_item_person_id]  DEFAULT ((-1)),
	[approved_by] [int] NULL,
	[date_approved] [datetime] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_item_date_approved]  DEFAULT ('1/1/1900'),
	[confirmed_by] [int] NULL,
	[is_confirmed] [bit] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_item_is_confirmed]  DEFAULT ((0)),
	[date_confirmed] [datetime] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_item_date_confirmed]  DEFAULT (((1)/(1))/(1900)),
	[created_by] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_item_created_by]  DEFAULT (''),
	[date_created] [datetime] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_item_date_created]  DEFAULT ('1/1/1900'),
	[modified_by] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_item_modified_by]  DEFAULT (''),
	[date_modified] [datetime] NOT NULL CONSTRAINT [DF_cust_cccev_bsch_schedule_item_date_modified]  DEFAULT ('1/1/1900'),
 CONSTRAINT [PK_cust_cccev_bsch_schedule_item] PRIMARY KEY CLUSTERED 
(
	[schedule_item_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[cust_cccev_bsch_schedule_item]  WITH CHECK ADD  CONSTRAINT [FK_cust_cccev_bsch_schedule_item_core_person] FOREIGN KEY([person_id])
REFERENCES [dbo].[core_person] ([person_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cust_cccev_bsch_schedule_item] CHECK CONSTRAINT [FK_cust_cccev_bsch_schedule_item_core_person]
GO
ALTER TABLE [dbo].[cust_cccev_bsch_schedule_item]  WITH CHECK ADD  CONSTRAINT [FK_cust_cccev_bsch_schedule_item_core_person2] FOREIGN KEY([approved_by])
REFERENCES [dbo].[core_person] ([person_id])
GO
ALTER TABLE [dbo].[cust_cccev_bsch_schedule_item] CHECK CONSTRAINT [FK_cust_cccev_bsch_schedule_item_core_person2]
GO
ALTER TABLE [dbo].[cust_cccev_bsch_schedule_item]  WITH CHECK ADD  CONSTRAINT [FK_cust_cccev_bsch_schedule_item_core_person3] FOREIGN KEY([confirmed_by])
REFERENCES [dbo].[core_person] ([person_id])
GO
ALTER TABLE [dbo].[cust_cccev_bsch_schedule_item] CHECK CONSTRAINT [FK_cust_cccev_bsch_schedule_item_core_person3]
GO
ALTER TABLE [dbo].[cust_cccev_bsch_schedule_item]  WITH CHECK ADD  CONSTRAINT [FK_cust_cccev_bsch_schedule_item_cust_cccev_bsch_schedule] FOREIGN KEY([schedule_id])
REFERENCES [dbo].[cust_cccev_bsch_schedule] ([schedule_id])
GO
ALTER TABLE [dbo].[cust_cccev_bsch_schedule_item] CHECK CONSTRAINT [FK_cust_cccev_bsch_schedule_item_cust_cccev_bsch_schedule]