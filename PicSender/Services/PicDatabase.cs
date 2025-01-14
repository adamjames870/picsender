using PicSender.Models;
using SQLite;
using Path = System.IO.Path;

namespace PicSender.Services;

public class PicDatabase
{
    private SQLiteAsyncConnection _db;
    private async Task Init()
    {
        if (_db is not null) return;
        
        var databasePath = Path.Combine(FileSystem.Current.AppDataDirectory, "PicSender.db");
        _db = new SQLiteAsyncConnection(databasePath);
        await _db.CreateTableAsync<SinglePicture>();
        await _db.CreateTableAsync<PictureGroup>();

    }
    
    public async Task<List<PictureGroup>> GetPictureGroupsAsync()
    {
        await Init();
        return await _db.Table<PictureGroup>().ToListAsync();
    }
    
    public async Task<List<SinglePicture>> GetPicturesAsync()
    {
        await Init();
        return await _db.Table<SinglePicture>().ToListAsync();
    }
    
    public async Task<List<SinglePicture>> GetPicturesAsync(PictureGroup group)
    {
        await Init();
        return await _db.Table<SinglePicture>().Where(p => p.PictureGroup.Id == group.Id).ToListAsync();
    }
    
    public async Task<SinglePicture> GetPictureAsync(int id)
    {
        await Init();
        return await _db.Table<SinglePicture>().Where(p => p.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task AddPictureAsync(SinglePicture picture)
    {
        await Init();
        await _db.InsertAsync(picture);
    }
    
    public async Task DeletePictureAsync(SinglePicture picture)
    {
        await Init();
        await _db.DeleteAsync(picture);
    }

    public async Task AddPictureGroupAsync(PictureGroup newGroup)
    {
        await Init();
        await _db.InsertAsync(newGroup);
    }
    
    public async Task DeletePictureGroupAsync(PictureGroup group)
    {
        await Init();
        await _db.Table<SinglePicture>().Where(p => p.PictureGroup.Id == group.Id).DeleteAsync();
        await _db.DeleteAsync(group);
    }
    
}