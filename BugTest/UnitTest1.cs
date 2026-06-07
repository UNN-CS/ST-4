using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        private Oshibka _testovaya_oshibka;

        [TestInitialize]
        public void Podgotovit_Sredu() => _testovaya_oshibka = new Oshibka("TEST", "Testovaya oshibka");

        [TestMethod]
        public void Novaya_Oshibka_Nachalnoe_Sostoyanie_Novaya()
        {
            var novaya = new Oshibka("NEW", "Novaya oshibka");
            Assert.AreEqual(Oshibka.Sostoyanie.Novaya, novaya.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Nachat_Razbor_Iz_Novaya_V_Razbor()
        {
            _testovaya_oshibka.Nachat_Razbor();
            Assert.AreEqual(Oshibka.Sostoyanie.Razbor, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Naznachat_Deva_Iz_Razbor_V_V_Rabote()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            Assert.AreEqual(Oshibka.Sostoyanie.V_Rabote, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Otmelit_Ispravleno_Iz_V_Rabote_V_Proverena()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            _testovaya_oshibka.Otmelit_Ispravleno();
            Assert.AreEqual(Oshibka.Sostoyanie.Proverena, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Proverka_OK_Iz_Proverena_V_Gotova_K_Relizu()
        {
            Nastroit_Proverenu_Oshibku();
            _testovaya_oshibka.Proverka_OK();
            Assert.AreEqual(Oshibka.Sostoyanie.Gotova_K_Relizu, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Vypustit_Iz_Gotova_K_Relizu_V_Zakryta()
        {
            Nastroit_Gotovuyu_K_Relizu();
            _testovaya_oshibka.Vypustit();
            Assert.AreEqual(Oshibka.Sostoyanie.Zakryta, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Zaprosit_Info_Iz_V_Rabote_V_Ozhidaet_Info()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            _testovaya_oshibka.Zaprosit_Info();
            Assert.AreEqual(Oshibka.Sostoyanie.Ozhidaet_Info, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Predostavit_Info_Iz_Ozhidaet_Info_V_V_Rabote()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            _testovaya_oshibka.Zaprosit_Info();
            _testovaya_oshibka.Predostavit_Info();
            Assert.AreEqual(Oshibka.Sostoyanie.V_Rabote, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Otlozhit_Iz_V_Rabote_V_Otlozhena()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            _testovaya_oshibka.Otlozhit();
            Assert.AreEqual(Oshibka.Sostoyanie.Otlozhena, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Vozobnovit_Iz_Otlozhena_V_V_Rabote()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            _testovaya_oshibka.Otlozhit();
            _testovaya_oshibka.Vozobnovit();
            Assert.AreEqual(Oshibka.Sostoyanie.V_Rabote, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Proverka_Fail_Iz_Proverena_V_V_Rabote()
        {
            Nastroit_Proverenu_Oshibku();
            _testovaya_oshibka.Proverka_Fail();
            Assert.AreEqual(Oshibka.Sostoyanie.V_Rabote, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Pereotkryt_Iz_Zakryta_V_Pereotkryta()
        {
            Nastroit_Zakrytuyu_Oshibku();
            _testovaya_oshibka.Pereotkryt();
            Assert.AreEqual(Oshibka.Sostoyanie.Pereotkryta, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Nachat_Razbor_Iz_Pereotkryta_V_Razbor()
        {
            Nastroit_Zakrytuyu_Oshibku();
            _testovaya_oshibka.Pereotkryt();
            _testovaya_oshibka.Nachat_Razbor();
            Assert.AreEqual(Oshibka.Sostoyanie.Razbor, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Ignorirovat_Zaprosit_Info_V_Razbor_Ne_Menyaet_Sostoyanie()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Zaprosit_Info();
            Assert.AreEqual(Oshibka.Sostoyanie.Razbor, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Nedopustimiy_Perehod_Otmelit_Ispravleno_Iz_Novaya_Vibrasyvaet_Iskluchenie()
        {
            bool vibrosilo = false;
            try { _testovaya_oshibka.Otmelit_Ispravleno(); }
            catch (InvalidOperationException) { vibrosilo = true; }
            Assert.IsTrue(vibrosilo);
        }

        [TestMethod]
        public void Nedopustimiy_Perehod_Proverka_OK_Iz_Razbor_Vibrasyvaet_Iskluchenie()
        {
            _testovaya_oshibka.Nachat_Razbor();
            bool vibrosilo = false;
            try { _testovaya_oshibka.Proverka_OK(); }
            catch (InvalidOperationException) { vibrosilo = true; }
            Assert.IsTrue(vibrosilo);
        }

        [TestMethod]
        public void Nedopustimiy_Perehod_Vypustit_Iz_Novaya_Vibrasyvaet_Iskluchenie()
        {
            bool vibrosilo = false;
            try { _testovaya_oshibka.Vypustit(); }
            catch (InvalidOperationException) { vibrosilo = true; }
            Assert.IsTrue(vibrosilo);
        }

        [TestMethod]
        public void Nedopustimiy_Perehod_Pereotkryt_Iz_Razbor_Vibrasyvaet_Iskluchenie()
        {
            _testovaya_oshibka.Nachat_Razbor();
            bool vibrosilo = false;
            try { _testovaya_oshibka.Pereotkryt(); }
            catch (InvalidOperationException) { vibrosilo = true; }
            Assert.IsTrue(vibrosilo);
        }

        [TestMethod]
        public void Nedopustimiy_Perehod_Vozobnovit_Iz_Novaya_Vibrasyvaet_Iskluchenie()
        {
            bool vibrosilo = false;
            try { _testovaya_oshibka.Vozobnovit(); }
            catch (InvalidOperationException) { vibrosilo = true; }
            Assert.IsTrue(vibrosilo);
        }

        [TestMethod]
        public void Nedopustimiy_Perehod_Predostavit_Info_Iz_Novaya_Vibrasyvaet_Iskluchenie()
        {
            bool vibrosilo = false;
            try { _testovaya_oshibka.Predostavit_Info(); }
            catch (InvalidOperationException) { vibrosilo = true; }
            Assert.IsTrue(vibrosilo);
        }

        [TestMethod]
        public void Mozhet_Vypolnit_Dlya_Dopustimogo_Deystviya_Vozvrashchaet_True()
        {
            Assert.IsTrue(_testovaya_oshibka.Mozhet_Vypolnit(Oshibka.Deystvie.Nachat_Razbor));
        }

        [TestMethod]
        public void Mozhet_Vypolnit_Dlya_Nedopustimogo_Deystviya_Vozvrashchaet_False()
        {
            Assert.IsFalse(_testovaya_oshibka.Mozhet_Vypolnit(Oshibka.Deystvie.Otmelit_Ispravleno));
        }

        [TestMethod]
        public void Poluchit_Dostupnye_Deystviya_Nachalnoe_Sostoyanie_Soderzhit_Nachat_Razbor()
        {
            var deystviya = _testovaya_oshibka.Poluchit_Dostupnye_Deystviya().ToList();
            Assert.IsTrue(deystviya.Contains(Oshibka.Deystvie.Nachat_Razbor));
        }

        [TestMethod]
        public void Istoriya_Soderzhit_Zapis_O_Sozdanii()
        {
            Assert.IsTrue(_testovaya_oshibka.Istoriya.Any(z => z.Contains("Sozdano")));
        }

        [TestMethod]
        public void Poprobovat_Otmelit_Ispravleno_Dopustimo_Vozvrashchaet_True_I_Menyaet_Sostoyanie()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            var rezultat = _testovaya_oshibka.Poprobovat_Otmelit_Ispravleno();
            Assert.IsTrue(rezultat);
            Assert.AreEqual(Oshibka.Sostoyanie.Proverena, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Poprobovat_Otmelit_Ispravleno_Nedopustimo_Vozvrashchaet_False_I_Sohranyaet_Sostoyanie()
        {
            var rezultat = _testovaya_oshibka.Poprobovat_Otmelit_Ispravleno();
            Assert.IsFalse(rezultat);
            Assert.AreEqual(Oshibka.Sostoyanie.Novaya, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void ToString_Vozvrashchaet_Formatirovannuyu_Stroku_S_Id_I_Sostoyaniem()
        {
            var result = _testovaya_oshibka.ToString();
            Assert.IsTrue(result.Contains("TEST"));
            Assert.IsTrue(result.Contains("Novaya"));
            Assert.IsTrue(result.Contains("Testovaya oshibka"));
        }

        [TestMethod]
        public void Prioritet_Sohranyaetsya_V_Svoystve()
        {
            var oshibka_s_prioritetom = new Oshibka("PR", "Test", prioritet: 5);
            Assert.AreEqual(5, oshibka_s_prioritetom.Prioritet);
        }

        [TestMethod]
        public void Polniy_Tsik_Novaya_Do_Zakryta_Cherez_Vse_Etapy()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            _testovaya_oshibka.Otmelit_Ispravleno();
            _testovaya_oshibka.Proverka_OK();
            _testovaya_oshibka.Vypustit();
            Assert.AreEqual(Oshibka.Sostoyanie.Zakryta, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Tsikl_Pereotkrytiya_Zakryta_Pereotkryta_Razbor_Zakryta()
        {
            Nastroit_Zakrytuyu_Oshibku();
            _testovaya_oshibka.Pereotkryt();
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            _testovaya_oshibka.Otmelit_Ispravleno();
            _testovaya_oshibka.Proverka_OK();
            _testovaya_oshibka.Vypustit();
            Assert.AreEqual(Oshibka.Sostoyanie.Zakryta, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        [TestMethod]
        public void Poluchit_Dostupnye_Deystviya_V_Sostoyanii_V_Rabote_Soderzhit_Tri_Deystviya()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            var deystviya = _testovaya_oshibka.Poluchit_Dostupnye_Deystviya().ToList();
            Assert.IsTrue(deystviya.Contains(Oshibka.Deystvie.Otmelit_Ispravleno));
            Assert.IsTrue(deystviya.Contains(Oshibka.Deystvie.Zaprosit_Info));
            Assert.IsTrue(deystviya.Contains(Oshibka.Deystvie.Otlozhit));
            Assert.AreEqual(3, deystviya.Count);
        }

        [TestMethod]
        public void Istoriya_Rastet_Pri_Kazhdom_Perehode()
        {
            var nachalnaya_dlina = _testovaya_oshibka.Istoriya.Count;
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            Assert.IsTrue(_testovaya_oshibka.Istoriya.Count > nachalnaya_dlina + 1);
        }

        [TestMethod]
        public void Nahoditsya_V_Sostoyanii_Proveryaet_Tekushchee_Sostoyanie()
        {
            Assert.IsTrue(_testovaya_oshibka.Nahoditsya_V_Sostoyanii(Oshibka.Sostoyanie.Novaya));
            _testovaya_oshibka.Nachat_Razbor();
            Assert.IsFalse(_testovaya_oshibka.Nahoditsya_V_Sostoyanii(Oshibka.Sostoyanie.Novaya));
            Assert.IsTrue(_testovaya_oshibka.Nahoditsya_V_Sostoyanii(Oshibka.Sostoyanie.Razbor));
        }

        [TestMethod]
        public void Zakryt_Iz_Razbor_Pryamoy_Perehod_V_Zakryta()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Zakryt();
            Assert.AreEqual(Oshibka.Sostoyanie.Zakryta, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        private void Nastroit_Proverenu_Oshibku()
        {
            _testovaya_oshibka.Nachat_Razbor();
            _testovaya_oshibka.Naznachat_Deva();
            _testovaya_oshibka.Otmelit_Ispravleno();
            Assert.AreEqual(Oshibka.Sostoyanie.Proverena, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        private void Nastroit_Gotovuyu_K_Relizu()
        {
            Nastroit_Proverenu_Oshibku();
            _testovaya_oshibka.Proverka_OK();
            Assert.AreEqual(Oshibka.Sostoyanie.Gotova_K_Relizu, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }

        private void Nastroit_Zakrytuyu_Oshibku()
        {
            Nastroit_Gotovuyu_K_Relizu();
            _testovaya_oshibka.Vypustit();
            Assert.AreEqual(Oshibka.Sostoyanie.Zakryta, _testovaya_oshibka.Tekushchee_Sostoyanie);
        }
    }
}