using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MyWalletApp.Mobile.Services;
using MyWalletApp.Mobile.Models;

namespace MyWalletApp.Mobile.Fragments.Fuentes
{
    public class FuentesAgregarFragment : Fragment
    {
        private EditText _nombre;
        private Button _btnGuardar;
        private FuenteService _fuenteService;

        public FuentesAgregarFragment()
        {
            _fuenteService = new FuenteService();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            FindViews();
            HandleEvents();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.FuentesAgregarFragment, container, false);
        }

        private void FindViews()
        {
            _nombre = View.FindViewById<EditText>(Resource.Id.txtNombre);
            _btnGuardar = View.FindViewById<Button>(Resource.Id.btnGuardar);
        }

        private void HandleEvents()
        {
            _btnGuardar.Click += _btnGuardar_Click;
        }

        private async void _btnGuardar_Click(object sender, EventArgs e)
        {
            if (!CamposInvalidos())
            {
                try
                {
                    var fuente = new Fuente()
                    {
                        Nombre = _nombre.Text
                    };

                    await _fuenteService.AgregarFuente(fuente);

                    Toast.MakeText(this.Activity, "Se ha agregado la fuente correctamente.", ToastLength.Long).Show();
                }
                catch(Exception ex)
                {
                    Toast.MakeText(this.Activity, ex.Message, ToastLength.Long).Show();
                }
            }
        }

        private bool CamposInvalidos()
        {
            return _nombre.Text.Equals(string.Empty);
        }
    }
}