using System;
using System.Collections.Generic;
using System.Threading;

namespace LegacyApp
{
    public interface IUserCreditService : IDisposable
    {
        int GetCreditLimit(string lastName, DateTime dateOfBirth);
    }

    public class UserCreditService : IUserCreditService
    {
        private readonly Dictionary<string, int> _database = new Dictionary<string, int>()
        {
            {"Kowalski", 200},
            {"Malewski", 20000},
            {"Smith", 10000},
            {"Doe", 3000},
            {"Kwiatkowski", 1000}
        };

        public int GetCreditLimit(string lastName, DateTime dateOfBirth)
        {
            // Simulating contact with remote service to get client's credit limit
            int randomWaitingTime = new Random().Next(3000);
            Thread.Sleep(randomWaitingTime);

            if (_database.ContainsKey(lastName))
                return _database[lastName];

            throw new ArgumentException($"Client {lastName} does not exist");
        }
        public void Dispose()
        {
            //Simulating disposing of resources
        }
    }
}