using Assistt.BLL.Layer.Exceptions;
using Assistt.BLL.Layer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistt.BLL.Layer.Services
{
  public class SampleService : ISampleService
  {
    public async Task ExecuteAsync(SampleEntity entity)
    {
      if (string.IsNullOrEmpty(entity.Name))
      {
        throw new EntityNotFoundException();
        //throw new ValidationException($"Name : {entity.Name}");
      }
     

      await Task.CompletedTask;
    }
  }
}
