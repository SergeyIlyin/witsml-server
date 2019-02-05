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
    [Export(typeof(IEtpDataProvider))]
    [Export(typeof(IEtpDataProvider<WellTest>))]
    [Export200("WellTest", typeof(IEtpDataProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class WellTest200DataProvider : ProdmlEtpDataProvider<WellTest>
    {
        [ImportingConstructor]
        public WellTest200DataProvider(IContainer container, IWitsmlDataAdapter<WellTest> dataAdapter) : base(container, dataAdapter)
        {
        }
    }
}
