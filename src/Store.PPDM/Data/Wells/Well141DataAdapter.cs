using Energistics.DataAccess.WITSML141;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using Energistics.Etp.Common.Datatypes;
using LinqToQuerystring;
using YARUS.API;

namespace PDS.WITSMLstudio.Store.Data.Wells
{
    [Export(typeof(IWitsml141Configuration))]
    [Export(typeof(IWitsmlDataAdapter<Well>))]
    [Export141(ObjectTypes.Well, typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Well141DataAdapter : WitsmlDataAdapter<Well>, PDS.WITSMLstudio.Store.Configuration.IWitsml141Configuration
    {
        //  IModulesCollection modulesCollection;
        [ImportingConstructor]
        public Well141DataAdapter( IContainer container) : base(container)
        {
            Logger.Debug("Instance created.");
            //this.modulesCollection = new PPDM39.Modules.ModulesCollection((t) => t.UseSqlServer("Server=srvugeo07; 
        DbCollectionName = ObjectNames.Well141;
            IdPropertyName = ObjectTypes.Uid;
            NamePropertyName = ObjectTypes.NameProperty;
        }

    protected string DatabaseProvider { get; set; }
    protected string DbCollectionName { get; set; }
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

        public override  List<Well> Query(WitsmlQueryParser parser, ResponseContext context)
        {
            var client = new WellsClient("http://srvugeo07:53537");
            var saved = client.GetAllAsync(null,null, null, null,0,10).Result ;
            var mapped = saved.Items.Select(t => new Well()
            {
                Uid=t.Id ,
                Name=t.WELL_NAME 
            }
            ).ToList();
            return mapped;
        }


    }
}
