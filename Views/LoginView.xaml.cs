﻿using System;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApDownloader.UI.Views;

public partial class LoginView : UserControl
{
    private const string LoginUrl = @"https://www.armstrongpowerhouse.com/index.php?route=account/login";
    private const string LogoutUrl = @"https://www.armstrongpowerhouse.com/index.php?route=account/logout";
    private static readonly HttpClientHandler Handler = new() {AllowAutoRedirect = false};

    public LoginView()
    {
        InitializeComponent();
        Client = new HttpClient(Handler);
        Client.Timeout = TimeSpan.FromMinutes(60);
        Loaded += Login_Loaded;
    }

    public static bool IsLoggedIn { get; private set; }

    public static HttpClient? Client { get; private set; } = new(Handler);

    private void Login_Loaded(object sender, RoutedEventArgs e)
    {
        LoginButton.IsEnabled = !IsLoggedIn;
        LogoutButton.IsEnabled = IsLoggedIn;
    }

    private async void Login(object sender, RoutedEventArgs e)
    {
        LoginResult.Text = "Attempting Login";
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(EmailBox.Text), "email");
        content.Add(new StringContent(PasswordBoxName.Password), "password");
        try
        {
            var response = await Client.PostAsync(LoginUrl, content);
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                LoginResult.Text = "Retrieving Purchased Addons";

                LoginResult.Text = "Login Successful";
                LoginButton.IsEnabled = false;
                LogoutButton.IsEnabled = true;
                IsLoggedIn = true;
            }
            else
            {
                LoginResult.Text = "Login Failed";
                LoginButton.IsEnabled = true;
                LogoutButton.IsEnabled = false;
                IsLoggedIn = false;
            }
        }
        catch (HttpRequestException exception)
        {
            LoginResult.Text = "Unable to connect to website";
            LoginButton.IsEnabled = true;
            LogoutButton.IsEnabled = false;
            IsLoggedIn = false;
        }
    }

    private void Logout(object sender, RoutedEventArgs e)
    {
        Client.GetAsync(LogoutUrl);
        LoginButton.IsEnabled = true;
        LogoutButton.IsEnabled = false;
        LoginResult.Text = "Logged out";
        IsLoggedIn = false;
    }

    private void OnEnterKeyHandler(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return)
            Login(sender, e);
    }
}