using System.ComponentModel.Composition;
using Energistics.DataAccess.WITSML141;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Store.Data.Wells
{
    [Export(typeof(IDataObjectValidator<Field>))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class Field141Validator : DataObjectValidator<Field>
    {
        [ImportingConstructor]
        public Field141Validator(IContainer container) : base(container)
        {
        }
    }
}
