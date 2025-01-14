using System.Collections.ObjectModel;

namespace PicSender.Services;

public static class Extensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this List<T> enumerable)
    {
        return new ObservableCollection<T>(enumerable);
    }
}