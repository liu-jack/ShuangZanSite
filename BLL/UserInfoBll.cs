using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BLL
{
    public class UserInfoBll : BaseBll<UserInfo>, IUserInfoBll
    {
        /// <summary>
        /// 子类拿到当前的dal去给基类赋值。
        /// </summary>
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.UserInfoDal;
        }
        #region 批量删除管理人员          
        public int DeleteUsers(IList<int> ids)
        {
            foreach (var id in ids)
            {
                var user = dbSession.UserInfoDal.LoadEntities(u => u.Id == id).FirstOrDefault();
                if (user != null)
                {
                    user.DelFlag = (short)Models.Enum.DelFlagEnum.Deleted;
                    dbSession.UserInfoDal.Update(user);
                }
            }
            return dbSession.SaveChanges();
        }
        #endregion
        #region 分页加载数据
        public IQueryable<UserInfo> LoadPageUserInfos(Models.Params.QueryParam param)
        {
            short delNormal = (short)Models.Enum.DelFlagEnum.Normal;
            //先查出所有未删除的数据
            var temp = dbSession.UserInfoDal.LoadEntities(u => u.DelFlag == delNormal);           
            if (!string.IsNullOrEmpty(param.SearchName))
            {
                temp = temp.Where(u => u.UName.Contains(param.SearchName));
            }
            
            param.Total = temp.Count();
            return temp.OrderByDescending(u=>u.InTime)
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize).AsQueryable();
        }
        #endregion 
        #region 设置用户角色            
        public bool SetUserRole(int userId, List<int> selectRoleIds)
        {
            //查询出当前设置角色的用户
            var user = dbSession.UserInfoDal.LoadEntities(u => u.Id == userId).FirstOrDefault();
          
            //但是：如果用户之前已经有了，角色。先 把之前的角色都清空掉。
            user.RoleInfo.Clear();

            //把所有的角色查询出来。---选中的角色selectRoleIds里面如果包含数据表里的id就查询出来
            var allRoles = dbSession.RoleInfoDal.LoadEntities(r => selectRoleIds.Contains(r.Id)).ToList();

            foreach (var roleInfo in allRoles)
            {
                user.RoleInfo.Add(roleInfo);
            }
            return dbSession.SaveChanges() > 0;
        }
        #endregion
    }
}
