using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppHotel.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContratacaoHospedagem : ContentPage
    {
        App PropriedadesApp;

        public ContratacaoHospedagem()
        {
            InitializeComponent();

            PropriedadesApp = (App)Application.Current;

            pck_quarto.ItemsSource = PropriedadesApp.tipos_quartos;

            dtpck_data_checkin.MinimumDate = DateTime.Now;
            dtpck_data_checkin.MaximumDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 6, DateTime.Now.Day);

            dtpck_data_checkout.MinimumDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1);
            dtpck_data_checkout.MaximumDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 6, DateTime.Now.Day + 7);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                int qnt_adultos = Convert.ToInt32(lbl_qnt_adultos.Text);
                int qnt_criancas = Convert.ToInt32(lbl_qnt_criancas.Text);

                if (qnt_adultos == 0 && qnt_criancas == 0)
                    throw new Exception("Desculpe, informe pelo menos um adulto ou uma criança.");

                Model.Suite quarto_selecionado = (Model.Suite)pck_quarto.SelectedItem;

                if (quarto_selecionado == null)
                    throw new Exception("Desculpe, selecione um quarto.");

                Model.Hospedagem dados_hospedagem = new Model.Hospedagem()
                {
                    Quarto = quarto_selecionado,

                    QuantidadeAdultos = qnt_adultos,
                    QuantidadeCriancas = qnt_criancas,

                    QuantidadeDias = Model.Hospedagem.CalcularTempoEstadia(dtpck_data_checkin.Date, dtpck_data_checkout.Date),

                    DataCheckIn = dtpck_data_checkin.Date,
                    DataCheckOut = dtpck_data_checkout.Date,
                };

                dados_hospedagem.ValorTotal = dados_hospedagem.CalcularValorEstadia();

                var hospedagem = new HospedagemCalculada();
                hospedagem.BindingContext = dados_hospedagem;

                await Navigation.PushAsync(hospedagem);
            } catch (Exception ex)
            {
                await DisplayAlert("Ops!", ex.Message, "OK");
            }
        }
        private void dtpck_data_checkin_DateSelected(object sender, DateChangedEventArgs e)
        {
            DatePicker elemento = (DatePicker)sender;
            DateTime data_checkin = elemento.Date;

            dtpck_data_checkout.MinimumDate = new DateTime(DateTime.Now.Year, data_checkin.Month, data_checkin.Day + 1);

        }
    }
}