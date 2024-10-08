using System.ComponentModel.DataAnnotations;


namespace QLcaulacbosinhvien.ViewModels
{
    public class ArticleViewModel
    {
   public int ArticleID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
        public int View { get; set; }
        public string AuthorName { get; set; }
    }
}
