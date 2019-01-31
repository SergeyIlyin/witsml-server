using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.Common.Datatypes.Object;
using Energistics.Etp.v11.Datatypes.Object;
using Energistics.Etp.v11.Protocol.Discovery;
using Energistics.Etp.v12.Datatypes.Object;
using Energistics.Etp.v12.Protocol.Discovery;
using Energistics.Etp.v12.Protocol.DiscoveryQuery;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;
using PDS.WITSMLstudio.Store.Data;
using Energistics.DataAccess.PRODML200.ComponentSchemas;
using PDS.WITSMLstudio.Framework;
using Energistics.DataAccess.PRODML200;
namespace PDS.WITSMLstudio.Store.Providers.Discovery
{

    [Export(typeof(IDiscoveryStoreProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Prodml200Provider : IDiscoveryStoreProvider
    {
        private readonly IContainer _container;

        private readonly IList<EtpContentType> _contentTypes;

        [ImportingConstructor]
        public Prodml200Provider(IContainer container)
        {
            _container = container;
            _contentTypes = new List<EtpContentType>();
        }


        public string DataSchemaVersion => OptionsIn.DataVersion.Version200.Value;

        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<IEtpDataProvider> Providers { get; set; }

        public void FindResources(IEtpAdapter etpAdapter, ProtocolEventArgs<FindResources, IList<Energistics.Etp.v12.Datatypes.Object.Resource>> args)
        {
            throw new NotImplementedException();
        }

        public void GetResources(IEtpAdapter etpAdapter, ProtocolEventArgs<Energistics.Etp.v11.Protocol.Discovery.GetResources, IList<Energistics.Etp.v11.Datatypes.Object.Resource>> args)
        {
            GetResources(etpAdapter, args.Message.Uri, args.Context);
        }

        public void GetResources(IEtpAdapter etpAdapter, ProtocolEventArgs<Energistics.Etp.v12.Protocol.Discovery.GetResources, IList<Energistics.Etp.v12.Datatypes.Object.Resource>> args)
        {
            GetResources(etpAdapter, args.Message.Uri, args.Context);
        }
        private void GetResources<T>(IEtpAdapter etpAdapter, string uri, IList<T> resources) where T : IResource
        {
            if (EtpUris.IsRootUri(uri))
            {
                resources.Add(etpAdapter.NewProtocol(EtpUris.Prodml200, "PRODML Store (2.0)"));
                return;
            }

            var etpUri = new EtpUri(uri);
            var parentUri = etpUri.Parent;

            // Append query string, if any
            if (!string.IsNullOrWhiteSpace(etpUri.Query))
                parentUri = new EtpUri(parentUri + etpUri.Query);

            if (!etpUri.IsRelatedTo(EtpUris.Prodml200))
            {
                return;
            }
            if (etpUri.IsBaseUri)
            {
                CreateFoldersByObjectType(etpAdapter, etpUri)
                    .ForEach(resources.Add);
            }
            else
            {
                var propertyName = etpUri.ObjectType.ToPascalCase();

                CreateFoldersByObjectType(etpAdapter, etpUri, propertyName)
                    .ForEach(resources.Add);
            }
        }
        private IList<IResource> CreateFoldersByObjectType(IEtpAdapter etpAdapter, EtpUri uri, string propertyName = null, string additionalObjectType = null, int childCount = 0)
        {
            if (!_contentTypes.Any())
            {
                var contentTypes = new List<EtpContentType>();
                Providers.ForEach(x => x.GetSupportedObjects(contentTypes));

                contentTypes
                    .Where(x => x.IsRelatedTo(EtpContentTypes.Prodml200) )
                    .OrderBy(x => x.ObjectType)
                    .ForEach(_contentTypes.Add);
            }

            return _contentTypes
                .Select(x => new
                {
                    ContentType = x,
                    DataType = ObjectTypes_Extentions.GetObjectType(x.Family, x.ObjectType, DataSchemaVersion)
                })
                .Select(x => new
                {
                    x.ContentType,
                    x.DataType,
                    PropertyInfo = string.IsNullOrWhiteSpace(propertyName) ? null : x.DataType.GetProperty(propertyName),
                    ReferenceInfo = x.DataType.GetProperties().FirstOrDefault(p => p.PropertyType == typeof(DataObjectReference))
                })
                .Where(x =>
                {
                    // Top level folders
                    if (string.IsNullOrWhiteSpace(uri.ObjectId) || string.IsNullOrWhiteSpace(propertyName))
                        return x.ContentType.IsRelatedTo(EtpContentTypes.Prodml200); // || x.ReferenceInfo == null;

                    // Data object sub folders, e.g. Well and Wellbore
                    return (x.ContentType.IsRelatedTo(EtpContentTypes.Prodml200) && x.ReferenceInfo != null) ||
                           x.PropertyInfo?.PropertyType == typeof(DataObjectReference) ||
                           x.ContentType.ObjectType.EqualsIgnoreCase(additionalObjectType) ||
                           ObjectTypes.IsDecoratorObject(x.ContentType.ObjectType);
                })
                .Select(x =>
                {
                    var folderName = ObjectTypes.SingleToPlural(x.ContentType.ObjectType, false).ToPascalCase();
                    var dataProvider = GetDataProvider(x.DataType);
                    var hasChildren = childCount;

                    // Query for child object count if this is not the specified "additionalObjectType"
                    if (!x.ContentType.ObjectType.EqualsIgnoreCase(additionalObjectType))
                        hasChildren = dataProvider.Count(uri);

                    return etpAdapter.NewFolder(uri, x.ContentType, folderName, hasChildren);
                })
                .ToList();
        }
        private IEtpDataProvider GetDataProvider(Type objectType)
        {
            var providerType = typeof(IEtpDataProvider<>).MakeGenericType(new Type[] { objectType });
            return (IEtpDataProvider)_container.Resolve(providerType);
        }
    }
}
