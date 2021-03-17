using InstagramConsume.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramConsume.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstagramController : ControllerBase
    {
        private IInstagramManager _instagramManager;

        public InstagramController(IInstagramManager instagramManager)
        {
            _instagramManager = instagramManager;
        }

        [HttpGet("getcode")]
        public IActionResult GetCode()
        {
            try
            {
                _instagramManager.GetCodeAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getaccesstoken")]
        public async Task<IActionResult> GetAccessToken(string code)
        {
            try
            {
                return Ok(await _instagramManager.GetAccessTokenAsync(code));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getdata")]
        public async Task<IActionResult> GetData(string accessToken)
        {
            try
            {
                return Ok(await _instagramManager.GetData(accessToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
