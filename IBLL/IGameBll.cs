using Models;
using Models.Enum;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IBLL
{
    public interface IGameBll : IBaseBll<Game>
    {
        IQueryable<Game> LoadGamePutLibrary(Models.Params.GameParam param);
        IQueryable<Game> Rec_GameList(Models.Params.Rec_GameParam param);
        List<Game> GetAllTypeGame(string type, GameParam param);
        #region 游戏详情
        List<GameViewModel> GetGameRelatedNews(int id);
        /// <summary>
        /// 根据游戏名得到相关的攻略
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<UserRaidersViewModel> GetGameRelatedRaiders(int id);
        /// <summary>
        /// 游戏相关的2条开服
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<ServiceViewModel> GetGameRelatedService(int id);
        /// <summary>
        /// 相关游戏的礼包
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Package_Card> GetGameRelatedPackage(int id);
        /// <summary>
        /// 该游戏下最新发布的试玩数据(福利)     
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        List<Sql_GameModel> GetGameRelatedDemo(int id);
        //联运
        List<Sql_CpyModel> GetGameCpy(int id);
        /// <summary>
        /// 游戏截图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Game> GetGameCutImgInfo(int id);
        /// <summary>
        /// 游戏同类型数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Game> GetGameSameTypeData(int id); 
        #endregion
     
        /// <summary>
        /// 根据参数查出所有的游戏
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<Game> GetAllGame(GameParam param);
        IList<SqlGameModel> GetAllGamesInfo(Models.Params.GameParam param);
        /// <summary>
        /// 是否包含该游戏名称
        /// </summary>
        /// <param name="games"></param>
        /// <returns></returns>
        bool  IsContainsNewestData(string games);
        /// <summary>
        /// 根据点赞数查出热游排行的游戏
        /// </summary>
        /// <returns></returns>
        List<FrontGame> InLikeNumHotGame();
        IList<Sql_CityModel> GetAllCity();


    }
}
