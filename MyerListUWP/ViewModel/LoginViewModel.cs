﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JP.Utils.Data;
using JP.Utils.Debug;
using JP.Utils.Functions;
using JP.Utils.Network;
using MyerList.Helper;
using MyerList.Interface;
using MyerList.Model;
using MyerListUWP;
using MyerListUWP.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyerList.ViewModel
{
    public class LoginViewModel:ViewModelBase, INavigable
    {
        private LoginMode LOGINMODE;

        /// <summary>
        /// Login or Register
        /// </summary>
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                    _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        /// <summary>
        /// Register or login
        /// </summary>
        private string _btnContent;
        public string BtnContent
        {
            get
            {
                return _btnContent;
            }
            set
            {
                if (_btnContent != value)
                    _btnContent = value;
                RaisePropertyChanged(() => BtnContent);
            }
        }

        /// <summary>
        /// Show register btn
        /// </summary>
        private Visibility _showregister;
        public Visibility ShowRegister
        {
            get
            {
                return _showregister;
            }
            set
            {
                _showregister = value;
                RaisePropertyChanged(() => ShowRegister);
            }
        }

        /// <summary>
        /// For Progress Bar
        /// </summary>
        private Visibility _isLoading;
        public Visibility IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        #region privacy info.
        private string _tempemail;
        public string TempEmail
        {
            get
            {
                return _tempemail;
            }
            set
            {
                if (_tempemail != value)
                    _tempemail = value;
                RaisePropertyChanged(() => TempEmail);
            }
        }

        private string _inputpassword;
        public string InputPassword
        {
            get
            {
                return _inputpassword;
            }
            set
            {
                if (_inputpassword != value)
                    _inputpassword = value;
                RaisePropertyChanged(() => InputPassword);
            }
        }

        private string _confirmpassword;
        public string ConfirmPassword
        {
            get
            {
                return _confirmpassword;
            }
            set
            {
                if (_confirmpassword != value)
                    _confirmpassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }
        #endregion

        /// <summary>
        /// When press the register btn
        /// </summary>
        private RelayCommand _nextCommand;
        public RelayCommand NextCommand
        {
            get
            {
                if (_nextCommand != null) return _nextCommand;
                return _nextCommand = new RelayCommand(async () =>
                {
                    IsLoading = Visibility.Visible;
                    try
                    {
                        var loader = new ResourceLoader();

                        if (string.IsNullOrEmpty(TempEmail) || string.IsNullOrEmpty(InputPassword))
                        {
                            Messenger.Default.Send(new GenericMessage<string>(loader.GetString("InputAlert")), "toast");
                            
                            IsLoading = Visibility.Collapsed;
                            return;
                        }

                        if (!Functions.IsValidEmail(TempEmail))
                        {
                            Messenger.Default.Send(new GenericMessage<string>(loader.GetString("EmailInvaild")), "toast");

                            IsLoading = Visibility.Collapsed;
                            return;
                        }

                        if (LOGINMODE==LoginMode.Register)
                        {
                           
                            if (InputPassword != ConfirmPassword)
                            {
                                Messenger.Default.Send(new GenericMessage<string>(loader.GetString("PasswordInvaild")), "toast");

                                IsLoading = Visibility.Collapsed;
                                return;
                            }
                            var isRegisterSuccessfully = await Register();
                            if (!isRegisterSuccessfully)
                            {
                                Messenger.Default.Send(new GenericMessage<string>(loader.GetString("AccountExist")), "toast");

                                IsLoading = Visibility.Collapsed;
                            }
                            else
                            {
                                var isLogin = await Login();
                                if(isLogin)
                                {
                                    Frame rootframe = Window.Current.Content as Frame;
                                    if (rootframe != null) rootframe.Navigate(typeof(MainPage), LoginMode.Login);
                                }
                            }
                        }
                        else if(LOGINMODE == LoginMode.Login)
                        {
                           
                            var isLoginSuccessfylly = await Login();
                            if (isLoginSuccessfylly)
                            {
                                Frame rootframe = Window.Current.Content as Frame;
                                if (rootframe != null) rootframe.Navigate(typeof(MainPage),LoginMode.Login);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var task = ExceptionHelper.WriteRecord(e);
                    }
                });
            }
        }

        /// <summary>
        /// For Windows, navigate back
        /// </summary>
        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get
            {
                if (_backCommand != null)
                {
                    return _backCommand;
                }
                return _backCommand = new RelayCommand(() =>
                {
                    Frame rootframe = Window.Current.Content as Frame;
                    if (rootframe != null && rootframe.CanGoBack)
                    {
                        rootframe.GoBack();
                    }
                });
            }
        }

        public LoginViewModel()
        {
            IsLoading = Visibility.Collapsed;

            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.PressEnterToLoginToken, act =>
            {
                NextCommand.Execute(null);
            });
        }

        private async Task<bool> Register()
        {
            try
            {
                //注册
                IsLoading = Visibility.Visible;

                var loader = new ResourceLoader();

                var check = await PostHelper.CheckExist(TempEmail);
                if (check)
                {
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(loader.GetString("EmailExistContent")), "toast");

                    IsLoading = Visibility.Collapsed;
                    return false;
                }
                string salt = await PostHelper.Register(TempEmail, InputPassword);
                if (!String.IsNullOrEmpty(salt))
                {
                    LocalSettingHelper.AddValue("email", TempEmail);
                    LocalSettingHelper.AddValue("password", InputPassword);
                    return true;
                }
                else
                {
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(loader.GetString("RegisterFailedContent")), "toast");

                    IsLoading = Visibility.Collapsed;
                    return false;
                }
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
                return false;
            }

        }

        private async Task<bool> Login()
        {
            try
            {

                var loader = new ResourceLoader();

                IsLoading = Visibility.Visible;

                var check = await PostHelper.CheckExist(TempEmail);
                if (check)
                {
                    string salt = await PostHelper.GetSalt(TempEmail);
                    if (!String.IsNullOrEmpty(salt))
                    {
                        //尝试登录
                        var login = await PostHelper.Login(TempEmail, InputPassword, salt);
                        if (login)
                        {
                            App.isInOfflineMode = false;

                            LocalSettingHelper.AddValue("OfflineMode", "false");
                            return true;
                        }
                        else
                        {
                            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(loader.GetString("NotCorrectContent")), "toast");

                            IsLoading = Visibility.Collapsed;
                            return false;
                        }
                    }
                    else
                    {
                        Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(loader.GetString("NotCorrectContent")), "toast");

                        IsLoading = Visibility.Collapsed;
                        return false;
                    }

                }
                else
                {
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(loader.GetString("AccountNotExistContent")), "toast");

                    IsLoading = Visibility.Collapsed;
                    return false;
                }
            }
            catch (Exception e)
            {
                var task = ExceptionHelper.WriteRecord(e);
                return false;
            }

        }

        public void Activate(object param)
        {
            if(param is LoginMode)
            {
                var mode = (LoginMode)param;
                LOGINMODE = mode;

                switch (LOGINMODE)
                {
                    case LoginMode.Login:
                        {
                            ShowRegister = Visibility.Collapsed;

                            var loader = new ResourceLoader();
                            Title = loader.GetString("Login");
                            BtnContent = loader.GetString("Login");
                        }; break;
                    case LoginMode.Register:
                        {
                            ShowRegister = Visibility.Visible;

                            var loader = new ResourceLoader();
                            Title = loader.GetString("Register");
                            BtnContent = loader.GetString("Register");

                        }; break;
                }
            }
        }

        public void Deactivate(object param)
        {
            TempEmail = "";
            InputPassword = "";
            ConfirmPassword = "";
            IsLoading = Visibility.Collapsed;
        }
    }
}
