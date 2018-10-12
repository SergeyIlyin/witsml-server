using Energistics.DataAccess.WITSML141;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using YARUS.API;

namespace PDS.WITSMLstudio.Store.Data.Wells
{
    [Export(typeof(IWitsml141Configuration))]
    [Export(typeof(IWitsmlDataAdapter<Well>))]
    [Export141(ObjectTypes.Well, typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Well141DataAdapter : YARUSapiAdapter<Well>, PDS.WITSMLstudio.Store.Configuration.IWitsml141Configuration
    {
        //  IModulesCollection modulesCollection;
        [ImportingConstructor]
        public Well141DataAdapter(IContainer container) : base(container, ObjectNames.Well141)
        {
            Logger.Debug("Instance created.");
            IdPropertyName = ObjectTypes.Uid;
            NamePropertyName = ObjectTypes.NameProperty;
        }
        protected string IdPropertyName { get; set; }
        protected string NamePropertyName { get; set; }

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
