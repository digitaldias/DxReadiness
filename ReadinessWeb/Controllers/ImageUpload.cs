using DxReadinessSolution.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ReadinessWeb.Controllers
{
    class ImageUpload : ApiController
    {
        private readonly IByteStreamVerifier _byteStreamVerifier;

        public ImageUpload(IByteStreamVerifier byteStreamVerifier)
        {
            _byteStreamVerifier = byteStreamVerifier;
        }


        [HttpPut]
        public void Put(byte[] imageStream)
        {
            if (!_byteStreamVerifier.IsValid(imageStream))
                return;

        }
    }
}
