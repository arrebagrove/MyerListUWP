﻿#pragma checksum "D:\Dev\apps\MyerListUWP\MyerListUWP\View\SettingPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DC6BE83AF6B8C0DCE2C8ED1CACC1626C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyerList
{
    partial class SettingPage : 
        global::MyerList.Base.BindablePage, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        internal class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_ToggleSwitch_IsOn(global::Windows.UI.Xaml.Controls.ToggleSwitch obj, global::System.Boolean value)
            {
                obj.IsOn = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_Primitives_Selector_SelectedIndex(global::Windows.UI.Xaml.Controls.Primitives.Selector obj, global::System.Int32 value)
            {
                obj.SelectedIndex = value;
            }
        };

        private class SettingPage_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            ISettingPage_Bindings
        {
            private global::MyerList.SettingPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.ToggleSwitch obj2;
            private global::Windows.UI.Xaml.Controls.ToggleSwitch obj3;
            private global::Windows.UI.Xaml.Controls.ToggleSwitch obj4;
            private global::Windows.UI.Xaml.Controls.ToggleSwitch obj5;
            private global::Windows.UI.Xaml.Controls.ComboBox obj6;
            private global::Windows.UI.Xaml.Controls.ComboBox obj7;
            private global::Windows.UI.Xaml.Controls.ComboBox obj8;

            private SettingPage_obj1_BindingsTracking bindingsTracking;

            public SettingPage_obj1_Bindings()
            {
                this.bindingsTracking = new SettingPage_obj1_BindingsTracking(this);
            }

            ~SettingPage_obj1_Bindings()
            {
                StopTracking();
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 2:
                        this.obj2 = (global::Windows.UI.Xaml.Controls.ToggleSwitch)target;
                        (this.obj2).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.ToggleSwitch.IsOnProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                                if (this.initialized)
                                {
                                    // Update Two Way binding
                                    this.dataRoot.SettingVM.EnableGesture = (this.obj2).IsOn;
                                }
                            });
                        break;
                    case 3:
                        this.obj3 = (global::Windows.UI.Xaml.Controls.ToggleSwitch)target;
                        (this.obj3).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.ToggleSwitch.IsOnProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                                if (this.initialized)
                                {
                                    // Update Two Way binding
                                    this.dataRoot.SettingVM.ShowKeyboard = (this.obj3).IsOn;
                                }
                            });
                        break;
                    case 4:
                        this.obj4 = (global::Windows.UI.Xaml.Controls.ToggleSwitch)target;
                        (this.obj4).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.ToggleSwitch.IsOnProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                                if (this.initialized)
                                {
                                    // Update Two Way binding
                                    this.dataRoot.SettingVM.IsAddToBottom = (this.obj4).IsOn;
                                }
                            });
                        break;
                    case 5:
                        this.obj5 = (global::Windows.UI.Xaml.Controls.ToggleSwitch)target;
                        (this.obj5).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.ToggleSwitch.IsOnProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                                if (this.initialized)
                                {
                                    // Update Two Way binding
                                    this.dataRoot.SettingVM.EnableTile = (this.obj5).IsOn;
                                }
                            });
                        break;
                    case 6:
                        this.obj6 = (global::Windows.UI.Xaml.Controls.ComboBox)target;
                        (this.obj6).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.Primitives.Selector.SelectedIndexProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                                if (this.initialized)
                                {
                                    // Update Two Way binding
                                    this.dataRoot.SettingVM.CurrentLanguage = (this.obj6).SelectedIndex;
                                }
                            });
                        break;
                    case 7:
                        this.obj7 = (global::Windows.UI.Xaml.Controls.ComboBox)target;
                        (this.obj7).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.Primitives.Selector.SelectedIndexProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                                if (this.initialized)
                                {
                                    // Update Two Way binding
                                    this.dataRoot.SettingVM.CurrentLanguage = (this.obj7).SelectedIndex;
                                }
                            });
                        break;
                    case 8:
                        this.obj8 = (global::Windows.UI.Xaml.Controls.ComboBox)target;
                        (this.obj8).RegisterPropertyChangedCallback(global::Windows.UI.Xaml.Controls.Primitives.Selector.SelectedIndexProperty,
                            (global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop) =>
                            {
                                if (this.initialized)
                                {
                                    // Update Two Way binding
                                    this.dataRoot.SettingVM.CurrentLanguage = (this.obj8).SelectedIndex;
                                }
                            });
                        break;
                    default:
                        break;
                }
            }

            // ISettingPage_Bindings

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

            // SettingPage_obj1_Bindings

            public void SetDataRoot(global::MyerList.SettingPage newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.dataRoot = newDataRoot;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::MyerList.SettingPage obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_SettingVM(obj.SettingVM, phase);
                    }
                }
            }
            private void Update_SettingVM(global::MyerList.ViewModel.SettingPageViewModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_SettingVM(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_SettingVM_EnableGesture(obj.EnableGesture, phase);
                        this.Update_SettingVM_ShowKeyboard(obj.ShowKeyboard, phase);
                        this.Update_SettingVM_IsAddToBottom(obj.IsAddToBottom, phase);
                        this.Update_SettingVM_EnableTile(obj.EnableTile, phase);
                        this.Update_SettingVM_CurrentLanguage(obj.CurrentLanguage, phase);
                    }
                }
            }
            private void Update_SettingVM_EnableGesture(global::System.Boolean obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ToggleSwitch_IsOn(this.obj2, obj);
                }
            }
            private void Update_SettingVM_ShowKeyboard(global::System.Boolean obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ToggleSwitch_IsOn(this.obj3, obj);
                }
            }
            private void Update_SettingVM_IsAddToBottom(global::System.Boolean obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ToggleSwitch_IsOn(this.obj4, obj);
                }
            }
            private void Update_SettingVM_EnableTile(global::System.Boolean obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ToggleSwitch_IsOn(this.obj5, obj);
                }
            }
            private void Update_SettingVM_CurrentLanguage(global::System.Int32 obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_Selector_SelectedIndex(this.obj6, obj);
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_Selector_SelectedIndex(this.obj7, obj);
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_Selector_SelectedIndex(this.obj8, obj);
                }
            }

            private class SettingPage_obj1_BindingsTracking
            {
                global::System.WeakReference<SettingPage_obj1_Bindings> WeakRefToBindingObj; 

                public SettingPage_obj1_BindingsTracking(SettingPage_obj1_Bindings obj)
                {
                    WeakRefToBindingObj = new global::System.WeakReference<SettingPage_obj1_Bindings>(obj);
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_SettingVM(null);
                }

                public void PropertyChanged_SettingVM(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    SettingPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::MyerList.ViewModel.SettingPageViewModel obj = sender as global::MyerList.ViewModel.SettingPageViewModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                    bindings.Update_SettingVM_EnableGesture(obj.EnableGesture, DATA_CHANGED);
                                    bindings.Update_SettingVM_ShowKeyboard(obj.ShowKeyboard, DATA_CHANGED);
                                    bindings.Update_SettingVM_IsAddToBottom(obj.IsAddToBottom, DATA_CHANGED);
                                    bindings.Update_SettingVM_EnableTile(obj.EnableTile, DATA_CHANGED);
                                    bindings.Update_SettingVM_CurrentLanguage(obj.CurrentLanguage, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "EnableGesture" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_SettingVM_EnableGesture(obj.EnableGesture, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "ShowKeyboard" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_SettingVM_ShowKeyboard(obj.ShowKeyboard, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "IsAddToBottom" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_SettingVM_IsAddToBottom(obj.IsAddToBottom, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "EnableTile" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_SettingVM_EnableTile(obj.EnableTile, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "CurrentLanguage" :
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_SettingVM_CurrentLanguage(obj.CurrentLanguage, DATA_CHANGED);
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
                private global::MyerList.ViewModel.SettingPageViewModel cache_SettingVM = null;
                public void UpdateChildListeners_SettingVM(global::MyerList.ViewModel.SettingPageViewModel obj)
                {
                    if (obj != cache_SettingVM)
                    {
                        if (cache_SettingVM != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_SettingVM).PropertyChanged -= PropertyChanged_SettingVM;
                            cache_SettingVM = null;
                        }
                        if (obj != null)
                        {
                            cache_SettingVM = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_SettingVM;
                        }
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
                    SettingPage_obj1_Bindings bindings = new SettingPage_obj1_Bindings();
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

