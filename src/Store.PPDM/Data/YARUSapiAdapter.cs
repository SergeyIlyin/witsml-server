﻿using Energistics.Etp.Common.Datatypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YARUS.API;

namespace PDS.WITSMLstudio.Store.Data
{
    public class YARUSapiAdapter<T> : WitsmlDataAdapter<T>
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

        public override List<T> Query(WitsmlQueryParser parser, ResponseContext context)
        {
            try
            {
              


                var response = RequestApi(MethodNames.GetObject, parser.Element().ToString(), parser.Options).Result;

                if (response.Code != 0)
                {
                    throw new WitsmlException(ErrorCodes.ErrorReadingFromDataStore, response.ErrorMessege);
                }

                return Energistics.DataAccess.EnergisticsConverter.XmlToObject<List<T>>(response.Content);
            }

            catch (Exception ex)
            {
                Logger.Error($"Error query {DbCollectionName} YARUS collection: {ex}");
                throw new WitsmlException(ErrorCodes.ErrorReadingFromDataStore, ex);
            }
        }

        public override void Update(WitsmlQueryParser parser, T dataObject)
        {
            try
            {

                using (var transaction = GetTransaction())
                {
                    var client = new StoreServiceClient(ApiUri);

                    var request = new YARUS.API.Models.SendMeassegeRequest();
                    request.Action = MethodNames.UpdateObject;
                    request.ObjectTypeName = DbCollectionName.Name;
                    request.Version = DbCollectionName.Version;
                    request.Content = parser.Element().ToString();
                    request.OptionsIn = JsonConvert.SerializeObject(parser.Options, new KeyValuePairConverter());


                    var response = client.Send_MeassageAsync(request).Result;
                    if (response.Code != 0)
                    {
                        throw new WitsmlException(ErrorCodes.ErrorUpdatingInDataStore, response.ErrorMessege);
                    }
                    transaction.Commit();
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error updating {0} YARUS collection: {1}", DbCollectionName, ex);
                throw new WitsmlException(ErrorCodes.ErrorUpdatingInDataStore, ex);
            }
        }



        public override void Add(WitsmlQueryParser parser, T dataObject)
        {
            try
            {
                using (var transaction = GetTransaction())
                {
                    var response = RequestApi(MethodNames.AddObject, dataObject, parser.Options).Result;
                    if (response.Code != 0)
                    {
                        throw new WitsmlException(ErrorCodes.ErrorUpdatingInDataStore, response.ErrorMessege);
                    }
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error adding {0} YARUS collection: {1}", DbCollectionName, ex);
                throw new WitsmlException(ErrorCodes.ErrorUpdatingInDataStore, ex);
            }

        }


        public override bool Exists(EtpUri uri)
        {
            var dataObject = FromUri(uri);

            var response = RequestApi(MethodNames.ExistsObject, dataObject).Result;

            if (response.Code != 0)
            {
                throw new WitsmlException(ErrorCodes.ErrorUpdatingInDataStore, response.ErrorMessege);
            }

            bool result;
            if (bool.TryParse(response.Content, out result))
            {
                return result;
            }
            else
            {
                throw new WitsmlException(ErrorCodes.DataObjectNotExist);
            }
        }

        protected T FromUri(EtpUri uri)
        {
            T dataObject = Activator.CreateInstance<T>();
            Type type = typeof(T);
            string objectType = DbCollectionName.Name;
            var objectIds = uri.GetObjectIds()
              .ToDictionary(x => x.ObjectType, x => x.ObjectId, StringComparer.InvariantCultureIgnoreCase);
            if (!string.IsNullOrWhiteSpace(uri.ObjectId))
            {
                type.GetProperty("Uid").SetValue(dataObject, uri.ObjectId);
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
        public override void Delete(WitsmlQueryParser parser)
        {
            var uri = parser.GetUri<T>();
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
            try
            {
                T dataObject = FromUri(uri);

                var response = RequestApi(MethodNames.DeleteObject, dataObject).Result;

                if (response.Code != 0)
                {
                    throw new WitsmlException(ErrorCodes.ErrorUpdatingInDataStore, response.ErrorMessege);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error deleting from {0} MongoDb collection: {1}", DbCollectionName.Name, ex);
                throw new WitsmlException(ErrorCodes.ErrorDeletingFromDataStore, ex);
            }
        }



        protected Task<YARUS.API.Models.Response> RequestApi(string action, T dataObject, IDictionary<string, string> options = null)
        {
            var content = Energistics.DataAccess.EnergisticsConverter.ObjectToXml(dataObject);

            return RequestApi(action, content, options);
        }
        protected Task<YARUS.API.Models.Response> RequestApi(string action, string content, IDictionary<string, string> options = null)
        {
            var request = new YARUS.API.Models.SendMeassegeRequest();
            request.Action = action;
            request.ObjectTypeName = DbCollectionName.Name;
            request.Version = DbCollectionName.Version;
            request.Content = content;
            request.OptionsIn = options == null ? null : string.Join(";", options.Select(t => t.Key + "=" + t.Value));

            var client = new StoreServiceClient(ApiUri);

            return client.Send_MeassageAsync(request);
        }
    }
}
