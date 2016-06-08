using DxReadinessSolution.Domain.Contracts;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace ReadinessWeb.Controllers
{
    public class ImagesController : ApiController
    {
        private readonly IImageManager _imageManager;


        public ImagesController()
        {           
            _imageManager = Startup.DiContainer.GetInstance<IImageManager>();
        }


        [HttpPut]
        public async Task Put()
        {
            var imageStream  = await Request.Content.ReadAsStreamAsync();
            
            await _imageManager.Process(imageStream);
        }


        [HttpGet]
        public async Task<string> Get()
        {
            return await Task.FromResult("Go Away!");
        }
    }
}
