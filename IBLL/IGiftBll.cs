using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IGiftBll : IBaseBll<Gift>
    {
        /// <summary>
        /// 礼品管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IQueryable<Gift> LoadGift(Models.Params.GiftParam param);
        /// <summary>
        /// 最新发布的礼品
        /// </summary>
        /// <returns></returns>
        List<Gift> NewestPublishGift();
        /// <summary>
        /// 礼品兑换
        /// </summary>
        /// <param name="userId">礼品兑换</param>
        /// <param name="giftId">当前礼物id</param>
        /// <param name="addressId">当前地址id</param>
        /// <param name="num">礼物数量</param>
        /// <param name="color">礼物颜色</param>
        /// <param name="orderId">生成的订单id</param>
        /// <param name="error">返回错误的消息提示</param>
        /// <returns></returns>
        /// //, out int orderId, out string error
        GiftViewModel ExChangeGift(int userId, int giftId, int addressId, int num, string color);
    }
}
