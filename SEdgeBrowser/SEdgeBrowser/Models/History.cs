using System;

namespace SEdgeBrowser.Models
{
    public class History
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Favicon { get; set; }
        public DateTime VisitDateTime { get; set; }
    }
}
