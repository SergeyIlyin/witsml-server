using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.Common.Datatypes.Object;
using PDS.WITSMLstudio.Store.Data;

namespace PDS.WITSMLstudio.Store
{
    public class YarusApiDataProvider<TObject> : IEtpDataProvider<TObject>, IEtpDataProvider
    {
        public bool Any(EtpUri? parentUri)
        {
            throw new NotImplementedException();
        }

        public virtual  int Count(EtpUri? parentUri)
        {
            throw new NotImplementedException();
        }

        public void Delete(EtpUri uri)
        {
            throw new NotImplementedException();
        }

        public void Ensure(EtpUri uri)
        {
            throw new NotImplementedException();
        }

        public bool Exists(EtpUri uri)
        {
            throw new NotImplementedException();
        }

        public TObject Get(EtpUri uri)
        {
            throw new NotImplementedException();
        }

        public List<TObject> GetAll(EtpUri? parentUri = null)
        {
            throw new NotImplementedException();
        }

        public void GetSupportedObjects(IList<EtpContentType> contentTypes)
        {
            var type = typeof(TObject);

            var contentType = EtpUris.GetUriFamily(type)
                .Append(ObjectTypes.GetObjectType(type))
                .ContentType;

            contentTypes.Add(contentType);
        }

        public void Put(IDataObject dataObject)
        {
            throw new NotImplementedException();
        }

        object IEtpDataProvider.Get(EtpUri uri) => Get(uri);

        IList IEtpDataProvider.GetAll(EtpUri? parentUri) => GetAll(parentUri);
    }
}
