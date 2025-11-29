using System;
using System.Text;

namespace CSI_3480_Group_Project
{
	internal class Generation
	{
		// possible characters
		private const string lowercase = "abcdefghijklmnopqrstuvwxyz";
		private const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string Numbers = "0123456789";
		private const string Symbols = "!@#$%^&*()_-+=";

		public static string GeneratePassword(int length, bool useSpecialChars)
		{
			
			string validChars = lowercase + uppercase + Numbers;
		

			// if user wants special characters
			if (useSpecialChars)
			{
				// add symbols to valid characters string
				validChars = validChars + Symbols;
			}


			StringBuilder  result = new StringBuilder();
			Random rand = new Random();
		
			// build password using loop based on user specified length
			for (int i = 0; i < length; i++)
			{
				int index = rand.Next(0, validChars.Length);
				result.Append(validChars[index]);
			}

			return result.ToString();
		}
	}
}