using Models;
using Models.Enum;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
    public interface IGameDal : IBaseDal<Game>
    {
        IList<SqlGameModel> GetAllGamesInfo(GameParam param); 
    }
}
