using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Energistics.DataAccess.WITSML200;
using Energistics.DataAccess.WITSML200.ComponentSchemas;
using Energistics.Etp.Common.Datatypes;
using LinqToQuerystring;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;

namespace PDS.WITSMLstudio.Store.Data.Wells
{
    /// <summary>
    /// Data adapter that encapsulates CRUD functionality for <see cref="Well" />
    /// </summary>
    /// <seealso cref="PDS.WITSMLstudio.Store.Data.MongoDbDataAdapter{Well}" />
    [Export(typeof(IWitsmlDataAdapter<Well>))]
    [Export200(ObjectTypes.Well, typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Well200DataAdapter : WitsmlDataAdapter<Well>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Well200DataAdapter" /> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="databaseProvider">The database provider.</param>
        [ImportingConstructor]
        public Well200DataAdapter(IContainer container)
            : base(container)
        {

            Logger.Debug("Instance created.");
        }
        //public void GetCapabilities(CapServer capServer)
        //{
        //    Logger.DebugFormat("Getting the supported capabilities for Well data version {0}.", capServer.Version);

        //    capServer.Add(Functions.GetFromStore, ObjectTypes.Well);
        //    capServer.Add(Functions.AddToStore, ObjectTypes.Well);
        //    capServer.Add(Functions.UpdateInStore, ObjectTypes.Well);
        //    capServer.Add(Functions.DeleteFromStore, ObjectTypes.Well);
        //}
    }
}