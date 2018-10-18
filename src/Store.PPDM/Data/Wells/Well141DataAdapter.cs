using Energistics.DataAccess.WITSML141;
using Energistics.Etp.Common.Datatypes;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Configuration;
using System;
using System.ComponentModel.Composition;
using YARUS.API;

namespace PDS.WITSMLstudio.Store.Data.Wells
{
    [Export(typeof(IWitsml141Configuration))]
    [Export(typeof(IWitsmlDataAdapter<Well>))]
    [Export141(ObjectTypes.Well, typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Well141DataAdapter : YARUSapiAdapter<Well>, PDS.WITSMLstudio.Store.Configuration.IWitsml141Configuration
    {
        //  IModulesCollection modulesCollection;
        [ImportingConstructor]
        public Well141DataAdapter(IContainer container) : base(container, ObjectNames.Well141)
        {
            Logger.Debug("Instance created.");
            IdPropertyName = ObjectTypes.Uid;
            NamePropertyName = ObjectTypes.NameProperty;
        }
        protected string IdPropertyName { get; set; }
        protected string NamePropertyName { get; set; }

        public void GetCapabilities(CapServer capServer)
        {
            Logger.DebugFormat("Getting the supported capabilities for Well data version {0}.", capServer.Version);

            capServer.Add(Functions.GetFromStore, ObjectTypes.Well);
            capServer.Add(Functions.AddToStore, ObjectTypes.Well);
            capServer.Add(Functions.UpdateInStore, ObjectTypes.Well);
            capServer.Add(Functions.DeleteFromStore, ObjectTypes.Well);
        }


        public override int Count(EtpUri? parentUri = null)
        {
            Well dataObject = Activator.CreateInstance<Well>();
            if (parentUri == null)
            {
                dataObject.Uid = parentUri.Value.ObjectId;
            }

            var client = new StoreServiceClient(ApiUri);

            var request = new YARUS.API.Models.SendMeassegeRequest();
            request.Action = MethodNames.AnyObject;
            request.ObjectTypeName = DbCollectionName.Name;
            request.Version = DbCollectionName.Version;
            request.Content = Energistics.DataAccess.EnergisticsConverter.ObjectToXml(dataObject);

            var response = client.Send_MeassageAsync(request).Result;

            if (response.Code != 0)
            {
                throw new WitsmlException(ErrorCodes.ErrorUpdatingInDataStore, response.ErrorMessege);
            }

            int result;

            if (int.TryParse(response.Content, out result))
            {
                return result;
            }
            else
            {
                throw new WitsmlException(ErrorCodes.ErrorReadingFromDataStore);
            }
        }


        public override bool Any(EtpUri? parentUri = null)

        {
            Well dataObject = Activator.CreateInstance<Well>();
            if (parentUri == null)
            {
                dataObject.Uid = parentUri.Value.ObjectId;
            }

            var client = new StoreServiceClient(ApiUri);

            var request = new YARUS.API.Models.SendMeassegeRequest();
            request.Action = MethodNames.AnyObject;
            request.ObjectTypeName = DbCollectionName.Name;
            request.Version = DbCollectionName.Version;
            request.Content = Energistics.DataAccess.EnergisticsConverter.ObjectToXml(dataObject);

            var response = client.Send_MeassageAsync(request).Result;

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
                throw new WitsmlException(ErrorCodes.ErrorReadingFromDataStore);
            }
        }

    }
}
