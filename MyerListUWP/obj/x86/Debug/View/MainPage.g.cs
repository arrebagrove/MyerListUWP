﻿#pragma checksum "D:\Dev\apps\MyerListUWP\MyerListUWP\View\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6494D08A9C3201FB48832EC849B14901"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyerListUWP.View
{
    partial class MainPage : 
        global::MyerList.Base.BindablePage, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        internal class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_Panel_Background(global::Windows.UI.Xaml.Controls.Panel obj, global::Windows.UI.Xaml.Media.Brush value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Windows.UI.Xaml.Media.Brush) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Windows.UI.Xaml.Media.Brush), targetNullValue);
                }
                obj.Background = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_Primitives_ButtonBase_Command(global::Windows.UI.Xaml.Controls.Primitives.ButtonBase obj, global::System.Windows.Input.ICommand value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Windows.Input.ICommand) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Windows.Input.ICommand), targetNullValue);
                }
                obj.Command = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_Pivot_SelectedIndex(global::Windows.UI.Xaml.Controls.Pivot obj, global::System.Int32 value)
            {
                obj.SelectedIndex = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(global::Windows.UI.Xaml.Controls.ItemsControl obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.ItemsSource = value;
            }
            public static void Set_Windows_UI_Xaml_UIElement_Visibility(global::Windows.UI.Xaml.UIElement obj, global::Windows.UI.Xaml.Visibility value)
            {
                obj.Visibility = value;
            }
            public static void Set_JP_Utils_Framework_ListViewBaseCommandEx_ItemClickCommand(global::Windows.UI.Xaml.FrameworkElement obj, global::System.Windows.Input.ICommand value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Windows.Input.ICommand) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Windows.Input.ICommand), targetNullValue);
                }
                global::JP.Utils.Framework.ListViewBaseCommandEx.SetItemClickCommand(obj, value);
            }
            public static void Set_JP_Utils_UI_AnimatedTextBlock_TextContent(global::JP.Utils.UI.AnimatedTextBlock obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.TextContent = value ?? global::System.String.Empty;
            }
            public static void Set_Windows_UI_Xaml_UIElement_Opacity(global::Windows.UI.Xaml.UIElement obj, global::System.Double value)
            {
                obj.Opacity = value;
            }
        };

        private class MainPage_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMainPage_Bindings
        {
            private global::MyerListUWP.View.MainPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.Grid obj6;
            private global::Windows.UI.Xaml.Controls.AppBarButton obj14;
            private global::Windows.UI.Xaml.Controls.AppBarButton obj15;
            private global::Windows.UI.Xaml.Controls.AppBarToggleButton obj16;
            private global::Windows.UI.Xaml.Controls.Pivot obj17;
            private global::Windows.UI.Xaml.Controls.ListView obj18;
            private global::Windows.UI.Xaml.Controls.StackPanel obj19;
            private global::Windows.UI.Xaml.Controls.ListView obj20;
            private global::Windows.UI.Xaml.Controls.StackPanel obj21;
            private global::JP.Utils.UI.AnimatedTextBlock obj23;
            private global::Windows.UI.Xaml.Controls.ProgressRing obj24;
            private global::Windows.UI.Xaml.Controls.Button obj25;
            private global::Windows.UI.Xaml.Controls.Button obj26;
            private global::Windows.UI.Xaml.Controls.Image obj27;
            private global::Windows.UI.Xaml.Controls.Image obj28;
            private global::Windows.UI.Xaml.Controls.Grid obj29;

            private MainPage_obj1_BindingsTracking bindingsTracking;

            public MainPage_obj1_Bindings()
            {
                this.bindingsTracking = new MainPage_obj1_BindingsTracking(this);
            }

            ~MainPage_obj1_Bindings()
            {
                StopTracking();
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 6:
                        this.obj6 = (global::Windows.UI.Xaml.Controls.Grid)target;
                        break;
                    case 14:
                        this.obj14 = (global::Windows.UI.Xaml.Controls.AppBarButton)target;
                        break;
                    case 15:
                        this.obj15 = (global::Windows.UI.Xaml.Controls.AppBarButton)target;
                        break;
                    case 16:
                        this.obj16 = (global::Windows.UI.Xaml.Controls.AppBarToggleButton)target;
                        break;
                    case 17:
                        this.obj17 = (global::Windows.UI.Xaml.Controls.Pivot)target;
                        (this.obj17).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.Pivot.SelectedIndexProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                                if (this.initialized)
                                {
                                    // Update Two Way binding
                                    this.dataRoot.MainVM.SelectedIndex = (this.obj17).SelectedIndex;
                                }
                            });
                        break;
                    case 18:
                        this.obj18 = (global::Windows.UI.Xaml.Controls.ListView)target;
                        break;
                    case 19:
                        this.obj19 = (global::Windows.UI.Xaml.Controls.StackPanel)target;
                        break;
                    case 20:
                        this.obj20 = (global::Windows.UI.Xaml.Controls.ListView)target;
                        break;
                    case 21:
                        this.obj21 = (global::Windows.UI.Xaml.Controls.StackPanel)target;
                        break;
                    case 23:
                        this.obj23 = (global::JP.Utils.UI.AnimatedTextBlock)target;
                        break;
                    case 24:
                        this.obj24 = (global::Windows.UI.Xaml.Controls.ProgressRing)target;
                        break;
                    case 25:
                        this.obj25 = (global::Windows.UI.Xaml.Controls.Button)target;
                        break;
                    case 26:
                        this.obj26 = (global::Windows.UI.Xaml.Controls.Button)target;
                        break;
                    case 27:
                        this.obj27 = (global::Windows.UI.Xaml.Controls.Image)target;
                        break;
                    case 28:
                        this.obj28 = (global::Windows.UI.Xaml.Controls.Image)target;
                        break;
                    case 29:
                        this.obj29 = (global::Windows.UI.Xaml.Controls.Grid)target;
                        break;
                    default:
                        break;
                }
            }

            // IMainPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            // MainPage_obj1_Bindings

            public void SetDataRoot(global::MyerListUWP.View.MainPage newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.dataRoot = newDataRoot;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::MyerListUWP.View.MainPage obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_MainVM(obj.MainVM, phase);
                    }
                }
            }
            private void Update_MainVM(global::MyerList.ViewModel.MainViewModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_MainVM(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_MainVM_CateColor(obj.CateColor, phase);
                    }
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_MainVM_AddCommand(obj.AddCommand, phase);
                        this.Update_MainVM_SyncCommand(obj.SyncCommand, phase);
                        this.Update_MainVM_ToggleReorderCommand(obj.ToggleReorderCommand, phase);
                    }
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_MainVM_SelectedIndex(obj.SelectedIndex, phase);
                        this.Update_MainVM_DeletedToDos(obj.DeletedToDos, phase);
                        this.Update_MainVM_NoDeletedItemsVisibility(obj.NoDeletedItemsVisibility, phase);
                        this.Update_MainVM_CurrrentDisplayToDos(obj.CurrrentDisplayToDos, phase);
                    }
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_MainVM_ModifyCommand(obj.ModifyCommand, phase);
                    }
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_MainVM_ShowNoItems(obj.ShowNoItems, phase);
                        this.Update_MainVM_Title(obj.Title, phase);
                        this.Update_MainVM_IsLoading(obj.IsLoading, phase);
                    }
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_MainVM_SelectToDoCommand(obj.SelectToDoCommand, phase);
                        this.Update_MainVM_SelectDeleteCommand(obj.SelectDeleteCommand, phase);
                    }
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_MainVM_DeleteIconAlpha(obj.DeleteIconAlpha, phase);
                        this.Update_MainVM_TodoIconAlpha(obj.TodoIconAlpha, phase);
                    }
                }
            }
            private void Update_MainVM_CateColor(global::Windows.UI.Xaml.Media.SolidColorBrush obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Panel_Background(this.obj6, obj, null);
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Panel_Background(this.obj29, obj, null);
                }
            }
            private void Update_MainVM_AddCommand(global::GalaSoft.MvvmLight.Command.RelayCommand obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_ButtonBase_Command(this.obj14, obj, null);
                }
            }
            private void Update_MainVM_SyncCommand(global::GalaSoft.MvvmLight.Command.RelayCommand obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_ButtonBase_Command(this.obj15, obj, null);
                }
            }
            private void Update_MainVM_ToggleReorderCommand(global::GalaSoft.MvvmLight.Command.RelayCommand obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_ButtonBase_Command(this.obj16, obj, null);
                }
            }
            private void Update_MainVM_SelectedIndex(global::System.Int32 obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Pivot_SelectedIndex(this.obj17, obj);
                }
            }
            private void Update_MainVM_DeletedToDos(global::System.Collections.ObjectModel.ObservableCollection<global::MyerList.Model.ToDo> obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj18, obj, null);
                }
            }
            private void Update_MainVM_NoDeletedItemsVisibility(global::Windows.UI.Xaml.Visibility obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj19, obj);
                }
            }
            private void Update_MainVM_CurrrentDisplayToDos(global::System.Collections.ObjectModel.ObservableCollection<global::MyerList.Model.ToDo> obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj20, obj, null);
                }
            }
            private void Update_MainVM_ModifyCommand(global::GalaSoft.MvvmLight.Command.RelayCommand<global::MyerList.Model.ToDo> obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_JP_Utils_Framework_ListViewBaseCommandEx_ItemClickCommand(this.obj20, obj, null);
                }
            }
            private void Update_MainVM_ShowNoItems(global::Windows.UI.Xaml.Visibility obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj21, obj);
                }
            }
            private void Update_MainVM_Title(global::System.String obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_JP_Utils_UI_AnimatedTextBlock_TextContent(this.obj23, obj, null);
                }
            }
            private void Update_MainVM_IsLoading(global::Windows.UI.Xaml.Visibility obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj24, obj);
                }
            }
            private void Update_MainVM_SelectToDoCommand(global::GalaSoft.MvvmLight.Command.RelayCommand obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_ButtonBase_Command(this.obj25, obj, null);
                }
            }
            private void Update_MainVM_SelectDeleteCommand(global::GalaSoft.MvvmLight.Command.RelayCommand obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_ButtonBase_Command(this.obj26, obj, null);
                }
            }
            private void Update_MainVM_DeleteIconAlpha(global::System.Double obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Opacity(this.obj27, obj);
                }
            }
            private void Update_MainVM_TodoIconAlpha(global::System.Double obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Opacity(this.obj28, obj);
                }
            }

            private class MainPage_obj1_BindingsTracking
            {
                global::System.WeakReference<MainPage_obj1_Bindings> WeakRefToBindingObj; 

                public MainPage_obj1_BindingsTracking(MainPage_obj1_Bindings obj)
                {
                    WeakRefToBindingObj = new global::System.WeakReference<MainPage_obj1_Bindings>(obj);
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_MainVM(null);
                }

                public void PropertyChanged_MainVM(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    MainPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::MyerList.ViewModel.MainViewModel obj = sender as global::MyerList.ViewModel.MainViewModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                    bindings.Update_MainVM_CateColor(obj.CateColor, DATA_CHANGED);
                                    bindings.Update_MainVM_SelectedIndex(obj.SelectedIndex, DATA_CHANGED);
                                    bindings.Update_MainVM_DeletedToDos(obj.DeletedToDos, DATA_CHANGED);
                                    bindings.Update_MainVM_NoDeletedItemsVisibility(obj.NoDeletedItemsVisibility, DATA_CHANGED);
                                    bindings.Update_MainVM_CurrrentDisplayToDos(obj.CurrrentDisplayToDos, DATA_CHANGED);
                                    bindings.Update_MainVM_ShowNoItems(obj.ShowNoItems, DATA_CHANGED);
                                    bindings.Update_MainVM_Title(obj.Title, DATA_CHANGED);
                                    bindings.Update_MainVM_IsLoading(obj.IsLoading, DATA_CHANGED);
                                    bindings.Update_MainVM_DeleteIconAlpha(obj.DeleteIconAlpha, DATA_CHANGED);
                                    bindings.Update_MainVM_TodoIconAlpha(obj.TodoIconAlpha, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "CateColor" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_MainVM_CateColor(obj.CateColor, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "SelectedIndex" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_MainVM_SelectedIndex(obj.SelectedIndex, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "DeletedToDos" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_MainVM_DeletedToDos(obj.DeletedToDos, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "NoDeletedItemsVisibility" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_MainVM_NoDeletedItemsVisibility(obj.NoDeletedItemsVisibility, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "CurrrentDisplayToDos" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_MainVM_CurrrentDisplayToDos(obj.CurrrentDisplayToDos, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "ShowNoItems" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_MainVM_ShowNoItems(obj.ShowNoItems, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "Title" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_MainVM_Title(obj.Title, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "IsLoading" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_MainVM_IsLoading(obj.IsLoading, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "DeleteIconAlpha" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_MainVM_DeleteIconAlpha(obj.DeleteIconAlpha, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "TodoIconAlpha" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_MainVM_TodoIconAlpha(obj.TodoIconAlpha, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        ReleaseAllListeners();
                    }
                }
                private global::MyerList.ViewModel.MainViewModel cache_MainVM = null;
                public void UpdateChildListeners_MainVM(global::MyerList.ViewModel.MainViewModel obj)
                {
                    if (obj != cache_MainVM)
                    {
                        if (cache_MainVM != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_MainVM).PropertyChanged -= PropertyChanged_MainVM;
                            cache_MainVM = null;
                        }
                        if (obj != null)
                        {
                            cache_MainVM = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_MainVM;
                        }
                    }
                }
                public void PropertyChanged_MainVM_DeletedToDos(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    MainPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::System.Collections.ObjectModel.ObservableCollection<global::MyerList.Model.ToDo> obj = sender as global::System.Collections.ObjectModel.ObservableCollection<global::MyerList.Model.ToDo>;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                        }
                        else
                        {
                            switch (propName)
                            {
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        ReleaseAllListeners();
                    }
                }
                public void CollectionChanged_MainVM_DeletedToDos(object sender, global::System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    MainPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        global::System.Collections.ObjectModel.ObservableCollection<global::MyerList.Model.ToDo> obj = sender as global::System.Collections.ObjectModel.ObservableCollection<global::MyerList.Model.ToDo>;
                    }
                    else
                    {
                        ReleaseAllListeners();
                    }
                }
                public void PropertyChanged_MainVM_CurrrentDisplayToDos(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    MainPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::System.Collections.ObjectModel.ObservableCollection<global::MyerList.Model.ToDo> obj = sender as global::System.Collections.ObjectModel.ObservableCollection<global::MyerList.Model.ToDo>;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                        }
                        else
                        {
                            switch (propName)
                            {
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        ReleaseAllListeners();
                    }
                }
                public void CollectionChanged_MainVM_CurrrentDisplayToDos(object sender, global::System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    MainPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        global::System.Collections.ObjectModel.ObservableCollection<global::MyerList.Model.ToDo> obj = sender as global::System.Collections.ObjectModel.ObservableCollection<global::MyerList.Model.ToDo>;
                    }
                    else
                    {
                        ReleaseAllListeners();
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2:
                {
                    this.AddStory = (global::Windows.UI.Xaml.Media.Animation.Storyboard)(target);
                }
                break;
            case 3:
                {
                    this.RemoveStory = (global::Windows.UI.Xaml.Media.Animation.Storyboard)(target);
                }
                break;
            case 4:
                {
                    this.HamInStory = (global::Windows.UI.Xaml.Media.Animation.Storyboard)(target);
                }
                break;
            case 5:
                {
                    this.HamOutStory = (global::Windows.UI.Xaml.Media.Animation.Storyboard)(target);
                }
                break;
            case 7:
                {
                    this.TileControl = (global::MyerList.UC.LiveTileTemplate)(target);
                }
                break;
            case 8:
                {
                    global::Windows.UI.Xaml.Controls.Grid element8 = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    #line 240 "..\..\..\View\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element8).ManipulationDelta += this.Grid_ManipulationDelta;
                    #line 240 "..\..\..\View\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element8).ManipulationStarted += this.Grid_ManipulationStarted;
                    #line default
                }
                break;
            case 9:
                {
                    this.BottomCommandBar = (global::Windows.UI.Xaml.Controls.CommandBar)(target);
                }
                break;
            case 10:
                {
                    this.MaskBorder = (global::Windows.UI.Xaml.Controls.Border)(target);
                    #line 298 "..\..\..\View\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Border)this.MaskBorder).Tapped += this.MaskBorder_Tapped;
                    #line default
                }
                break;
            case 11:
                {
                    this.AddingPane = (global::MyerList.UC.AddingPane)(target);
                }
                break;
            case 12:
                {
                    this.Drawer = (global::MyerList.UC.NavigationDrawer)(target);
                }
                break;
            case 13:
                {
                    this.ToastControl = (global::MyerList.UC.ToastUC)(target);
                }
                break;
            case 14:
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element14 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 292 "..\..\..\View\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element14).Click += this.AddClick;
                    #line default
                }
                break;
            case 17:
                {
                    this.pivot = (global::Windows.UI.Xaml.Controls.Pivot)(target);
                }
                break;
            case 20:
                {
                    this.Listview = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            case 21:
                {
                    this.stackPanel1 = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 22:
                {
                    global::Windows.UI.Xaml.Controls.Button element22 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 198 "..\..\..\View\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element22).Click += this.HamClick;
                    #line default
                }
                break;
            case 30:
                {
                    this.border = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 31:
                {
                    this.border1 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 32:
                {
                    this.border2 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1:
                {
                    global::MyerList.Base.BindablePage element1 = (global::MyerList.Base.BindablePage)target;
                    MainPage_obj1_Bindings bindings = new MainPage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            }
            return returnValue;
        }
    }
}

