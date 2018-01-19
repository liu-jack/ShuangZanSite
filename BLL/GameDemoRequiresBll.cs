using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class GameDemoRequiresBll : BaseBll<GameDemoRequires>, IGameDemoRequiresBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.GameDemoRequiresDal;
        }      
    }
}
