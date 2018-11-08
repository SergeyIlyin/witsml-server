using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Energistics.DataAccess.WITSML200;
using Energistics.DataAccess.WITSML200.ComponentSchemas;
using Energistics.Etp.Common.Datatypes;
using LinqToQuerystring;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;

namespace PDS.WITSMLstudio.Store.Data.Wells
{
    [Export(typeof(IWitsmlDataAdapter<Well>))]
    [Export200(ObjectTypes.Well, typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Well200DataAdapter : YARUSapiAdapter<Well>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Well200DataAdapter" /> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="databaseProvider">The database provider.</param>
        [ImportingConstructor]
        public Well200DataAdapter(IContainer container) : base(container, ObjectNames.Well200)
        {

            Logger.Debug("Instance created.");
            IdPropertyName = ObjectTypes.Uuid ;
            NamePropertyName = ObjectTypes.NameProperty;
        }
        protected string IdPropertyName { get; set; }
        protected string NamePropertyName { get; set; }



    }
}