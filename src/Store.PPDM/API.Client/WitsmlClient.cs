using Energistics.DataAccess.WITSML141;
using System;
using System.Threading.Tasks;

namespace YARUS.API
{
    public partial class WitsmlClient
    {

        public async Task<WellList> Get_Wells(WellList wellList, string OptionsIn)
        {
            var content = ObjectToString(wellList);
            var responce = await Get_ObjectAsync("well", content, OptionsIn);
            if (responce.Result != 0)
            {
                throw new YarusApiException(responce.SuppMsgOut, nameof(Get_Wells), null, null);
            }

            return ObjectFromString(typeof(WellList), responce.XmLout) as WellList;
        }
        object ObjectFromString(Type type, string content)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(type);
            string contentToDeseril = $"<?xml version=\"1.0\" encoding=\"utf - 8\" ?>" + content;
            using (System.IO.StringReader reader = new System.IO.StringReader(content))
            {
                return serializer.Deserialize(reader);
            }
        }

        string ObjectToString(object obj)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            using (System.IO.StringWriter writer = new System.IO.StringWriter())
            {
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }
    }

    public static class OptionsIn
    {
        public static string all = "all";
        public static string id_only = "id_only";
    }
}
