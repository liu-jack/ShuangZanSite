using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class GameDemoRechargeBll :BaseBll<GameDemoRecharge>,IGameDemoRechargeBll
    {

        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.GameDemoRechargeDal;
        }
    }
}
