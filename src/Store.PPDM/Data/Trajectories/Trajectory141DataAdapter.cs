﻿//----------------------------------------------------------------------- 
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

// ----------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost
//     if the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using Energistics.Etp.Common.Datatypes;
using LinqToQuerystring;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;

namespace PDS.WITSMLstudio.Store.Data.Trajectories
{
    /// <summary>
    /// Data adapter that encapsulates CRUD functionality for <see cref="Trajectory" />
    /// </summary>
    /// <seealso cref="PDS.WITSMLstudio.Store.Data.Trajectories.TrajectoryDataAdapter{Trajectory,TrajectoryStation}" />
    [Export(typeof(IWitsml141Configuration))]
    [Export(typeof(IWitsmlDataAdapter<Trajectory>))]
    [Export141(ObjectTypes.Trajectory, typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Trajectory141DataAdapter : TrajectoryDataAdapter<Wellbore, Trajectory, TrajectoryStation>, IWitsml141Configuration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trajectory141DataAdapter" /> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="databaseProvider">The database provider.</param>
        [ImportingConstructor]
        public Trajectory141DataAdapter(IContainer container)
            : base(container, ObjectNames.Trajectory141)
        {
            Logger.Debug("Instance created.");
        }

        /// <summary>
        /// Gets the supported capabilities for the <see cref="Trajectory"/> object.
        /// </summary>
        /// <param name="capServer">The capServer instance.</param>
        public void GetCapabilities(CapServer capServer)
        {
            Logger.DebugFormat("Getting the supported capabilities for Trajectory data version {0}.", capServer.Version);

            capServer.Add(Functions.GetFromStore, ObjectTypes.Trajectory, WitsmlSettings.TrajectoryMaxDataNodesGet);
            capServer.Add(Functions.AddToStore, ObjectTypes.Trajectory, WitsmlSettings.TrajectoryMaxDataNodesAdd);
            capServer.Add(Functions.UpdateInStore, ObjectTypes.Trajectory, WitsmlSettings.TrajectoryMaxDataNodesUpdate);
            capServer.Add(Functions.DeleteFromStore, ObjectTypes.Trajectory, WitsmlSettings.TrajectoryMaxDataNodesDelete);
            capServer.SetGrowingTimeoutPeriod(ObjectTypes.Trajectory, WitsmlSettings.TrajectoryGrowingTimeoutPeriod);
        }


        protected override Trajectory FromParentUri(EtpUri? parentUri = null)
        {
            var dataObject = Activator.CreateInstance<Trajectory>();

            if (parentUri != null)
            {
                var ids = parentUri.Value.GetObjectIds().ToDictionary(x => x.ObjectType, y => y.ObjectId, StringComparer.CurrentCultureIgnoreCase);
                var uidWellbore = ids[ObjectTypes.Wellbore];
                var uidWell = ids[ObjectTypes.Well];

                if (!string.IsNullOrWhiteSpace(uidWell))
                    dataObject.UidWell = uidWell;
                if (!string.IsNullOrWhiteSpace(uidWellbore))
                    dataObject.UidWellbore = uidWellbore;
            }

            return dataObject;
        }
    }
}