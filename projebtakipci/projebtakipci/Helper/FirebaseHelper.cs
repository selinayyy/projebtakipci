using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using projebtakipci.model;

namespace projebtakipci.Helper
{
     class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient("https://xamarinfirebase-4637e-default-rtdb.firebaseio.com/");


        public async Task<List<Person>> GetAllPersons()
        {

            return (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Select(item => new Person
              {
                  kullanici_email=item.Object.kullanici_email,
                  kullanici_sifresi = item.Object.kullanici_sifresi,
                  kullanici_id=item.Object.kullanici_id
              }).ToList();
        }

        public async Task AddPerson(int personId, string email, string sifre, DateTime date)
        {

            await firebase
              .Child("Persons")
              .PostAsync(new Person() { kullanici_id = personId, kullanici_email = email ,kullanici_sifresi=sifre , kullanici_date=date});
        }

        public async Task<Person> GetPerson(string mail)
        {
            var allPersons = await GetAllPersons();
            await firebase
              .Child("Persons")
              .OnceAsync<Person>();
            return allPersons.Where(a => a.kullanici_email == mail).FirstOrDefault();
        }

        public async Task UpdatePerson(int personId, string email)
        {
            var toUpdatePerson = (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Where(a => a.Object.kullanici_id == personId).FirstOrDefault();

            await firebase
              .Child("Persons")
              .Child(toUpdatePerson.Key)
              .PutAsync(new Person() { kullanici_id = personId, kullanici_email = email });
        }

        public async Task DeletePerson(int personId)
        {
            var toDeletePerson = (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Where(a => a.Object.kullanici_id == personId).FirstOrDefault();
            await firebase.Child("Persons").Child(toDeletePerson.Key).DeleteAsync();

        }


        public async Task<List<Duty>> GetAllDuties()
        {

            return (await firebase
              .Child("Duties")
              .OnceAsync<Duty>()).Select(item => new Duty
              {
                  duty_id = item.Object.duty_id,
                  duty_make = item.Object.duty_make,
                  duty_text = item.Object.duty_text,
                  kullanici_id = item.Object.kullanici_id
              }).ToList();
        }

        public async Task<List<Duty>> GetDuty(int  id)
        {
            var allDuties = await GetAllDuties();
            List<Duty> Duties = new List<Duty>();
            foreach (var item in allDuties)
            {
                if (item.kullanici_id == id)
                    Duties.Add(item);
            }
            return Duties;
        }

        public async Task UpdateDuty(int duty_id, bool duty_make)
        {
            var toUpdateDuty = (await firebase
              .Child("Duties")
              .OnceAsync<Duty>()).Where(a => a.Object.duty_id == duty_id).FirstOrDefault();

            await firebase
              .Child("Duties")
              .Child(toUpdateDuty.Key)
              .PutAsync(new Duty() { duty_id = duty_id, duty_make = duty_make, duty_text = ((Duty)toUpdateDuty.Object).duty_text , kullanici_id = ((Duty)toUpdateDuty.Object).kullanici_id });
        }

        public async Task UpdateLastLogin(Person person)
        {
            var toUpdateLogin = (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Where(a => a.Object.kullanici_id == person.kullanici_id).FirstOrDefault();

            await firebase
              .Child("Persons")
              .Child(toUpdateLogin.Key)
              .PutAsync(new Person() { kullanici_id = person.kullanici_id, kullanici_date = DateTime.Now, kullanici_email = person.kullanici_email, kullanici_sifresi=person.kullanici_sifresi });
        }
    }


}
