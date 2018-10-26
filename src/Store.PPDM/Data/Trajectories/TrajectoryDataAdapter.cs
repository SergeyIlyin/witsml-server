using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energistics.DataAccess;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Store.Data.Trajectories
{
    public class TrajectoryDataAdapter<TParent, TEntity, TChild> : GrowingDataAdapter<TParent, TEntity> where TEntity : IWellboreObject where TChild : IUniqueId
    {
        public TrajectoryDataAdapter(IContainer container, ObjectName objectName) : base(container, objectName)
        {
        }
    }
}
