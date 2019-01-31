using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energistics.DataAccess.PRODML200;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.Common.Datatypes.Object;
using PDS.WITSMLstudio.Store.Data;

namespace PDS.WITSMLstudio.Store.PRODML.WellTests
{
    [Export(typeof(IEtpDataProvider))]
    [Export(typeof(IEtpDataProvider<WellTest>))]
    public class WellTestDataProvider :YarusApiDataProvider<WellTest>
    {
        [ImportingConstructor ]
        public WellTestDataProvider()
        {
        }

        public override int Count(EtpUri? parentUri)
        {
            if (parentUri.Value.IsBaseUri)
            {
                return 0;
            }
            throw new NotImplementedException();
        }
    }
}
