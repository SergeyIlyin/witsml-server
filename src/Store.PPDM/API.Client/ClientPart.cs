using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using YARUS.API.Models;

namespace YARUS.API
{
    public class RequestResultAJAX<T>
    {
        public bool success { get; set; }
        public T result { get; set; }
        public string error { get; set; }
        public string targetUrl { get; set; }
        public string unAuthorizedRequest { get; set; }
        public string __abp { get; set; }
    }

    public partial class YarusApiException : System.Exception
    {
        public string targetUrl { get; set; }
        public string unAuthorizedRequest { get; private set; }
        public string __abp { get; set; }

        public YarusApiException(string message, string targetUrl, string unAuthorizedRequest, string __abp)
            : base(message)
        {
            this.targetUrl = targetUrl;
            this.unAuthorizedRequest = unAuthorizedRequest;
            this.__abp = __abp;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", targetUrl, base.ToString());
        }
    }


    public interface IObjectsSource<T>
    {
        string ObjectsType { get; }
        Task<T> GetAsync(string id);
    }

    public partial class WellsClient : IObjectsSource<WellDto>
    {
        public string ObjectsType => "WELL";

        public Task<PagedResultDtoOfWellDto> GetAllAsync(string facilityId, string fieldId, string wELL_NAME, string sortOrder)
        {
            return GetAllAsync(facilityId, fieldId, wELL_NAME, sortOrder, System.Threading.CancellationToken.None);
        }

        public Task<PagedResultDtoOfWellDto> GetAllAsync(string facilityId, string fieldId, string wELL_NAME, string sortOrder, System.Threading.CancellationToken cancellationToken)
        {
            return GetAllAsync(facilityId, fieldId, wELL_NAME, sortOrder, 0, int.MaxValue, cancellationToken);
        }
    }

    public partial class AreasClient : IObjectsSource<AreaDto>
    {
        public string ObjectsType => "AREA";

        public Task<PagedResultDtoOfAreaDto> GetAllAsync()
        {
            return GetAllAsync(System.Threading.CancellationToken.None);
        }

        public Task<PagedResultDtoOfAreaDto> GetAllAsync(System.Threading.CancellationToken cancellationToken)
        {
            return GetAllAsync(0, int.MaxValue, cancellationToken);
        }
    }
}


