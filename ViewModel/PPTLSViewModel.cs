using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PPTLS.Model;
using PPTLS.Repositori;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Xps.Serialization;

namespace PPTLS.ViewModel
{
    partial class PPTLSViewModel : ObservableObject
    {
        IRepositoriDeJugadors repositoriDeJugadors;
        [NotifyCanExecuteChangedFor(nameof(ComençaCommand))]
        [ObservableProperty]
        Jugador jugador;
        [ObservableProperty]
        string fotoBot;
        [ObservableProperty]
        Jugador bot;
        #region CAMPS
        [ObservableProperty]
        string id;

        [ObservableProperty]
        string nom;

        [ObservableProperty]
        int maxVictories;

        [ObservableProperty]
        int puntuacio;

        [ObservableProperty]
        int partidesGuanyades;

        [ObservableProperty]
        int rondesGuanyades;

        [ObservableProperty]
        int rondesPerdudes;

        [ObservableProperty]
        ObservableCollection<Jugador> jugadors;

        [ObservableProperty]
        ObservableCollection<Jugador> botsGuanyats;
        #endregion

        public PPTLSViewModel()
        {
            repositoriDeJugadors = Repo.ObreBDJugador();
            Jugadors = repositoriDeJugadors.ObtenJugadors();
        }

        [NotifyCanExecuteChangedFor(nameof(ObrePuntuacionsCommand))]
        [NotifyCanExecuteChangedFor(nameof(ObreConfiguracioCommand))]
        [NotifyCanExecuteChangedFor(nameof(ObreJocCommand))]
        [ObservableProperty]
        bool jugant = false;

        [NotifyCanExecuteChangedFor(nameof(ObrePuntuacionsCommand))]
        [NotifyCanExecuteChangedFor(nameof(ObreConfiguracioCommand))]
        [NotifyCanExecuteChangedFor(nameof(ObreJocCommand))]
        [ObservableProperty]
        bool configurant = true;

        [NotifyCanExecuteChangedFor(nameof(ObrePuntuacionsCommand))]
        [NotifyCanExecuteChangedFor(nameof(ObreConfiguracioCommand))]
        [NotifyCanExecuteChangedFor(nameof(ObreJocCommand))]
        [ObservableProperty]
        bool puntuant = false;

        [RelayCommand(CanExecute = nameof(PotObrirConfiguracio))]
        private void ObreConfiguracio()
        {
            Configurant = true;
            Jugant = false;
            Puntuant = false;
        }

        private bool PotObrirConfiguracio()
        {
            return !Configurant;
        }

        [RelayCommand(CanExecute = nameof(PotObrirPuntuacions))]
        private void ObrePuntuacions()
        {
            Configurant = false;
            Jugant = false;
            Puntuant = true;
        }

        private bool PotObrirPuntuacions()
        {
            return !Puntuant;
        }

        [RelayCommand(CanExecute = nameof(PotObrirJoc))]
        private void ObreJoc()
        {
            Configurant = false;
            Jugant = true;
            Puntuant = false;
        }

        private bool PotObrirJoc()
        {
            return Nom != null && !Jugant;
        }

        [RelayCommand(CanExecute = nameof(PotAbandonar))]
        private void Abandona()
        {
            this.ObreConfiguracio();
        }

        private bool PotAbandonar()
        {
            return Jugant;
        }

        [RelayCommand]
        private void Sortir()
        {
            repositoriDeJugadors.Surt();
        }

        [NotifyCanExecuteChangedFor(nameof(ClassicCommand))]
        [NotifyCanExecuteChangedFor(nameof(ExtensCommand))]
        [ObservableProperty]
        bool modeExtens = true;

        [RelayCommand(CanExecute = nameof(PotClassic))]
        private void Classic()
        {
            ModeExtens = false;
        }

        private bool PotClassic()
        {
            return ModeExtens;
        }

        [RelayCommand(CanExecute = nameof(PotExtens))]
        private void Extens()
        {
            ModeExtens = true;
        }

        private bool PotExtens()
        {
            return !ModeExtens;
        }

        [NotifyCanExecuteChangedFor(nameof(TiradaCommand))]
        [ObservableProperty]
        int rondes;

        [RelayCommand]
        private void NumRondes(string nRondes)
        {
            Rondes = Convert.ToInt32(nRondes);
        }

        [RelayCommand]
        private void Comença()
        {
            if (Nom != null && Rondes > 0)
            {
                Jugador = new Jugador()
                {
                    Id = Guid.NewGuid().ToString(),
                    Nom = this.Nom,
                    PartidesGuanyadesTorneig = 0,
                    Puntuacio = 0,
                    PartidesGuanyades = 0,
                    RondesGuanyades = 0,
                    RondesPerdudes = 0
                };
                Jugador.Foto = $"https://loremflickr.com/320/240/portrait?lock={Jugador.Puntuacio.GetHashCode() % 113}";
            }
            if (Jugador != null)
            {
                BotsGuanyats = new ObservableCollection<Jugador>();
                RondesGuanyadesaBot = 0;
                RondesGuanyadesJugador = 0;
                FotoJugador = "../Imatges/bot.png";
                Ronda = 0;
                Resultat = "";
                Configurant = false;
                Jugant = true;
                Puntuant = false;
                Bot = repositoriDeJugadors.CrearBot();
                repositoriDeJugadors.Afegeix(Jugador);
                Jugadors = repositoriDeJugadors.ObtenJugadors();
                FotoBot = Bot.Foto;
            }
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EliminaCommand))]
        int posicio;

        [RelayCommand(CanExecute = nameof(PotEliminar))]
        public void Elimina()
        {
            repositoriDeJugadors.Esborra(Jugadors[Posicio].Id);
            if (Posicio == Jugadors.Count - 1) Posicio++;
            Jugadors = repositoriDeJugadors.ObtenJugadors();
        }

        public bool PotEliminar()
        {
            return Posicio != -1;
        }

        [NotifyCanExecuteChangedFor(nameof(TiradaCommand))]
        [ObservableProperty]
        int ronda = 0;
        [ObservableProperty]
        int rondesGuanyadesJugador = 0;
        [ObservableProperty]
        int rondesGuanyadesaBot = 0;
        [NotifyCanExecuteChangedFor(nameof(TirarCommand))]
        [ObservableProperty]
        int ma;
        [ObservableProperty]
        string fotoJugador = "../Imatges/bot.png";
        [NotifyCanExecuteChangedFor(nameof(SeguentCommand))]
        [ObservableProperty]
        string resultat;

        [RelayCommand(CanExecute = nameof(PotTirada))]
        public void Tirada(string nMa)
        {
            Ma = Convert.ToInt32(nMa);
            switch (Ma)
            {
                case 1:
                    FotoJugador = "../Imatges/pedra.png";
                    break;
                case 2:
                    FotoJugador = "../Imatges/paper.png";
                    break;
                case 3:
                    FotoJugador = "../Imatges/tisores.png";
                    break;
                case 4:
                    FotoJugador = "../Imatges/llangardaix.png";
                    break;
                case 5:
                    FotoJugador = "../Imatges/spock.png";
                    break;
            }
        }

        private bool PotTirada()
        {
            return Ronda < Rondes;
        }

        [RelayCommand(CanExecute = nameof(PotTirar))]
        public void Tirar()
        {
            Random r = new Random();
            if (Ma > 0 && Ma < 6)
            {
                int maBot;
                if (ModeExtens == false) maBot = r.Next(1, 4);
                else maBot = r.Next(1, 6);
                int resultat = repositoriDeJugadors.HaGuanyat(Ma, maBot);
                if (resultat == 1) RondesGuanyadesJugador++;
                else if (resultat == 3) RondesGuanyadesaBot++;
                if (resultat != 2) Ronda++;
                switch (maBot)
                {
                    case 1:
                        FotoBot = "../Imatges/pedra.png";
                        break;
                    case 2:
                        FotoBot = "../Imatges/paper.png";
                        break;
                    case 3:
                        FotoBot = "../Imatges/tisores.png";
                        break;
                    case 4:
                        FotoBot = "../Imatges/llangardaix.png";
                        break;
                    case 5:
                        FotoBot = "../Imatges/spock.png";
                        break;
                }
                if (RondesGuanyadesaBot == Rondes / 2 + 1) { Resultat = "DERROTA"; }
                else if (RondesGuanyadesJugador == Rondes / 2 + 1) { Resultat = "VICTORIA"; BotsGuanyats.Add(Bot); }
                else Bot = repositoriDeJugadors.CrearBot();
            }
            Ma = 0;
        }

        public bool PotTirar()
        {
            return Ma > 0 && Ma < 6 && Ronda < Rondes && RondesGuanyadesaBot < Rondes / 2 + 1 && RondesGuanyadesJugador < Rondes / 2 + 1;
        }

        [RelayCommand(CanExecute = nameof(PotSeguent))]
        private void Seguent()
        {
            Resultat = "";
            FotoJugador = "../Imatges/bot.png";
            Ronda = 0;
            RondesGuanyadesaBot = 0;
            RondesGuanyadesJugador = 0;
            Bot = repositoriDeJugadors.CrearBot();
            FotoBot = Bot.Foto;
        }

        private bool PotSeguent()
        {
            return Resultat == "VICTORIA";
        }
    }
}
