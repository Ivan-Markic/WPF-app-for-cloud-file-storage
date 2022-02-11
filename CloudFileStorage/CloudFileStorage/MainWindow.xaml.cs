using Azure.Storage.Blobs.Models;
using CloudFileStorage.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CloudFileStorage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FileViewModel fileViewModel;

        public MainWindow()
        {
            InitializeComponent();
            fileViewModel = new FileViewModel();
            Init();
        }

        private void Init()
        {
            LbBlobs.ItemsSource = fileViewModel.Items;
            CbDirectories.ItemsSource = fileViewModel.Directories;
            if (fileViewModel.Directories.Count > 0)
            {
                CbDirectories.SelectedItem = fileViewModel.Directories[0];
            }
        }

        private void LbBlobs_SelectionChanged(object sender, SelectionChangedEventArgs e) => DataContext = LbBlobs.SelectedItem as BlobItem;


        private async void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG|*.jpg;*.jpeg|TIFF|*.tif;*.tiff|PNG|*.png|SVG|*.svg|GIF|*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                string extension = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf(".") + 1);

                await fileViewModel.UploadAsync(openFileDialog.FileName, extension.ToUpper());
            }
            CbDirectories.Text = fileViewModel.Directory;
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(LbBlobs.SelectedItem is BlobItem blobItem))
            {
                return;
            }
            await fileViewModel.DeleteAsync(blobItem);
            CbDirectories.Text = fileViewModel.Directory;
        }

        private async void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (!(LbBlobs.SelectedItem is BlobItem blobItem))
            {
                return;
            }
            var saveFileDialog = new SaveFileDialog()
            {
                FileName = blobItem.Name.Contains(FileViewModel.ForwardSlash)
                ? blobItem.Name.Substring(blobItem.Name.LastIndexOf(FileViewModel.ForwardSlash) + 1)
                : blobItem.Name
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                await fileViewModel.DownloadAsync(blobItem, saveFileDialog.FileName);
            }
            CbDirectories.Text = fileViewModel.Directory;
        }

        private void CbDirectories_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                fileViewModel.Directory = CbDirectories.Text;
            }
        }

        private void CbDirectories_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fileViewModel.Directories.Contains(CbDirectories.Text))
            {
                fileViewModel.Directory = CbDirectories.Text;
                CbDirectories.SelectedItem = fileViewModel.Directory;

            }
        }
    }
}
