using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistt.BLL.Layer.Exceptions
{
  public class EntityNotFoundException : CustomException
  {
    public EntityNotFoundException(string message = "Nesne sistemde bulunamdı") : base(message)
    {
    }
  }
}
