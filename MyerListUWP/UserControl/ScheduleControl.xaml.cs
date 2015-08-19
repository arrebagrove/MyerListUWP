using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight.Messaging;

namespace MyerList.UC
{
    public sealed partial class ScheduleControl : UserControl
    {

        TranslateTransform _tranTemplete = new TranslateTransform();
        bool _isInDoneMode = false;
        bool _isInDeleteMode = false;

        ManipulationModes defaultmode;
        ManipulationModes reordermode = ManipulationModes.TranslateY;

        public ScheduleControl()
        {
            this.InitializeComponent();

            defaultmode = SchduleTempleteGrid.ManipulationMode;

            Messenger.Default.Register<GenericMessage<string>>(this, "InReorder", act =>
                {
                    SchduleTempleteGrid.ManipulationMode = reordermode;
                });
            Messenger.Default.Register<GenericMessage<string>>(this, "OutReorder", act =>
            {
                SchduleTempleteGrid.ManipulationMode = defaultmode;
            });
        }

        /// <summary>
        /// 启动动画
        /// </summary>
        /// <param name="x">当前的X位置</param>
        private void PlayBackStoryBoard(double x)
        {
            SchduleTempleteGrid.RenderTransform = new CompositeTransform();
            StartX.Value = x;
            BackStory.Begin();
        }

        private void SchduleTempleteGrid_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {

            SchduleTempleteGrid.ManipulationDelta -= _grid_ManipulationDelta;
            SchduleTempleteGrid.ManipulationCompleted -= _grid_ManipulationCompleted;

            SchduleTempleteGrid.ManipulationDelta += _grid_ManipulationDelta;
            SchduleTempleteGrid.ManipulationCompleted += _grid_ManipulationCompleted;

            _tranTemplete = new TranslateTransform();
            SchduleTempleteGrid.RenderTransform = _tranTemplete;
 
        }

        private void _grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {

            if(e.Delta.Translation.Y>50 || e.Delta.Translation.Y<-50)
            {
                //return;
            }

            //finish 
            if (e.Delta.Translation.X > 0)
            {
                _tranTemplete.X += e.Delta.Translation.X;
                if (_isInDeleteMode)
                {
                    return;
                }
                if (_tranTemplete.X > 100)
                {
                    if (!_isInDoneMode)
                    {
                        TurnGreenStory.Begin();
                        _isInDoneMode = true;
                        _isInDeleteMode = false;
                    }
                }
            }
            //delete
            else
            {
                _tranTemplete.X += e.Delta.Translation.X;
                if (_isInDoneMode)
                {
                    return;
                }
                if (_tranTemplete.X < -100)
                {
                    if (!_isInDeleteMode)
                    {
                        TurnRedStory.Begin();
                        _isInDoneMode = false;
                        _isInDeleteMode = true;
                    }
                }
            }

        }

        private void _grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Grid _grid = sender as Grid;
            //CheckBox cb = _grid.Children.ElementAt(2) as CheckBox;

            if (e.Cumulative.Translation.X > 10)
            {
                if (e.Cumulative.Translation.X > 100)
                {
                   //cb.IsChecked = (bool)cb.IsChecked ? false : true;
                   Messenger.Default.Send(new GenericMessage<string>((string)_grid.Tag), "Check");
                }
                ReturnGreenStory.Begin();
                PlayBackStoryBoard(e.Cumulative.Translation.X);
            }
            else if (e.Cumulative.Translation.X < -10)
            {
                if (e.Cumulative.Translation.X < -100)
                {
                    if (_grid != null)
                        Messenger.Default.Send(new GenericMessage<string>((string)_grid.Tag), "Delete");
                }
                ReturnRedStory.Begin();
                PlayBackStoryBoard(e.Cumulative.Translation.X);

            }

            _isInDoneMode = false;
            _isInDeleteMode = false;

            Messenger.Default.Send(new GenericMessage<string>(""), "OutSwipe");

        }

    }
}
