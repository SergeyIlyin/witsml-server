using Energistics.DataAccess.PRODML200;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Store.Data.Reports
{
    [Export(typeof(IDataObjectValidator<Report>))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class Report200Validator : DataObjectValidator<Report>
    {
        private readonly IWitsmlDataAdapter<Report> _adapter;

        [ImportingConstructor]
        public Report200Validator(
            IContainer container,
            IWitsmlDataAdapter<Report> adapter)
            : base(container)
        {
            _adapter = adapter;
        }
    }
}
