using FIT_PONG.Database.DTOs;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.ML
{
    public class CoPostovanje_prediction
    {
        public float Score { get; set; }
    }

    public class UserEntry
    {
        [KeyType(count: 2000)]
        public uint UserID { get; set; }

        [KeyType(count: 2000)]
        public uint CoPostovanjeUserID { get; set; }

        public float Label { get; set; }
    }

    public class PomocniModelUserPostovanja
    {
        public Igrac Igrac { get; set; }
        public List<Postovanje> Postovanja { get; set; }
    }
}
