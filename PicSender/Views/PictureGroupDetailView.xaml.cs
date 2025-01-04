using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PicSender.ViewModels;

namespace PicSender.Views;

public partial class PictureGroupDetailView : ContentPage
{
    public PictureGroupDetailView(PictureGroupDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}