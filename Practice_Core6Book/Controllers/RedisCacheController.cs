using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Practice_Core6Book.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;

namespace Practice_Core6Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisCacheController : Controller
    {
        private IDistributedCache _DistributedCache { get; set; }
        private static List<Post> _PostsInDb = new List<Post>();
        private static bool DbAlreadyInit = false;
        private static object _Lock = new object();
        private const string SourceFromDatabase = "Source From Database";
        private const string SourceFromRedisCache = "Source From RedisCache";
        private const string PostKeyPrefix = "post:";

        public RedisCacheController(IDistributedCache distributedCache)
        {
            _DistributedCache = distributedCache;

            // fake database data
            lock (_Lock)
            {
                if (!DbAlreadyInit)
                {
                    _PostsInDb = new List<Post>();
                    for (int i = 1; i <= 10; i++)
                    {
                        Post post = new Post()
                        {
                            Id = i,
                            Content = $"Post {i}"
                        };
                        _PostsInDb.Add(post);
                    }
                    DbAlreadyInit = true;
                }
            }
        }

        // https://localhost:7286/api/RedisCache/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var json = _DistributedCache.GetString(PostKeyPrefix + id);
            if (json == null)
            {
                // cache data not found
                var postInDb = _PostsInDb.FirstOrDefault(c => c.Id == id);
                if (postInDb == null)
                {
                    // database data not found
                    return NotFound();
                }

                // set into cache
                _DistributedCache.SetString(PostKeyPrefix + postInDb.Id, JsonSerializer.Serialize(postInDb));
                return Ok(new PostWithSourceFrom(postInDb, SourceFromDatabase));
            }
            else
            {
#pragma warning disable CS8604 // 可能有 Null 參考引數。
                return Ok(new PostWithSourceFrom(JsonSerializer.Deserialize<Post>(json), SourceFromRedisCache));
#pragma warning restore CS8604 // 可能有 Null 參考引數。
            }
        }

        // https://localhost:7286/api/RedisCache
        [HttpPost]
        public IActionResult Post(Post post)
        {
            if (post == null)
                return BadRequest();
            if (_DistributedCache.GetString(PostKeyPrefix + post.Id) != null)
                return Conflict();
            if (_PostsInDb.Exists(c => c.Id == post.Id))
                return Conflict();

            _DistributedCache.SetString(PostKeyPrefix + post.Id, JsonSerializer.Serialize(post));
            _PostsInDb.Add(post);
            return Ok();
        }

        // https://localhost:7286/api/RedisCache/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _DistributedCache.Remove(PostKeyPrefix + id);
            var postInDb = _PostsInDb.FirstOrDefault(c => c.Id == id);
            if (postInDb == null)
            {
                // database data not found
                return NotFound();
            }
            _PostsInDb.Remove(postInDb);
            return Ok();
        }

        // https://localhost:7286/api/RedisCache
        [HttpPut]
        public IActionResult Update(Post post)
        {
            var postInDb = _PostsInDb.FirstOrDefault(c => c.Id == post.Id);
            if (postInDb == null)
            {
                // database data not found
                return NotFound();
            }
            _DistributedCache.SetString(PostKeyPrefix + post.Id, JsonSerializer.Serialize(post));
            return Ok();
        }
    }
}
