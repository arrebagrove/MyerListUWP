﻿using GalaSoft.MvvmLight.Messaging;
using JP.Utils.UI;
using JP.UWP.CustomControl;
using Lousy.Mon;
using MyerList.Helper;
using MyerList.ViewModel;
using MyerListCustomControl;
using MyerListUWP;
using MyerListUWP.Common;
using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace MyerList.UC
{
    public sealed partial class CatePersonalizationControl : UserControl
    {
        public MainViewModel MainVM
        {
            get
            {
                return App.MainVM;
            }
        }

        private CategoryColorViewModel CateColorsVM;

        public event Action OnClickOKBtn;
        public event Action OnClickCancelBtn;

        private int _selectedID;

        public CatePersonalizationControl()
        {
            this.InitializeComponent();
            this.DataContext = this;
            CateColorsVM = new CategoryColorViewModel();
            this.Loaded += CatePersonalizationControl_Loaded;

            Messenger.Default.Register<GenericMessage<int>>(this, MessengerTokens.ShowPickCatePanel, msg =>
              {
                  var id = msg.Content;
                  _selectedID = id;
                  ShowColorStory.Begin();

                  Oli.Fade(AddBtn).From(1).To(0).For(0.5, OrSo.Seconds).Now();
                  Oli.MoveXOf(AddBtn).From(0).To(20).For(0.5, OrSo.Seconds).With(new CubicEase() { EasingMode=EasingMode.EaseOut}).Now();
              });
        }

        private void CatePersonalizationControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.RootGrid.Clip = new RectangleGeometry();
            this.RootGrid.Clip.Rect = new Rect(0, 0, this.RootGrid.ActualWidth, this.RootGrid.ActualHeight);
            ColorGridTransform.TranslateY = this.RootGrid.ActualHeight;
            key1.Value = this.RootGrid.ActualHeight;
            key2.Value = this.RootGrid.ActualHeight;
        }

        private async void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadingMaskGrid.Visibility = Visibility.Visible;
            OkBtn.IsEnabled = false;
            if (await MainVM.CateVM.SaveCatesToModify())
            {
                if (PopupService.CurrentShownCPEX != null)
                {
                    PopupService.CurrentShownCPEX.Hide();
                }
                OkBtn.IsEnabled = true;
                LoadingMaskGrid.Visibility = Visibility.Collapsed;

                OnClickOKBtn?.Invoke();
            }
            else
            {
                OkBtn.IsEnabled = true;
                LoadingMaskGrid.Visibility = Visibility.Collapsed;

                await ToastService.SendToastAsync(ResourcesHelper.GetResString("RequestError"));
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if(PopupService.CurrentShownCPEX!= null)
            {
                PopupService.CurrentShownCPEX.Hide();
            }
            OnClickCancelBtn?.Invoke();
        }

        private void PickColorOKBtn_Click(object sender, RoutedEventArgs e)
        {
            Oli.Fade(AddBtn).From(0).To(1).For(0.5, OrSo.Seconds).Now();
            Oli.MoveXOf(AddBtn).From(20).To(0).For(0.5, OrSo.Seconds).With(new CubicEase() { EasingMode = EasingMode.EaseOut }).Now();

            HideColorStory.Begin();
            var color = ColorGirdView.SelectedItem as SolidColorBrush;
            var cate = MainVM.CateVM.CatesToModify.ToList().Find(s => s.CateColorID == _selectedID);
            if(cate!= null)
            {
                cate.CateColor = color;
            }
        }

        private void PickColorCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            HideColorStory.Begin();

            Oli.Fade(AddBtn).From(0).To(1).For(0.5,OrSo.Seconds).Now();
            Oli.MoveXOf(AddBtn).From(20).To(0).For(0.5, OrSo.Seconds).With(new CubicEase() { EasingMode = EasingMode.EaseOut }).Now();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.CateVM.CatesToModify.Add(new Model.ToDoCategory()
            {
                CateColor=CateColorsVM.CateColors[0],
                CateColorID=CreateNewID(),
                CateName=ResourcesHelper.GetResString("NewCateName"),
            });
            var sv = CateListView.GetScrollViewer();
            sv.ChangeView(null, 1000, null);
        }

        public int CreateNewID()
        {
            var id = 0;
            var ids = from e in MainVM.CateVM.CatesToModify select e.CateColorID;
            if (ids.Count() > 0)
            {
                id = ids.Max();
            }
            else id = 0;
            return ++id;
        }
    }
}
