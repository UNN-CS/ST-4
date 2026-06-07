using System;
using System.Collections.Generic;
using System.Linq;
using Stateless;

namespace BugPro
{
    public class Oshibka
    {
        public enum Sostoyanie
        {
            Novaya, Razbor, V_Rabote, Ozhidaet_Info, Otlozhena, Proverena, Gotova_K_Relizu, Zakryta, Pereotkryta
        }

        public enum Deystvie
        {
            Nachat_Razbor, Naznachat_Deva, Otmelit_Ispravleno, Zaprosit_Info, Predostavit_Info,
            Otlozhit, Vozobnovit, Proverka_OK, Proverka_Fail, Pereotkryt, Zakryt, Vypustit
        }

        private readonly StateMachine<Sostoyanie, Deystvie> _mashina_sostoyaniy;
        private readonly List<string> _istoriya;

        public string Id { get; }
        public string Zagolovok { get; }
        public int Prioritet { get; }
        public Sostoyanie Tekushchee_Sostoyanie => _mashina_sostoyaniy.State;
        public IReadOnlyList<string> Istoriya => _istoriya.AsReadOnly();

        public Oshibka(string id, string zagolovok, int prioritet = 3)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Zagolovok = zagolovok ?? throw new ArgumentNullException(nameof(zagolovok));
            Prioritet = prioritet;
            _istoriya = new List<string>();

            _mashina_sostoyaniy = new StateMachine<Sostoyanie, Deystvie>(Sostoyanie.Novaya);
            Nastroit_Rabochiy_Protsess();
            Zapisat_V_Istoriyu($"Sozdano: {Id} - {Zagolovok}");
        }

        private void Nastroit_Rabochiy_Protsess()
        {
            _mashina_sostoyaniy.Configure(Sostoyanie.Novaya)
                .Permit(Deystvie.Nachat_Razbor, Sostoyanie.Razbor)
                .OnEntry(() => Zapisat_V_Istoriyu("Voshli_v_Novaya"));

            _mashina_sostoyaniy.Configure(Sostoyanie.Razbor)
                .Permit(Deystvie.Naznachat_Deva, Sostoyanie.V_Rabote)
                .Permit(Deystvie.Otlozhit, Sostoyanie.Otlozhena)
                .Permit(Deystvie.Zakryt, Sostoyanie.Zakryta)
                .Ignore(Deystvie.Zaprosit_Info)
                .Ignore(Deystvie.Nachat_Razbor)
                .OnEntry(() => Zapisat_V_Istoriyu("Voshli_v_Razbor"));

            _mashina_sostoyaniy.Configure(Sostoyanie.V_Rabote)
                .Permit(Deystvie.Otmelit_Ispravleno, Sostoyanie.Proverena)
                .Permit(Deystvie.Zaprosit_Info, Sostoyanie.Ozhidaet_Info)
                .Permit(Deystvie.Otlozhit, Sostoyanie.Otlozhena)
                .OnEntry(() => Zapisat_V_Istoriyu("Voshli_v_V_Rabote"));

            _mashina_sostoyaniy.Configure(Sostoyanie.Ozhidaet_Info)
                .Permit(Deystvie.Predostavit_Info, Sostoyanie.V_Rabote)
                .OnEntry(() => Zapisat_V_Istoriyu("Voshli_v_Ozhidaet_Info"));

            _mashina_sostoyaniy.Configure(Sostoyanie.Otlozhena)
                .Permit(Deystvie.Vozobnovit, Sostoyanie.V_Rabote)
                .OnEntry(() => Zapisat_V_Istoriyu("Voshli_v_Otlozhena"));

            _mashina_sostoyaniy.Configure(Sostoyanie.Proverena)
                .Permit(Deystvie.Proverka_OK, Sostoyanie.Gotova_K_Relizu)
                .Permit(Deystvie.Proverka_Fail, Sostoyanie.V_Rabote)
                .OnEntry(() => Zapisat_V_Istoriyu("Voshli_v_Proverena"));

            _mashina_sostoyaniy.Configure(Sostoyanie.Gotova_K_Relizu)
                .Permit(Deystvie.Vypustit, Sostoyanie.Zakryta)
                .OnEntry(() => Zapisat_V_Istoriyu("Voshli_v_Gotova_K_Relizu"));

            _mashina_sostoyaniy.Configure(Sostoyanie.Zakryta)
                .Permit(Deystvie.Pereotkryt, Sostoyanie.Pereotkryta)
                .Ignore(Deystvie.Vypustit)
                .Ignore(Deystvie.Otmelit_Ispravleno)
                .OnEntry(() => Zapisat_V_Istoriyu("Voshli_v_Zakryta"));

            _mashina_sostoyaniy.Configure(Sostoyanie.Pereotkryta)
                .Permit(Deystvie.Nachat_Razbor, Sostoyanie.Razbor)
                .Ignore(Deystvie.Pereotkryt)
                .OnEntry(() => Zapisat_V_Istoriyu("Voshli_v_Pereotkryta"));

            _mashina_sostoyaniy.OnTransitioned(t =>
                Zapisat_V_Istoriyu($"Perehod: {t.Source} -> {t.Destination} cherez {t.Trigger}"));
        }

        private void Zapisat_V_Istoriyu(string soobshchenie) => _istoriya.Add($"[{DateTime.Now:HH:mm:ss}] {soobshchenie}");

        public void Nachat_Razbor() => _mashina_sostoyaniy.Fire(Deystvie.Nachat_Razbor);
        public void Naznachat_Deva() => _mashina_sostoyaniy.Fire(Deystvie.Naznachat_Deva);
        public void Otmelit_Ispravleno() => _mashina_sostoyaniy.Fire(Deystvie.Otmelit_Ispravleno);
        public void Zaprosit_Info() => _mashina_sostoyaniy.Fire(Deystvie.Zaprosit_Info);
        public void Predostavit_Info() => _mashina_sostoyaniy.Fire(Deystvie.Predostavit_Info);
        public void Otlozhit() => _mashina_sostoyaniy.Fire(Deystvie.Otlozhit);
        public void Vozobnovit() => _mashina_sostoyaniy.Fire(Deystvie.Vozobnovit);
        public void Proverka_OK() => _mashina_sostoyaniy.Fire(Deystvie.Proverka_OK);
        public void Proverka_Fail() => _mashina_sostoyaniy.Fire(Deystvie.Proverka_Fail);
        public void Pereotkryt() => _mashina_sostoyaniy.Fire(Deystvie.Pereotkryt);
        public void Zakryt() => _mashina_sostoyaniy.Fire(Deystvie.Zakryt);
        public void Vypustit() => _mashina_sostoyaniy.Fire(Deystvie.Vypustit);

        public bool Poprobovat_Otmelit_Ispravleno() => Vypolnit_Esli_Dostupno(Deystvie.Otmelit_Ispravleno);
        public bool Poprobovat_Pereotkryt() => Vypolnit_Esli_Dostupno(Deystvie.Pereotkryt);
        public bool Poprobovat_Vypustit() => Vypolnit_Esli_Dostupno(Deystvie.Vypustit);

        private bool Vypolnit_Esli_Dostupno(Deystvie deystvie)
        {
            if (!_mashina_sostoyaniy.CanFire(deystvie)) return false;
            _mashina_sostoyaniy.Fire(deystvie);
            return true;
        }

        public IEnumerable<Deystvie> Poluchit_Dostupnye_Deystviya() => _mashina_sostoyaniy.PermittedTriggers;
        public bool Mozhet_Vypolnit(Deystvie deystvie) => _mashina_sostoyaniy.CanFire(deystvie);
        public bool Nahoditsya_V_Sostoyanii(Sostoyanie ozhidaemoe) => _mashina_sostoyaniy.State == ozhidaemoe;

        public override string ToString() => $"Oshibka#{Id} [{Tekushchee_Sostoyanie}] {Zagolovok}";
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var oshibka = new Oshibka("BUG", "Testovaya oshibka");

            oshibka.Nachat_Razbor();
            oshibka.Naznachat_Deva();
            oshibka.Otmelit_Ispravleno();
            oshibka.Proverka_OK();
            oshibka.Vypustit();

            Console.WriteLine($"Itog: {oshibka.Tekushchee_Sostoyanie}");
        }
    }
}