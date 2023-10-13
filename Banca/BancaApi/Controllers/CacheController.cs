
using BancaService.Service.IService;
using Microsoft.AspNetCore.Mvc;
using BancaModels.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Redis_Cache.Service.IRedisService;
using BancaModels.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BancaApi.Controllers
{
    [Route("api/cachecontroller")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IRedisP _redisP;
        public CacheController(IRedisP redisP)
        {
            _redisP = redisP;
        }



        [HttpGet("[action]")]
        public async Task<IActionResult> GetUser([FromQuery] string key)
        {
            var res = await _redisP.GetData<User>(key);
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task PostUser([FromBody] User user)
        {
            await _redisP.SetData(user.UserName, user);
        }

        //// PUT api/<CacheController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<CacheController>/5
        [HttpDelete("[action]/$Key")]
        public async Task DeleteUser(string Key)
        {
            await _redisP.Remove<User>(Key);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetIdentityUser([FromQuery] string key)
        {
            var res = await _redisP.GetData<ApplicationUser>(key);
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task PostIdentityUser([FromBody] ApplicationUser user)
        {
            await _redisP.SetData(user.UserName, user);
        }

        //// PUT api/<CacheController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<CacheController>/5
        [HttpDelete("[action]/$Key")]
        public async Task DeleteIdentityUser(string Key)
        {
            await _redisP.Remove<ApplicationUser>(Key);
        }
    }
}
