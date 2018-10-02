using System.Collections.ObjectModel;

namespace YARUS.API.Models
{
    //public interface IPagedResultDto<Tdto>
    //{
    //    int? TotalCount { get; set; }
    //    ObservableCollection<Tdto> Items { get; set; }
    //}

    public interface IPagedResult<T>
    {
        int? TotalCount { get; set; }
        ObservableCollection<T> Items { get; set; }
    }
    public partial class PagedResultDtoOfFieldShortDto : IPagedResult<FieldShortDto> { }
    public partial class PagedResultDtoOfFieldDto : IPagedResult<FieldDto> { }
    public partial class PagedResultDtoOfFacilityDto : IPagedResult<FacilityDto> { }

    public partial class PagedResultDtoOfWellDto : IPagedResult<WellDto> { }

    // public partial class PagedResultDtoOfWellDirSrvyStation : IPagedResult<WellDirSrvyStationDto> { }

    // public partial class PagedResultDtoOfWellDirectionalSurveyDto : IPagedResult<WellDirectionalSurveyDto> { }
}

