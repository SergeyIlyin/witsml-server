using Energistics.DataAccess;
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
        public YARUSapiAdapter(IContainer container, string collectionName, string version) : base(container)
        {

            CollectionName = collectionName;
            Version = version;
        }
        public YARUSapiAdapter(IContainer container, ObjectName objectNames) : this(container, objectNames.Name, objectNames.Version)
        { }

        protected string CollectionName { get; }
        protected string Version { get; }

        public override List<TEntity> Query(WitsmlQueryParser parser, ResponseContext context)
        {
            var response = RequestApi<List<TEntity>>(MethodNames.GetObject, parser.Element().ToString(), parser.Options).Result;
            return response;
        }

        public override List<TEntity> GetAll(EtpUri? parentUri = null)
        {
            TEntity dataObject = FromParentUri(parentUri.Value);
            Dictionary<string, string> options = new Dictionary<string, string>();
            var n = OptionsIn.ReturnElements.ReturnElements.IdOnly;
            options.Add(n.Key, n.Value);
            var response = RequestApi<List<TEntity>>(MethodNames.GetObject, dataObject, options).Result;

            var max = WitsmlSettings.MaxGetResourcesResponse;
            while (response.Count > max)
                response.RemoveAt(response.Count - 1);
            return response;
        }

        private Dictionary<string, string> ReturnElements(string option)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            options.Add(OptionsIn.ReturnElements.Keyword, option);
            return options;
        }

        public override TEntity Get(EtpUri uri, params string[] fields)
        {
            var entity = FromUri(uri);
            var response = RequestApi<List<TEntity>>(MethodNames.GetObject, entity, ReturnElements(OptionsIn.ReturnElements.HeaderOnly)).Result;
            return response.FirstOrDefault();
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
            var max = WitsmlSettings.MaxGetResourcesResponse;
            return Math.Min(max, response);
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
            string objectType = CollectionName;
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
        protected virtual TEntity FromParentUri(EtpUri? parentUri = null)
        {
            throw new NotImplementedException();
        }




        protected Task<T> RequestApi<T>(string action, TEntity dataObject, IDictionary<string, string> options = null)
        {


            var content = WitsmlParser.ToXml(dataObject, false, true);

            return RequestApi<T>(action, content, options);
        }

        protected async Task<T> RequestApi<T>(string action, string content, IDictionary<string, string> options = null)
        {
            var request = new YARUS.API.Models.SendMeassegeRequest();
            request.Action = action;
            request.ObjectTypeName = CollectionName;
            request.Version = Version;
            request.Content = content;
            request.OptionsIn = options == null ? null : string.Join(";", options.Select(t => t.Key + "=" + t.Value));

            var client = new StoreServiceClient(ApiUri);
            try
            {
                var response = await client.Send_MeassageAsync(request).ConfigureAwait(false);

                if (response.Code != 0)
                {
                    throw new WitsmlException(ErrorCodes.YarusStoreApiError, $"Action: {action} Code:{response.Code} Meassage:{ response.ErrorMessege }");
                }

                return StringParser.ParseString<T>(response.Content);
            }
            catch (Exception ex)
            {
                throw new WitsmlException(ErrorCodes.YarusStoreApiError, $"Action: {action} Exception:{ ex.Message  }");
            }
        }

    }
}
