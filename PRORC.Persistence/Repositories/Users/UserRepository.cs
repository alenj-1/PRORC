using PRORC.Domain.Entities.Users;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Users
{
    public class UserRepository : BaseRepository<User, int>, IUserRepository
    {
        public UserRepository(PRORCContext context) : base(context)
        {
        }
    }
}
