using System.Collections.ObjectModel;
using PicSender.Models;
using PicSender.ViewModels;

namespace PicSender.Services;

public static class SampleData
{
    public static ObservableCollection<PictureGroup> GetSampleData()
    {
        var pic1 = new SinglePicture { Name = "Pic 1-1" };
        var pic2 = new SinglePicture { Name = "Pic 1-2" };
        var pic3 = new SinglePicture { Name = "Pic 1-3" };
        
        var pic4 = new SinglePicture { Name = "Pic 2-1" };
        var pic5 = new SinglePicture { Name = "Pic 2-2" };
        
        var pic6 = new SinglePicture { Name = "Pic 3-1" };
        var pic7 = new SinglePicture { Name = "Pic 3-2" };
        var pic8 = new SinglePicture { Name = "Pic 3-3" };
        
        var picGroup1 = new PictureGroup { Title = "Group 1" };
        picGroup1.Pictures.Add(pic1);
        picGroup1.Pictures.Add(pic2);
        picGroup1.Pictures.Add(pic3);
        
        var picGroup2 = new PictureGroup { Title = "Group 2" };
        picGroup2.Pictures.Add(pic4);
        picGroup2.Pictures.Add(pic5);
        
        var picGroup3 = new PictureGroup { Title = "Group 3" };
        picGroup3.Pictures.Add(pic6);
        picGroup3.Pictures.Add(pic7);
        picGroup3.Pictures.Add(pic8);
        
        var pictureGroups = new ObservableCollection<PictureGroup>
        {
            picGroup1,
            picGroup2,
            picGroup3
        };
        
        return pictureGroups;
        
    } 
}