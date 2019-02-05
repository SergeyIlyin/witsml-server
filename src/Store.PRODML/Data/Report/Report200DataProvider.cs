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
    [Export(typeof(IEtpDataProvider))]
    [Export(typeof(IEtpDataProvider<Report>))]
    [Export200("WellTest", typeof(IEtpDataProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Report200DataProvider : ProdmlEtpDataProvider<Report>
    {
        [ImportingConstructor]
        public Report200DataProvider(IContainer container, IWitsmlDataAdapter<Report> dataAdapter) : base(container, dataAdapter)
        {
        }

    }
}
