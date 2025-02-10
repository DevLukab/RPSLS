using PPTLS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace PPTLS.Repositori.XML
{
    class JugadorsXML : IRepositoriDeJugadors
    {
        const string NOM_FITXER_XML = @"BBDD\Jugadors.xml";
        string RUTA_FITXER_XML = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), NOM_FITXER_XML);
        private MainWindow finestra = Application.Current.MainWindow as MainWindow;

        public bool Afegeix(Jugador jugador)
        {
            bool afegit = false;
            ObservableCollection<Jugador> jugadors = ObtenJugadors();
            Jugador jugadorModificable = jugadors.FirstOrDefault(jugadorActual => jugadorActual.Nom == jugador.Nom);
            if (jugadorModificable == null)
            {
                jugador.Foto = $"https://loremflickr.com/320/240/portrait?lock={jugador.Id.GetHashCode() % 113}";
                jugadors.Add(jugador);
                afegit = true;
            }
            Desa(jugadors);
            return afegit;
        }

        public Jugador CrearBot()
        {
            Random r = new Random();
            List<string> noms = new List<string> { "Rayo", "Sombra", "Trueno", "Fuego", "Espectro", "Órbita", "Draco", "Ceniza", "Quasar", "Nebula" };

            Jugador bot = null;

            bot = new Jugador()
            {
                Nom = noms[r.Next(noms.Count)],
                PartidesGuanyadesTorneig = r.Next(0, 10),
                Puntuacio = r.Next(0, 1000),
                PartidesGuanyades = r.Next(0, 100),
                RondesGuanyades = r.Next(0, 100),
                RondesPerdudes = r.Next(0, 100),
                Foto = $"https://loremflickr.com/320/240/portrait?lock={r.Next(0, 100)}"
            };

            return bot;
        }

        public void Desa(ObservableCollection<Jugador> jugador)
        {
            using (TextWriter fitxer = new StreamWriter(RUTA_FITXER_XML))
            {
                XmlSerializer serialitzador = new XmlSerializer(typeof(ObservableCollection<Jugador>));
                serialitzador.Serialize(fitxer, jugador);
            }
        }

        public bool Esborra(string id)
        {
            bool esborrat = false;
            ObservableCollection<Jugador> jugadors = ObtenJugadors();
            Jugador jugadorEliminat = jugadors.FirstOrDefault(jugador => jugador.Id == id);
            if (jugadorEliminat != null)
            {
                jugadors.Remove(jugadorEliminat);
                esborrat = true;
            }
            Desa(jugadors);
            return esborrat;

        }

        public bool Modifica(Jugador jugador)
        {
            bool modificat = false;
            ObservableCollection<Jugador> jugadors = ObtenJugadors();
            Jugador jugadorModificat = jugadors.FirstOrDefault(jugadorActual => jugadorActual.Id == jugador.Id);
            if (jugadorModificat != null)
            {
                jugadorModificat.Nom = jugador.Nom;
                jugadorModificat.PartidesGuanyadesTorneig = jugador.PartidesGuanyadesTorneig;
                jugadorModificat.Puntuacio = jugador.Puntuacio;
                jugadorModificat.PartidesGuanyades = jugador.PartidesGuanyades;
                jugadorModificat.RondesGuanyades = jugador.RondesGuanyades;
                jugadorModificat.RondesPerdudes = jugador.RondesPerdudes;
                modificat = true;
            }
            Desa(jugadors);
            return modificat;
        }

        public void ObreConfiguracio()
        {
            finestra.dckConfiguracio.Visibility = Visibility.Visible;
            finestra.grdJoc.Visibility = Visibility.Collapsed;
            finestra.grdPuntuacions.Visibility = Visibility.Collapsed;
        }

        public void ObreJoc()
        {
            finestra.grdJoc.Visibility = Visibility.Visible;
            finestra.dckConfiguracio.Visibility = Visibility.Collapsed;
            finestra.grdPuntuacions.Visibility = Visibility.Collapsed;
        }

        public void ObrePuntuacio()
        {
            finestra.grdPuntuacions.Visibility = Visibility.Visible;
            finestra.dckConfiguracio.Visibility = Visibility.Collapsed;
            finestra.grdJoc.Visibility = Visibility.Collapsed;
        }

        public ObservableCollection<Jugador> ObtenJugadors()
        {
            ObservableCollection<Jugador> jugadors;

            using (TextReader fitxer = new StreamReader(RUTA_FITXER_XML))
            {
                if (fitxer.Peek() != -1)
                {
                    XmlSerializer serialitzador = new XmlSerializer(typeof(ObservableCollection<Jugador>));
                    jugadors = (ObservableCollection<Jugador>)serialitzador.Deserialize(fitxer);
                }
                else
                {
                    jugadors = new ObservableCollection<Jugador>();
                }
            }
            return Ordena(jugadors);
        }

        public void Surt()
        {
            finestra.Close();
        }

        public int HaGuanyat(int ma, int maBot)
        {
            int haGuanyat = 0;

            if (ma == maBot)
            {
                haGuanyat = 2;
            }
            else
            {
                switch (ma)
                {
                    case 1:
                        haGuanyat = (maBot == 3 || maBot == 4) ? 1 : 3;
                        break;
                    case 2:
                        haGuanyat = (maBot == 1 || maBot == 5) ? 1 : 3;
                        break;
                    case 3:
                        haGuanyat = (maBot == 2 || maBot == 4) ? 1 : 3;
                        break;
                    case 4:
                        haGuanyat = (maBot == 2 || maBot == 5) ? 1 : 3;
                        break;
                    case 5:
                        haGuanyat = (maBot == 1 || maBot == 3) ? 1 : 3;
                        break;
                }
            }

            return haGuanyat;
        }

        public ObservableCollection<Jugador> Ordena(ObservableCollection<Jugador> jugadors)
        {
            List<Jugador> llistaJugadors = new List<Jugador>(jugadors);
            llistaJugadors.Sort();

            jugadors.Clear();
            foreach (var jugador in llistaJugadors)
            {
                jugadors.Add(jugador);
            }
            return jugadors;
        }
    }
}
