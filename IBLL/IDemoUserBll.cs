using Models;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IDemoUserBll : IBaseBll<DemoUser>
    {
        IQueryable<DemoCutImgCheckViewModel> LoadDemoCutImgCheckData(DemoCutImgParam param);
        IQueryable<DemoCutImgCheckViewModel> LoadDemoCutImgDetails(int demoid, int accountid, int userid);
        IQueryable<DemoCutImgCheckViewModel> LoadDemoRechargeCheckData(DemoCutImgParam param);
        Sql_DemoCheckModel DemoCheckEvent(int accountId, int demoId, int userId, int requireId, string state, string type, string reason);
        bool DemoGameCutImgUpload(int gamedemoId, int accountId, int requiredId, int userId,string img);

    }
}
