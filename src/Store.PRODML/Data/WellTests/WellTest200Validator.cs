using Energistics.DataAccess.PRODML200;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Store.Data.WellTests
{
    [Export(typeof(IDataObjectValidator<WellTest>))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class WellTest200Validator : DataObjectValidator<WellTest>
    {
        private readonly IWitsmlDataAdapter<WellTest> _adapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WellTest" /> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="wellboreDataAdapter">The wellbore data adapter.</param>
        [ImportingConstructor]
        public WellTest200Validator(
            IContainer container,
            IWitsmlDataAdapter<WellTest> adapter)
            : base(container)
        {
            _adapter = adapter;
        }
    }
}
