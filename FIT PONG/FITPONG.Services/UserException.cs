using System;
using System.Collections.Generic;

namespace FIT_PONG.Services
{
    public class UserException : Exception
    {
        public List<(string key, string message)> Errori = new List<(string key, string message)>();
        public UserException() { }
        public UserException(string msg) : base(msg)
        {
        }
        public void AddError(string key, string msg)
        {
            Errori.Add((key, msg));
        }
    }
}
