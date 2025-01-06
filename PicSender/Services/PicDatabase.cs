using PicSender.Models;
using SQLite;
using Path = System.IO.Path;

namespace PicSender.Services;

public static class PicDatabase
{
    private static SQLiteAsyncConnection _db;
    static async Task Init()
    {
        if (_db is null) return;
        
        var databasePath = Path.Combine(FileSystem.Current.AppDataDirectory, "PicSender.db");
        _db = new SQLiteAsyncConnection(databasePath);
        await _db.CreateTableAsync<SinglePicture>();

    }
    
    public static async Task AddPictureAsync(SinglePicture picture)
    {
        await Init();
        await _db.InsertAsync(picture);
    }
    
    public static async Task DeletePictureAsync(SinglePicture picture)
    {
        await Init();
        await _db.DeleteAsync(picture);
    }
    
}