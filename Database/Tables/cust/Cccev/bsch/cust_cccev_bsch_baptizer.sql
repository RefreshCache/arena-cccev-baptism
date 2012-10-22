/**********************************************************************
* Description:  Create Baptizer Table
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/24/2009
*
* $Workfile: cust_cccev_bsch_baptizer.sql $
* $Revision: 1 $
* $Header: /trunk/Database/Tables/cust/Cccev/bsch/cust_cccev_bsch_baptizer.sql   1   2009-12-24 13:45:15-07:00   JasonO $
*
* $Log: /trunk/Database/Tables/cust/Cccev/bsch/cust_cccev_bsch_baptizer.sql $
*  
*  Revision: 1   Date: 2009-12-24 20:45:15Z   User: JasonO 
**********************************************************************/

USE [ArenaDB]
GO
/****** Object:  Table [dbo].[cust_cccev_bsch_baptizer]    Script Date: 12/24/2009 13:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cust_cccev_bsch_baptizer](
	[baptizer_id] [int] IDENTITY(1,1) NOT NULL,
	[schedule_item_id] [int] NOT NULL,
	[person_id] [int] NOT NULL,
 CONSTRAINT [PK_cust_cccev_bsch_baptizer] PRIMARY KEY CLUSTERED 
(
	[baptizer_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[cust_cccev_bsch_baptizer]  WITH CHECK ADD  CONSTRAINT [FK_cust_cccev_bsch_baptizer_core_person] FOREIGN KEY([person_id])
REFERENCES [dbo].[core_person] ([person_id])
GO
ALTER TABLE [dbo].[cust_cccev_bsch_baptizer] CHECK CONSTRAINT [FK_cust_cccev_bsch_baptizer_core_person]
GO
ALTER TABLE [dbo].[cust_cccev_bsch_baptizer]  WITH CHECK ADD  CONSTRAINT [FK_cust_cccev_bsch_baptizer_cust_cccev_bsch_schedule_item] FOREIGN KEY([schedule_item_id])
REFERENCES [dbo].[cust_cccev_bsch_schedule_item] ([schedule_item_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cust_cccev_bsch_baptizer] CHECK CONSTRAINT [FK_cust_cccev_bsch_baptizer_cust_cccev_bsch_schedule_item]