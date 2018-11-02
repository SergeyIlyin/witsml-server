using System.ComponentModel.Composition;
using Energistics.DataAccess.WITSML200;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Store.Data.Fields
{
    /// <summary>
    /// Provides validation for <see cref="Well" /> data objects.
    /// </summary>

    [Export(typeof(IDataObjectValidator<Field>))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class Field200Validator : DataObjectValidator<Field>
    {
        [ImportingConstructor]
        public Field200Validator(IContainer container) : base(container)
        {
        }
    }
}
