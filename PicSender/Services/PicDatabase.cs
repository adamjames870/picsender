using System.Diagnostics;
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
    
    public async Task<List<SinglePicture>> GetPicturesAsync(int pictureGroupId)
    {
        await Init();
        return await _db.Table<SinglePicture>().Where(p => p.PictureGroupId == pictureGroupId).ToListAsync();
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
    
    public async Task DeletePictureGroupAsync(int groupId)
    {
        
        try
        {
            await Init();
            await _db.Table<PictureGroup>().Where(g => g.Id == groupId).DeleteAsync();
            var pics = _db.Table<SinglePicture>().Where(p => p.PictureGroupId == groupId);
            if (pics is not null) await pics.DeleteAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert($"Error in {nameof(DeletePictureGroupAsync)} in PicDb", $"Exception: {ex.Message}", "OK");
        }
    }

    public async Task<PictureGroup> GetPictureGroupAsync(int pictureGroupId)
    {
        await Init();
        return await _db.Table<PictureGroup>().Where(g => g.Id == pictureGroupId).FirstOrDefaultAsync();
    }

    public async Task<int> UpdatePictureGroupAsync(PictureGroup pictureGroup)
    {
        await Init();
        var newGroup = await _db.UpdateAsync(pictureGroup);
        return newGroup;
    }
    
}