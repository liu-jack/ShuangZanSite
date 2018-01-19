using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{

    public class VerificationCodeBll : BaseBll<VerificationCode>, IVerificationCodeBll
    {
        public override void SetCurrentDal()
        {
             CurrentDal=dbSession.VerificationCodeDal;
        }
    }
}
