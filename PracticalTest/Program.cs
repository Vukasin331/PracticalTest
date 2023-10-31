using AvioKarte.DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;
using PracticalTest.Models;
using System.Runtime.InteropServices;

namespace PracticalTest
{
	internal class Program
	{
		static void Main(string[] args)
		{
			MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017"); // connection to MongoDB database
			var database = dbClient.GetDatabase("passangers");
			var collection = database.GetCollection<BsonDocument>("passanger_chart");

			PassangerDataAccess db = new PassangerDataAccess();		// creating object from PassangerDataAccess

			Console.WriteLine("Enter a Name, Email, adress and Date of Birth");
			string name = GetString("Name");		// GetString(); gets string input
			string email = GetString("email");
			string adress = GetString("Adress");
			int day = GetInt("Day");				// GetInt(); gets int input also checking if it's entered correctly
			int month = GetInt("Month");
			int year = GetInt("Year");
			Console.WriteLine("Enter a gender: [Male]/[Female]");
			string gender = GetString("Gender").ToUpper();  // Gets the gender of the passanger

			Passanger passanger = new Passanger(name, email, adress, day, month, year, gender);		// creates a passanger and also validating infomation (see in passanger class)

			string meal = MealChoice();     // Gets information on what meal the passanger wants

			string flightDestination = FlightDestinationValidation(); // Gets information on what flight destination the passanger wants

			string flightClass = FlightClassValidation(passanger, meal, flightDestination); // Gets information on what flight class the passanger wants			

			Console.WriteLine("Are you traveling from Europe: [Y]/[N]");			
			bool b = false;
			string flightFromEurope = "";
			while (b = false)
			{
				string a = GetString("").ToUpper();				
				if (a == "Y")
				{
					flightFromEurope = "-EU"; b = true;
				}					
				else if (a == "N")
				{
					flightFromEurope = "-ZZ"; b = true;
				}
				else
					Console.WriteLine("Invalid input");
			}
			

			db.CreatePassanger(new Models.Passanger(name, email, adress, day, month, year, gender, meal, flightDestination, flightClass, flightFromEurope) // Creates a passanger in MongoDB database
			{
				Name = name,
				Email = email,
				Adress = adress,
				BirthDate = $"{day}/{month}/{year}",
				PassangerInfo = $"{meal}{flightDestination}{flightClass}{flightFromEurope}"
			}); ;			

			ShowAllPassangers(collection);  // Shows all passangers in C# console

			
			Console.ReadKey();
		}
		private static void ShowAllPassangers(IMongoCollection<BsonDocument> collection)
		{
			var projection = Builders<BsonDocument>.Projection
			.Include("Name")
			.Include("Email")
			.Include("Adress")
			.Include("Birthday")
			.Include("Meal")
			.Include("FlightDestionation")
			.Include("FlightClass")
			.Include("flightFromEurope");

			var documents = collection.Find(new BsonDocument()).Project(projection).ToList();

			foreach (BsonDocument doc in documents)
			{
				Console.WriteLine(doc.ToString());
			}
		}
		private static string MealChoice()
		{
			Console.WriteLine("Select a meal for the flight:");
			Console.WriteLine("[1] Europian meal\n[2] Asial meal\n[3] Vegeterian Meal\n[4] See childern's meal");
			int chooseMeal = GetInt("");
			string meal = "";
			bool b = false;
			while (b == false)
			{
				switch (chooseMeal)
				{
					case 1: meal = "G"; b = true; break;
					case 2: meal = "H"; b = true; break;
					case 3: meal = "K"; b = true; break;
					case 4:
						Console.WriteLine("Select a children's meal: \n[1] Europian meal\n[2] Asial meal\n[3] Vegeterian Meal");
						int chooseChildrensMeal = GetInt("");
						switch (chooseChildrensMeal)
						{
							case 1: meal = "g"; b = true; break;
							case 2: meal = "h"; b = true; break;
							case 3: meal = "k"; b = true; break;
						}
						break;
					default: Console.WriteLine("Invalid choice for meal."); break;
				}
			}
			return meal;
		}

		private static string FlightDestinationValidation()
		{
			Console.WriteLine("Select a flight destination:");
			Console.WriteLine("[1] United Kingdoms\n[2] Europe\n[3] Asia\n[4] America");
			int chooseDestination = GetInt("");
			string flightDestination = "";
			Console.WriteLine("Enter a time you would like to take a flight: [00/24]");
			int timeOfFlight = GetInt("");
			if (timeOfFlight > 23 && timeOfFlight < 0)
			{
				throw new Exception("Invalid time");
			}
			bool b = false;
			while (b == false)
			{
				switch (chooseDestination)
				{
					case 1:

						flightDestination = "u";
						if (timeOfFlight > 6 && timeOfFlight < 22)
							flightDestination = "U";
						b = true;
						break;
					case 2:
						flightDestination = "e";
						if (timeOfFlight > 6 && timeOfFlight < 22)
							flightDestination = "E";
						b = true;
						break;
					case 3:
						flightDestination = "a";
						if (timeOfFlight > 6 && timeOfFlight < 22)
							flightDestination = "A";
						b = true;
						break;
					case 4:
						flightDestination = "z";
						if (timeOfFlight > 6 && timeOfFlight < 22)
							flightDestination = "Z";
						b = true;
						break;
					default: Console.WriteLine("Invalid Choice"); break;
				}
			}
			return flightDestination;
		}

		private static string FlightClassValidation(Passanger passanger, string meal, string flightDestination)
		{
			string flightClass = "";
			bool b = false;
			while (b == false)
			{
				Console.WriteLine("What flight class ticket you want to buy:");
				Console.WriteLine("[1] First class\n[2] Business class\n[3] Economy class");
				int chooseFlightClass = GetInt("");

				switch (chooseFlightClass)
				{
					case 1:
						if (flightDestination == "U")
						{
							Console.WriteLine("There is no first class to UK");
							Console.WriteLine("Choose new travel class: ");
							break;
						}
						else
							flightClass = "P"; b = true; break;
					case 2:
						if ((passanger.Year >= 2022) || passanger.Year < 1943)
							Console.WriteLine("You cant buy this class ticket");
						if (passanger.Year >= 2005)
						{	
							
							while (meal == "g" || meal == "h" || meal == "k")
							{
								Console.WriteLine("You cant have children's meal on business class");
								Console.WriteLine("Chose new meal: ");								
								meal = MealChoice();
							}
							flightClass = "B"; b = true; break;
						}
						else
							flightClass = "B"; b = true; break;

					case 3: flightClass = "R"; b = true; break;
					default: Console.WriteLine("Invalid choice for flight class."); break;
				}
			}
			return flightClass;
		}
		static public string GetString(string prompt)
		{
			Console.Write($"{prompt} > ");
			string input = Console.ReadLine();
			return input;
		}
		static public int GetInt(string prompt)
		{
			Console.Write($"{prompt} > ");
			if (!int.TryParse(Console.ReadLine(), out int value))
			{
				throw new Exception("Invalid input");
			}
			return value;
		}
	}
}