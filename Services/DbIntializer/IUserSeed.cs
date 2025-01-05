using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DbIntializer
{
    public interface IUserSeed
    {
        public Task IntializeUser();
    }
}
