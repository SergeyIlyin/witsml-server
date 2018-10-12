using PDS.WITSMLstudio.Store.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Store.Data.Transactions
{
    [Export(typeof(IWitsmlTransaction))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public   class ApiTransaction : WitsmlTransaction
    {
        public ApiTransaction ()
        {
            InitializeRootTransaction();
        }
        public override void Commit()
        {
            Committed = true;
        }
    }
}
