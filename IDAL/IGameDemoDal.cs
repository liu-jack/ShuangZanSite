using Models;
using Models.MapModel;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
    public interface IGameDemoDal : IBaseDal<GameDemo>
    {
        /// <summary>
        /// 前台列表试玩
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<GameDemoViewModel> GetGameDemo(DemoParam param);
        /// <summary>
        /// 前台列表的总条数
        /// </summary>
        /// <returns></returns>
        int GetGameDemoCount();
    }
}
