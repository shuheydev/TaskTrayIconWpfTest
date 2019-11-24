using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace TaskTrayIconWpfTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        HubConnection connection;

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            NotifyIcon icon = new NotifyIcon();

            icon.Icon = new System.Drawing.Icon("./test.ico");
            icon.Visible = true;
            icon.Text = "My Icon";

            //Menuのインスタンス化
            var menu = new ContextMenuStrip();

            //MenuItemの作成
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "&Exit";
            menuItem.Click += (s, e) =>
            {
                System.Windows.Application.Current.Shutdown();
            };

            //MenuにMenuItemを追加
            menu.Items.Add(menuItem);

            //Menuをタスクトレイのアイコンに追加
            icon.ContextMenuStrip = menu;

            //icon.BalloonTipText = "Hello from My Kitten";
            //icon.BalloonTipTitle = "Cat Talk";
            //icon.BalloonTipIcon = ToolTipIcon.Info;
            //icon.ShowBalloonTip(2000);

            connection = new HubConnectionBuilder()
                .WithUrl(@"https://signalrcorepractice20191006060526.azurewebsites.net/chathub")//サーバー側でMapHubで指定したURLを指定する.
                .WithAutomaticReconnect()
                .Build();

            connection.On<string, string>("Receive", (message, from) =>
            {
                icon.BalloonTipTitle = from;
                icon.BalloonTipText = message;
                icon.ShowBalloonTip(2000);

                switch (message)
                {
                    case "next":
                        SendKeys.SendWait("{RIGHT}");
                        break;
                    case "prev":
                        SendKeys.SendWait("{LEFT}");
                        break;
                    default:
                        break;
                }
            });

            await connection.StartAsync();
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
