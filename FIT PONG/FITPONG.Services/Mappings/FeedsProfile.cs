using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Mappings
{
    public class FeedsProfile:Profile
    {
        public FeedsProfile()
        {
            CreateMap<Database.DTOs.Feed, SharedModels.Feeds>();
        }
    }
}
