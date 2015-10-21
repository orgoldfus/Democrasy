using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemocrasyModel.Entities;
using DemocrasyServices.Time;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace DemocrasyBackOffice.Manifesto
{
    public class ManifestServices
    {
        public Manifest CreateManifest(string text, string author)
        {
            Manifest manifesto = new Manifest()
            {
                Author = author,
                Timestamp = TimeServices.GetCurrentUnixTimeStamp(),
                Text = text,
                Rank = 0
            };

            // Save manifest to db
            var manifestCollection = GetManifestCollection();
            manifestCollection.InsertOneAsync(manifesto);

            return manifesto;
        }

        public dynamic GetTopManifests(int numOfRecords, int skip = 0)
        {
            var manifestCollection = GetManifestCollection();

            var filter = new BsonDocument();
            var sort = Builders<Manifest>.Sort.Descending("Rank");

            var task = manifestCollection.Find(filter).Sort(sort).Skip(skip).Limit(numOfRecords).ToListAsync();
            var result = task.Result;

            var manifests = result.Select(x => new 
            { 
                Id = x.Id.ToString(), 
                Timestamp = TimeServices.UnixTimeStampToDateTime(x.Timestamp).ToString(),
                Author = x.Author,
                Text = x.Text,
                Rank = x.Rank
            });

            return manifests;
        }

        public dynamic GetNewManifests(int numOfRecords, int skip = 0)
        {
            var manifestCollection = GetManifestCollection();

            var filter = new BsonDocument();
            var sort = Builders<Manifest>.Sort.Descending("Timestamp");

            var task = manifestCollection.Find(filter).Sort(sort).Skip(skip).Limit(numOfRecords).ToListAsync();
            var result = task.Result;

            var manifests = result.Select(x => new
            {
                Id = x.Id.ToString(),
                Timestamp = TimeServices.UnixTimeStampToDateTime(x.Timestamp).ToString(),
                Author = x.Author,
                Text = x.Text,
                Rank = x.Rank
            });

            return manifests;
        }

        public bool UpvoteManifest(string id)
        {
            return UpdateManifestRank(id, 1);
        }

        public bool DownvoteManifest(string id)
        {
            return UpdateManifestRank(id, -1);
        }

        private bool UpdateManifestRank(string id, int updateVal)
        {
            try
            {
                var manifestCollection = GetManifestCollection();

                var filter = Builders<Manifest>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
                var update = Builders<Manifest>.Update.Inc(x => x.Rank, updateVal);

                manifestCollection.FindOneAndUpdateAsync(filter, update);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private IMongoCollection<Manifest> GetManifestCollection()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Democrasy");
            
            return database.GetCollection<Manifest>("Manifest");
        }
    }
}
