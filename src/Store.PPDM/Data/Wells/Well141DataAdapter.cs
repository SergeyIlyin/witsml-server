using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;


using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;


namespace PDS.WITSMLstudio.Store.Data.Wells
{
    [Export(typeof(PDS.WITSMLstudio.Store.Configuration.IWitsml131Configuration))]
    [Export(typeof(IWitsmlDataAdapter<Well>))]
    [Export131(ObjectTypes.Well, typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Well141DataAdapter : WitsmlDataAdapter<Well>, PDS.WITSMLstudio.Store.Configuration.IWitsml141Configuration
    {
      //  IModulesCollection modulesCollection;
        public Well141DataAdapter( IContainer container) : base(container)
        {
            Logger.Debug("Instance of Well141DataAdapter created.");
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
    
        /// <summary>
        /// Gets a data object by the specified URI.
        /// </summary>
        /// <param name="uri">The data object URI.</param>
        /// <param name="fields">The requested fields.</param>
        /// <returns>The data object instance.</returns>
        //public override Well Get(EtpUri uri, params string[] fields)
        //{
        //    var context = modulesCollection.GetDbContext<Well_Context>();
        //    var WELL = context.WELLs.Where(t => t.UWI == uri.ObjectId).FirstOrDefault();
        //    if (WELL != null)
        //    {
        //        return new Well()
        //        {
        //            Name = WELL.WELL_NAME,
        //            Uid = WELL.UWI
        //        };
        //    }
        //    return null;
        //}


    }
}
