namespace Practice_Core6Book.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
    }

    public class PostWithSourceFrom : Post
    {
        public PostWithSourceFrom(Post post, string sourceFrom)
        {
            this.Id = post.Id;
            this.Content = post.Content;
            this.SourceFrom = sourceFrom;
        }

        public string SourceFrom { get; set; } = "";
    }
}
