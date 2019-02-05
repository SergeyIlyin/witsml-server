using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Prodml200 = Energistics.DataAccess.PRODML200;
using System.Collections.Generic;
using System.Reflection;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Store.Data
{
    [Export(typeof(IMongoDbClassMapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MongoDbClassMapper : IMongoDbClassMapper
    {
        public void Register()
        {
            RegisterDataTypes();
            RegisterDerivedTypes();



        }
        private void RegisterDataTypes()
        {
            Register2<Prodml200.WellTest>();
        }
        private void RegisterDerivedTypes()
        {
            var type = typeof(Prodml200.ComponentSchemas.Production);

            type.Assembly
                .GetTypes()
                .Where(t => t.Namespace == type.Namespace && !t.IsAbstract && t.GetCustomAttributes<XmlIncludeAttribute>().Any())
                .SelectMany(t => t.GetCustomAttributes<XmlIncludeAttribute>())
                .ForEach(a => Register2(a.Type, true));
        }
        private void Register2<T>() where T : Prodml200.AbstractObject
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T).BaseType))
            {
                BsonClassMap.RegisterClassMap<Prodml200.AbstractObject>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }

            Register3<T>();
        }
        private void Register2(Type type, bool autoMap = false)
        {
            Register3(type.BaseType, true);
            Register3(type, autoMap);
        }

        private void Register3<T>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                var cm = BsonClassMap.RegisterClassMap<T>();
                cm.SetIgnoreExtraElements(true);
            }
        }

        private void Register3(Type type, bool autoMap = false)
        {
            if (!BsonClassMap.IsClassMapRegistered(type))
            {
                var cm = new BsonClassMap(type);

                if (autoMap)
                    cm.AutoMap();

                cm.SetIgnoreExtraElements(true);
                cm.SetIgnoreExtraElementsIsInherited(true);

                BsonClassMap.RegisterClassMap(cm);
            }
        }
    }
}
