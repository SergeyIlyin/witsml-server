using Energistics.DataAccess.WITSML200;
using Energistics.Etp.Common.Datatypes;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Store.Data.Fields
{
    [Export(typeof(IWitsmlDataAdapter<Field>))]
    [Export200("field", typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Field200DataAdapter : YARUSapiAdapter<Field>
    {
        [ImportingConstructor]
        public Field200DataAdapter(IContainer container) : base(container, "field", "2.0")
        {
        }

        public override List<Field> GetAll(EtpUri? parentUri = null)
        {
            return MyFields;
        }
        public override Field Get(EtpUri uri, params string[] fields)
        {
            var id = uri.ObjectId;
            return MyFields.FirstOrDefault(t=>string.Equals( t.Uuid , id,StringComparison.InvariantCultureIgnoreCase));
        }

        List<Field> MyFields = new List<Field>() {
             new Field() { Uuid  = "ZNGKM", Citation=new Energistics.DataAccess.WITSML200.ComponentSchemas.Citation(){Title  = "Заполярное" } },
             new Field() { Uuid  = "YNGKM", Citation=new Energistics.DataAccess.WITSML200.ComponentSchemas.Citation(){Title  = "Ямбургское"   } } };
    }
}
