using PracticalTest.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using PracticalTest.Models;

namespace PracticalTest
{
	internal class PassangerDataAccess
	{
		private const string ConnectionString = "mongodb://127.0.0.1:27017";
		private const string DatabaseName = "passangers";
		private const string PassangerCollection = "passanger_chart";

		private IMongoCollection<T> ConnectToMongo<T>(in string collection)
		{
			var client = new MongoClient(ConnectionString);
			var db = client.GetDatabase(DatabaseName);
			return db.GetCollection<T>(collection);
		}
		public async Task<List<Passanger>> GetAllPassangers()
		{
			var usersCollection = ConnectToMongo<Passanger>(PassangerCollection);
			var results = await usersCollection.FindAsync(_ => true);
			return results.ToList();
		}
		public Task CreatePassanger(Passanger user)
		{
			var passangersCollection = ConnectToMongo<Passanger>(PassangerDataAccess.PassangerCollection);
			return passangersCollection.InsertOneAsync(user);
		}
	}
}
