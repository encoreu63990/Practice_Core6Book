using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Practice_Core6Book.Models;

namespace Practice_Core6Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemoryCacheController : Controller
    {
        private IMemoryCache _MemoryCache { get; set; }
        private static List<Post> _PostsInDb = new List<Post>();
        private static bool DbAlreadyInit = false;
        private static object _Lock = new object();
        private const string SourceFromDatabase = "Source From Database";
        private const string SourceFromMemoryCache = "Source From MemoryCache";
        private const string PostKeyPrefix = "post:";

        public MemoryCacheController(IMemoryCache memoryCache)
        {
            _MemoryCache = memoryCache;

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

        // https://localhost:7286/api/MemoryCache/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_MemoryCache.TryGetValue<Post>(PostKeyPrefix + id, out var postInCache))
            {
                // cache data not found
                var postInDb = _PostsInDb.FirstOrDefault(c => c.Id == id);
                if (postInDb == null)
                {
                    // database data not found
                    return NotFound();
                }

                // set into cache
                var cahceOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
                _MemoryCache.Set(PostKeyPrefix + postInDb.Id, postInDb, cahceOption);
                return Ok(new PostWithSourceFrom(postInDb, SourceFromDatabase));
            }
            else
                return Ok(new PostWithSourceFrom(postInCache, SourceFromMemoryCache));
        }

        // https://localhost:7286/api/MemoryCache
        [HttpPost]
        public IActionResult Post(Post post)
        {
            if (post == null)
                return BadRequest();
            if (_MemoryCache.TryGetValue<Post>(PostKeyPrefix + post.Id, out var postInCache))
                return Conflict();
            if (_PostsInDb.Exists(c => c.Id == post.Id))
                return Conflict();

            var cahceOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
            _MemoryCache.Set(PostKeyPrefix + post.Id, post, cahceOption);
            _PostsInDb.Add(post);
            return Ok();
        }

        // https://localhost:7286/api/MemoryCache/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _MemoryCache.Remove(PostKeyPrefix + id);
            var postInDb = _PostsInDb.FirstOrDefault(c => c.Id == id);
            if (postInDb == null)
            {
                // database data not found
                return NotFound();
            }
            _PostsInDb.Remove(postInDb);
            return Ok();
        }

        // https://localhost:7286/api/MemoryCache
        [HttpPut]
        public IActionResult Update(Post post)
        {
            var postInDb = _PostsInDb.FirstOrDefault(c => c.Id == post.Id);
            if (postInDb == null)
            {
                // database data not found
                return NotFound();
            }
            var cahceOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
            _MemoryCache.Set(PostKeyPrefix + post.Id, post, cahceOption);
            return Ok();
        }
    }
}
