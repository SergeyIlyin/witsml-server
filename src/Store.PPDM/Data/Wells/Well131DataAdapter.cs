using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Energistics.DataAccess.WITSML131;
using Energistics.DataAccess.WITSML131.ComponentSchemas;
using Energistics.Etp.Common.Datatypes;
using LinqToQuerystring;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;
namespace PDS.WITSMLstudio.Store.Data.Wells
{
    [Export(typeof(PDS.WITSMLstudio.Store.Configuration.IWitsml131Configuration))]
    [Export(typeof(IWitsmlDataAdapter<Well>))]
    [Export131(ObjectTypes.Well, typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Well131DataAdapter : WitsmlDataAdapter<Well>, PDS.WITSMLstudio.Store.Configuration.IWitsml131Configuration
    {
        [ImportingConstructor]
        public Well131DataAdapter(IContainer container) : base(container)
        {
            Logger.Debug("Instance created.");
            //this.modulesCollection = new PPDM39.Modules.ModulesCollection((t) => t.UseSqlServer("Server=srvugeo07; Database=yarus; user=YarusApplication;Password=qwe123"));
        }

        public void GetCapabilities(CapServer capServer)
        {
            Logger.DebugFormat("Getting the supported capabilities for Well data version {0}.", capServer.Version);

            capServer.Add(Functions.GetFromStore, ObjectTypes.Well);
            capServer.Add(Functions.AddToStore, ObjectTypes.Well);
            capServer.Add(Functions.UpdateInStore, ObjectTypes.Well);
            capServer.Add(Functions.DeleteFromStore, ObjectTypes.Well);
        }

    }
}
