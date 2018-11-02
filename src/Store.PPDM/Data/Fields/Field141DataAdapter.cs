using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energistics.DataAccess.WITSML141;
using Energistics.Etp.Common.Datatypes;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;

namespace PDS.WITSMLstudio.Store.Data.Fields
{

    [Export(typeof(IWitsml141Configuration))]
    [Export(typeof(IWitsmlDataAdapter<Field>))]
    [Export141("field", typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Field141DataAdapter : YARUSapiAdapter<Field, Field>, IWitsml141Configuration
    {
        [ImportingConstructor]
        public Field141DataAdapter(IContainer container) : base(container,  "field", "1.4.1.1")
        {

        }

        /// <summary>
        /// Gets the supported capabilities for the <see cref="Wellbore"/> object.
        /// </summary>
        /// <param name="capServer">The capServer instance.</param>
        public void GetCapabilities(CapServer capServer)
        {
            Logger.DebugFormat("Getting the supported capabilities for Wellbore data version {0}.", capServer.Version);

            capServer.Add(Functions.GetFromStore, "field");
            capServer.Add(Functions.AddToStore, "field");
            capServer.Add(Functions.UpdateInStore, "field");
            capServer.Add(Functions.DeleteFromStore, "field");
        }

        public override List<Field> Query(WitsmlQueryParser parser, ResponseContext context)
        {
            return new List<Field>() { new Field() { Uid = "ZNGKM", Name = "Заполярное" }, new Field() { Uid = "YNGKM", Name = "Ямбургское" } };

        }
        public override List<Field> GetAll(EtpUri? parentUri = null)
        {
            return new List<Field>() { new Field() { Uid = "ZNGKM", Name = "Заполярное" }, new Field() { Uid = "YNGKM", Name = "Ямбургское" } };
        }
    }
}
