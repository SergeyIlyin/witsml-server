using System.Threading.Tasks;
using YARUS.API.Models;

namespace YARUS.API
{
    public partial class ObjectsResolverClient
    {

        public Task<T> Get<T>(ObjectRef @ref) where T : class
        {
            return GetAsync<T>(obj_type: @ref.Obj_type, obj_id: @ref.Obj_id, obj_name: @ref.Obj_type, cancellationToken: System.Threading.CancellationToken.None);
        }
        public Task<T> Get<T>(ObjectRef @ref, System.Threading.CancellationToken cancellationToken) where T : class
        {
            return GetAsync<T>(obj_type: @ref.Obj_type, obj_id: @ref.Obj_id, obj_name: @ref.Obj_type, cancellationToken: cancellationToken);
        }
        public Task<T> Get<T>(string obj_type, string obj_id, string obj_name) where T : class
        {
            return GetAsync<T>(obj_type, obj_id, obj_name, System.Threading.CancellationToken.None);
        }
        public async Task<T> GetAsync<T>(string obj_type, string obj_id, string obj_name, System.Threading.CancellationToken cancellationToken) where T : class
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/services/app/ObjectsResolver/Get?");
            if (obj_type != null) urlBuilder_.Append("obj_type=").Append(System.Uri.EscapeDataString(ConvertToString(obj_type, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            if (obj_id != null) urlBuilder_.Append("obj_id=").Append(System.Uri.EscapeDataString(ConvertToString(obj_id, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            if (obj_name != null) urlBuilder_.Append("obj_name=").Append(System.Uri.EscapeDataString(ConvertToString(obj_name, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            urlBuilder_.Length--;

            var client_ = new System.Net.Http.HttpClient();
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var responseData_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var result_ = default(T);
                            try
                            {
                                result_ = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseData_, _settings.Value);
                                return result_;
                            }
                            catch (System.Exception exception_)
                            {
                                throw new SwaggerException("Could not deserialize the response body.", (int)response_.StatusCode, responseData_, headers_, exception_);
                            }
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new SwaggerException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }
                        return default(T);
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (client_ != null)
                    client_.Dispose();
            }
        }


    }
}
