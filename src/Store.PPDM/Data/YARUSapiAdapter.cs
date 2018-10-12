using Energistics.Etp.Common.Datatypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using YARUS.API;

namespace PDS.WITSMLstudio.Store.Data
{
    public class YARUSapiAdapter<T> : WitsmlDataAdapter<T>
    {
        //static readonly string ApiUri = "http://srvugeo07:53537";
#if (DEBUG)
        static readonly string ApiUri = "http://localhost:53538";
#else
         static readonly string ApiUri = "http://srvugeo07:53537";
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
                var objType = DbCollectionName.Name;
                var version = DbCollectionName.Version;
                var optionsIn = JsonConvert.SerializeObject(parser.Options, new KeyValuePairConverter());

                string ContentXML=parser.Root.ToString();
               

                var client = new StoreServiceClient(ApiUri);
                var response = client.Get_ObjectAsync(objType, version, optionsIn, ContentXML).Result;
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
            UpdateEntity<T>(DbCollectionName.Name, DbCollectionName.Version, parser);
        }


        protected void UpdateEntity<TObject>(string dbCollectionName, string version, WitsmlQueryParser parser)
        {
            try
            {
                using (var transaction = GetTransaction())
                {
                    Logger.DebugFormat($"Updating {dbCollectionName} YARUS collection");


                    string ContentXML = parser.Root.ToString();

                    StoreServiceClient client = new StoreServiceClient(ApiUri);
                    var response = client.Update_ObjectAsync(new YARUS.API.Models.Update_Request()
                    {
                        ContentType = dbCollectionName,
                        Protocol = version,

                        Content = ContentXML
                    }).Result;

                    if (response.Code != 0)
                    {
                        throw new WitsmlException(ErrorCodes.ErrorUpdatingInDataStore, response.ErrorMessege);
                    }
                    transaction.Commit();
                }
            }

            catch (Exception ex)
            {
                Logger.ErrorFormat("Error updating {0} YARUS collection: {1}", dbCollectionName, ex);
                throw new WitsmlException(ErrorCodes.ErrorUpdatingInDataStore, ex);
            }
        }

        public override bool Exists(EtpUri uri)
        {
            StoreServiceClient client = new StoreServiceClient(ApiUri);

            var response = client.ExistAsync(new YARUS.API.Models.Exist_Request
            {
                ObjectType = DbCollectionName.Name,
                Version = DbCollectionName.Version,
                ObjectId = uri.ObjectId
            }).Result;

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
    }
}
