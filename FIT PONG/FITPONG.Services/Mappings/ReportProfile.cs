using AutoMapper;

namespace FIT_PONG.Services.Mappings
{
    public class ReportProfile:Profile
    {
        public ReportProfile()
        {
            CreateMap<Database.DTOs.Report, SharedModels.Reports>();
            CreateMap<SharedModels.Requests.Reports.ReportsInsert, Database.DTOs.Report>();

        }
    }
}
