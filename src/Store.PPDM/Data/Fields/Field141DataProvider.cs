using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energistics.DataAccess.WITSML141;
using Energistics.Etp.Common.Datatypes;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Store.Data.Fields
{
    [Export(typeof(IEtpDataProvider))]
    [Export(typeof(IEtpDataProvider<Field>))]
    [Export141("field", typeof(IEtpDataProvider))]
    [Export141("field", typeof(IWitsmlDataProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Field141DataProvider : WitsmlDataProvider<FieldList, Field>
    {
        [ImportingConstructor]
        public Field141DataProvider(IContainer container, IWitsmlDataAdapter<Field> dataAdapter) : base(container, dataAdapter)
        {
        }

        protected override FieldList CreateCollection(List<Field> dataObjects)
        {
            return new FieldList { Field = dataObjects };
        }

        protected override EtpUri GetUri(Field dataObject)
        {
            return dataObject.GetUri();
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
