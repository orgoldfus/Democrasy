using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DemocrasyModel.Entities
{
    public class Manifest
    {
        public ObjectId Id { get; set; }
        public int Timestamp { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public int Rank { get; set; }
    }
}
