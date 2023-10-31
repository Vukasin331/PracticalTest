using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PracticalTest.Models
{
	internal partial class Passanger
	{
		public string PassangerCode { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Adress { get; set; }
		public string BirthDate { get; set; }
		public int Day { get; set; }
		public string PassangerInfo { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public string Gender { get; set; }
		public string Meal { get; set; }
		public string FlightDestionation { get; set; }
		public string FlightClass { get; set; }
		public Passanger(string name, string email, string address, int day, int month, int year, string gender, string meal, string flightDestination, string flightClass)
		{
			Name = name;
			Email = email;
			Adress = address;
			Day = day;
			Month = month;
			Year = year;
			Gender = gender;
			Meal = meal;
			FlightDestionation = flightDestination;
			FlightClass = flightClass;
		}
		public Passanger(string name, string email, string adress, int day, int month, int year, string gender)
		{
			Name = name;
			Email = email;
			Adress = adress;
			Day = day;
			Month = month;
			Year = year;
			Gender = gender;

			EmailValidation(email);

			DateValidaton(day, month, year);
			GenderValidation(gender);
		}
		private string GenderValidation(string gender)
		{
			if (gender == "MALE")
				Gender = "X";
			else if (gender == "FEMALE")
				Gender = "Y";

			if (Year > 2011 && gender == "X")
				Gender = "x";
			else if (Year > 2011 && gender == "Y")
				Gender = "y";
			return Gender;
		}
		private static void EmailValidation(string email) // This method is used to see if the email is inserted correctly
		{
			var emailvalidator = new EmailAddressAttribute();
			if (emailvalidator.IsValid(email) == false)
			{
				throw new Exception("Invalid email");
			}
		}
		private static void DateValidaton(int day, int month, int year)     // This method is used to see if the date is inserted correctly
		{
			if (year > 2023 && month > 11)
				throw new ArgumentException("You were not born yet");

			if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
			{
				if (day > 31 || day < 1)
					throw new ArgumentException("Invalid day");
			}
			else if (month == 4 || month == 6 || month == 9 || month == 11)
			{
				if (day > 30 || day < 1)
					throw new ArgumentException("Invalid day");
			}

			if (month == 2 && (year % 4) == 0)
			{
				if (year % 100 == 0 && year % 400 != 0)
				{
					if (day > 28 || day < 1)
						throw new ArgumentException("Invalid day");
				}
				else if (year % 100 == 0 && year % 400 == 0)
				{
					if (day > 28 || day < 1)
						throw new ArgumentException("Invalid day");
				}
			}
			if (month == 2 && (year % 4) != 0)
			{
				if (day > 28 || day < 1)
					throw new ArgumentException("Invalid day");
			}
		}

	}
}
