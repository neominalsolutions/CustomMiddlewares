using Assistt.BLL.Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistt.BLL.Layer.Services
{
  public interface ISampleService
  {
    Task ExecuteAsync(SampleEntity entity);
  }
}
