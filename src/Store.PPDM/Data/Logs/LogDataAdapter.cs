//----------------------------------------------------------------------- 
// PDS WITSMLstudio Store, 2018.3
//
// Copyright 2018 PDS Americas LLC
// 
// Licensed under the PDS Open Source WITSML Product License Agreement (the
// "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.pds.group/WITSMLstudio/OpenSource/ProductLicenseAgreement
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Energistics.DataAccess;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.Common.Datatypes.ChannelData;

using PDS.WITSMLstudio.Compatibility;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Data.Channels;
using PDS.WITSMLstudio.Store.Configuration;
using PDS.WITSMLstudio.Store.Data.Channels;
using PDS.WITSMLstudio.Store.Data.GrowingObjects;


namespace PDS.WITSMLstudio.Store.Data.Logs
{
    /// <summary>
    /// MongoDb data adapter that encapsulates CRUD functionality for Log objects.
    /// </summary>
    /// <typeparam name="T">The data object type</typeparam>
    /// <typeparam name="TChild">The type of the child.</typeparam>
    /// <seealso cref="PDS.WITSMLstudio.Store.Data.MongoDbDataAdapter{T}" />
    /// <seealso cref="PDS.WITSMLstudio.Store.Data.Channels.IChannelDataProvider" />
    [Export(typeof(IChannelDataProvider))]
    public abstract class LogDataAdapter<T, TChild> :GrowingDataAdapter<T>, IChannelDataProvider where T : IWellboreObject where TChild : IUniqueId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogDataAdapter{T, TChild}" /> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="databaseProvider">The database provider.</param>
        /// <param name="dbCollectionName">Name of the database collection.</param>
        protected LogDataAdapter(IContainer container, ObjectName objectName) :base(container, objectName)
        {
        }

        public IEnumerable<IChannelDataRecord> GetChannelData(EtpUri uri, Range<double?> range)
        {
            throw new NotImplementedException();
        }

        public List<List<List<object>>> GetChannelData(EtpUri uri, Range<double?> range, List<string> mnemonics, int? requestLatestValues, bool optimizeStart = false)
        {
            throw new NotImplementedException();
        }

        public IList<IChannelMetadataRecord> GetChannelMetadata(IEtpAdapter etpAdapter, params EtpUri[] uris)
        {
            throw new NotImplementedException();
        }

        public void UpdateChannelData(EtpUri uri, ChannelDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
