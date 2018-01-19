using Models;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
  public  interface IKeyWordsBll : IBaseBll<KeyWords>
    {
      IQueryable<KeyWords> LoadKeyWords(GameKeyWords param);
    }
}
