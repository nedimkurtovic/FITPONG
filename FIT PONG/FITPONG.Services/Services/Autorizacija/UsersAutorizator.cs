using FIT_PONG.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public class UsersAutorizator : IUsersAutorizator
    {
        private readonly MyDb db;

        public UsersAutorizator(MyDb db)
        {
            this.db = db;
        }

        public bool AuthorizePostovanje()
        {
            throw new NotImplementedException();
        }

        public bool AuthorizePromjenaPasswordaMail()
        {
            throw new NotImplementedException();
        }

        public bool AuthorizePromjenaPasswordaPotvrda()
        {
            throw new NotImplementedException();
        }
    }
}
