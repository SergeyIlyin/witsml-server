using Energistics.DataAccess.PRODML200;
using Energistics.DataAccess.PRODML200.ReferenceData;
using Energistics.Etp.Common.Datatypes;
using LinqToQuerystring;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Store.Data.Reports
{
    [Export(typeof(IWitsmlDataAdapter<Report>))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Report200DataAdapter : MongoDbDataAdapter<Report>
    {
        [ImportingConstructor]
        public Report200DataAdapter(IContainer container, IDatabaseProvider databaseProvider)
            : base(container, databaseProvider, "Report_20", ObjectTypes.Uuid)
        {
            Logger.Debug("Instance created.");
        }

        public override List<Report> GetAll(EtpUri? parentUri)
        {
            Logger.DebugFormat("Fetching all Reports; Parent URI: {0}", parentUri);

            return GetAllQuery(parentUri)
                .OrderBy(x => x.Citation.Title)
                .ToList();
        }

        protected override IQueryable<Report> GetAllQuery(EtpUri? parentUri)
        {
            var query = GetQuery().AsQueryable();

            if (parentUri != null)
            {
                var objectType = parentUri.Value.ObjectType;
                var objectId = parentUri.Value.ObjectId;


                if (ObjectTypes.Well.EqualsIgnoreCase(objectType))
                    query = query.Where(
                        x => x.Installation != null
                        && x.Installation.Kind == ReportingFacility.well
                        && x.Installation.UidRef == objectId);

                if (!string.IsNullOrWhiteSpace(parentUri.Value.Query))
                    query = query.LinqToQuerystring(parentUri.Value.Query);
            }

            return query;
        }
    }
}
