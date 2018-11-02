using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Energistics.DataAccess.WITSML200;
using Energistics.DataAccess.WITSML200.ComponentSchemas;
using Energistics.Etp.Common.Datatypes;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Store.Data.Fields
{

    [Export(typeof(IEtpDataProvider))]
    [Export(typeof(IEtpDataProvider<Field>))]
    [Export200("Field", typeof(IEtpDataProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Field200DataProvider : EtpDataProvider<Field>
    {
        [ImportingConstructor]
        public Field200DataProvider(IContainer container, IWitsmlDataAdapter<Field> dataAdapter) : base(container, dataAdapter)
        {
        }
        public override void GetSupportedObjects(IList<EtpContentType> contentTypes)
        {
            var type = typeof(Field);



            var contentType = EtpUris.GetUriFamily(type)
                .Append(ObjectTypes.GetObjectType(type))
                .ContentType;

            contentTypes.Add(contentType);
        }
    }
}
