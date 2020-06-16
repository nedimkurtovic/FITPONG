using AutoMapper;
using FIT_PONG.Models;
using FIT_PONG.Services.Bazni;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Services
{
    public class FeedsService:BaseService<SharedModels.Feeds,Database.DTOs.Feed,object>, IFeedsService
    {
        public FeedsService(MyDb db, IMapper mapko):base(db,mapko)
        {

        }
        
    }
}
