//----------------------------------------------------------------------- 
// PDS WITSMLstudio Core, 2018.3
//
// Copyright 2018 PDS Americas LLC
// 
// Licensed under the PDS Open Source WITSML Product License Agreement (the
// "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.pds.group/WITSMLstudio/OpenSource/ProductLicenseAgreement
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

using Energistics.DataAccess;
using Prodml200 = Energistics.DataAccess.PRODML200;

using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Properties;

namespace PDS.WITSMLstudio
{
    /// <summary>
    /// Provides extension methods that can be used with common WITSML types and interfaces.
    /// </summary>
    public static class Extensions
    {


        /// <summary>
        /// Initializes a new UID value if one was not specified.
        /// </summary>
        /// <typeparam name="T">The type of data object.</typeparam>
        /// <param name="dataObject">The data object.</param>
        /// <returns>The supplied UID if not null; otherwise, a generated UID.</returns>
        public static string NewUid<T>(this T dataObject) where T : IUniqueId
        {
            return string.IsNullOrEmpty(dataObject.Uid)
                ? Guid.NewGuid().ToString()
                : dataObject.Uid;
        }

        /// <summary>
        /// Initializes a new UUID value if one was not specified.
        /// </summary>
        /// <typeparam name="T">The type of data object.</typeparam>
        /// <param name="dataObject">The data object.</param>
        /// <returns>The supplied UUID if not null; otherwise, a generated UID.</returns>
        public static string NewUuid2<T>(this T dataObject) where T : Prodml200.AbstractObject
        {
            return string.IsNullOrEmpty(dataObject.Uuid)
                ? Guid.NewGuid().ToString()
                : dataObject.Uuid;
        }

        /// <summary>
        /// Gets the description associated with the specified WITSML error code.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <returns>The description for the error code.</returns>
        public static string GetDescription(this ErrorCodes errorCode)
        {
            return errorCode.ToString();
        }

        /// <summary>
        /// Gets the value of the Version property for specified container object.
        /// </summary>
        /// <typeparam name="T">The data object type.</typeparam>
        /// <param name="dataObject">The data object.</param>
        /// <returns>The value of the Version property.</returns>
        public static string GetVersion<T>(this T dataObject) where T : IEnergisticsCollection
        {
            return (string)dataObject.GetType().GetProperty("Version")?.GetValue(dataObject, null);
        }

        /// <summary>
        /// Sets the value of the Version property for the specified container object.
        /// </summary>
        /// <typeparam name="T">The data object type.</typeparam>
        /// <param name="dataObject">The data object.</param>
        /// <param name="version">The version.</param>
        /// <returns>The data object instance.</returns>
        public static T SetVersion<T>(this T dataObject, string version) where T : IEnergisticsCollection
        {
            dataObject.GetType().GetProperty("Version")?.SetValue(dataObject, version);
            return dataObject;
        }




        /// <summary>
        /// Gets the last changed date time in microseconds.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The last changed date time in microseconds.</returns>
        public static long GetLastChangedMicroseconds(this Prodml200.AbstractObject entity)
        {
            return entity?.Citation?.LastUpdate?.ToUnixTimeMicroseconds() ?? 0;
        }






    }
}
