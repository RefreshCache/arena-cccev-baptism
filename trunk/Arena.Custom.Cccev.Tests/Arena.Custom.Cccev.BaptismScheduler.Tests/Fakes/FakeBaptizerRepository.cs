/**********************************************************************
* Description:  Fake data repository to simulate ORM
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/9/2009
*
* $Workfile: FakeBaptizerRepository.cs $
* $Revision: 7 $
* $Header: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakeBaptizerRepository.cs   7   2009-12-23 14:04:15-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakeBaptizerRepository.cs $
*  
*  Revision: 7   Date: 2009-12-23 21:04:15Z   User: JasonO 
*  
*  Revision: 6   Date: 2009-12-23 21:04:05Z   User: JasonO 
*  
*  Revision: 5   Date: 2009-12-22 21:28:03Z   User: JasonO 
*  Updating tests to reflect code changes. 
*  
*  Revision: 4   Date: 2009-12-22 20:55:14Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-22 20:53:47Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-15 00:11:18Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:57:19Z   User: JasonO 
**********************************************************************/

using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Arena.Custom.Cccev.BaptismScheduler.Data;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.BaptismScheduler.Tests.Util;

namespace Arena.Custom.Cccev.BaptismScheduler.Tests.Fakes
{
    public class FakeBaptizerRepository : IBaptizerRepository
    {
        private static int count;
        private static readonly List<Baptizer> baptizers = new List<Baptizer>();

        public List<Baptizer> Baptizers { get { return baptizers; } }

        public FakeBaptizerRepository()
        {
            if (baptizers.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Baptizer baptizer = TestFactories.GetBaptizer();
                    baptizer.BaptizerID = i + 1;
                    baptizers.Add(baptizer);
                    count++;
                }
            }
        }

        // ReSharper disable UnusedParameter.Local
        public FakeBaptizerRepository(DataContext dataContext) : this() { }
        // ReSharper restore UnusedParameter.Local

        public void AddBaptizer(Baptizer baptizer)
        {
            count++;
            baptizer.BaptizerID = count;
            baptizers.Add(baptizer);
        }

        public void Delete(Baptizer baptizer)
        {
            baptizers.Remove(baptizer);
        }

        public Baptizer GetBaptizer(int id)
        {
            return baptizers.SingleOrDefault(b => b.BaptizerID == id);
        }

        public bool Exists(int id)
        {
            return baptizers.Any(b => b.BaptizerID == id);
        }

        public void Save()
        {
        }
    }
}
