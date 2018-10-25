using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energistics.DataAccess;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Store.Data.Trajectories
{
    public class TrajectoryDataAdapter <T, TChild> : GrowingDataAdapter<T> where T : IWellboreObject where TChild : IUniqueId
    {
        public TrajectoryDataAdapter(IContainer container, ObjectName objectName) : base(container, objectName)
        {
        }
    }
}
