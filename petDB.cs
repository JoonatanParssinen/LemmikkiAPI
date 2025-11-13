using System.Data;
using Microsoft.Data.Sqlite;

namespace LemmikkiAPI;

public class petDB
{
    public petDB()
    {
        CreateDB();
    }

    public void CreateDB()
    {
        using (SqliteConnection connection = new SqliteConnection("Data source=pet.db"))
        {
            connection.Open();
            using var createDBCommand = connection.CreateCommand();
            createDBCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS Owners (
                    id INTEGER PRIMARY KEY,
                    name VARCHAR(255),
                    phone VARCHAR(255)
                );
                CREATE TABLE IF NOT EXISTS Pet (
                    id INTEGER PRIMARY KEY,
                    name VARCHAR(255),
                    race VARCHAR(255),
                    ownerId INTEGER
                );";

            createDBCommand.ExecuteNonQuery();

        }
    }

    public static void addPet(int id, string name, string phone, int owner_id, string petName, string race, string petId)
    {
        using (SqliteConnection connection = new SqliteConnection("Data source=pet.db"))
        {
            connection.Open();
            using var addOwnerCommand = connection.CreateCommand();
            addOwnerCommand.CommandText = @"
            INSERT INTO Owners (id, name, phone)
            VALUES (@id, @name, @phone)
            ";

            connection.Open();
            using var addPetCommand = connection.CreateCommand();
            addPetCommand.CommandText = @"
            INSERT INTO Pet (id, name, race, ownerId)
            VALUES (@id, @name, @race, @ownerId)
            ";

            addOwnerCommand.Parameters.AddWithValue("@id", id);
            addOwnerCommand.Parameters.AddWithValue("@name", name);
            addOwnerCommand.Parameters.AddWithValue("@phone", phone);

            addPetCommand.Parameters.AddWithValue("@id", petId);
            addPetCommand.Parameters.AddWithValue("@name", petName);
            addPetCommand.Parameters.AddWithValue("@race", race);
            addPetCommand.Parameters.AddWithValue("@ownerId", owner_id);

            addOwnerCommand.ExecuteNonQuery();
            addPetCommand.ExecuteNonQuery();
        }
    }

    public static string findOwnerNumber(string petname)
    {
        using (SqliteConnection connection = new SqliteConnection("Data source=pet.db"))
        {
            int ownerId = 0;
            connection.Open();
            var findOwnerCommand = connection.CreateCommand();
            findOwnerCommand.CommandText = @"
            SELECT ownerId FROM Pet WHERE name = @petname
            ";

            findOwnerCommand.Parameters.AddWithValue("@petname", petname);
            var idReader = findOwnerCommand.ExecuteReader();
            while (idReader.Read())
            {
                ownerId = idReader.GetInt32(0);
            }

            var getPhoneNumber = connection.CreateCommand();
            getPhoneNumber.CommandText = @"
            SELECT phone FROM Owners WHERE id = @ownerId
            ";

            getPhoneNumber.Parameters.AddWithValue("@ownerId", ownerId);
            var phoneReader = getPhoneNumber.ExecuteReader();
            while (phoneReader.Read())
            {
                string ownerPhoneNumber = phoneReader.GetString(0);
                return ownerPhoneNumber;
            }


            return "Did not get a phone number";

        }
    }

    public static void updatePhoneNumber(int ownerID, string newPhonenumber)
    {
        using (SqliteConnection connection = new SqliteConnection("Data source=pet.db"))
        {
            connection.Open();

            var updateNumberCommand = connection.CreateCommand();
            updateNumberCommand.CommandText = @"
            UPDATE Owners SET phone=@newPhonenumber WHERE id=@ownerID
            ";

            updateNumberCommand.Parameters.AddWithValue("@newPhonenumber", newPhonenumber);
            updateNumberCommand.Parameters.AddWithValue("@ownerID", ownerID);

            updateNumberCommand.ExecuteNonQuery();
        }
    }

}