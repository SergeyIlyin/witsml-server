using Energistics.DataAccess.Reflection;
using Energistics.DataAccess.Validation;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using Energistics.DataAccess.WITSML141.ReferenceData;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Energistics.DataAccess.WITSML141
{
    [XmlType(Namespace = "http://www.witsml.org/schemas/1series", TypeName = "obj_field")]
    [EnergisticsDataObject(StandardFamily.WITSML, "1.4.1.1")]
    [Description("The non-contextual content of a WITSML Well object.")]
    [Serializable]
    public class Field : IDataObject, IUniqueId
    {
        [RegularExpression("[^ ]*")]
        [StringLength(64)]
        [XmlAttribute("uid")]
        public string Uid { get; set; }

        [Required]
        [StringLength(64)]
        [XmlElement("name")]
        public string Name { get; set; }
    }
}

namespace Energistics.DataAccess.WITSML200
{
    [XmlType(Namespace = "http://www.energistics.org/energyml/data/witsmlv2")]
    [XmlRoot("", IsNullable = false, Namespace = "http://www.energistics.org/energyml/data/witsmlv2")]
    [EnergisticsDataObject(StandardFamily.WITSML, "2.0")]
    [Serializable]
    public class Field : AbstractObject, INotifyPropertyChanged
    {
    }
}