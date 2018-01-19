using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BLL
{
    public class WonderfulTxtImgBll : BaseBll<WonderfulTxtImg>,IWonderfulTxtImgBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.WonderfulTxtImgDal;
        }
    }
}
