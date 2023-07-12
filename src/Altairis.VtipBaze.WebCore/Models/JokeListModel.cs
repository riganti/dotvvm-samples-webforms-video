using System;
using System.Collections.Generic;

namespace Altairis.VtipBaze.WebCore.Models
{
    public class JokeListModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Approved { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string TagToAdd { get; set; }
    }
}