using Energistics.Etp.Common.Datatypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YARUS.API;
using YARUS.WITSML.Application;
using static PDS.WITSMLstudio.OptionsIn;

namespace PDS.WITSMLstudio.Store.Data
{
    public class YARUSapiAdapter<TParent, TEntity> : WitsmlDataAdapter<TEntity>
    {
        //static readonly string ApiUri = "http://srvugeo07:53537";
#if (DEBUG)
        protected static readonly string ApiUri = "http://localhost:53538";
#else
        protected     static readonly string ApiUri = "http://srvugeo07:53537";
#endif
        public YARUSapiAdapter(IContainer container, ObjectName objectName) : base(container)
        {

            DbCollectionName = objectName;
        }

        protected ObjectName DbCollectionName { get; set; }

        public override List<TEntity> Query(WitsmlQueryParser parser, ResponseContext context)
        {
            var response = RequestApi<List<TEntity>>(MethodNames.GetObject, parser.Element().ToString(), parser.Options).Result;
            return response;
        }

        public override List<TEntity> GetAll(EtpUri? parentUri = null)
        {
            TEntity dataObject = FromParentUri(parentUri.Value);
            Dictionary<string, string> options = new Dictionary<string, string>();
            var n = ReturnElements.IdOnly;
            options.Add(n.Key, n.Value);
            var response = RequestApi<List<TEntity>>(MethodNames.GetObject, dataObject, options).Result;
            return response;
        }

        public override void Update(WitsmlQueryParser parser, TEntity dataObject)
        {
            using (var transaction = GetTransaction())
            {
                var response = RequestApi<bool>(MethodNames.UpdateObject, parser.Element().ToString(), parser.Options).Result;
                transaction.Commit();
            }
        }

        public override void Add(WitsmlQueryParser parser, TEntity dataObject)
        {

            using (var transaction = GetTransaction())
            {
                var response = RequestApi<bool>(MethodNames.AddObject, dataObject, parser.Options).Result;

                transaction.Commit();
            }
        }


        public override bool Exists(EtpUri uri)
        {
            var dataObject = FromUri(uri);
            var content = Energistics.DataAccess.EnergisticsConverter.ObjectToXml(dataObject);
            var response = RequestApi<bool>(MethodNames.ExistsObject, content).Result;
            return response;
        }

        public override bool Any(EtpUri? parentUri = null)

        {
            TEntity dataObject = FromParentUri(parentUri);

            var response = RequestApi<bool>(MethodNames.AnyObject, dataObject).Result;
            return response;
        }

        public override int Count(EtpUri? parentUri = null)
        {
            TEntity dataObject = FromParentUri(parentUri.Value);

            var response = RequestApi<int>(MethodNames.CountObject, dataObject).Result;
            return response;
        }

        public override void Delete(WitsmlQueryParser parser)
        {
            var uri = parser.GetUri<TEntity>();
            Delete(uri);
        }

        public override void Delete(EtpUri uri)
        {
            using (var transaction = GetTransaction())
            {
                if (WitsmlOperationContext.Current.IsCascadeDelete)
                {
                    DeleteAll(uri);
                }

                DeleteEntity(uri);
                transaction.Commit();
            }
        }

        protected void DeleteEntity(EtpUri uri)
        {
            TEntity dataObject = FromUri(uri);
            var response = RequestApi<bool>(MethodNames.DeleteObject, dataObject).Result;
        }

        protected TEntity FromUri(EtpUri? uri)
        {

            TEntity dataObject = Activator.CreateInstance<TEntity>();
            if (!uri.HasValue)
            {
                return dataObject;
            }
            Type type = typeof(TEntity);
            string objectType = DbCollectionName.Name;
            var objectIds = uri.Value.GetObjectIds()
              .ToDictionary(x => x.ObjectType, x => x.ObjectId, StringComparer.InvariantCultureIgnoreCase);
            if (!string.IsNullOrWhiteSpace(uri.Value.ObjectId))
            {
                type.GetProperty("Uid").SetValue(dataObject, uri.Value.ObjectId);
            }
            if (objectIds.ContainsKey(ObjectTypes.Well) && !ObjectTypes.Well.EqualsIgnoreCase(objectType))
            {
                type.GetProperty("UidWell").SetValue(dataObject, objectIds[ObjectTypes.Well]);
            }
            if (objectIds.ContainsKey(ObjectTypes.Wellbore) && !ObjectTypes.Wellbore.EqualsIgnoreCase(objectType))
            {
                type.GetProperty("UidWellbore").SetValue(dataObject, objectIds[ObjectTypes.Wellbore]);
            }

            return dataObject;

        }
        protected TEntity FromParentUri(EtpUri? parentUri = null)
        {
            TEntity dataObject = Activator.CreateInstance<TEntity>();
            if (!parentUri.HasValue)
            {
                return dataObject;
            }
            throw new NotImplementedException();
        }



        protected async Task<T> RequestApi<T>(string action, TEntity dataObject, IDictionary<string, string> options = null)
        {
            var content = Energistics.DataAccess.EnergisticsConverter.ObjectToXml(dataObject);
            return await RequestApi<T>(action, content, options);
        }

        protected async Task<T> RequestApi<T>(string action, string content, IDictionary<string, string> options = null)
        {
            var request = new YARUS.API.Models.SendMeassegeRequest();
            request.Action = action;
            request.ObjectTypeName = DbCollectionName.Name;
            request.Version = DbCollectionName.Version;
            request.Content = content;
            request.OptionsIn = options == null ? null : string.Join(";", options.Select(t => t.Key + "=" + t.Value));

            var client = new StoreServiceClient(ApiUri);
            var response = await client.Send_MeassageAsync(request);

            if (response.Code != 0)
            {
                throw new WitsmlException(ErrorCodes.YarusStoreApiError, $"Action: {action} Meassage:{ response.ErrorMessege }");
            }

            return StringParser.ParseString<T>(response.Content);
        }

    }
}
