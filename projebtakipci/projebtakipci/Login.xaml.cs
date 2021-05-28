using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using projebtakipci.Helper;
using projebtakipci.model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace projebtakipci
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        FirebaseHelper firebaseHelper =new FirebaseHelper();
        

        public Login()
        {
            InitializeComponent();
        }

        private async void btn_giris_Clicked(object sender, EventArgs e)
        {
            var person = await firebaseHelper.GetPerson(txt_mail.Text);
            if (person != null)
            {
                if (person.kullanici_sifresi == txt_sifre.Text)
                {
                    DateTime a = DateTime.Now;
                    DateTime b = Convert.ToDateTime(person.kullanici_date);
                    List<Duty> Duties = await firebaseHelper.GetDuty(person.kullanici_id);
                    
                    if (a.Subtract(b).TotalDays >= 1)
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress("projebtakipci@gmail.com");
                        mail.To.Add("projebtakipci@gmail.com");
                        mail.Subject = "Yapılan Aktiviteler";
                        mail.Body = "Kullanıcı ID: " + person.kullanici_id + "\n";
                        foreach (var item in Duties)
                        {
                            string temp = item.duty_make == true ? "Yapıldı" : "Yapılmadı";
                            mail.Body += "Görev İsmi :  " + item.duty_text + " Görev yapılma durumu:   "+ temp + "\n";

                        }

                        SmtpServer.Port = 587;
                        SmtpServer.Host = "smtp.gmail.com";
                        SmtpServer.EnableSsl = true;
                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("projebtakipci@gmail.com", "4767199Ev");

                        SmtpServer.Send(mail);
                    }
                    await firebaseHelper.UpdateLastLogin(person);
                    await DisplayAlert("Başarılı", "Kullanıcı girişi başarılı", "OK");
                    Application.Current.Properties["id"] = person.kullanici_id;
                    Application.Current.MainPage = new Home();

                }
                else
                {
                    await DisplayAlert("Başarısız", "şifre yanlış", "OK");

                }

            }
            else
            {
                await DisplayAlert("Hata", "Kayıtlı kullanıcı yok", "OK");
            }
        }

        private async void btn_sifreunut_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Şifremi Unuttum", "E-mail girin.");
            if (result != null)
            {
                var person = await firebaseHelper.GetPerson(result);
                if (person != null)
                {
                    try
                    {

                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress("projebtakipci@gmail.com");
                        mail.To.Add(result);
                        mail.Subject = "Şifremi Unuttum";
                        mail.Body ="Şifreniz: "+ person.kullanici_sifresi;

                        SmtpServer.Port = 587;
                        SmtpServer.Host = "smtp.gmail.com";
                        SmtpServer.EnableSsl = true;
                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("projebtakipci@gmail.com", "4767199Ev");

                        SmtpServer.Send(mail);
                        await DisplayAlert("Başarılı", "Mail Gönderme Başarılı", "OK");

                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Hata", "Mail Gönderme Hatası", "OK");
                    }

                }
                else
                {
                    await DisplayAlert("KUllanıcı Yok", "Kayıtlı kullanıcı yok", "OK");
                }
            }
        }
    }
}