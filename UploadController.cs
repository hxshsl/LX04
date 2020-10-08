using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public ActionResult Post(List<IFormFile> files)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "img", "Editor");
            string fname = Guid.NewGuid().ToString().Replace("-", "");
            var fileName = $"{path}/{fname}";
            try
            {
                var ext = DAL.Upload.Instance.UpImg(files[0], fileName);
                if (ext == null)
                    return Ok(Result.Err("请上传图片文件"));
                else
                {
                    var file = $"https://{HttpContext.Request.Host.Value}/img/Editor/{fname}{ext}";
                    return Ok(Result.Ok("上传成功", file));
                }
            }
            catch(Exception ex)
            {
                return Ok(Result.Err(ex.Message));
            }
        }
    }
}
