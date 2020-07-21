using System;
using MongoDB.Driver;
using MongoDB.Bson;
using DataTransfer;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess
{
    public class Mongo
    {
        //private ConnectionString _cnn = new ConnectionString();
        private MongoClient _dbClient;
        private IMongoCollection<BsonDocument> _collection;
        private IMongoDatabase _db;
        private string conn;
        

        public Mongo(string connect)
        {
            conn = connect.Replace(" ", "");
            _dbClient = new MongoClient(conn);
            _db = _dbClient.GetDatabase("dcdata");
            _collection = _db.GetCollection<BsonDocument>("dcdata");
        }

        public async Task CreateWish(DTOWish wish)
        {
            var bsonWish = new BsonDocument
            {
                {
                    "Empid", wish.EmpId
                },
                {
                    "Shift", wish.Shift
                },
                {
                    "Day", wish.Day
                },
                {
                    "Creator", wish.Creator
                },
                {
                    "Set", wish.Set
                }
            };
            await _collection.InsertOneAsync(bsonWish);
        }

        public async Task<List<DTOWish>> GetWishes(string set, string creator)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("Set", set),
                Builders<BsonDocument>.Filter.Eq("Creator", creator));
            var wishes = new List<BsonDocument>();
            await _collection.FindAsync(filter).Result.ForEachAsync(w => wishes.Add(w));


            var allWishes = new List<BsonDocument>();
            await _collection.Find(new BsonDocument()).ForEachAsync(w => allWishes.Add(w));
            var correctWishes = allWishes.Where(w => w.ElementAt(5).Value == set && w.ElementAt(4).Value == creator).ToList();
            var DTOWishes = ConvertToDTOWishes(wishes);

            return DTOWishes;
        }

        public async Task<List<DTOWish>> GetWishes()
        {
            var allWishes = new List<BsonDocument>();
            await _collection.Find(new BsonDocument()).ForEachAsync(w => allWishes.Add(w));
            var DTOWishes = ConvertToDTOWishes(allWishes);
            return DTOWishes;
        }

        public async Task DeleteWishSet(string set, string creator)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("Set", set),
                Builders<BsonDocument>.Filter.Eq("Creator", creator));
            await _collection.DeleteManyAsync(filter);
        }

        public async Task DeleteWish(string creator, string set, int day, int shift, int emp)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("Set", set),
                Builders<BsonDocument>.Filter.Eq("Creator", creator),
                Builders<BsonDocument>.Filter.Eq("Day", day),
                Builders<BsonDocument>.Filter.Eq("Shift", shift),
                Builders<BsonDocument>.Filter.Eq("Empid", emp));
            await _collection.DeleteOneAsync(filter);
        }

        private List<DTOWish> ConvertToDTOWishes(List<BsonDocument> correctWishes)
        {
            var wishes = new List<DTOWish>();
            foreach(var wish in correctWishes)
            {
                int id = (int)wish.ElementAt(1).Value;
                int shift = (int)wish.ElementAt(2).Value;
                int day = (int)wish.ElementAt(3).Value;
                string creator = wish.ElementAt(4).Value.ToString();
                string set = wish.ElementAt(5).Value.ToString();

                var DTO = new DTOWish(id, shift, day, creator, set);
                wishes.Add(DTO);
            }
            return wishes;
        }
    }
}
