using Energistics.DataAccess;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Store
{
    public static class ObjectTypes_Extentions
    {
        public static Type GetObjectType(string family, string objectType, string version)
        {
            string _ns = "Energistics.DataAccess";
            _ns = $"{_ns}.{FamelyNameSpace(family)}{VesrsionNameSpace(version)}.";


            if (ObjectTypes.WbGeometry.EqualsIgnoreCase(objectType) && !OptionsIn.DataVersion.Version200.Equals(version))
                objectType = $"StandAlone{ObjectTypes.WellboreGeometry.ToPascalCase()}";

            return typeof(IDataObject).Assembly.GetType(_ns + objectType.ToPascalCase());
        }
        private static string FamelyNameSpace(string family)
        {
            if ("witsml".Equals(family)) return "WITSML";
            if ("prodml".Equals(family)) return "PRODML";
            if ("resqml".Equals(family)) return "RESQML";
            return "";

        }
        private static string VesrsionNameSpace(string version)
        {
            var ns = OptionsIn.DataVersion.Version131.Equals(version)
            ? "131"
            : OptionsIn.DataVersion.Version200.Equals(version)
            ? "200"
            : OptionsIn.DataVersion.Version210.Equals(version)
            ? "210"
            : "141";
            return ns;
        }
    }
}
