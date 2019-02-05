using Energistics.DataAccess.PRODML200.ComponentSchemas;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.Common.Datatypes.Object;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Prodml200 = Energistics.DataAccess.PRODML200;
using Etp11 = Energistics.Etp.v11;
using Etp12 = Energistics.Etp.v12;
using Energistics.DataAccess.PRODML200;
using PDS.WITSMLstudio;
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

        public void FindResources(IEtpAdapter etpAdapter, ProtocolEventArgs<Etp12.Protocol.DiscoveryQuery.FindResources, Etp12.Protocol.DiscoveryQuery.ResourceResponse> args)
        {
            var count = args.Context.Resources.Count;
            string serverSortOrder;

            GetResources(etpAdapter, args.Message.Uri, args.Context.Resources, out serverSortOrder);

            if (args.Context.Resources.Count > count)
                args.Context.ServerSortOrder = serverSortOrder;
        }

        public void GetResources(IEtpAdapter etpAdapter, ProtocolEventArgs<Etp11.Protocol.Discovery.GetResources, IList<Etp11.Datatypes.Object.Resource>> args)
        {
            string serverSortOrder;
            GetResources(etpAdapter, args.Message.Uri, args.Context, out serverSortOrder);
        }


        public void GetResources(Energistics.Etp.Common.IEtpAdapter etpAdapter, Energistics.Etp.Common.ProtocolEventArgs<Energistics.Etp.v12.Protocol.Discovery.GetResources, IList<Energistics.Etp.v12.Datatypes.Object.Resource>> args)
        {
            string serverSortOrder;
            GetResources(etpAdapter, args.Message.Uri, args.Context, out serverSortOrder);
        }

        private void GetResources<T>(IEtpAdapter etpAdapter, string uri, IList<T> resources, out string serverSortOrder) where T : IResource
        {
            // Default to Name in IResource
            serverSortOrder = ObjectTypes.NameProperty;

            if (EtpUris.IsRootUri(uri))
            {
                var childCount = CreateFoldersByObjectType(etpAdapter, EtpUris.Witsml200, skipChildCount: true).Count;
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
            else if (string.IsNullOrWhiteSpace(etpUri.ObjectId))
            {

                var objectType = GetObjectType(etpUri.ObjectType, etpUri.Version);
                var contentType = EtpContentTypes.GetContentType(objectType);
                var hasChildren = contentType.IsRelatedTo(EtpContentTypes.Eml210) ? 0 : -1;
                var dataProvider = GetDataProvider(etpUri.ObjectType);

                dataProvider
                    .GetAll(parentUri)
                    .Cast<Prodml200.AbstractObject>()
                    .ForEach(x => resources.Add(ToResource(etpAdapter, x, parentUri, hasChildren)));

            }
            else
            {
                //var propertyName = etpUri.ObjectType.ToPascalCase();

                //CreateFoldersByObjectType(etpAdapter, etpUri, propertyName)
                //    .ForEach(resources.Add);
            }
        }
        private IList<IResource> CreateFoldersByObjectType(IEtpAdapter etpAdapter, EtpUri uri, string propertyName = null, string additionalObjectType = null, int childCount = 0, bool skipChildCount = false)
        {
            if (!_contentTypes.Any())
            {
                var contentTypes = new List<EtpContentType>();
                Providers.ForEach(x => x.GetSupportedObjects(contentTypes));

                contentTypes
                    .Where(x => x.IsRelatedTo(EtpContentTypes.Prodml200))
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

        private IEtpDataProvider GetDataProvider(string objectType)
        {
            return _container.Resolve<IEtpDataProvider>(new ObjectName(objectType, DataSchemaVersion));
        }
        private IEtpDataProvider GetDataProvider(Type objectType)
        {
            var providerType = typeof(IEtpDataProvider<>).MakeGenericType(new Type[] { objectType });
            return (IEtpDataProvider)_container.Resolve(providerType);
        }
        private IResource ToResource(IEtpAdapter etpAdapter, AbstractObject entity, EtpUri parentUri, int hasChildren = -1)
        {

            var resourse = etpAdapter.CreateResource(
             uuid: entity.Uuid,
             uri: entity.GetUri(),
             resourceType: ResourceTypes.DataObject,
             name: entity.Citation.Title,
             count: hasChildren,
             lastChanged: GetLastChanged(entity));
            if (entity.Citation.DescriptiveKeywords != null)
            {
                resourse.CustomData.Add("DescriptiveKeywords", entity.Citation.DescriptiveKeywords);
            }
            if (entity?.Citation?.Creation != null)
            {
                resourse.CustomData.Add("Creation", entity?.Citation.Creation.Value.ToString("o"));
            }
            return resourse;
        }
        private long GetLastChanged(AbstractObject entity)
        {
            return entity?.Citation?.LastUpdate?.ToUnixTimeMicroseconds() ?? 0;
        }
        public EtpUri GetUri(Prodml200.AbstractObject entity, EtpUri parentUri)
        {
            // Remove query string parameters, if any
            var uri = parentUri.GetLeftPart();

            if (!EtpUris.IsRootUri(uri))
            {
                // Remove trailing separator
                uri = uri.TrimEnd('/');
            }

            return new EtpUri(uri)
                .Append(ObjectTypes.GetObjectType(entity), entity.Uuid);
        }

        public static Type GetObjectType(string objectType, string version)
        {
            var ns =
                 OptionsIn.DataVersion.Version200.Equals(version)
                ? "Energistics.DataAccess.PRODML200."
                : "Energistics.DataAccess.WITSML141.";

            return typeof(Energistics.DataAccess.PRODML200.WellTest).Assembly.GetType(ns + objectType.ToPascalCase());
        }
    }
}
