using PPTLS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPTLS.Repositori
{
    public interface IRepositoriDeJugadors
    {
        bool Afegeix(Jugador jugador);
        Jugador CrearBot();
        void Desa(ObservableCollection<Jugador> jugador);
        bool Esborra(string id);
        bool Modifica(Jugador jugador);
        ObservableCollection<Jugador> ObtenJugadors();
        void Surt();
        int HaGuanyat(int ma, int maBot);
        ObservableCollection<Jugador> Ordena(ObservableCollection<Jugador> jugadors);
    }
}
