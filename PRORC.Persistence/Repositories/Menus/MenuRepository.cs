using PRORC.Persistence.Base;
using PRORC.Domain.Entities.Menus;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Menus;

public class MenuRepository : BaseRepository<Menu, int>, IMenuRepository
{
    public MenuRepository(PRORCContext context) : base(context)
    {

    }
}
