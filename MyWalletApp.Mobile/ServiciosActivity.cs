﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyWalletApp.Mobile.Fragments;

namespace MyWalletApp.Mobile
{
    [Activity(Label = "Servicios")]
    public class ServiciosActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Servicios);

            // Add tabs
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            //ActionBar.SetIcon(Resource.Drawable.icon);
            //ActionBar.SetDisplayShowHomeEnabled(true);
            //ActionBar.DisplayOptions = ActionBarDisplayOptions.HomeAsUp | ActionBarDisplayOptions.ShowTitle | ActionBarDisplayOptions.ShowHome;


            AddTab("Agregar", 0, new ServiciosAgregarFragment());
            AddTab("Buscar", 0, new ServiciosBuscarFragment());
        }

        private void AddTab(string text, int iconId, Fragment view)
        {
            var tab = ActionBar.NewTab();
            tab.SetText(text);
            //tab.SetIcon(iconId);

            tab.TabSelected += (sender, e) =>
            {
                var fragment = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };

            tab.TabUnselected += (sender, e) =>
            {
                e.FragmentTransaction.Remove(view);
            };

            ActionBar.AddTab(tab);
        }
    }
}