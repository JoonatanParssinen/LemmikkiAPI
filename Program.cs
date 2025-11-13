using LemmikkiAPI;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var petDB = new petDB();

app.MapPost("/addPet", (int id, string name, string phone, int owner_id, string petName, string race, string petId) =>
{
    petDB.addPet(id, name, phone, owner_id, petName, race, petId);
    return Results.Ok($"Pet {petName} was added succesfully! Pet info| ID: {id}, NAME: {name}, PHONE: {phone}, OWNER ID: {owner_id}, PETNAME: {petName}, RACE: {race}, PETID: {petId}");
});

app.MapGet("/findOwnersPhoneNumber", (string petname) =>
{
    string ownerPhoneNumber = petDB.findOwnerNumber(petname);
    return Results.Ok($"Phone Number: {ownerPhoneNumber}");
});

app.MapPost("/updatePhoneNumber", (int ownerID, string newPhonenumber) =>
{
    petDB.updatePhoneNumber(ownerID, newPhonenumber);
    return Results.Ok($"Owners Phone Number was updated!");
});

app.Run();
