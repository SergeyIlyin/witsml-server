using Energistics.DataAccess.Validation;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Energistics.DataAccess.WITSML141
{
    [XmlRoot("fields", IsNullable = false, Namespace = "http://www.witsml.org/schemas/131")]
    [XmlType(Namespace = "http://www.witsml.org/schemas/131")]
    [Serializable]
    public class FieldList : IEnergisticsCollection
    {
        private string versionField = "1.4.1.1";



        /// <summary>Information about the XML message instance.</summary>
        [Description("Information about the XML message instance.")]
        [ComponentElement]
        [XmlElement("documentInfo")]
        public DocumentInfo DocumentInfo { get; set; }

        /// <summary>
        /// Information about a single well. A well is a unique surface location from which wellbores are drilled into the Earth for the purpose of either (1) finding or producing underground resources; or (2) providing services related to the production of underground resources.
        /// </summary>
        [Required]
        [RecurringElement]
        [XmlElement("field")]
        public List<Energistics.DataAccess.WITSML141.Field > Field { get; set; }

        /// <summary>
        /// bool to indicate if Well has been set. Used for serialization.
        /// </summary>
        [XmlIgnore]
        public bool FieldSpecified
        {
            get
            {
                if (Field != null)
                    return this.Field.Count > 0;
                return false;
            }
        }

 
        [XmlIgnore]
        public IList Items
        {
            get
            {
                return (IList)this.Field;
            }
        }

   
        [Required]
        [RegularExpression("1\\.[4-9]\\.[0-9]\\.([0-9]|([1-9][0-9]))")]
        [StringLength(16)]
        [Description("Data object schema version.  The fourth level must match the  version of the schema constraints (enumerations and XML loader files) that are assumed by the document instance.")]
        [XmlAttribute("version")]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }
}
