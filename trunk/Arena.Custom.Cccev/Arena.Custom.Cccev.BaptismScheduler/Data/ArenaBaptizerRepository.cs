/**********************************************************************
* Description:  Ecapsulates data access logic for Baptism Scheduler
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/9/2009
*
* $Workfile: ArenaBaptizerRepository.cs $
* $Revision: 3 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/ArenaBaptizerRepository.cs   3   2009-12-23 13:52:24-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/ArenaBaptizerRepository.cs $
*  
*  Revision: 3   Date: 2009-12-23 20:52:24Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-22 18:07:09Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:56:27Z   User: JasonO 
**********************************************************************/

using System.Data.Linq;
using System.Linq;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.FrameworkUtils.Data;

namespace Arena.Custom.Cccev.BaptismScheduler.Data
{
    public class ArenaBaptizerRepository : IBaptizerRepository
    {
        private readonly ArenaDataContext db;

        public ArenaBaptizerRepository() : this(new ArenaDataContext(ArenaDataContext.CONNECTION_STRING)) { }

        public ArenaBaptizerRepository(DataContext dataContext)
        {
            db = dataContext as ArenaDataContext;
        }

        public Baptizer GetBaptizer(int id)
        {
            return db.GetTable<Baptizer>().SingleOrDefault(b => b.BaptizerID == id);
        }

        public bool Exists(int id)
        {
            return db.GetTable<Baptizer>().Any(b => b.BaptizerID == id);
        }

        public void Delete(Baptizer baptizer)
        {
            db.GetTable<Baptizer>().DeleteOnSubmit(baptizer);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
