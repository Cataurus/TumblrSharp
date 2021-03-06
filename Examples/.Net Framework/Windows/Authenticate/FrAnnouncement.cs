﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using DontPanic.TumblrSharp.OAuth;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using Common;
using System.Diagnostics.CodeAnalysis;

namespace Authenticate
{
    public partial class FrAuthenticate : Form
    {
        private string callbackUrl = "www.test.de";

        private ServiceHost host;

        private OAuthClient oAuthClient;

        [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed")]
        private TumblrClient tumblrClient;

        private TumblrUrlProtocol tumblrUrlProtocol;

        public FrAuthenticate(ServiceHost host)
        {
            InitializeComponent();

            tumblrClient = null;

            this.host = host;
        }

        private void BtnRegistrationProtocol_Click(object sender, EventArgs e)
        {
            // registration protocol
            tumblrUrlProtocol = new TumblrUrlProtocol(eProtocolname.Text, Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        {

            // create the oAuth-Client
            var lFactory = new OAuthClientFactory();

            oAuthClient = lFactory.Create(eConsumerKey.Text, eConsumerSecret.Text);

            // get the requesttoken
            Token requestToken;

            try
            {
                requestToken = await oAuthClient.GetRequestTokenAsync(eProtocolname.Text + "://" + callbackUrl);
            }
            catch (OAuthException ex)
            {
                MessageBox.Show(ex.Message);

                return;
            }
            
            eRequestKey.Text = requestToken.Key;
            eRequestSecret.Text = requestToken.Secret;

            // get the url for authorization
            var lAuthorizeUrl = oAuthClient.GetAuthorizeUrl(requestToken);

            // starting authorization
            System.Diagnostics.Process.Start(lAuthorizeUrl.AbsoluteUri);

            // start host for listen
            host.Open();
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            // create TumblrClient
            tumblrClient = new TumblrClientFactory().Create<TumblrClient>(eConsumerKey.Text, eConsumerSecret.Text, new Token(eAccessKey.Text, eAccessSecret.Text));

            // Queries user info
            var userInfo = await tumblrClient.GetUserInfoAsync();
            
            MessageBox.Show("Number of Blogs is you following: " + userInfo.FollowingCount.ToString());
        }

        // this is methode from wiki for
        // [SomeFlagThatTellsThisMethodWhatToOpen]
        internal async void SetAccessUrl(string accessUrl)
        {
            // get the accesstoken
            var accessToken = await oAuthClient.GetAccessTokenAsync(new Token(eRequestKey.Text, eRequestSecret.Text), accessUrl);

            eAccessKey.Text = accessToken.Key;
            eAccessSecret.Text = accessToken.Secret;

            // close host for listen
            host.Close();
        }

        private void FrAuthenticate_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tumblrClient != null)
            {
                tumblrClient.Dispose();

                tumblrUrlProtocol.Remove();
            }
        }
    }
}
