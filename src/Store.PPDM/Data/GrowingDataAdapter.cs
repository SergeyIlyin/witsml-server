using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.Common.Datatypes.Object;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Data.GrowingObjects;

namespace PDS.WITSMLstudio.Store.Data
{
    public abstract class GrowingDataAdapter<TEntity> : YARUSapiAdapter<TEntity>, IGrowingObjectDataAdapter
    {
        public GrowingDataAdapter(IContainer container, ObjectName objectName) : base(container, objectName)
        {
        }

        public virtual  bool CanSaveData()
        {
            return true;
        }

        public void DeleteGrowingPart(EtpUri uri, string uid)
        {
            throw new NotImplementedException();
        }

        public void DeleteGrowingParts(EtpUri uri, object startIndex, object endIndex)
        {
            throw new NotImplementedException();
        }

        public IDataObject GetGrowingPart(IEtpAdapter etpAdapter, EtpUri uri, string uid)
        {
            throw new NotImplementedException();
        }

        public List<IDataObject> GetGrowingParts(IEtpAdapter etpAdapter, EtpUri uri, object startIndex, object endIndex)
        {
            throw new NotImplementedException();
        }

        public void PutGrowingPart(IEtpAdapter etpAdapter, EtpUri uri, string contentType, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void UpdateObjectGrowing(EtpUri uri, bool isGrowing)
        {
            throw new NotImplementedException();
        }
    }
}
