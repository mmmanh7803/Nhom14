using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLcaulacbosinhvien.Models
{
  public class Article
{
    public int ArticleID { get; set; }
    
    public string? Title { get; set; }        // Nullable
    public string? Content { get; set; }      // Nullable
    public string? Image { get; set; }        // Nullable
    public DateTime Date { get; set; }
    public int View { get; set; }
    public bool? IsShow { get; set; }         // Nullable
    public int AccountID { get; set; }        // Không nullable
    public string? Description { get; set; }  // Nullable

    public string? AuthorName {get; set;}
}

}